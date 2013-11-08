using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteElementAccessExpression
    {
        public static void Go(ScalaWriter writer, ElementAccessExpressionSyntax expression)
        {
            Core.Write(writer, expression.Expression);

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
