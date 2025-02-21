﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CsScala.Translations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class TypeProcessor
    {
        public static string DefaultValue(TypeSyntax type)
        {
            return DefaultValue(TryGetTypeSymbol(type));
        }
        public static string DefaultValue(ITypeSymbol type)
        {
            if (type.TypeKind == TypeKind.TypeParameter)
                return "null.asInstanceOf[" + type.ToString() + "]";

            var scalaType = ConvertType(type);

            switch (scalaType)
            {
                case "Byte":
                case "Int":
                case "Float":
                case "Double":
                case "Short":
                    return "0";
                case "Long":
                    return "0L";
                case "Char":
                    return "'\\u0000'";
                case "Boolean":
                    return "false";
                default:
                    if (ValueToReference(type))
                    {
                        if (scalaType == "java.util.UUID")
                            return "new java.util.UUID(0,0)";
                        else
                            return "new " + scalaType + "()";
                    }
                    else
                        return "null";
            }
        }

        public static string TryConvertType(SyntaxNode node)
        {
            if (node == null)
                return null;

            var attrs = Utility.GetCsScalaAttribute(node);
            if (attrs.ContainsKey("ReplaceWithType"))
                return attrs["ReplaceWithType"];

            var sym = TryGetTypeSymbol(node);

            if (sym == null)
                throw new Exception("Could not get type of " + node.ToString() + " at " + Utility.Descriptor(node));
            return TryConvertType(sym);
        }

        public static ITypeSymbol GetTypeSymbol(SyntaxNode node)
        {
            var r = TryGetTypeSymbol(node);
            if (r == null)
                throw new Exception("Could not get type symbol for " + Utility.Descriptor(node));
            return r;
        }

        private static ITypeSymbol TryGetTypeSymbol(SyntaxNode node)
        {
            if (node == null)
                return null;

            var symbol = Program.GetModel(node).GetSymbolInfo(node);

            if (symbol.Symbol is ITypeSymbol)
                return (ITypeSymbol)symbol.Symbol;
            else if (symbol.Symbol is ILocalSymbol)
                return symbol.Symbol.As<ILocalSymbol>().Type;
            else if (symbol.Symbol is IFieldSymbol)
                return symbol.Symbol.As<IFieldSymbol>().Type;
            else if (symbol.Symbol is IParameterSymbol)
                return symbol.Symbol.As<IParameterSymbol>().Type;
            else
                return null;
        }

        public static string ConvertTypeWithColon(SyntaxNode node)
        {
            var ret = TryConvertType(node);

            if (ret == null)
                return "";
            else
                return ":" + ret;
        }


        public static string ConvertType(SyntaxNode node)
        {
            var ret = TryConvertType(node);

            if (ret == null)
                throw new Exception("Type could not be determined for " + node);

            return ret;
        }

        public static string ConvertTypeWithColon(ITypeSymbol node)
        {
            var ret = TryConvertType(node);

            if (ret == null)
                return "";
            else
                return ":" + ret;
        }

        private static ConcurrentDictionary<ITypeSymbol, string> _cachedTypes = new ConcurrentDictionary<ITypeSymbol, string>();

        public static string ConvertType(ITypeSymbol typeSymbol)
        {
            var ret = TryConvertType(typeSymbol);

            if (ret == null)
                throw new Exception("Could not convert type " + typeSymbol);
            return ret;
        }

        public static string TryConvertType(ITypeSymbol typeInfo)
        {
            //var specialized = SpecializedType.TryConvertType(typeInfo as INamedTypeSymbol);
            //if (specialized != null)
            //    return specialized;


            string cachedValue;
            if (_cachedTypes.TryGetValue(typeInfo, out cachedValue))
                return cachedValue;

            cachedValue = ConvertTypeUncached(typeInfo);
            _cachedTypes.TryAdd(typeInfo, cachedValue);
            return cachedValue;
        }

        private static string ConvertTypeUncached(ITypeSymbol typeSymbol)
        {
            if (typeSymbol.IsAnonymousType)
                return WriteAnonymousObjectCreationExpression.TypeName(typeSymbol.As<INamedTypeSymbol>());

            var array = typeSymbol as IArrayTypeSymbol;

            if (array != null)
                return "Array[" + TryConvertType(array.ElementType) + "]";

            var typeInfoStr = typeSymbol.ToString();

            var named = typeSymbol as INamedTypeSymbol;

            if (typeSymbol.TypeKind == TypeKind.TypeParameter)
                return typeSymbol.Name;

            if (typeSymbol.TypeKind == TypeKind.Delegate)
            {
                var dlg = named.DelegateInvokeMethod.As<IMethodSymbol>();
                if (dlg.Parameters.Length == 0)
                    return "(() => " + TryConvertType(dlg.ReturnType) + ")";
                else
                    return "((" + string.Join(", ", dlg.Parameters.ToList().Select(o => TryConvertType(o.Type))) + ") => " + TryConvertType(dlg.ReturnType) + ")";
            }

            if (typeSymbol.TypeKind == TypeKind.Enum)
                return "Int"; //enums are always ints

            if (named != null && named.Name == "Nullable" && named.ContainingNamespace.ToString() == "System")
            {
                //Nullable types, if value types, get replaced with the java.lang alternatives.  If reference types, just use them as-is
                var convertedType = TryConvertType(named.TypeArguments.Single());

                switch (convertedType)
                {
                    case "Int":
                        return "java.lang.Integer";
                    case "Boolean":
                        return "java.lang.Boolean";
                    case "Byte":
                        return "java.lang.Byte";
                    case "Short":
                        return "java.lang.Short";
                    case "Float":
                        return "java.lang.Float";
                    case "Double":
                        return "java.lang.Double";
                    case "Char":
                        return "java.lang.Char";
                    case "Long":
                        return "java.lang.Long";
                    default:
                        return convertedType;
                }
            }

            var typeStr = GenericTypeName(typeSymbol);

            var trans = TypeTranslation.Get(typeStr, named);

            if (named.IsGenericType() && !named.IsUnboundGenericType && (trans == null || trans.SkipGenericTypes == false))
                return TryConvertType(named.ConstructUnboundGenericType()) + "[" + string.Join(", ", TypeArguments(named).Select(o => TryConvertType(o))) + "]";


            switch (typeStr)
            {
                case "System.Void":
                    return "Unit";

                case "System.Boolean":
                    return "Boolean";

                case "System.Object":
                    return "Any";

                case "System.UInt64":
                case "System.Double":
                    return "Double";

                case "System.Single":
                    return "Float";

                case "System.String":
                    return "String";

                case "System.Int32":
                case "System.UInt16":
                    return "Int";


                case "System.Int64":
                case "System.UInt32":
                    return "Long";

                case "System.Byte":
                    return "Byte";

                case "System.Int16":
                    return "Short";

                case "System.Char":
                    return "Char";

                case "System.Array":
                    return null; //in scala, unlike C#, array must always have type arguments.  To work around this, just avoid printing the type anytime we see a bare Array type in C#. scala will infer it.

                default:
                    if (trans != null)
                        return trans.As<TypeTranslation>().Replace(named);

                    if (named != null)
                        return typeSymbol.ContainingNamespace.FullNameWithDot() + WriteType.TypeName(named);

                    //This type does not get translated and gets used as-is
                    return typeSymbol.ContainingNamespace.FullNameWithDot() + typeSymbol.Name;

            }

        }

        public static IEnumerable<ITypeSymbol> TypeArguments(INamedTypeSymbol named)
        {
            if (named.ContainingType != null)
            {
                //Hard-code generic types for dictionaries, since I can't find a way to determine them programatically
                switch (named.Name)
                {
                    case "ValueCollection":
                        return new[] { named.ContainingType.TypeArguments.ElementAt(1) };
                    case "KeyCollection":
                        return new[] { named.ContainingType.TypeArguments.ElementAt(0) };
                    default:
                        return named.TypeArguments.ToList();
                }
            }

            return named.TypeArguments.ToList();
        }



        public static string GenericTypeName(ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
                return null;

            var array = typeSymbol as IArrayTypeSymbol;
            if (array != null)
                return GenericTypeName(array.ElementType) + "[]";

            var named = typeSymbol as INamedTypeSymbol;

            if (named != null && named.IsGenericType && !named.IsUnboundGenericType)
                return GenericTypeName(named.ConstructUnboundGenericType());
            else if (named != null && named.SpecialType != SpecialType.None)
                return named.ContainingNamespace.FullNameWithDot() + named.Name; //this forces C# shortcuts like "int" to never be used, and instead returns System.Int32 etc.
            else
                return typeSymbol.ToString();
        }


        /// <summary>
        /// Returns true if this is a value type in C# but a reference type in scala.  We try to avoid this where possible, but scala doesn't let us define our own value types.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ValueToReference(ITypeSymbol typeSymbol)
        {
            if (typeSymbol.IsValueType == false)
                return false;

            if (typeSymbol.Name == "Nullable" && typeSymbol.ContainingNamespace.FullName() == "System")
                return false; //nullable types are reference types in .net

            var scalaType = TryConvertType(typeSymbol);
            return !IsPrimitiveType(scalaType);
        }

        public static bool IsPrimitiveType(string scalaType)
        {

            switch (scalaType)
            {
                case "Byte":
                case "Int":
                case "Float":
                case "Double":
                case "Short":
                case "Long":
                case "Char":
                case "Boolean":
                    return true;
                default:
                    return false;
            }
        }
    }
}
