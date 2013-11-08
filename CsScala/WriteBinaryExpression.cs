using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
	static class WriteBinaryExpression
	{
		public static void Go(ScalaWriter writer, BinaryExpressionSyntax expression)
		{



			if (expression.OperatorToken.Kind == SyntaxKind.AsKeyword)
			{
				writer.Write("CsScala.As[");
				writer.Write(TypeProcessor.ConvertType(expression.Right));
				writer.Write("](");
				Core.Write(writer, expression.Left);
				writer.Write(")");
			}
			else if (expression.OperatorToken.Kind == SyntaxKind.IsKeyword)
			{
				Core.Write(writer, expression.Left);
				writer.Write(".isInstanceOf[");
				writer.Write(TypeProcessor.ConvertType(expression.Right));
				writer.Write("]");
			}
			else if (expression.OperatorToken.Kind == SyntaxKind.QuestionQuestionToken)
			{
				writer.Write("CsScala.Coalesce(");
				Core.Write(writer, expression.Left);
				writer.Write(", ");
				Core.Write(writer, expression.Right);
				writer.Write(")");
			}
			else
			{
				Action<ExpressionSyntax> write = e =>
					{
						var type = Program.GetModel(expression).GetTypeInfo(e);
						//Check for enums being converted to strings by string concatenation
						if (expression.OperatorToken.Kind == SyntaxKind.PlusToken && type.Type.TypeKind == TypeKind.Enum)
						{
							writer.Write(type.Type.ContainingNamespace.FullNameWithDot());
							writer.Write(WriteType.TypeName(type.Type.As<NamedTypeSymbol>()));
							writer.Write(".ToString(");
							Core.Write(writer, e);
							writer.Write(")");
						}
						else
							Core.Write(writer, e);
					};

				write(expression.Left);
				writer.Write(" ");
				writer.Write(expression.OperatorToken.ToString());
				writer.Write(" ");
				write(expression.Right);
			}

			
		}


	}
}
