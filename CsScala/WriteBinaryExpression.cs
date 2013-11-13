using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScala.Translations;
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

                if (expression.Left is ElementAccessExpressionSyntax && IsAssignmentToken(expression.OperatorToken.Kind))
                {
                    var subExpr = expression.Left.As<ElementAccessExpressionSyntax>();
                    var typeStr = TypeProcessor.GenericTypeName(Program.GetModel(expression).GetTypeInfo(subExpr.Expression).Type);
                    var trans = ElementAccessTranslation.Get(typeStr);

                    if (trans != null)
                    {
                        Core.Write(writer, subExpr.Expression);
                        writer.Write(".");

                        if (expression.OperatorToken.Kind == SyntaxKind.EqualsToken)
                            writer.Write(trans.ReplaceAssign);
                        else
                            throw new Exception(expression.OperatorToken.Kind + " is not supported on " + typeStr + " " + Utility.Descriptor(expression));

                        writer.Write("(");
                        foreach(var arg in subExpr.ArgumentList.Arguments)
                        {
                            Core.Write(writer, arg.Expression);
                            writer.Write(", ");
                        }

                        Core.Write(writer, expression.Right);
                        writer.Write(")");

                        return;
                    }
                }

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

        private static bool IsAssignmentToken(SyntaxKind syntaxKind)
        {
            switch (syntaxKind)
            {
                case SyntaxKind.EqualsToken:
                case SyntaxKind.PlusEqualsToken:
                case SyntaxKind.MinusEqualsToken:
                case SyntaxKind.SlashEqualsToken:
                case SyntaxKind.AsteriskEqualsToken:
                    return true;
                default:
                    return false;
            }
        }



    }
}
