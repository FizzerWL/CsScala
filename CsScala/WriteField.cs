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
            {
                if (field.AttributeLists.Any(l => l.Attributes.Any(a => a.Name.As<IdentifierNameSyntax>().Identifier.ValueText == "ThreadStatic")))
                    WriteThreadStatic(writer, declaration, field);
                else
                    Go(writer, field.Modifiers, WriteIdentifierName.TransformIdentifier(declaration.Identifier.ValueText), field.Declaration.Type, declaration.Initializer);
            }
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

        private static void WriteThreadStatic(ScalaWriter writer, VariableDeclaratorSyntax declaration, FieldDeclarationSyntax field)
        {
            /*val __Init = new ThreadLocal[String]()
    { override def initialValue():String = ""initval""; };
    def Init:String = __Init.get();
    def Init_=(value:String) = __Init.set(value);*/

            var type = TypeProcessor.ConvertType(field.Declaration.Type);

            writer.WriteIndent();
            writer.Write("final val __");
            writer.Write(declaration.Identifier.ValueText);
            writer.Write(" = new ThreadLocal[");
            writer.Write(type);
            writer.Write("]()");

            if (declaration.Initializer != null)
            {
                writer.Write("\r\n");
                writer.WriteIndent();
                writer.Write("{ override def initialValue():");
                writer.Write(type);
                writer.Write(" = ");
                Core.Write(writer, declaration.Initializer.Value);
                writer.Write("; }");
            }

            writer.Write(";\r\n");

            writer.WriteIndent();
            writer.Write("def ");
            writer.Write(declaration.Identifier.ValueText);
            writer.Write(":");
            writer.Write(type);
            writer.Write(" = __");
            writer.Write(declaration.Identifier.ValueText);
            writer.Write(".get();\r\n");

            writer.WriteIndent();
            writer.Write("def ");
            writer.Write(declaration.Identifier.ValueText);
            writer.Write("_=(value:");
            writer.Write(type);
            writer.Write(") = __");
            writer.Write(declaration.Identifier.ValueText);
            writer.Write(".set(value);\r\n");
            
        }
    }
}
