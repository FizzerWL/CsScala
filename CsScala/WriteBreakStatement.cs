using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteBreakStatement
    {
        public static void Go(ScalaWriter writer, BreakStatementSyntax statement)
        {
            writer.WriteLine("CsScala.csbreak.break;");
        }
    }
}
