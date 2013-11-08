using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
	static class WriteConditionalExpression
	{
		public static void Go(ScalaWriter writer, ConditionalExpressionSyntax expression)
		{
			writer.Write("if (");
			Core.Write(writer, expression.Condition);
			writer.Write(") ");
			Core.Write(writer, expression.WhenTrue);
			writer.Write(" else ");
			Core.Write(writer, expression.WhenFalse);
		}
	}
}
