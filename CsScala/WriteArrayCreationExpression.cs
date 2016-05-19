﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class WriteArrayCreationExpression
    {
        public static void Go(ScalaWriter writer, ImplicitArrayCreationExpressionSyntax array)
        {
            writer.Write("Array");

            //var t = Program.GetModel(array).GetTypeInfo(array).Type;

            //if (t is IArrayTypeSymbol)
            //{
            //    writer.Write("[");
            //    writer.Write(TypeProcessor.ConvertType(t.As<IArrayTypeSymbol>().ElementType));
            //    writer.Write("]");
            //}


            writer.Write("(");

            bool first = true;
            foreach (var expression in array.Initializer.Expressions)
            {
                if (first)
                    first = false;
                else
                    writer.Write(", ");

                Core.Write(writer, expression);
            }

            writer.Write(")");
        }

        public static void Go(ScalaWriter writer, ArrayCreationExpressionSyntax array)
        {
            if (array.Type.RankSpecifiers.Count > 1 || array.Type.RankSpecifiers.Single().Sizes.Count > 1)
                throw new Exception("Multi-dimensional arrays are not supported " + Utility.Descriptor(array));

            var rankExpression = array.Type.RankSpecifiers.Single().Sizes.Single();
            if (rankExpression is OmittedArraySizeExpressionSyntax)
            {
                writer.Write("Array[");
                writer.Write(TypeProcessor.ConvertType(array.Type.ElementType));
                writer.Write("](");

                bool first = true;
                foreach (var expression in array.Initializer.Expressions)
                {
                    if (first)
                        first = false;
                    else
                        writer.Write(", ");

                    Core.Write(writer, expression);
                }

                writer.Write(")");
            }
            else
            {
                if (array.Initializer != null)
                    throw new Exception("Initalizers along with array sizes are not supported - please use a size or an initializer " + Utility.Descriptor(array));

                writer.Write("new Array[");
                writer.Write(TypeProcessor.ConvertType(array.Type.ElementType));
                writer.Write("](");
                Core.Write(writer, array.Type.RankSpecifiers.Single().Sizes.Single());
                writer.Write(")");
            }

        }

    }
}
