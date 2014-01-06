using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScala.Translations;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteMemberAccessExpression
    {
        public static void Go(ScalaWriter writer, MemberAccessExpressionSyntax expression)
        {
            var model = Program.GetModel(expression);

            var memberName = expression.Name.Identifier.ValueText;
            var type = model.GetTypeInfo(expression.Expression).ConvertedType;
            var typeStr = TypeProcessor.GenericTypeName(type);

            if (expression.Expression is PredefinedTypeSyntax)
            {
                if (memberName == "MaxValue" || memberName == "MinValue" || memberName == "NaN")
                {
                    var predefined = expression.Expression.ToString();

                    if (predefined.StartsWith("u"))
                    {
                        //Scala does not have unsigned types. Forward these to CsScala
                        writer.Write("System.CsScala.");
                        writer.Write(predefined);
                        writer.Write(memberName);
                    }
                    else
                    {
                        writer.Write(predefined[0].ToString().ToUpper());
                        writer.Write(predefined.Substring(1));
                        writer.Write(".");
                        writer.Write(memberName);
                    }
                }
                else
                {
                    var field = System.Type.GetType(typeStr).GetField(memberName);

                    if (field == null)
                        throw new Exception("Cannot use " + memberName + " as a field.  If you're passing a function, wrap a closure around it. " + Utility.Descriptor(expression));
                    var val = field.GetValue(null);
                    if (val is string)
                        writer.Write("\"" + val + "\"");
                    else
                        writer.Write(val.ToString());
                }
            }
            else if (type.OriginalDefinition is NamedTypeSymbol && type.OriginalDefinition.As<NamedTypeSymbol>().SpecialType == Roslyn.Compilers.SpecialType.System_Nullable_T)
            {

                switch (memberName)
                {
                    case "HasValue":
                        writer.Write("(");
                        WriteMember(writer, expression.Expression);
                        writer.Write(" != null)");
                        break;
                    case "Value":
                        var nullableType = TypeProcessor.ConvertType(type.As<NamedTypeSymbol>().TypeArguments.Single());
                        WriteMember(writer, expression.Expression);

                        if (TypeProcessor.IsPrimitiveType(nullableType))
                        {
                            writer.Write(".");
                            writer.Write(nullableType[0].ToString().ToLower());
                            writer.Write(nullableType.Substring(1));
                            writer.Write("Value()");
                        }
                        break;
                    default:
                        throw new Exception("Need handler for Nullable." + memberName + " " + Utility.Descriptor(expression));
                }
            }
            else
            {

                var translate = PropertyTranslation.Get(typeStr, memberName);

                if (translate != null && translate.ExtensionMethod != null)
                {
                    writer.Write(translate.ExtensionMethod);
                    writer.Write("(");
                    Core.Write(writer, expression.Expression);
                    writer.Write(")");
                    return;
                }

                if (translate != null)
                    memberName = translate.ReplaceWith;
                else
                    memberName = WriteIdentifierName.TransformIdentifier(memberName);

                if (type != null) //if type is null, then we're just a namespace.  We can ignore these.
                {
                    WriteMember(writer, expression.Expression);
                    writer.Write(".");
                }

                writer.Write(memberName);

                if (expression.Name is GenericNameSyntax)
                {
                    var gen = expression.Name.As<GenericNameSyntax>();

                    writer.Write("[");

                    bool first = true;
                    foreach (var g in gen.TypeArgumentList.Arguments)
                    {
                        if (first)
                            first = false;
                        else
                            writer.Write(", ");

                        writer.Write(TypeProcessor.ConvertTypeWithColon(g));
                    }

                    writer.Write("]");
                }
            }
        }

        public static void WriteMember(ScalaWriter writer, ExpressionSyntax expression)
        {
            var symbol = Program.GetModel(expression).GetSymbolInfo(expression).Symbol;
            if (symbol is NamedTypeSymbol)
            {
                var translateOpt = TypeTranslation.Get(symbol.ContainingNamespace.FullNameWithDot() + symbol.Name);

                if (translateOpt != null)
                    writer.Write(translateOpt.ReplaceWith);
                else
                    writer.Write(symbol.ContainingNamespace.FullNameWithDot() + WriteType.TypeName(symbol.As<NamedTypeSymbol>()));
            }
            else
                Core.Write(writer, expression);


        }
    }
}
