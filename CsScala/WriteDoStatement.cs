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
    static class WriteDoStatement
    {
        public static void Go(ScalaWriter writer, DoStatementSyntax statement)
        {
            var info = new LoopInfo(statement);

            info.WritePreLoop(writer);
            writer.WriteLine("do");
            writer.WriteOpenBrace();
            info.WriteLoopOpening(writer);
            Core.WriteStatementAsBlock(writer, statement.Statement, false);
            info.WriteLoopClosing(writer);
            writer.WriteCloseBrace();

            writer.WriteIndent();
            writer.Write("while (");
            Core.Write(writer, statement.Condition);
            writer.Write(");\r\n");
            info.WritePostLoop(writer);
        }
    }
}
