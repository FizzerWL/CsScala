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


            bool hasGetter, hasSetter;
            SyntaxNode getterBody, setterBody;
            bool isAutoProperty;

            if (property.AccessorList != null)
            {
                var g = property.AccessorList.Accessors.SingleOrDefault(o => o.Keyword.IsKind(SyntaxKind.GetKeyword));
                hasGetter = g != null;
                getterBody = hasGetter ? g.Body : null;

                var s = property.AccessorList.Accessors.SingleOrDefault(o => o.Keyword.IsKind(SyntaxKind.SetKeyword));
                hasSetter = s != null;
                setterBody = hasSetter ? s.Body : null;

                isAutoProperty = hasGetter && hasSetter && getterBody == null && setterBody == null && !property.Modifiers.Any(SyntaxKind.AbstractKeyword);
            }
            else
            {
                //If AccessorList is null, assume it's an expression bodied member
                hasGetter = true;
                getterBody = property.ExpressionBody.Expression;
                hasSetter = false;
                setterBody = null;
                isAutoProperty = false;
            }


            if (isAutoProperty)
            {
                //For our purposes, this is the equivilant of a field, just write that.
                WriteField.Go(writer, property.Modifiers, WriteIdentifierName.TransformIdentifier(property.Identifier.ValueText), property.Type);
                return;
            }


            Action<SyntaxNode, bool> writeRegion = (body, get) =>
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

                    if (property.Modifiers.Any(SyntaxKind.AbstractKeyword) || body == null)
                        writer.Write(";\r\n");
                    else
                    {
                        writer.Write(" =");

                        if (body is BlockSyntax)
                        {
                            writer.Write("\r\n");
                            Core.WriteBlock(writer, body.As<BlockSyntax>());
                        }
                        else
                        {
                            writer.Write(" ");
                            Core.Write(writer, body);
                            writer.Write(";\r\n");
                        }
                    }

                }
                else
                {
                    writer.Write("_=(value");
                    writer.Write(TypeProcessor.ConvertTypeWithColon(property.Type));
                    writer.Write(")");

                    if (property.Modifiers.Any(SyntaxKind.AbstractKeyword) || body == null)
                        writer.Write(":Unit;\r\n");
                    else
                    {
                        writer.Write(" =\r\n");
                        Core.Write(writer, body);
                    }

                }



            };

            if (hasGetter)
                writeRegion(getterBody, true);
            else if (hasSetter)
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

            if (hasSetter)
                writeRegion(setterBody, false);
        }
    }
}
