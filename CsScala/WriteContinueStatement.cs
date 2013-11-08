using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
	static class WriteContinueStatement
	{
		public static void Go(ScalaWriter writer, ContinueStatementSyntax statement)
		{
			writer.WriteLine("CsScala.cscontinue.break;");
		}
	}
}
