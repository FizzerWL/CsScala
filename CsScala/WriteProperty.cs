using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    class WriteProperty
    {
        public static void Go(ScalaWriter writer, PropertyDeclarationSyntax property)
        {
            Action<AccessorDeclarationSyntax, bool> writeRegion = (region, get) =>
            {
                writer.WriteIndent();

                if (property.Modifiers.Any(SyntaxKind.OverrideKeyword))
                    writer.Write("override ");
                if (property.Modifiers.Any(SyntaxKind.PrivateKeyword))
                    writer.Write("private ");

                writer.Write("def ");
                writer.Write(WriteIdentifierName.TransformIdentifier(property.Identifier.ValueText));

                if (get)
                {
                    writer.Write(TypeProcessor.ConvertTypeWithColon(property.Type));
                }
                else
                {
                    writer.Write("_=(value");
                    writer.Write(TypeProcessor.ConvertTypeWithColon(property.Type));
                    writer.Write(")");

                }

                if (property.Modifiers.Any(SyntaxKind.AbstractKeyword) || region.Body == null)
                    writer.Write(";\r\n");
                else
                {
                    writer.Write(" =\r\n");
                    Core.WriteBlock(writer, region.Body.As<BlockSyntax>());
                }


            };

            var getter = property.AccessorList.Accessors.SingleOrDefault(o => o.Keyword.Kind() == SyntaxKind.GetKeyword);
            var setter = property.AccessorList.Accessors.SingleOrDefault(o => o.Keyword.Kind() == SyntaxKind.SetKeyword);

            if (getter == null && setter == null)
                throw new Exception("Property must have either a get or a set");

            if (getter != null && setter != null && setter.Body == null && getter.Body == null)
            {
                //Both get and set are null, which means this is an automatic property.  For our purposes, this is the equivilant of a field
                WriteField.Go(writer, property.Modifiers, WriteIdentifierName.TransformIdentifier(property.Identifier.ValueText), property.Type);
            }
            else
            {

                if (getter != null)
                    writeRegion(getter, true);
                else if (setter != null)
                {
                    //Scala does not allow having a setter without a getter. Write out a getter.
                    writer.Write("def ");
                    writer.Write(WriteIdentifierName.TransformIdentifier(property.Identifier.ValueText));
                    writer.Write(TypeProcessor.ConvertTypeWithColon(property.Type));
                    writer.Write(" =\r\n");
                    writer.WriteOpenBrace();
                    writer.WriteLine("throw new Exception(\"No getter defined\");");
                    writer.WriteCloseBrace();
                }

                if (setter != null)
                    writeRegion(setter, false);
            }
        }
    }
}
