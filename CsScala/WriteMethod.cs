using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class WriteMethod
    {
        public static void Go(ScalaWriter writer, MethodDeclarationSyntax method)
        {
            if (method.Modifiers.Any(SyntaxKind.PartialKeyword) && method.Body == null)
            {
                //We only want to render out one of the two partial methods.  If there's another, skip this one.
                if (TypeState.Instance.Partials.SelectMany(o => o.Syntax.As<ClassDeclarationSyntax>().Members)
                    .OfType<MethodDeclarationSyntax>()
                    .Except(method)
                    .Where(o => o.Identifier.ValueText == method.Identifier.ValueText)
                    .Any())
                    return;
            }

            var methodSymbol = Program.GetModel(method).GetDeclaredSymbol(method);


            if (method.Identifier.ValueText == "GetEnumerator")
            {
                WriteGetEnumeratorFunction(writer, method, methodSymbol);
                return;
            }


            writer.WriteIndent();

            if (ShouldUseOverrideKeyword(method, methodSymbol))
                writer.Write("override ");
            if (method.Modifiers.Any(SyntaxKind.PrivateKeyword))
                writer.Write("private ");

            writer.Write("def ");
            var methodName = OverloadResolver.MethodName(methodSymbol);

            if (methodName == "ToString")
                methodName = "toString";
            else if (methodName == "Equals")
                methodName = "equals";
            else if (methodName == "GetHashCode")
                methodName = "hashCode";
            else if (methodName == "Main")
                methodName = "main";

            writer.Write(methodName);

            if (method.TypeParameterList != null)
            {
                writer.Write("[");
                writer.Write(string.Join(", ", method.TypeParameterList.Parameters.Select(o => TypeParameter(o, methodSymbol, method))));
                writer.Write("]");
            }

            writer.Write("(");

            var firstParam = true;
            foreach (var parameter in method.ParameterList.Parameters)
            {
                bool isRef = parameter.Modifiers.Any(SyntaxKind.OutKeyword) || parameter.Modifiers.Any(SyntaxKind.RefKeyword);

                if (firstParam)
                    firstParam = false;
                else
                    writer.Write(", ");

                writer.Write(WriteIdentifierName.TransformIdentifier(parameter.Identifier.ValueText));

                if (isRef)
                {
                    writer.Write(":CsRef[");
                    writer.Write(TypeProcessor.ConvertType(parameter.Type));
                    writer.Write("]");

                    Program.RefOutSymbols.TryAdd(Program.GetModel(method).GetDeclaredSymbol(parameter), null);
                }
                else
                    writer.Write(TypeProcessor.ConvertTypeWithColon(parameter.Type));

                if (parameter.Default != null)
                {
                    writer.Write(" = ");
                    Core.Write(writer, parameter.Default.Value);
                }
            }

            writer.Write(")");
            bool returnsVoid = method.ReturnType.ToString() == "void";
            if (!returnsVoid)
                writer.Write(TypeProcessor.ConvertTypeWithColon(method.ReturnType));

            if (method.Modifiers.Any(SyntaxKind.AbstractKeyword) || method.Parent is InterfaceDeclarationSyntax)
                writer.Write(";\r\n");
            else
            {
                if (!returnsVoid)
                    writer.Write(" =");

                writer.Write("\r\n");
                writer.WriteOpenBrace();

                if (method.Body != null)
                {
                    foreach (var statement in method.Body.Statements)
                        Core.Write(writer, statement);

                    TriviaProcessor.ProcessTrivias(writer, method.Body.DescendantTrivia());
                }

                writer.WriteCloseBrace();
            }
        }

        private static bool ShouldUseOverrideKeyword(MethodDeclarationSyntax method, IMethodSymbol symbol)
        {
            if (method.Modifiers.Any(SyntaxKind.StaticKeyword))
                return false;
            if (method.Modifiers.Any(SyntaxKind.NewKeyword))
                return true;

            if (method.Modifiers.Any(SyntaxKind.PartialKeyword)) //partial methods seem exempt from C#'s normal override keyword requirement, so we have to check manually to see if it exists in a base class
                return symbol.ContainingType.BaseType.GetMembers(symbol.Name).Any();

            return method.Modifiers.Any(SyntaxKind.OverrideKeyword);
        }

        public static string TypeParameter(TypeParameterSyntax prm, IMethodSymbol methodSymbol, MethodDeclarationSyntax methodSyntax)
        {
            var identifier = Utility.TypeConstraints(prm, methodSyntax.ConstraintClauses);

            if (ClassTags.NeedsClassTag(methodSymbol, methodSyntax, prm.Identifier.ValueText))
                identifier += ":ClassTag";

            return identifier;
        }


        private static void WriteGetEnumeratorFunction(ScalaWriter writer, MethodDeclarationSyntax method, IMethodSymbol methodSymbol)
        {
            var returnType = TypeProcessor.ConvertType(methodSymbol.ReturnType);

            if (!returnType.StartsWith("System.Collections.Generic.IEnumerator["))
                return; //we only support the generic IEnumerator form of GetEnumerator.  Anything else, just don't write out the method.

            var enumerableType = returnType.RemoveFromStartOfString("System.Collections.Generic.IEnumerator[").RemoveFromEndOfString("]");

            //We only support very simple GetEnumerator functions that pass on their call to some other collection.  The body should be like "return <expr>.GetEnumerator();", otherwise don't write out the function at all.
            if (method.Body == null)
                return;
            if (method.Body.Statements.Count > 1)
                return;
            var returnStatement = method.Body.Statements.Single() as ReturnStatementSyntax;
            if (returnStatement == null)
                return;
            var invocation = returnStatement.Expression as InvocationExpressionSyntax;
            if (invocation == null)
                return;
            var member = invocation.Expression as MemberAccessExpressionSyntax;
            if (member == null)
                return;

            writer.WriteIndent();
            writer.Write("def foreach[U](fn: ");
            writer.Write(enumerableType);
            writer.Write(" => U)\r\n");
            writer.WriteOpenBrace();

            writer.WriteIndent();
            Core.Write(writer, member.Expression);
            writer.Write(".foreach(fn);\r\n");
            writer.WriteCloseBrace();
        }

    }
} 
