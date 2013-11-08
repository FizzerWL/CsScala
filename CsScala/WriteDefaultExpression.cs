using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteDefaultExpression
    {
        public static void Go(ScalaWriter writer, DefaultExpressionSyntax node)
        {
            writer.Write("null");
        }
    }
}
