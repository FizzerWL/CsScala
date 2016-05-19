using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class WriteConstructorBody
    {


        public static void Go(ScalaWriter writer, ConstructorDeclarationSyntax constructor)
        {
            //Only write out static constructors here.  If we encounter an instance constructor, we can ignore it since it's already written out by WriteType
            if (constructor.Modifiers.Any(SyntaxKind.StaticKeyword))
                WriteStaticConstructor(writer, constructor);
        }

        //public static void WriteInstanceConstructor(ScalaWriter writer, ConstructorDeclarationSyntax ctor)
        //{


        //	foreach (var field in TypeState.Instance.AllMembers
        //				.OfType<BaseFieldDeclarationSyntax>()
        //				.Where(o => !o.Modifiers.Any(SyntaxKind.StaticKeyword))
        //				.SelectMany(o => o.Declaration.Variables)
        //				.Where(o =>
        //					(o.Initializer != null && !WriteField.IsConst(o.Parent.Parent.As<BaseFieldDeclarationSyntax>().Modifiers, o.Initializer, o.Parent.As<VariableDeclarationSyntax>().Type))
        //					||
        //					(o.Initializer == null && TypeProcessor.ValueToReference(o.Parent.As<VariableDeclarationSyntax>().Type))
        //					||
        //					o.Parent.Parent is EventFieldDeclarationSyntax))
        //	{
        //		writer.WriteIndent();
        //		writer.Write(field.Identifier.ValueText);
        //		writer.Write(" = ");

        //		if (field.Initializer == null)
        //		{
        //			//The only way to get here with a null initializer is for a TypeProcess.ValueToReference field.
        //			writer.Write("new ");
        //			writer.Write(TypeProcessor.ConvertType(field.Parent.As<VariableDeclarationSyntax>().Type));
        //			writer.Write("()");
        //		}
        //		else
        //		{
        //			Core.Write(writer, field.Initializer.Value);
        //		}

        //		writer.Write(";\r\n");
        //	}

        //}



        public static void WriteStaticConstructor(ScalaWriter writer, ConstructorDeclarationSyntax staticConstructor)
        {
            //var staticFieldsNeedingInitialization = TypeState.Instance.AllMembers
            //	.OfType<BaseFieldDeclarationSyntax>()
            //	.Where(o => o.Modifiers.Any(SyntaxKind.StaticKeyword))
            //	.SelectMany(o => o.Declaration.Variables)
            //	.Where(o =>
            //		(o.Initializer != null && !WriteField.IsConst(o.Parent.Parent.As<BaseFieldDeclarationSyntax>().Modifiers, o.Initializer, o.Parent.As<VariableDeclarationSyntax>().Type))
            //		||
            //		(o.Initializer == null && TypeProcessor.ValueToReference(o.Parent.As<VariableDeclarationSyntax>().Type))
            //		||
            //		o.Parent.Parent is EventFieldDeclarationSyntax)
            //	.ToList();

            if (staticConstructor.Body == null)
                return;


            writer.WriteLine("def cctor()");
            writer.WriteOpenBrace();

            //foreach (var field in staticFieldsNeedingInitialization)
            //{
            //	writer.WriteIndent();
            //	writer.Write(field.Identifier.ValueText);
            //	writer.Write(" = ");

            //	if (field.Initializer == null)
            //	{
            //		//The only way to get here without an initializer is if it's a TypeProcessor.ValueToReference.
            //		writer.Write("new ");
            //		writer.Write(TypeProcessor.ConvertType(field.Parent.As<VariableDeclarationSyntax>().Type));
            //		writer.Write("()");
            //	}
            //	else
            //	{
            //		Core.Write(writer, field.Initializer.As<EqualsValueClauseSyntax>().Value);
            //	}
            //	writer.Write(";\r\n");
            //}



            foreach (var statement in staticConstructor.Body.As<BlockSyntax>().Statements)
                Core.Write(writer, statement);

            writer.WriteCloseBrace();

            StaticConstructors.Add(TypeState.Instance.Partials.First().Symbol.ContainingNamespace.FullNameWithDot() + TypeState.Instance.TypeName);
        }

        static HashSet<string> StaticConstructors = new HashSet<string>();
        static HashSet<string> AllTypes = new HashSet<string>();


        public static void WriteConstructorsHelper(IEnumerable<INamedTypeSymbol> allTypes)
        {
            foreach (var t in allTypes.Select(o => o.ContainingNamespace.FullNameWithDot() + WriteType.TypeName(o)))
                AllTypes.Add(t);

            if (StaticConstructors.Count == 0)
                return; //no need for it.

            using (var writer = new ScalaWriter("CsRoot", "Constructors"))
            {
                writer.WriteLine(@"package CsRoot;
/*
This file lists all the static constructors.  Scala doesn't have the same concept of static constructors as C#, so CsScala generated cctor() methods.  You must call these manually.  If you call Constructors.init(), all static constructors will be called at once and you won't have to worry about calling each one manually.
*/");

                //foreach (var type in AllTypes.OrderBy(o => o))
                //	writer.WriteLine("import " + type + ";");

                writer.WriteLine("object Constructors");
                writer.WriteOpenBrace();

                writer.WriteLine("def init()");
                writer.WriteOpenBrace();
                foreach (var cctor in StaticConstructors.OrderBy(o => o))
                    writer.WriteLine(cctor + ".cctor();");
                writer.WriteCloseBrace();
                writer.WriteCloseBrace();
            }
        }

    }
}
