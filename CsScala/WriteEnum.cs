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
                writer.WriteLine("final val " + WriteIdentifierName.TransformIdentifier(value.Syntax.Identifier.ValueText) + ":Int = " + value.Value + ";");

            writer.WriteLine();
            writer.WriteLine(@"def ToString(n:java.lang.Integer):String = if (n == null) """" else ToString(n.intValue());");

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
                writer.WriteLine("case \"" + value.Syntax.Identifier.ValueText + "\" | \"" + value.Value + "\" => " + value.Value + ";");

            writer.WriteCloseBrace();
            writer.WriteCloseBrace();

            writer.WriteLine();
            writer.WriteIndent();
            writer.Write("final val Values:Array[Int] = Array(");
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



        public static void Check(SyntaxNode node)
        {
            //Check for enums being converted to objects.  There are common methods in .net that accept objects just to call .ToString() on them, such as Console.WriteLine and HttpResponse.Write.  In these cases, we would miss our enum-to-string conversion that ensures the strings are used instead of the number.  To work around this, you should call .ToString() on the enum before passing it in.  It's a good idea to do this anyway for performance, so we just fail instead of doing the conversion automatically.  This check is a bit overzealous, as there are also legitimate reasons to conver enums to objects, but we'd rather be safe and reject a legitimate use than have code behave incorrectly.

            var expression = node as ExpressionSyntax;
            if (expression == null)
                return;

            var typeInfo = Program.GetModel(node).GetTypeInfo(expression);

            if (typeInfo.Type == null || typeInfo.ConvertedType == null || typeInfo.Type == typeInfo.ConvertedType || typeInfo.Type.BaseType == null)
                return;
            if (typeInfo.ConvertedType.SpecialType != SpecialType.System_Object)
                return;

            if (typeInfo.Type.BaseType.SpecialType != SpecialType.System_Enum)
            {
                if (typeInfo.Type.Name != "Nullable" || typeInfo.Type.ContainingNamespace.FullName() != "System")
                    return;

                var nullableType = typeInfo.Type.As<INamedTypeSymbol>().TypeArguments.Single();

                if (nullableType.BaseType.SpecialType != SpecialType.System_Enum)
                    return;

            }


            throw new Exception("Enums cannot convert to objects.  Use .ToString() if you're using the enum as a string. " + Utility.Descriptor(node) + ", expr=" + expression.ToString());
        }
    }
}
