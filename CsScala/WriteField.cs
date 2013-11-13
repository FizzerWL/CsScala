using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;

namespace CsScala
{

    internal static class WriteField
    {
        public static void Go(ScalaWriter writer, FieldDeclarationSyntax field)
        {
            foreach (var declaration in field.Declaration.Variables)
                Go(writer, field.Modifiers, WriteIdentifierName.TransformIdentifier(declaration.Identifier.ValueText), field.Declaration.Type, declaration.Initializer);
        }

        public static void WriteFieldModifiers(ScalaWriter writer, SyntaxTokenList modifiers)
        {
            if (modifiers.Any(SyntaxKind.PrivateKeyword))
                writer.Write("private ");
        }

        public static void Go(ScalaWriter writer, SyntaxTokenList modifiers, string name, TypeSyntax type, EqualsValueClauseSyntax initializerOpt = null)
        {
            writer.WriteIndent();

            var isConst = IsConst(modifiers, initializerOpt, type);

            WriteFieldModifiers(writer, modifiers);
            if (isConst)
                writer.Write("final val ");
            else
                writer.Write("var ");

            writer.Write(name);
            writer.Write(TypeProcessor.ConvertTypeWithColon(type));
            writer.Write(" = ");

            if (initializerOpt != null)
                Core.Write(writer, initializerOpt.Value);
            else
                writer.Write(TypeProcessor.DefaultValue(type));

            writer.Write(";");
            writer.WriteLine();
        }

        public static bool IsConst(SyntaxTokenList modifiers, EqualsValueClauseSyntax initializerOpt, TypeSyntax type)
        {
            var t = TypeProcessor.ConvertType(type);

            return modifiers.Any(SyntaxKind.ConstKeyword);
        }
    }
}
