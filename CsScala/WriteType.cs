using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class WriteType
    {

        public static void Go()
        {
            var partials = TypeState.Instance.Partials;
            var first = partials.First();



            using (var writer = new ScalaWriter(first.Symbol.ContainingNamespace.FullName(), TypeState.Instance.TypeName))
            {
                var bases = partials
                    .Select(o => o.Syntax.BaseList)
                    .Where(o => o != null)
                    .SelectMany(o => o.Types)
                    .Select(o => (TypeSymbol)Program.GetModel(o).GetTypeInfo(o).ConvertedType)
                    .Distinct()
                    .ToList();

                var interfaces = bases.Where(o => o.TypeKind == TypeKind.Interface).ToList();

                //TypeState.Instance.DerivesFromObject = bases.Count == interfaces.Count;

                var package = first.Symbol.ContainingNamespace.FullName();
                if (package.Length > 0)
                    writer.WriteLine("package " + package + @";");

                WriteImports.Go(writer);

                if (first.Syntax is EnumDeclarationSyntax)
                {
                    WriteEnum.Go(writer, TypeState.Instance.Partials.Select(o => o.Syntax).Cast<EnumDeclarationSyntax>().SelectMany(o => o.Members).Where(o => !Program.DoNotWrite.ContainsKey(o)));
                    return;
                }


                TypeState.Instance.AllMembers = partials.Select(o => o.Syntax).Cast<TypeDeclarationSyntax>().SelectMany(o => o.Members).Where(o => !Program.DoNotWrite.ContainsKey(o)).ToList();

                var allMembersToWrite = TypeState.Instance.AllMembers
                    .Where(member => !(member is TypeDeclarationSyntax)
                        && !(member is EnumDeclarationSyntax)
                        && !(member is DelegateDeclarationSyntax))
                    .ToList();

                var instanceCtors = TypeState.Instance.AllMembers.OfType<ConstructorDeclarationSyntax>()
                    .Where(o => !o.Modifiers.Any(SyntaxKind.StaticKeyword))
                    .ToList();

                if (instanceCtors.Count > 1)
                    throw new Exception("Overloaded constructors are not supported.  Consider changing all but one to static Create methods " + Utility.Descriptor(first.Syntax));

                var ctorOpt = instanceCtors.SingleOrDefault();

                foreach (var staticMembers in new[] { true, false })
                {
                    var membersToWrite = allMembersToWrite.Where(o => IsStatic(o) == staticMembers).ToList();

                    if (membersToWrite.Count == 0 && (staticMembers || partials.Any(o => o.Syntax.Modifiers.Any(SyntaxKind.StaticKeyword))))
                        continue;

                    if (staticMembers)
                        writer.Write("object ");
                    else if (first.Syntax.Kind == SyntaxKind.InterfaceDeclaration)
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

                    foreach (var member in membersToWrite)
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



        public static string TypeName(NamedTypeSymbol type)
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
