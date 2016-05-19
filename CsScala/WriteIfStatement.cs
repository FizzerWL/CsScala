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
    static class WriteIfStatement
    {
        public static void Go(ScalaWriter writer, IfStatementSyntax ifStatement, bool indent = true)
        {
            if (indent)
                writer.WriteIndent();

            writer.Write("if (");
            Core.Write(writer, ifStatement.Condition);
            writer.Write(")\r\n");

            Core.WriteStatementAsBlock(writer, ifStatement.Statement);

            if (ifStatement.Else != null)
            {
                writer.WriteIndent();
                writer.Write("else");

                if (ifStatement.Else.Statement is BlockSyntax)
                {
                    writer.Write("\r\n");
                    Core.WriteBlock(writer, ifStatement.Else.Statement.As<BlockSyntax>());
                }
                else if (ifStatement.Else.Statement is IfStatementSyntax)
                {
                    writer.Write(" ");
                    WriteIfStatement.Go(writer, ifStatement.Else.Statement.As<IfStatementSyntax>(), false);
                }
                else
                {
                    writer.Write("\r\n");
                    Core.WriteStatementAsBlock(writer, ifStatement.Else.Statement);
                }
            }


        }
    }
}
