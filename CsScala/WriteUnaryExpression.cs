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
    static class WriteUnaryExpression
    {
        public static void WritePrefix(ScalaWriter writer, PrefixUnaryExpressionSyntax expression)
        {
            if (expression.OperatorToken.Kind() == SyntaxKind.MinusMinusToken)
            {
                Core.Write(writer, expression.Operand);
                writer.Write(" -= 1");
            }
            else if (expression.OperatorToken.Kind() == SyntaxKind.PlusPlusToken)
            {
                Core.Write(writer, expression.Operand);
                writer.Write(" += 1");
            }
            else
            {
                writer.Write(expression.OperatorToken.ToString());
                Core.Write(writer, expression.Operand);
            }
        }

        public static void WritePostfix(ScalaWriter writer, PostfixUnaryExpressionSyntax expression)
        {
            if (expression.OperatorToken.Kind() == SyntaxKind.MinusMinusToken)
            {
                Core.Write(writer, expression.Operand);
                writer.Write(" -= 1");
            }
            else if (expression.OperatorToken.Kind() == SyntaxKind.PlusPlusToken)
            {
                Core.Write(writer, expression.Operand);
                writer.Write(" += 1");
            }
            else
                throw new Exception("No support for " + expression.OperatorToken.Kind() + " at " + Utility.Descriptor(expression));
        }
    }
}
