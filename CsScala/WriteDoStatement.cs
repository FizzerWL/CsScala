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
            //scala3 no longer supports do/while so instead we use the workaround described by https://docs.scala-lang.org/scala3/reference/dropped-features/do-while.html
            var info = new LoopInfo(statement);

            info.WritePreLoop(writer);
            writer.WriteLine("while");
            writer.WriteOpenBrace();
            info.WriteLoopOpening(writer);
            Core.WriteStatementAsBlock(writer, statement.Statement, false);
            info.WriteLoopClosing(writer);


            writer.WriteIndent();
            Core.Write(writer, statement.Condition);
            writer.Write(";\r\n");

            writer.WriteCloseBrace();

            writer.WriteIndent();
            writer.Write("do();\r\n");
            info.WritePostLoop(writer);
        }
    }
}
