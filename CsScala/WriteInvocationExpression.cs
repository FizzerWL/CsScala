using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CsScala.Translations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class WriteInvocationExpression
    {
        public static void Go(ScalaWriter writer, InvocationExpressionSyntax invocationExpression)
        {
            var model = Program.GetModel(invocationExpression);

            var symbolInfo = model.GetSymbolInfo(invocationExpression);
            var expressionSymbol = model.GetSymbolInfo(invocationExpression.Expression);
            var methodSymbol = symbolInfo.Symbol.OriginalDefinition.As<IMethodSymbol>().UnReduce();

            var translateOpt = MethodTranslation.Get(symbolInfo.Symbol.As<IMethodSymbol>());
            var memberReferenceExpressionOpt = invocationExpression.Expression as MemberAccessExpressionSyntax;
            var firstParameter = true;

            var extensionNamespace = methodSymbol.IsExtensionMethod ? methodSymbol.ContainingNamespace.FullNameWithDot() + methodSymbol.ContainingType.Name : null; //null means it's not an extension method, non-null means it is
            string methodName;
            string typeParameters = null;
            ExpressionSyntax subExpressionOpt;

            if (methodSymbol.ContainingType.Name == "Enum")
            {
                if (methodSymbol.Name == "Parse")
                {
                    WriteEnumParse(writer, invocationExpression);
                    return;
                }

                if (methodSymbol.Name == "TryParse")
                {
                    WriteEnumTryParse(writer, invocationExpression);
                    return;
                }

                if (methodSymbol.Name == "GetValues")
                {
                    WriteEnumGetValues(writer, invocationExpression);
                    return;
                }
            }

            if (expressionSymbol.Symbol is IEventSymbol)
            {
                methodName = "Invoke"; //Would need to append the number of arguments to this to support events.  However, events are not currently supported
            }
            else if (memberReferenceExpressionOpt != null && memberReferenceExpressionOpt.Expression is PredefinedTypeSyntax)
            {
                switch (methodSymbol.Name)
                {
                    case "Parse":
                        Core.Write(writer, invocationExpression.ArgumentList.Arguments.Single().Expression);

                        writer.Write(".to");
                        writer.Write(TypeProcessor.ConvertType(methodSymbol.ReturnType));

                        return;
                    case "TryParse":
                        methodName = "TryParse" + TypeProcessor.ConvertType(methodSymbol.Parameters[1].Type);
                        extensionNamespace = "CsScala";
                        break;
                    default:
                        methodName = methodSymbol.Name;
                        extensionNamespace = "CsScala";
                        break;
                }
            }
            else if (translateOpt != null && translateOpt.ReplaceWith != null)
                methodName = translateOpt.ReplaceWith;
            else if (methodSymbol.MethodKind == MethodKind.DelegateInvoke)
                methodName = null;
            else
                methodName = OverloadResolver.MethodName(methodSymbol);

            if (translateOpt != null && translateOpt.HasComplexReplaceWith)
            {
                translateOpt.DoComplexReplaceWith(writer, memberReferenceExpressionOpt);
                return;
            }

            if (translateOpt != null && translateOpt.SkipExtensionParameter)
                subExpressionOpt = null;
            else if (methodSymbol.MethodKind == MethodKind.DelegateInvoke)
                subExpressionOpt = invocationExpression.Expression;
            else if (memberReferenceExpressionOpt != null)
            {
                if (memberReferenceExpressionOpt.Expression is PredefinedTypeSyntax)
                    subExpressionOpt = null;
                else
                    subExpressionOpt = memberReferenceExpressionOpt.Expression;
            }
            else
                subExpressionOpt = null;




            //When the code specifically names generic arguments, include them in the method name
            var genNameExpression = invocationExpression.Expression as GenericNameSyntax;
            if (genNameExpression == null && memberReferenceExpressionOpt != null)
                genNameExpression = memberReferenceExpressionOpt.Name as GenericNameSyntax;
            if (genNameExpression != null && genNameExpression.TypeArgumentList.Arguments.Count > 0)
                typeParameters = "[" + string.Join(", ", genNameExpression.TypeArgumentList.Arguments.Select(TypeProcessor.ConvertType)) + "]";


            //Determine if it's an extension method called in a non-extension way.  In this case, just pretend it's not an extension method
            if (extensionNamespace != null && subExpressionOpt != null && model.GetTypeInfo(subExpressionOpt).ConvertedType.ToString() == methodSymbol.ContainingNamespace + "." + methodSymbol.ContainingType.Name)
                extensionNamespace = null;

            if (translateOpt != null && !string.IsNullOrEmpty(translateOpt.ExtensionNamespace))
                extensionNamespace = translateOpt.ExtensionNamespace;
            else if (translateOpt != null && translateOpt.ExtensionNamespace == "")
                extensionNamespace = null;

            var memberType = memberReferenceExpressionOpt == null ? null : model.GetTypeInfo(memberReferenceExpressionOpt.Expression).Type;
            var isNullableEnum = memberType != null && (memberType.Name == "Nullable" && memberType.ContainingNamespace.FullName() == "System") && memberType.As<INamedTypeSymbol>().TypeArguments.Single().TypeKind == TypeKind.Enum;
            if (isNullableEnum && methodSymbol.Name == "ToString")
            {
                extensionNamespace = null; //override Translations.xml for nullable enums. We want them to convert to the enum's ToString method
                methodName = "toString";
            }


            if (extensionNamespace != null)
            {
                writer.Write(extensionNamespace);

                if (methodName != null)
                {
                    writer.Write(".");
                    writer.Write(methodName);
                }

                WriteTypeParameters(writer, translateOpt, typeParameters, invocationExpression);


                writer.Write("(");

                if (subExpressionOpt != null)
                {
                    firstParameter = false;
                    Core.Write(writer, subExpressionOpt);
                }
            }
            else
            {
                if (memberReferenceExpressionOpt != null)
                {
                    //Check against lowercase toString since it gets replaced with the lowered version before we get here
                    if (methodName == "toString")
                    {
                        if (memberType.TypeKind == TypeKind.Enum || isNullableEnum)
                        {
                            var enumType = memberType.TypeKind == TypeKind.Enum ? memberType : memberType.As<INamedTypeSymbol>().TypeArguments.Single();

                            //calling ToString() on an enum forwards to our enum's special ToString method
                            writer.Write(enumType.ContainingNamespace.FullNameWithDot());
                            writer.Write(WriteType.TypeName((INamedTypeSymbol)enumType));
                            writer.Write(".ToString(");
                            Core.Write(writer, memberReferenceExpressionOpt.Expression);
                            writer.Write(")");

                            if (invocationExpression.ArgumentList.Arguments.Count > 0)
                                throw new Exception("Enum's ToString detected with parameters.  These are not supported " + Utility.Descriptor(invocationExpression));

                            return;
                        }

                        if (memberType.SpecialType == SpecialType.System_Byte)
                        {
                            //Calling ToString on a byte needs to take special care since it's signed in the JVM
                            writer.Write("System.CsScala.ByteToString(");
                            Core.Write(writer, memberReferenceExpressionOpt.Expression);
                            writer.Write(")");

                            if (invocationExpression.ArgumentList.Arguments.Count > 0)
                                throw new Exception("Byte's ToString detected with parameters.  These are not supported " + Utility.Descriptor(invocationExpression));

                            return;
                        }

                    }

                }

                if (subExpressionOpt != null)
                {
                    WriteMemberAccessExpression.WriteMember(writer, subExpressionOpt);

                    if (methodName != null)
                        writer.Write(".");
                }
                else if (methodSymbol.IsStatic && extensionNamespace == null)
                {
                    writer.Write(methodSymbol.ContainingNamespace.FullNameWithDot());
                    writer.Write(WriteType.TypeName(methodSymbol.ContainingType));
                    writer.Write(".");
                }


                writer.Write(methodName);
                WriteTypeParameters(writer, translateOpt, typeParameters, invocationExpression);
                writer.Write("(");
            }

            bool inParams = false;
            bool foundParamsArray = false;
            foreach (var arg in TranslateParameters(translateOpt, invocationExpression.ArgumentList.Arguments, invocationExpression))
            {
                if (firstParameter)
                    firstParameter = false;
                else
                    writer.Write(", ");

                if (!inParams && IsParamsArgument(invocationExpression, arg.ArgumentOpt, methodSymbol))
                {
                    foundParamsArray = true;

                    if (!TypeProcessor.ConvertType(model.GetTypeInfo(arg.ArgumentOpt.Expression).Type).StartsWith("Array["))
                    {
                        inParams = true;
                        writer.Write("Array(");
                    }
                }


                if (arg.ArgumentOpt != null
                    && arg.ArgumentOpt.RefOrOutKeyword.Kind() != SyntaxKind.None
                    && model.GetSymbolInfo(arg.ArgumentOpt.Expression).Symbol is IFieldSymbol)
                    throw new Exception("ref/out cannot reference fields, only local variables.  Consider using ref/out on a local variable and then assigning it into the field. " + Utility.Descriptor(invocationExpression));


                //When passing an argument by ref or out, leave off the .Value suffix
                if (arg.ArgumentOpt != null && arg.ArgumentOpt.RefOrOutKeyword.Kind() != SyntaxKind.None)
                    WriteIdentifierName.Go(writer, arg.ArgumentOpt.Expression.As<IdentifierNameSyntax>(), true);
                else
                    arg.Write(writer);

            }

            if (inParams)
                writer.Write(")");
            else if (!foundParamsArray && methodSymbol.Parameters.Any() && methodSymbol.Parameters.Last().IsParams)
                writer.Write(", Array()"); //params method called without any params argument.  Send an empty array.


            writer.Write(")");
        }

        private static void WriteTypeParameters(ScalaWriter writer, MethodTranslation translateOpt, string typeParameters, InvocationExpressionSyntax invoke)
        {
            if (translateOpt != null)
            {
                if (translateOpt.WriteTypeParameters(writer, invoke))
                    return;
            }

            if (typeParameters != null)
                writer.Write(typeParameters);
        }

        private static bool IsParamsArgument(InvocationExpressionSyntax invocationExpression, ArgumentSyntax argumentOpt, IMethodSymbol methodSymbol)
        {
            if (argumentOpt == null)
                return false;

            if (invocationExpression.ArgumentList.Arguments.Any(o => o.NameColon != null))
                return false; //params cannot be used with named arguments

            int i = invocationExpression.ArgumentList.Arguments.IndexOf(argumentOpt);
            return methodSymbol.Parameters.ElementAt(i).IsParams;
        }

        /// <summary>
        /// calls to Enum.Parse get re-written as calls to our special Parse methods on each enum.  We assume the first parameter to Enum.Parse is a a typeof()
        /// </summary>
        private static void WriteEnumParse(ScalaWriter writer, InvocationExpressionSyntax invocationExpression)
        {
            var args = invocationExpression.ArgumentList.Arguments;

            if (args.Count < 2 || args.Count > 3)
                throw new Exception("Expected 2-3 args to Enum.Parse");

            if (args.Count == 3 && (!(args[2].Expression is LiteralExpressionSyntax) || args[2].Expression.As<LiteralExpressionSyntax>().ToString() != "false"))
                throw new NotImplementedException("Case-insensitive Enum.Parse is not supported " + Utility.Descriptor(invocationExpression));

            if (!(args[0].Expression is TypeOfExpressionSyntax))
                throw new Exception("Expected a typeof() expression as the first parameter of Enum.Parse " + Utility.Descriptor(invocationExpression));

            var type = Program.GetModel(invocationExpression).GetTypeInfo(args[0].Expression.As<TypeOfExpressionSyntax>().Type).Type;
            writer.Write(type.ContainingNamespace.FullNameWithDot());
            writer.Write(WriteType.TypeName((INamedTypeSymbol)type));
            writer.Write(".Parse(");
            Core.Write(writer, args[1].Expression);
            writer.Write(")");
        }

        /// <summary>
        /// calls to Enum.Parse get re-written as calls to CsScala.EnumTryParse, where we pass in our special Parse methods on each enum.  
        /// </summary>
        private static void WriteEnumTryParse(ScalaWriter writer, InvocationExpressionSyntax invocationExpression)
        {
            var args = invocationExpression.ArgumentList.Arguments;

            if (args.Count != 2)
                throw new Exception("Expected 2 args to Enum.TryParse.  Other overloads are not supported");

            writer.Write("System.CsScala.EnumTryParse(");
            Core.Write(writer, args[0].Expression);
            writer.Write(", ");
            writer.Write(args[1].Expression.As<IdentifierNameSyntax>().Identifier.ToString());
            writer.Write(", ");

            var type = Program.GetModel(invocationExpression).GetTypeInfo(args[1].Expression).Type;
            writer.Write(type.ContainingNamespace.FullNameWithDot());
            writer.Write(WriteType.TypeName((INamedTypeSymbol)type));
            writer.Write(".Parse)");
        }

        private static void WriteEnumGetValues(ScalaWriter writer, InvocationExpressionSyntax invocationExpression)
        {
            if (!(invocationExpression.ArgumentList.Arguments[0].Expression is TypeOfExpressionSyntax))
                throw new Exception("Expected a typeof() expression as the first parameter of Enum.GetValues " + Utility.Descriptor(invocationExpression));

            var type = Program.GetModel(invocationExpression).GetTypeInfo(invocationExpression.ArgumentList.Arguments[0].Expression.As<TypeOfExpressionSyntax>().Type).Type;

            writer.Write(type.ContainingNamespace.FullNameWithDot());
            writer.Write(WriteType.TypeName((INamedTypeSymbol)type));
            writer.Write(".Values");
        }


        private static IEnumerable<TransformedArgument> TranslateParameters(MethodTranslation translateOpt, IEnumerable<ArgumentSyntax> list, InvocationExpressionSyntax invoke)
        {
            if (translateOpt == null)
                return list.Select(o => new TransformedArgument(o));
            else if (translateOpt is Translations.MethodTranslation)
                return translateOpt.As<Translations.MethodTranslation>().TranslateParameters(list, invoke.Expression);
            else
                throw new Exception("Need handler for " + translateOpt.GetType().Name);
        }


    }
}
