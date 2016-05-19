﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class WriteCastExpression
    {
        public static void Go(ScalaWriter writer, CastExpressionSyntax expression)
        {
            var model = Program.GetModel(expression);

            var symbol = model.GetSymbolInfo(expression);

            var castingFrom = model.GetTypeInfo(expression.Expression).Type;
            if (castingFrom == null)
                castingFrom = model.GetTypeInfo(expression).Type;

            var srcTypeScala = TypeProcessor.ConvertType((ITypeSymbol)castingFrom);
            var destType = model.GetTypeInfo(expression.Type).Type;
            var destTypeScala = TypeProcessor.TryConvertType(expression.Type);

            if (destTypeScala == srcTypeScala)
            {
                //Eat casts where the types are identical.  Enums getting casted to int fall here, and since we use ints to represent enums anyway, it's not necessary.  
                Core.Write(writer, expression.Expression);
            }
            else if (symbol.Symbol != null && srcTypeScala != "Int" && srcTypeScala != "String" && srcTypeScala != "Bool")
            {
                //when the symbol is non-null, this indicates we're calling a cast operator function
                writer.Write(TypeProcessor.ConvertType(symbol.Symbol.ContainingType));
                writer.Write(".op_Explicit_");
                writer.Write(destTypeScala.TrySubstringBeforeFirst('[').Replace('.', '_'));
                writer.Write("(");
                Core.Write(writer, expression.Expression);
                writer.Write(")");
            }
            else if (TypeProcessor.IsPrimitiveType(srcTypeScala) && TypeProcessor.IsPrimitiveType(destTypeScala))
            {
                if (srcTypeScala == "Byte")
                {
                    //JVM's bytes are signed, so we must take care when upcasting
                    writer.Write("System.CsScala.ByteTo");
                    writer.Write(destTypeScala);
                    writer.Write("(");
                    Core.Write(writer, expression.Expression);
                    writer.Write(")");
                }
                else
                {
                    //Casting between primitives is handled in scala bo the .toXX functions
                    Core.Write(writer, expression.Expression);
                    writer.Write(".to");
                    writer.Write(destTypeScala);
                }
            }
            else
            {
                Core.Write(writer, expression.Expression);
                writer.Write(".asInstanceOf[");
                writer.Write(destTypeScala);
                writer.Write("]");
            }
        }

    }
}
