using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class WriteGenericName
    {
        public static void Go(ScalaWriter writer, GenericNameSyntax name)
        {
            writer.Write(WriteIdentifierName.TransformIdentifier(name.Identifier.ValueText));
            writer.Write("[");

            bool first = true;
            foreach (var gen in name.TypeArgumentList.Arguments)
            {
                if (first)
                    first = false;
                else
                    writer.Write(", ");

                writer.Write(TypeProcessor.ConvertType(gen));
            }
            writer.Write("]");
        }
    }
}
