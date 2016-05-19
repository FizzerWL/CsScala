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
    static class WriteLockStatement
    {
        public static void Go(ScalaWriter writer, LockStatementSyntax statement)
        {
            if (statement.DescendantNodes().OfType<ReturnStatementSyntax>().Any())
                throw new Exception("Cannot return from within a lock statement " + Utility.Descriptor(statement));

            writer.WriteIndent();
            writer.Write("CsLock.Lock(");
            Core.Write(writer, statement.Expression);
            writer.Write(", () =>\r\n");
            writer.WriteOpenBrace();
            Core.WriteStatementAsBlock(writer, statement.Statement, false);
            writer.Indent--;
            writer.WriteIndent();
            writer.Write("});\r\n");

        }
    }
}
