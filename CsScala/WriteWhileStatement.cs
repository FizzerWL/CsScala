using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteWhileStatement
    {
        public static void Go(ScalaWriter writer, WhileStatementSyntax whileStatement)
        {
            var info = new LoopInfo(whileStatement);

            info.WritePreLoop(writer);
            writer.WriteIndent();
            writer.Write("while (");
            Core.Write(writer, whileStatement.Condition);
            writer.Write(")\r\n");

            writer.WriteOpenBrace();
            info.WriteLoopOpening(writer);
            Core.WriteStatementAsBlock(writer, whileStatement.Statement, false);
            info.WriteLoopClosing(writer);
            writer.WriteCloseBrace();
            info.WritePostLoop(writer);
        }
    }
}
