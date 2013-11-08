using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteEnum
    {
        public static void Go(ScalaWriter writer, IEnumerable<EnumMemberDeclarationSyntax> allChildren)
        {
            writer.Write("object ");
            writer.Write(TypeState.Instance.TypeName);
            writer.Write("\r\n");
            writer.WriteOpenBrace();

            int lastEnumValue = 0;

            var values = allChildren.Select(o => new { Syntax = o, Value = DetermineEnumValue(o, ref lastEnumValue) }).ToList();

            foreach (var value in values)
                writer.WriteLine("val " + WriteIdentifierName.TransformIdentifier(value.Syntax.Identifier.ValueText) + ":Int = " + value.Value + ";");

            writer.WriteLine();

            writer.WriteLine("def ToString(e:Int):String =");
            writer.WriteOpenBrace();
            writer.WriteLine("return e match");
            writer.WriteOpenBrace();

            foreach (var value in values)
                writer.WriteLine("case " + value.Value + " => \"" + value.Syntax.Identifier.ValueText + "\";");

            writer.WriteCloseBrace();
            writer.WriteCloseBrace();

            writer.WriteLine();
            writer.WriteLine("def Parse(s:String):Int =");
            writer.WriteOpenBrace();
            writer.WriteLine("return s match");
            writer.WriteOpenBrace();

            foreach (var value in values)
                writer.WriteLine("case \"" + value.Syntax.Identifier.ValueText + "\" => " + value.Value + ";");

            writer.WriteCloseBrace();
            writer.WriteCloseBrace();

            writer.WriteLine();
            writer.WriteIndent();
            writer.Write("val Values:Array[Int] = Array(");
            writer.Write(string.Join(", ", values.Select(o => o.Value.ToString())));
            writer.Write(");\r\n");


            writer.WriteCloseBrace();
        }

        private static int DetermineEnumValue(EnumMemberDeclarationSyntax syntax, ref int lastEnumValue)
        {
            if (syntax.EqualsValue == null)
                return ++lastEnumValue;


            if (!int.TryParse(syntax.EqualsValue.Value.ToString(), out lastEnumValue))
                throw new Exception("Enums must be assigned with an integer " + Utility.Descriptor(syntax));

            return lastEnumValue;
        }


    }
}
