using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteLiteralExpression
    {
        public static void Go(ScalaWriter writer, LiteralExpressionSyntax expression, bool isConst)
        {
            var str = expression.ToString();

            if (str.StartsWith("@"))
                str = "\"" + str.RemoveFromStartOfString("@\"").RemoveFromEndOfString("\"").Replace("\\", "\\\\").Replace("\"\"", "\\\"").Replace("\r\n", "\\n") + "\"";

            var typeInfo = Program.GetModel(expression).GetTypeInfo(expression);


            writer.Write(str);

            if (typeInfo.Type != null && typeInfo.ConvertedType != null)
            {
                if (isConst == false && typeInfo.ConvertedType.SpecialType == Roslyn.Compilers.SpecialType.System_Byte && typeInfo.Type.SpecialType == Roslyn.Compilers.SpecialType.System_Int32)
                    writer.Write(".toByte");
            }
        }

    }
}
