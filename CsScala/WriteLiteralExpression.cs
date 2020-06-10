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
    static class WriteLiteralExpression
    {
        public static void Go(ScalaWriter writer, LiteralExpressionSyntax expression, bool isConst)
        {
            var str = expression.ToString();

            if (str.StartsWith("@"))
                str = "\"" + str.RemoveFromStartOfString("@\"").RemoveFromEndOfString("\"").Replace("\\", "\\\\").Replace("\"\"", "\\\"").Replace("\r\n", "\\n") + "\"";


            if (str.Length > 65000)
            {
                //Big strings have to be broken up, scala can't render big string constants, even when concatenated.  So we have to pass them to a function to concat at runtime
                writer.Write("System.CsScala.JoinConstants(");
                var raw = str.RemoveFromStartOfString("\"").RemoveFromEndOfString("\"");

                var subLength = 65000;
                for (int i = 0; i < raw.Length; i += subLength)
                {
                    var sub = raw.SubstringSafe(i, subLength);
                    //Make sure we never break in the middle of a backslash sequence.  TODO: This assumes backslash sequences are only ever two characters long, we could break on longer ones.
                    if (sub[sub.Length - 1] == '\\' && sub[sub.Length - 2] != '\\')
                    {
                        sub += raw[i + subLength];
                        i++;
                    }

                    writer.Write("\"");
                    writer.Write(sub);
                    writer.Write("\"");
                    if (i + subLength < raw.Length)
                        writer.Write(", ");
                }
                writer.Write(")");
            }
            else
                writer.Write(str);

            var typeInfo = Program.GetModel(expression).GetTypeInfo(expression);
            if (typeInfo.Type != null && typeInfo.ConvertedType != null)
            {
                if (isConst == false && typeInfo.ConvertedType.SpecialType == SpecialType.System_Byte && typeInfo.Type.SpecialType == SpecialType.System_Int32)
                    writer.Write(".toByte");
            }
        }

    }
}
