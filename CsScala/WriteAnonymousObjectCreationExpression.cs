using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScala.Translations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class WriteAnonymousObjectCreationExpression
    {
        public static void Go(ScalaWriter writer, AnonymousObjectCreationExpressionSyntax expression)
        {
            writer.Write("new ");
            writer.Write(TypeName(expression));
            writer.Write("(");

            bool first = true;
            foreach (var field in expression.Initializers.OrderBy(o => o.Name()))
            {
                if (first)
                    first = false;
                else
                    writer.Write(", ");

                Core.Write(writer, field.Expression);
            }

            writer.Write(")");
        }

        public static string TypeName(AnonymousObjectCreationExpressionSyntax expression)
        {
            return TypeName(Program.GetModel(expression).GetTypeInfo(expression).Type.As<INamedTypeSymbol>());
        }

        public static string TypeName(INamedTypeSymbol symbol)
        {
            var fields = symbol.GetMembers().OfType<IPropertySymbol>();

            var typeParams = fields.Where(o => o.Type.TypeKind == TypeKind.TypeParameter);

            var genericSuffix = typeParams.None() ? "" : ("[" + string.Join(", ", typeParams.Select(o => o.Type.Name).Distinct()) + "]");

            return "Anon_" + string.Join("__",
                fields
                .OrderBy(o => o.Name)
                .Select(o => o.Name + "_" + TypeProcessor.ConvertType(o.Type).Replace('.', '_')))
                + genericSuffix;
        }

        public static void WriteAnonymousType(AnonymousObjectCreationExpressionSyntax syntax)
        {
            var type = Program.GetModel(syntax).GetTypeInfo(syntax).Type.As<INamedTypeSymbol>();
            var anonName = TypeName(type);
            using (var writer = new ScalaWriter("anonymoustypes", StripGeneric(anonName)))
            {
                var fields = type.GetMembers().OfType<IPropertySymbol>().OrderBy(o => o.Name).ToList();

                writer.WriteLine("package anonymoustypes;");
                WriteImports.Go(writer);

                writer.WriteIndent();
                writer.Write("class ");
                writer.Write(anonName);
                writer.Write("(");
                bool first = true;
                foreach (var field in fields)
                {
                    if (first)
                        first = false;
                    else
                        writer.Write(", ");

                    writer.Write("_");
                    writer.Write(WriteIdentifierName.TransformIdentifier(field.Name));
                    writer.Write(TypeProcessor.ConvertTypeWithColon(field.Type));
                }
                writer.Write(")\r\n");


                writer.WriteOpenBrace();



                foreach (var field in fields)
                {
                    writer.WriteIndent();
                    writer.Write("final var ");
                    writer.Write(WriteIdentifierName.TransformIdentifier(field.Name));
                    writer.Write(TypeProcessor.ConvertTypeWithColon(field.Type));
                    writer.Write(" = _");
                    writer.Write(WriteIdentifierName.TransformIdentifier(field.Name));
                    writer.Write(";\r\n");
                }



                writer.WriteCloseBrace();
            }
        }

        private static string StripGeneric(string anonName)
        {
            var i = anonName.IndexOf('[');
            if (i == -1)
                return anonName;
            else
                return anonName.Substring(0, i);
        }
    }
}
