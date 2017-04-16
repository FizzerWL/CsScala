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
    static class WriteTryStatement
    {
        public static void Go(ScalaWriter writer, TryStatementSyntax tryStatement)
        {

            writer.WriteLine("try");
            Core.Write(writer, tryStatement.Block);

            var catches = tryStatement.Catches.Where(o => Program.DoNotWrite.ContainsKey(o) == false).ToList();

            if (catches.Count > 0)
            {
                writer.WriteLine("catch");
                writer.WriteOpenBrace();

                foreach (var catchClause in catches)
                {
                    writer.WriteIndent();

                    //In C#, the base exception type is Exception, but on the JVM it is Throwable.  Normally, JVM programs should not catch throwable, so we map the C# Exception type to the JVM Exception type by default.  We attempted to change Exception to map to Throwable but ran into issues with things getting caught that shouldn't, such as Scala's "BreakControl" that's used on break statements.
                    //if C# code really wants to catch all throwables, catch Exception and name the variable "allThrowables".  This is a signal to CSScala that all throwables should be caught.  However, use it with care, as it can cause complications.
                    if (catchClause.Declaration == null)
                    {
                        writer.Write("case __ex: java.lang.Exception => ");
                    }
                    else
                    {

                        writer.Write("case ");
                        writer.Write(string.IsNullOrWhiteSpace(catchClause.Declaration.Identifier.ValueText) ? "__ex" : WriteIdentifierName.TransformIdentifier(catchClause.Declaration.Identifier.ValueText));
                        writer.Write(": ");


                        if (catchClause.Declaration.Identifier.ValueText == "allThrowables")
                            writer.Write("java.lang.Throwable");
                        else
                            writer.Write(TypeProcessor.ConvertType(catchClause.Declaration.Type));

                        writer.Write(" =>\r\n");
                    }

                    writer.Indent++;
                    foreach (var statement in catchClause.Block.Statements)
                        Core.Write(writer, statement);
                    writer.Indent--;
                }

                writer.WriteCloseBrace();
            }

            if (tryStatement.Finally != null)
            {
                writer.WriteLine("finally");
                Core.Write(writer, tryStatement.Finally.Block);
            }
        }
    }
}
