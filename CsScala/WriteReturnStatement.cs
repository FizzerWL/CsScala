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
    static class WriteReturnStatement
    {
        public static void Go(ScalaWriter writer, ReturnStatementSyntax statement)
        {

            
            if (TypeState.Instance.InLambdaBreakable > 0)
            {
                if (statement.Expression != null)
                {
                    writer.WriteIndent();
                    writer.Write("__lambdareturn = ");
                    Core.Write(writer, statement.Expression);
                    writer.Write(";\r\n");
                }

                writer.WriteLine("__lambdabreak.break();");
            }
            else if (TypeState.Instance.InFunctionBreakable)
            {
                if (statement.Expression != null)
                {
                    writer.WriteIndent();
                    writer.Write("__fnreturn = ");
                    Core.Write(writer, statement.Expression);
                    writer.Write(";\r\n");
                }

                //TODO: If we're the last line of the function, we could skip calling __fnbreak.break, as we'd just fall out of the breakable naturally.  May be better perf but I'm not sure if scala optimizes this away
                writer.WriteLine("__fnbreak.break();");
            }
            else
            {
                writer.WriteIndent();
                writer.Write("return");

                if (statement.Expression != null)
                {
                    writer.Write(" ");
                    Core.Write(writer, statement.Expression);
                }
                writer.Write(";\r\n");
            }



        }
    }
}
