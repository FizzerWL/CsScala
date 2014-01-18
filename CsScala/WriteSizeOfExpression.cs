using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteSizeOfExpression
    {
        public static void Go(ScalaWriter writer, SizeOfExpressionSyntax expression)
        {
            var type = Program.GetModel(expression).GetTypeInfo(expression.Type);

            writer.Write(SizeOf(type.Type).ToString());
        }

        private static int SizeOf(TypeSymbol type)
        {
            switch (type.SpecialType)
            {
                case SpecialType.System_Byte:
                case SpecialType.System_SByte:
                    return 1;
                case SpecialType.System_Int16:
                case SpecialType.System_UInt16:
                    return 2;
                case SpecialType.System_Int32:
                case SpecialType.System_UInt32:
                    return 4;
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                    return 8;
                case SpecialType.System_Single:
                    return 4;
                case SpecialType.System_Double:
                    return 8;
                case SpecialType.System_Char:
                    return 2;
                case SpecialType.System_Boolean:
                    return 1;
                default:
                    throw new Exception("Need handler for sizeof " + type);
            }
        }
    }
}
