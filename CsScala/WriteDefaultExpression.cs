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
    static class WriteDefaultExpression
    {
        public static void Go(ScalaWriter writer, DefaultExpressionSyntax node)
        {
            writer.Write("null");
        }
    }
}
