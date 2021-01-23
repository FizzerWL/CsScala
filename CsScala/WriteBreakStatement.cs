using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class WriteBreakStatement
    {
        public static void Go(ScalaWriter writer, BreakStatementSyntax statement)
        {
            //Traverse up to figure out what we're breaking from.  If we're breaking from a loop, it's fine.  However, if we're breaking from a switch statement, throw an error as we don't allow this except directly inside of a case block.
            var breakingFrom = statement.Parent;
            while (!(breakingFrom is WhileStatementSyntax || breakingFrom is ForStatementSyntax || breakingFrom is DoStatementSyntax || breakingFrom is ForEachStatementSyntax))
            {
                if (breakingFrom is SwitchStatementSyntax)
                    throw new Exception("Cannot \"break\" from within a switch statement except directly inside of a case statement. " + Utility.Descriptor(statement));

                breakingFrom = breakingFrom.Parent;
            }

            writer.WriteLine("CsScala.csbreak.break;");
        }
    }
}
