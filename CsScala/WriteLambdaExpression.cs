using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteLambdaExpression
    {
        public static void Go(ScalaWriter writer, ParenthesizedLambdaExpressionSyntax expression)
        {
            Go(writer, expression.ParameterList.Parameters, expression.Body, Program.GetModel(expression).GetTypeInfo(expression));
        }

        public static void Go(ScalaWriter writer, SimpleLambdaExpressionSyntax expression)
        {
            Go(writer, new[] { expression.Parameter }, expression.Body, Program.GetModel(expression).GetTypeInfo(expression));
        }

        private static void Go(ScalaWriter writer, IEnumerable<ParameterSyntax> parameters, SyntaxNode body, TypeInfo type)
        {
            var methodSymbol = type.ConvertedType.As<NamedTypeSymbol>().DelegateInvokeMethod.As<MethodSymbol>();

            writer.Write("(");

            for (int pi = 0; pi < parameters.Count(); pi++)
            {
                var parameter = parameters.ElementAt(pi);
                if (pi > 0)
                    writer.Write(", ");

                writer.Write(WriteIdentifierName.TransformIdentifier(parameter.Identifier.ValueText));
                if (parameter.Type != null)
                    writer.Write(TypeProcessor.ConvertTypeWithColon(parameter.Type));
                else
                    writer.Write(TypeProcessor.ConvertTypeWithColon(methodSymbol.Parameters[pi].Type));
            }

            writer.Write(") => ");

            bool returnsVoid = methodSymbol.ReturnType.ToString() == "void";

            if (body is BlockSyntax)
            {
                writer.Write("\r\n");
                writer.WriteOpenBrace();

                var statements = body.As<BlockSyntax>().Statements;

                var lastStatement = statements.LastOrDefault() as ReturnStatementSyntax;

                var returnStatements = FindReturnStatements(body);

                if (returnStatements.Count > 0 && (lastStatement == null || returnStatements.Except(lastStatement).Any()))
                {
                    //Lambda has branching returns.  We must use a breakable block since scala can't return from a lambda like C# can
                    TypeState.Instance.InLambdaBreakable++;

                    writer.WriteLine("val __lambdabreak = new Breaks;");

                    if (!returnsVoid)
                    {
                        writer.WriteIndent();
                        writer.Write("var __lambdareturn:");
                        writer.Write(TypeProcessor.ConvertType(methodSymbol.ReturnType));
                        writer.Write(" = ");
                        writer.Write(TypeProcessor.DefaultValue(methodSymbol.ReturnType));
                        writer.Write(";\r\n");
                    }

                    writer.WriteLine("__lambdabreak.breakable");
                    writer.WriteOpenBrace();

                    foreach (var statement in statements)
                    {
                        if (statement == lastStatement && !returnsVoid)
                        {
                            //Manually write it so we avoid the final break that WriteReturnStatement does
                            writer.WriteIndent();
                            writer.Write("__lambdareturn = ");
                            Core.Write(writer, lastStatement.Expression);
                            writer.Write(";\r\n");
                        }
                        else
                            Core.Write(writer, statement);
                    }

                    writer.WriteCloseBrace();

                    if (!returnsVoid)
                    {
                        writer.WriteLine("__lambdareturn;");
                    }

                    TypeState.Instance.InLambdaBreakable--;
                }
                else
                {

                    foreach (var statement in statements)
                    {
                        if (statement == lastStatement)
                        {
                            writer.WriteIndent();
                            Core.Write(writer, lastStatement.Expression);
                            writer.Write(";\r\n");
                        }
                        else
                            Core.Write(writer, statement);
                    }
                }

                writer.Indent--;
                writer.WriteIndent();
                writer.Write("}");
            }
            else
            {
                writer.Write(" { ");
                Core.Write(writer, body);
                writer.Write("; }");
            }

            if (!returnsVoid)
                writer.Write(TypeProcessor.ConvertTypeWithColon(methodSymbol.ReturnType));

        }

        private static List<ReturnStatementSyntax> FindReturnStatements(SyntaxNode body)
        {
            var ret = new List<ReturnStatementSyntax>();
            FindReturnStatements(body, ret);
            return ret;
        }

        private static void FindReturnStatements(SyntaxNode node, List<ReturnStatementSyntax> ret)
        {
            if (node is ParenthesizedLambdaExpressionSyntax || node is SimpleLambdaExpressionSyntax)
                return; //any returns in a sub-lambda will be for that lambda. Ignore them.

            if (node is ReturnStatementSyntax)
                ret.Add(node.As<ReturnStatementSyntax>());

            foreach (var child in node.ChildNodes())
                FindReturnStatements(child, ret);

        }
    }
}
