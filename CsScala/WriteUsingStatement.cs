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
    static class WriteUsingStatement
    {
        public static void Go(ScalaWriter writer, UsingStatementSyntax usingStatement)
        {
            var expression = usingStatement.Expression;

            //Ensure the using statement is a local variable - we can't deal with things we can't reliably repeat in the finally block
            var resource = Utility.TryGetIdentifier(expression);
            if (resource == null)
                throw new Exception("Using statements must reference a local variable. " + Utility.Descriptor(usingStatement));

            writer.WriteLine("try");
            Core.WriteStatementAsBlock(writer, usingStatement.Statement);
            writer.WriteLine("finally");
            writer.WriteOpenBrace();
            writer.WriteLine(resource + ".Dispose();");
            writer.WriteCloseBrace();
        }
    }
}
