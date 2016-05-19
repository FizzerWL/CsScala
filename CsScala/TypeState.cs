using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    class TypeState
    {
        [ThreadStatic]
        public static TypeState Instance;

        public List<SyntaxAndSymbol> Partials;
        public string TypeName;

        public List<MemberDeclarationSyntax> AllMembers;
        public int InLambdaBreakable = 0;


        public class SyntaxAndSymbol
        {
            public BaseTypeDeclarationSyntax Syntax;
            public INamedTypeSymbol Symbol;
        }
    }
}
