using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScala.Translations;
using Roslyn.Compilers.CSharp;

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

            if (type.SpecialType == Roslyn.Compilers.SpecialType.System_Object)
            {
                //new object() results in the CsObject type being made.  This is only really useful for locking
                writer.Write("new CsObject()");
            }
            else if (type.OriginalDefinition.As<NamedTypeSymbol>().SpecialType == Roslyn.Compilers.SpecialType.System_Nullable_T)
            {
                //new'ing up a Nullable<T> has special sematics in C#.  If we're calling this with no parameters, just use null. Otherwise just use the parameter.
                if (expression.ArgumentList.Arguments.Count == 0)
                    writer.Write("null");
                else
                    Core.Write(writer, expression.ArgumentList.Arguments.Single().Expression);
            }
            else
            {
                var methodSymbol = model.GetSymbolInfo(expression).Symbol.As<MethodSymbol>();

                var translateOpt = MethodTranslation.Get(methodSymbol);


                writer.Write("new ");
                writer.Write(TypeProcessor.ConvertType(expression.Type));
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
