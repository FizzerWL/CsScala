using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteForEachStatement
    {
        public static void Go(ScalaWriter writer, ForEachStatementSyntax foreachStatement)
        {
            var info = new LoopInfo(foreachStatement);

            info.WritePreLoop(writer);
            writer.WriteIndent();
            writer.Write("for (");
            writer.Write(WriteIdentifierName.TransformIdentifier(foreachStatement.Identifier.ValueText));
            writer.Write(" <- ");
            Core.Write(writer, foreachStatement.Expression);
            writer.Write(")\r\n");
            writer.WriteOpenBrace();
            info.WriteLoopOpening(writer);

            if (foreachStatement.Statement is BlockSyntax)
                foreach (var statement in foreachStatement.Statement.As<BlockSyntax>().Statements)
                    Core.Write(writer, statement);
            else
                Core.Write(writer, foreachStatement.Statement);

            info.WriteLoopClosing(writer);
            writer.WriteCloseBrace();
            info.WritePostLoop(writer);
        }

    }
}
