using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

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

					if (catchClause.Declaration == null)
					{
						writer.Write("case __ex: java.lang.Exception => ");
					}
					else
					{

						writer.Write("case ");
						writer.Write(string.IsNullOrWhiteSpace(catchClause.Declaration.Identifier.ValueText) ? "__ex" : WriteIdentifierName.TransformIdentifier(catchClause.Declaration.Identifier.ValueText));
						writer.Write(": ");
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
