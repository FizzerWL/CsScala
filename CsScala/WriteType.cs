using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace CsScala
{
    static class WriteType
    {

        public static void Go()
        {
            var partials = TypeState.Instance.Partials;
            var first = partials.First();


            if (first.Syntax is EnumDeclarationSyntax)
            {
                using (var writer = new ScalaWriter(first.Symbol.ContainingNamespace.FullName(), TypeState.Instance.TypeName))
                {
                    var package = first.Symbol.ContainingNamespace.FullName();
                    if (package.Length > 0)
                        writer.WriteLine("package " + package + @";");

                    WriteImports.Go(writer);

                    WriteEnum.Go(writer, TypeState.Instance.Partials.Select(o => o.Syntax).Cast<EnumDeclarationSyntax>().SelectMany(o => o.Members).Where(o => !Program.DoNotWrite.ContainsKey(o)));
                }
                return;
            }


            var bases = partials
                .Select(o => o.Syntax.BaseList)
                .Where(o => o != null)
                .SelectMany(o => o.Types)
                .Select(o => Program.GetModel(o).GetTypeInfo(o.Type).ConvertedType)
                .Distinct()
                .ToList();

            var interfaces = bases.Where(o => o.TypeKind == TypeKind.Interface).ToList();

            TypeState.Instance.AllMembers = partials.Select(o => o.Syntax).Cast<TypeDeclarationSyntax>().SelectMany(o => o.Members).Where(o => !Program.DoNotWrite.ContainsKey(o)).ToList();

            var allMembersToWrite = TypeState.Instance.AllMembers
                .Where(member => !(member is TypeDeclarationSyntax)
                    && !(member is EnumDeclarationSyntax)
                    && !(member is DelegateDeclarationSyntax))
                .ToList();

            var isStaticClass = partials.Any(o => o.Syntax.Modifiers.Any(SyntaxKind.StaticKeyword));

            if (allMembersToWrite.Count == 0 && isStaticClass)
                return; //don't write empty classes, scala generates a warning

            using (var writer = new ScalaWriter(first.Symbol.ContainingNamespace.FullName(), TypeState.Instance.TypeName))
            {
                //TypeState.Instance.DerivesFromObject = bases.Count == interfaces.Count;

                var package = first.Symbol.ContainingNamespace.FullName();
                if (package.Length > 0)
                    writer.WriteLine("package " + package + @";");

                WriteImports.Go(writer);

                var instanceCtors = TypeState.Instance.AllMembers.OfType<ConstructorDeclarationSyntax>()
                    .Where(o => !o.Modifiers.Any(SyntaxKind.StaticKeyword))
                    .ToList();

                if (instanceCtors.Count > 1)
                    throw new Exception("Overloaded constructors are not supported.  Consider changing all but one to static Create methods " + Utility.Descriptor(first.Syntax));

                var ctorOpt = instanceCtors.SingleOrDefault();

                foreach (var staticMembers in new[] { true, false })
                {
                    var membersToWrite = allMembersToWrite.Where(o => IsStatic(o) == staticMembers).ToList();

                    if (membersToWrite.Count == 0 && (staticMembers || isStaticClass))
                        continue;

                    if (staticMembers)
                        writer.Write("object ");
                    else if (first.Syntax.Kind() == SyntaxKind.InterfaceDeclaration)
                        writer.Write("trait ");
                    else
                    {
                        if (partials.Any(o => o.Syntax.Modifiers.Any(SyntaxKind.AbstractKeyword)))
                            writer.Write("abstract ");

                        writer.Write("class ");
                    }

                    writer.Write(TypeState.Instance.TypeName);



                    if (!staticMembers && first.Syntax is TypeDeclarationSyntax)
                    {
                        //Look for generic arguments 
                        var genericArgs = partials
                            .Select(o => o.Syntax)
                            .Cast<TypeDeclarationSyntax>()
                            .Where(o => o.TypeParameterList != null)
                            .SelectMany(o => o.TypeParameterList.Parameters)
                            .ToList();

                        if (genericArgs.Count > 0)
                        {
                            writer.Write("[");
                            writer.Write(string.Join(", ", genericArgs.Select(o => TypeParameter(o))));
                            writer.Write("]");
                        }

                        //Write constructor arguments
                        if (ctorOpt != null && ctorOpt.ParameterList.Parameters.Count > 0)
                        {
                            writer.Write("(");
                            var firstParameter = true;
                            foreach (var parameter in ctorOpt.ParameterList.Parameters)
                            {
                                if (firstParameter)
                                    firstParameter = false;
                                else
                                    writer.Write(", ");

                                writer.Write(WriteIdentifierName.TransformIdentifier(parameter.Identifier.ValueText));
                                writer.Write(TypeProcessor.ConvertTypeWithColon(parameter.Type));

                                if (parameter.Default != null)
                                {
                                    writer.Write(" = ");
                                    Core.Write(writer, parameter.Default.Value);
                                }
                            }
                            writer.Write(")");
                        }

                        bool firstBase = true;
                        foreach (var baseType in bases.OrderBy(o => o.TypeKind == TypeKind.Interface ? 1 : 0))
                        {
                            if (firstBase)
                                writer.Write(" extends ");
                            else
                                writer.Write(" with ");

                            writer.Write(TypeProcessor.ConvertType(baseType));


                            if (firstBase && ctorOpt != null && ctorOpt.Initializer != null && ctorOpt.Initializer.ArgumentList.Arguments.Count > 0)
                            {
                                writer.Write("(");
                                bool firstArg = true;
                                foreach (var init in ctorOpt.Initializer.ArgumentList.Arguments)
                                {
                                    if (firstArg)
                                        firstArg = false;
                                    else
                                        writer.Write(", ");

                                    Core.Write(writer, init.Expression);
                                }
                                writer.Write(")");
                            }

                            firstBase = false;
                        }
                    }

                    writer.Write("\r\n");

                    writer.WriteOpenBrace();

                    var fields = membersToWrite.OfType<FieldDeclarationSyntax>().ToList();
                    var nonFields = membersToWrite.Except(fields);

                    fields = SortFields(fields);

                    foreach (var member in fields)
                        Core.Write(writer, member);
                    foreach (var member in nonFields)
                        Core.Write(writer, member);


                    if (!staticMembers && ctorOpt != null && ctorOpt.Body != null && ctorOpt.Body.As<BlockSyntax>().Statements.Count > 0)
                    {
                        writer.WriteLine();
                        Core.WriteBlock(writer, ctorOpt.Body.As<BlockSyntax>(), true); //render braces so local ctor variables don't bleed out into fields
                    }


                    writer.WriteCloseBrace();
                }
            }
        }

        private static List<FieldDeclarationSyntax> SortFields(List<FieldDeclarationSyntax> fields)
        {
            if (fields.Count == 0)
                return fields;

            var dependencies = fields.ToDictionary(o => Program.GetModel(o).GetDeclaredSymbol(o.Declaration.Variables.First()).As<IFieldSymbol>(), o => new { Syntax = o, Dependicies = new List<IFieldSymbol>() });

            

            foreach(var dep in dependencies)
            {

                foreach (var fieldDepend in dep.Value.Syntax.DescendantNodes().OfType<ExpressionSyntax>().Select(o => Program.GetModel(o).GetSymbolInfo(o).Symbol).OfType<IFieldSymbol>())
                    if (dependencies.ContainsKey(fieldDepend))
                        dep.Value.Dependicies.Add(fieldDepend);
            }

            var ret = new List<FieldDeclarationSyntax>();
            var symbolsAdded = new HashSet<IFieldSymbol>();

            while (dependencies.Count > 0)
                foreach(var dep in dependencies.ToList())
                {
                    for(int i=0;i<dep.Value.Dependicies.Count;i++)
                    {
                        if (symbolsAdded.Contains(dep.Value.Dependicies[i]))
                            dep.Value.Dependicies.RemoveAt(i--);

                    }

                    if (dep.Value.Dependicies.Count == 0)
                    {
                        ret.Add(dep.Value.Syntax);
                        symbolsAdded.Add(dep.Key);
                        dependencies.Remove(dep.Key);
                    }
                }

            return ret;
        }

        private static string TypeParameter(TypeParameterSyntax type)
        {
            var ret = Utility.TypeConstraints(type, TypeState.Instance.Partials.SelectMany(z => z.Syntax.As<TypeDeclarationSyntax>().ConstraintClauses));

            if (ClassTags.NeedsClassTag(TypeState.Instance.Partials.First().Symbol, type.Identifier.ValueText))
                ret += " :ClassTag";

            return ret;
        }

        private static bool IsStatic(MemberDeclarationSyntax member)
        {
            var modifiers = member.GetModifiers();
            return modifiers.Any(SyntaxKind.StaticKeyword) || modifiers.Any(SyntaxKind.ConstKeyword);
        }



        public static string TypeName(INamedTypeSymbol type)
        {
            var sb = new StringBuilder(type.Name);

            while (type.ContainingType != null)
            {
                type = type.ContainingType;
                sb.Insert(0, type.Name + "_");
            }

            return sb.ToString();
        }
    }
}
