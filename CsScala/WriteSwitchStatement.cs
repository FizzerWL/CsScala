using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
	static class WriteSwitchStatement
	{
		public static void Go(ScalaWriter writer, SwitchStatementSyntax switchStatement)
		{
			writer.WriteIndent();
			Core.Write(writer, switchStatement.Expression);
			writer.Write(" match\r\n");
			writer.WriteOpenBrace();

			//First process all blocks except the section with the default block
			foreach (var section in switchStatement.Sections.Where(o => o.Labels.None(z => z.CaseOrDefaultKeyword.Kind == SyntaxKind.DefaultKeyword)))
			{
				writer.WriteIndent();
				writer.Write("case ");


				var firstLabel = true;
				foreach (var label in section.Labels)
				{
					if (firstLabel)
						firstLabel = false;
					else
						writer.Write(" | ");

					Core.Write(writer, label.Value, true);

				}
				writer.Write(" =>\r\n");
				writer.Indent++;

				foreach (var statement in section.Statements)
					if (!(statement is BreakStatementSyntax))
						Core.Write(writer, statement);

				writer.Indent--;
			}

			//Now write the default section
			var defaultSection = switchStatement.Sections.SingleOrDefault(o => o.Labels.Any(z => z.CaseOrDefaultKeyword.Kind == SyntaxKind.DefaultKeyword));
			if (defaultSection != null)
			{
				if (defaultSection.Labels.Count > 1)
					throw new Exception("Cannot fall-through into or out of the default section of switch statement " + Utility.Descriptor(defaultSection));

				writer.WriteLine("case _ =>");
				writer.Indent++;

				foreach (var statement in defaultSection.Statements)
					if (!(statement is BreakStatementSyntax))
						Core.Write(writer, statement);

				writer.Indent--;

			}


			writer.WriteCloseBrace();
		}
	}
}
