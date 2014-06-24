using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScala.Translations;
using Roslyn.Compilers;
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
                        foreach (var arg in subExpr.ArgumentList.Arguments)
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
                        var model = Program.GetModel(expression);
                        var type = model.GetTypeInfo(e);

                        //Check for enums being converted to strings by string concatenation
                        if (expression.OperatorToken.Kind == SyntaxKind.PlusToken && type.Type.TypeKind == TypeKind.Enum)
                        {
                            writer.Write(type.Type.ContainingNamespace.FullNameWithDot());
                            writer.Write(WriteType.TypeName(type.Type.As<NamedTypeSymbol>()));
                            writer.Write(".ToString(");
                            Core.Write(writer, e);
                            writer.Write(")");
                        }
                        else if (expression.OperatorToken.Kind == SyntaxKind.PlusToken && (type.Type.Name == "Nullable" && type.Type.ContainingNamespace.FullName() == "System" && type.Type.As<NamedTypeSymbol>().TypeArguments.Single().TypeKind == TypeKind.Enum))
                        {
                            var enumType = type.Type.As<NamedTypeSymbol>().TypeArguments.Single();
                            writer.Write(enumType.ContainingNamespace.FullNameWithDot());
                            writer.Write(WriteType.TypeName(enumType.As<NamedTypeSymbol>()));
                            writer.Write(".ToString(");
                            Core.Write(writer, e);
                            writer.Write(")");
                        }
                        else if (expression.OperatorToken.Kind == SyntaxKind.PlusToken && IsException(type.Type)) //Check for exceptions being converted to strings by string concatenation
                        {
                            writer.Write("System.CsScala.ExceptionToString(");
                            Core.Write(writer, e);
                            writer.Write(")");
                        }
                        else if (expression.OperatorToken.Kind == SyntaxKind.PlusToken && type.Type.SpecialType == SpecialType.System_Byte && !Utility.IsNumeric(type.ConvertedType)) 
                        {
                            //bytes are signed in the JVM, so we need to take care when converting them to strings.  Exclude numeric types, since Core.Writer will convert these to ints
                            writer.Write("System.CsScala.ByteToString(");
                            Core.Write(writer, e);
                            writer.Write(")");
                        }
                        else if (expression.OperatorToken.Kind == SyntaxKind.PlusToken && !(e is BinaryExpressionSyntax) && type.Type.SpecialType == Roslyn.Compilers.SpecialType.System_String && CouldBeNullString(model, e))
                        {
                            //In .net, concatenating a null string does not alter the output. However, in the JVM, it produces the "null" string. To counter this, we must check non-const strings.
                            writer.Write("System.CsScala.NullCheck(");
                            Core.Write(writer, e);
                            writer.Write(")");
                        }
                        else if (expression.OperatorToken.Kind == SyntaxKind.PlusToken && !(e is BinaryExpressionSyntax) && type.Type.As<NamedTypeSymbol>().ConstructedFrom.SpecialType == Roslyn.Compilers.SpecialType.System_Nullable_T)
                        {
                            //Concatening a nullable type in .net just produces an empty string if it's null.  In scala it produces "null" or a null reference exception -- we want neither.
                            writer.Write("System.CsScala.NullCheck(");
                            Core.Write(writer, e);
                            writer.Write(")");
                        }
                        else if (expression.OperatorToken.Kind == SyntaxKind.PlusToken && !(e is BinaryExpressionSyntax) && type.Type.SpecialType == SpecialType.System_Boolean)
                        {
                            writer.Write("System.CsScala.BooleanToString(");
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

        private static bool CouldBeNullString(SemanticModel model, ExpressionSyntax e)
        {
            if (model.GetConstantValue(e).HasValue)
                return false; //constants are never null

            //For in-line conditions, just recurse on both results.
            var cond = e as ConditionalExpressionSyntax;
            if (cond != null)
                return CouldBeNullString(model, cond.WhenTrue) || CouldBeNullString(model, cond.WhenFalse);

            var paren = e as ParenthesizedExpressionSyntax;
            if (paren != null)
                return CouldBeNullString(model, paren.Expression);

            var invoke = e as InvocationExpressionSyntax;
            if (invoke != null)
            {
                var methodSymbol = model.GetSymbolInfo(invoke).Symbol;
                //Hard-code some well-known functions as an optimization
                if (methodSymbol.Name == "HtmlEncode" && methodSymbol.ContainingNamespace.FullName() == "System.Web")
                    return false;
                if (methodSymbol.Name == "ToString")
                    return false;
            }

            return true;
        }


        private static bool IsException(TypeSymbol typeSymbol)
        {
            if (typeSymbol.Name == "Exception" && typeSymbol.ContainingNamespace.FullName() == "System")
                return true;

            if (typeSymbol.BaseType == null)
                return false;
            return IsException(typeSymbol.BaseType);
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
