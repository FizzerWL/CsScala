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
    static class WriteTypeOfExpression
    {
        public static void Go(ScalaWriter writer, TypeOfExpressionSyntax expression)
        {
            writer.Write("new System.Type(classOf[");
            writer.Write(TypeProcessor.ConvertType(expression.Type));
            writer.Write("])");
        }
    }
}
