using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteSizeOfExpression
    {
        public static void Go(ScalaWriter writer, SizeOfExpressionSyntax expression)
        {
            var scalaType = TypeProcessor.ConvertType(expression.Type);

            writer.Write(SizeOf(scalaType).ToString());
        }

        private static int SizeOf(string scalaType)
        {
            switch (scalaType)
            {
                case "Byte":
                    return 1;
                case "Int":
                    return 4;
                case "Float":
                    return 4;
                case "Double":
                    return 8;
                case "Short":
                    return 2;
                case "Long":
                    return 8;
                case "Char":
                    return 2;
                case "Boolean":
                    return 1;

                default:
                    throw new Exception("Need handler for sizeof " + scalaType);
            }
        }
    }
}
