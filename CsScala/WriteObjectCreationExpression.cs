using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScala.Translations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class WriteObjectCreationExpression
    {
        public static void Go(ScalaWriter writer, ObjectCreationExpressionSyntax expression)
        {
            var model = Program.GetModel(expression);
            var type = model.GetTypeInfo(expression).Type;

            if (expression.Initializer != null)
                throw new Exception("Object initializers are not supported " + Utility.Descriptor(expression));

            if (type.SpecialType == SpecialType.System_Object)
            {
                //new object() results in the CsObject type being made.  This is only really useful for locking
                writer.Write("new CsObject()");
            }

            else if (type.SpecialType == SpecialType.System_String)
            {
                //new String()
                writer.Write("System.CsScala.NewString(");
                bool first = true;
                foreach (var param in expression.ArgumentList.Arguments)
                {
                    if (first)
                        first = false;
                    else
                        writer.Write(", ");

                    Core.Write(writer, param.Expression);
                }
                writer.Write(")");
            }
            else if (type.OriginalDefinition is INamedTypeSymbol && type.OriginalDefinition.As<INamedTypeSymbol>().SpecialType == SpecialType.System_Nullable_T)
            {
                //new'ing up a Nullable<T> has special sematics in C#.  If we're calling this with no parameters, just use null. Otherwise just use the parameter.
                if (expression.ArgumentList.Arguments.Count == 0)
                    writer.Write("null");
                else
                    Core.Write(writer, expression.ArgumentList.Arguments.Single().Expression);
            }
            else
            {
                var methodSymbol = model.GetSymbolInfo(expression).Symbol.As<IMethodSymbol>();

                var translateOpt = MethodTranslation.Get(methodSymbol);

                if (translateOpt != null && translateOpt.ExtensionNamespace != null)
                {
                    writer.Write(translateOpt.ExtensionNamespace);
                    writer.Write(".");
                    writer.Write(translateOpt.ReplaceWith);
                }
                else
                {
                    writer.Write("new ");
                    writer.Write(TypeProcessor.ConvertType(expression.Type));
                }

                writer.Write("(");

                if (expression.ArgumentList != null)
                {
                    bool first = true;
                    foreach (var param in TranslateParameters(translateOpt, expression.ArgumentList.Arguments, expression))
                    {
                        if (first)
                            first = false;
                        else
                            writer.Write(", ");

                        param.Write(writer);
                    }
                }

                writer.Write(")");

            }
        }

        private static IEnumerable<TransformedArgument> TranslateParameters(MethodTranslation translateOpt, IEnumerable<ArgumentSyntax> list, ObjectCreationExpressionSyntax invoke)
        {
            if (translateOpt == null)
                return list.Select(o => new TransformedArgument(o));
            else if (translateOpt is MethodTranslation)
                return translateOpt.As<MethodTranslation>().TranslateParameters(list, invoke);
            else
                throw new Exception("Need handler for " + translateOpt.GetType().Name);
        }

    }
}
