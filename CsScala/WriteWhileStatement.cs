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

            if (whileStatement.Statement is BlockSyntax)
                foreach (var statement in whileStatement.Statement.As<BlockSyntax>().Statements)
                    Core.Write(writer, statement);
            else
                Core.Write(writer, whileStatement.Statement);

            info.WriteLoopClosing(writer);
            writer.WriteCloseBrace();
            info.WritePostLoop(writer);
        }
    }
}
