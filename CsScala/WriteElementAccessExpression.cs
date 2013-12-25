using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScala.Translations;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteElementAccessExpression
    {
        public static void Go(ScalaWriter writer, ElementAccessExpressionSyntax expression)
        {
            var typeStr = TypeProcessor.GenericTypeName(Program.GetModel(expression).GetTypeInfo(expression.Expression).Type);
            var trans = ElementAccessTranslation.Get(typeStr);

            Core.Write(writer, expression.Expression);

            if (trans != null)
            {
                writer.Write(".");
                writer.Write(trans.ReplaceGet);
            }

            writer.Write("(");

            bool first = true;
            foreach (var argument in expression.ArgumentList.Arguments)
            {
                if (first)
                    first = false;
                else
                    writer.Write(", ");

                Core.Write(writer, argument.Expression);
            }
            writer.Write(")");

        }
    }
}
