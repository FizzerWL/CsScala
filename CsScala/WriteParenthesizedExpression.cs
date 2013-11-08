using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
	static class WriteParenthesizedExpression
	{
		public static void Go(ScalaWriter writer, ParenthesizedExpressionSyntax expression)
		{
			writer.Write("(");
			Core.Write(writer, expression.Expression);
			writer.Write(")");
		}
	}
}
