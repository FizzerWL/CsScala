using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;

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
            public NamedTypeSymbol Symbol;
        }
    }
}
