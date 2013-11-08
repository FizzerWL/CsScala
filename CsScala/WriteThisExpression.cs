using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteThisExpression
    {
        public static void Go(ScalaWriter writer, ThisExpressionSyntax expression)
        {
            writer.Write("this");
        }
    }
}
