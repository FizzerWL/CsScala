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

            var types = Program.GetModel(foreachStatement).GetTypeInfo(foreachStatement.Expression);
            var typeStr = TypeProcessor.GenericTypeName(types.Type);
            if (types.Type is ArrayTypeSymbol)
            {
                //It's faster to "while" through arrays than "for" through them
                writer.WriteOpenBrace();
                info.WritePreLoop(writer);

                writer.WriteLine("var __foreachindex:Int = 0;");

                writer.WriteIndent();
                writer.Write("val __foreacharray = ");
                Core.Write(writer, foreachStatement.Expression);
                writer.Write(";\r\n");


                writer.WriteLine("while (__foreachindex < __foreacharray.length)");
                writer.WriteOpenBrace();

                writer.WriteIndent();
                writer.Write("val ");
                writer.Write(WriteIdentifierName.TransformIdentifier(foreachStatement.Identifier.ValueText));
                writer.Write(" = __foreacharray(__foreachindex);\r\n");

                info.WriteLoopOpening(writer);

                if (foreachStatement.Statement is BlockSyntax)
                    foreach (var statement in foreachStatement.Statement.As<BlockSyntax>().Statements)
                        Core.Write(writer, statement);
                else
                    Core.Write(writer, foreachStatement.Statement);



                info.WriteLoopClosing(writer);

                writer.WriteLine("__foreachindex += 1;");
                writer.WriteCloseBrace();

                info.WritePostLoop(writer);
                writer.WriteCloseBrace();
            }
            else if (typeStr == "System.Collections.Generic.List<>"
                //|| typeStr == "System.Collections.Generic.Dictionary<,>" 
                || typeStr == "System.Collections.Generic.Dictionary<,>.KeyCollection" 
                || typeStr == "System.Collections.Generic.Dictionary<,>.ValueCollection")
            {
                //It's faster to "while" over a list's iterator than to "for" through it
                writer.WriteOpenBrace();
                info.WritePreLoop(writer);

                writer.WriteIndent();
                writer.Write("val __foreachiterator = ");
                Core.Write(writer, foreachStatement.Expression);
                writer.Write(".iterator();\r\n");


                writer.WriteLine("while (__foreachiterator.hasNext())");
                writer.WriteOpenBrace();

                writer.WriteIndent();
                writer.Write("val ");
                writer.Write(WriteIdentifierName.TransformIdentifier(foreachStatement.Identifier.ValueText));
                writer.Write(" = __foreachiterator.next();\r\n");

                info.WriteLoopOpening(writer);

                if (foreachStatement.Statement is BlockSyntax)
                    foreach (var statement in foreachStatement.Statement.As<BlockSyntax>().Statements)
                        Core.Write(writer, statement);
                else
                    Core.Write(writer, foreachStatement.Statement);



                info.WriteLoopClosing(writer);
                writer.WriteCloseBrace();

                info.WritePostLoop(writer);
                writer.WriteCloseBrace();
            }
            else
            {

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
}
