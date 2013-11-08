using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
	static class WriteForStatement
	{
		public static void Go(ScalaWriter writer, ForStatementSyntax forStatement)
		{
			var info = new LoopInfo(forStatement);
			writer.WriteLine("{ //for");
			writer.Indent++;
			info.WritePreLoop(writer);

			if (forStatement.Declaration != null)
				foreach (var variable in forStatement.Declaration.Variables)
				{
					writer.WriteIndent();
					writer.Write("var ");
					writer.Write(WriteIdentifierName.TransformIdentifier(variable.Identifier.ValueText));
					writer.Write(TypeProcessor.ConvertTypeWithColon(forStatement.Declaration.Type));

					if (variable.Initializer != null)
					{
						writer.Write(" = ");
						Core.Write(writer, variable.Initializer.Value);
					}

					writer.Write(";\r\n");
				}

			foreach (var init in forStatement.Initializers)
			{
				writer.WriteIndent();
				Core.Write(writer, init);
				writer.Write(";\r\n");
			}

			writer.WriteIndent();
			writer.Write("while (");

			if (forStatement.Condition == null)
				writer.Write("true");
			else
				Core.Write(writer, forStatement.Condition);

			writer.Write(")\r\n");
			writer.WriteOpenBrace();

			info.WriteLoopOpening(writer);

			if (forStatement.Statement is BlockSyntax)
			{
				foreach (var statement in forStatement.Statement.As<BlockSyntax>().Statements)
					Core.Write(writer, statement);
			}
			else
				Core.Write(writer, forStatement.Statement);

			info.WriteLoopClosing(writer);

			foreach (var iterator in forStatement.Incrementors)
			{
				writer.WriteIndent();
				Core.Write(writer, iterator);
				writer.Write(";\r\n");
			}

			writer.WriteCloseBrace();
			info.WritePostLoop(writer);
			writer.Indent--;
			writer.WriteLine("} //end for");
		}
	}
}
