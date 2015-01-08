using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteIdentifierName
    {
        public static void Go(ScalaWriter writer, IdentifierNameSyntax identifier, bool byRef = false)
        {
            var symbol = Program.GetModel(identifier).GetSymbolInfo(identifier).Symbol;

            if (symbol.IsStatic)
            {
                writer.Write(symbol.ContainingNamespace.FullNameWithDot());
                writer.Write(symbol.ContainingType.Name);
                writer.Write(".");
            }

            writer.Write(TransformIdentifier(identifier.Identifier.ToString()));

            if (!byRef)
            {

                if (Program.RefOutSymbols.ContainsKey(symbol))
                    writer.Write(".Value");
            }

        }

        public static string TransformIdentifier(string ident)
        {
            if (ident.StartsWith("@"))
                return TransformIdentifier(ident.Substring(1));

            if (ident.EndsWith("_"))
                return ident + "cs";

            switch (ident)
            {
                case "val":
                case "final":
                case "type":
                case "def":
                case "match":
                    return "cs" + ident;
                default:
                    return ident;
            }
        }
    }
}
