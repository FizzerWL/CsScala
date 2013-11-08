using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Roslyn.Compilers.CSharp;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CsScala
{
    public static class Utility
    {

        public static void Parallel<T>(this IEnumerable<T> list, Action<T> action)
        {
#if true
            System.Threading.Tasks.Parallel.ForEach(list, action);
#else
			foreach (var t in list)
				action(t);
#endif
        }

        public static SyntaxTokenList GetModifiers(this MemberDeclarationSyntax member)
        {
            var field = member as FieldDeclarationSyntax;
            if (field != null)
                return field.Modifiers;

            var method = member as BaseMethodDeclarationSyntax;
            if (method != null)
                return method.Modifiers;

            var property = member as PropertyDeclarationSyntax;
            if (property != null)
                return property.Modifiers;

            var dlg = member as DelegateDeclarationSyntax;
            if (dlg != null)
                return dlg.Modifiers;

            throw new Exception("Need handler for member of type " + member.GetType().Name);
        }

        public static string FullName(this NamespaceSymbol ns)
        {
            if (ns.IsGlobalNamespace)
                return "CsRoot";
            else
                return ns.ToString();
        }
        public static string FullNameWithDot(this NamespaceSymbol ns)
        {
            if (ns.IsGlobalNamespace)
                return "CsRoot.";
            else
                return ns.ToString() + ".";
        }

        public static string AttributeOrNull(this XElement element, string attrName)
        {
            var a = element.Attribute(attrName);
            if (a == null)
                return null;
            else
                return a.Value;
        }

        public static T As<T>(this object o)
        {
            return (T)o;
        }

        public static string SubstringSafe(this string s, int startAt, int length)
        {
            if (s.Length < startAt + length)
                return s.Substring(startAt);
            else
                return s.Substring(startAt, length);
        }

        public static bool None<T>(this IEnumerable<T> a, Func<T, bool> pred)
        {
            return !a.Any(pred);
        }

        public static bool None<T>(this IEnumerable<T> a)
        {
            return !a.Any();
        }

        public static string SubstringAfterLast(this string s, char c)
        {
            int i = s.LastIndexOf(c);
            if (i == -1)
                throw new Exception("char not found");
            return s.Substring(i + 1);
        }
        public static string TrySubstringAfterLast(this string s, char c)
        {
            int i = s.LastIndexOf(c);
            if (i == -1)
                return s;
            else
                return s.Substring(i + 1);
        }
        public static string TrySubstringBeforeFirst(this string s, char c)
        {
            int i = s.IndexOf(c);
            if (i == -1)
                return s;
            else
                return s.Substring(0, i);
        }

        public static string SubstringBeforeLast(this string s, char c)
        {
            int i = s.LastIndexOf(c);
            if (i == -1)
                throw new Exception("char not found");
            return s.Substring(0, i);
        }

        public static MethodSymbol UnReduce(this MethodSymbol methodSymbol)
        {
            while (methodSymbol.ReducedFrom != null)
                methodSymbol = methodSymbol.ReducedFrom;

            return methodSymbol;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> array, bool throwOnDuplicate)
        {
            var hs = new HashSet<T>();
            foreach (var t in array)
            {
                if (throwOnDuplicate && hs.Contains(t))
                    throw new ArgumentException("Duplicate key: " + t.ToString());
                hs.Add(t);
            }
            return hs;
        }


        public static IEnumerable<T> Concat<T>(this IEnumerable<T> array, T item)
        {
            return array.Concat(new T[] { item });
        }
        public static IEnumerable<T> Except<T>(this IEnumerable<T> array, T item)
        {
            return array.Except(new T[] { item });
        }



        public static string Descriptor(SyntaxNode node)
        {
            var sb = new StringBuilder();
            sb.Append(node.Span.ToString() + " ");

            while (node != null)
            {


                if (node is BaseTypeDeclarationSyntax)
                    sb.Append("Type: " + node.As<BaseTypeDeclarationSyntax>().Identifier.ValueText + ", ");
                else if (node is MethodDeclarationSyntax)
                    sb.Append("Method: " + node.As<MethodDeclarationSyntax>().Identifier.ValueText + ", ");
                else if (node is PropertyDeclarationSyntax)
                    sb.Append("Property: " + node.As<PropertyDeclarationSyntax>().Identifier.ValueText + ", ");
                node = node.Parent;
            }

            if (sb.Length > 2)
                sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        public static Dictionary<string, string> GetCsScalaAttribute(SyntaxNode node)
        {
            AttributeSyntax attr = null;

            while (node != null)
            {
                if (node is BaseMethodDeclarationSyntax || node is BaseTypeDeclarationSyntax || node is BaseFieldDeclarationSyntax)
                {
                    var list = node is BaseMethodDeclarationSyntax ? node.As<BaseMethodDeclarationSyntax>().AttributeLists
                        : node is BaseTypeDeclarationSyntax ? node.As<BaseTypeDeclarationSyntax>().AttributeLists
                        : node.As<BaseFieldDeclarationSyntax>().AttributeLists;

                    attr = list.SelectMany(o => o.Attributes).SingleOrDefault(o => o.Name.ToString() == "CsScala");
                    if (attr != null)
                        break;
                }
                node = node.Parent;
            }

            if (attr == null || attr.ArgumentList == null)
                return new Dictionary<string, string>();

            return attr.ArgumentList.Arguments.ToDictionary(GetAttributeName, o => o.Expression.As<LiteralExpressionSyntax>().Token.ValueText);
        }

        private static string GetAttributeName(AttributeArgumentSyntax attr)
        {
            return attr.NameEquals.Name.ToString();
        }
        public static string RemoveFromStartOfString(this string s, string toRemove)
        {
            if (!s.StartsWith(toRemove))
                throw new Exception("Does not start string: " + s);
            return s.Substring(toRemove.Length);
        }

        public static string RemoveFromEndOfString(this string s, string toRemove)
        {
            if (!s.EndsWith(toRemove))
                throw new Exception("Does not end string: " + s);
            return s.Substring(0, s.Length - toRemove.Length);
        }

        public static string TryGetIdentifier(ExpressionSyntax expression)
        {
            var identifier = expression as IdentifierNameSyntax;

            if (identifier != null)
                return WriteIdentifierName.TransformIdentifier(identifier.Identifier.ValueText);

            var thisSyntax = expression as ThisExpressionSyntax;
            if (thisSyntax != null)
                return "this";

            var memAccess = expression as MemberAccessExpressionSyntax;
            if (memAccess != null && memAccess.Expression is ThisExpressionSyntax)
                return memAccess.ToString();

            return null;

        }

        internal static string TypeConstraints(TypeParameterSyntax prm, IEnumerable<TypeParameterConstraintClauseSyntax> constraints)
        {
            var identifier = prm.Identifier.ValueText;

            var constraint = constraints.SingleOrDefault(o => o.Name.Identifier.ValueText == identifier);

            if (constraint != null)
                return identifier + string.Join("", constraint.Constraints.Select(TransformTypeConstraint));

            return identifier;
        }

        private static string TransformTypeConstraint(TypeParameterConstraintSyntax constraint)
        {
            if (constraint is TypeConstraintSyntax)
                return " <% " + TypeProcessor.ConvertType(constraint.As<TypeConstraintSyntax>().Type);
            else if (constraint is ClassOrStructConstraintSyntax)
            {
                if (constraint.As<ClassOrStructConstraintSyntax>().ClassOrStructKeyword.Kind == SyntaxKind.ClassKeyword)
                    return " >: Null";
                else
                    throw new Exception("struct type constraint not supported " + Utility.Descriptor(constraint));
            }
            else
                throw new Exception(constraint.GetType().Name);
        }
    }
}
