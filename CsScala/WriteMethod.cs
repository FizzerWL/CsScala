using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;

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

            if (method.Identifier.ValueText == "GetEnumerator")
                return; //TODO: Support enumerator methods

            var methodSymbol = Program.GetModel(method).GetDeclaredSymbol(method);

            writer.WriteIndent();

            if (ShouldUseOverrideKeyword(method, methodSymbol))
                writer.Write("override ");
            if (method.Modifiers.Any(SyntaxKind.PrivateKeyword))
                writer.Write("private ");

            writer.Write("def ");
            var methodName = methodSymbol.Name;

            if (methodName == "ToString")
                methodName = "toString";

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

        private static bool ShouldUseOverrideKeyword(MethodDeclarationSyntax method, MethodSymbol symbol)
        {
            if (method.Modifiers.Any(SyntaxKind.StaticKeyword))
                return false;
            if (method.Modifiers.Any(SyntaxKind.NewKeyword))
                return true;

            if (method.Modifiers.Any(SyntaxKind.PartialKeyword)) //partial methods seem exempt from C#'s normal override keyword requirement, so we have to check manually to see if it exists in a base class
                return symbol.ContainingType.BaseType.GetMembers(symbol.Name).Any();

            return method.Modifiers.Any(SyntaxKind.OverrideKeyword);
        }

        public static string TypeParameter(TypeParameterSyntax prm, MethodSymbol methodSymbol, MethodDeclarationSyntax methodSyntax)
        {
            var identifier = Utility.TypeConstraints(prm, methodSyntax.ConstraintClauses);

            if (ClassTags.NeedsClassTag(methodSymbol, methodSyntax, prm.Identifier.ValueText))
                identifier += ":ClassTag";

            return identifier;
        }
    }
} 
