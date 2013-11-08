using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
	static class WriteTypeOfExpression
	{
		public static void Go(ScalaWriter writer, TypeOfExpressionSyntax expression)
		{
			throw new Exception("typeof is not supported unless part of Enum.Parse or Enum.GetValues " + Utility.Descriptor(expression));
		}
	}
}
