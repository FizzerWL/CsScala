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

    public class TransformedArgument
    {
        //Either String will be populated, or Argument will be. Never both.
        public readonly string StringOpt;
        public readonly ArgumentSyntax ArgumentOpt;


        public TransformedArgument(ArgumentSyntax argument)
        {
            this.ArgumentOpt = argument;
        }

        public TransformedArgument(string str)
        {
            this.StringOpt = str;
        }

        public void Write(ScalaWriter writer)
        {
            if (this.StringOpt != null)
                writer.Write(this.StringOpt);
            else
            {
                if (ArgumentOpt.NameColon != null)
                {
                    Core.Write(writer, ArgumentOpt.NameColon.Name);
                    writer.Write(" = ");
                }

                Core.Write(writer, this.ArgumentOpt.Expression);

                WriteEnum.Check(this.ArgumentOpt.Expression);
            }
        }
    }
}
