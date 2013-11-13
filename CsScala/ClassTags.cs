using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScala.Translations;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    static class ClassTags
    {
        class ClassTagInfo
        {
            public MethodDeclarationSyntax Syntax;
            public HashSet<string> NeedsClassTagOpt; //null until we've processed the method

            public ClassTagInfo(MethodDeclarationSyntax syntax)
            {
                this.Syntax = syntax;
            }
        }

        private static ConcurrentDictionary<MethodSymbol, ClassTagInfo> _methods = new ConcurrentDictionary<MethodSymbol, ClassTagInfo>();

        public static void InitMethod(MethodSymbol symbol, MethodDeclarationSyntax syntax)
        {
            if (syntax.TypeParameterList.Parameters.Count == 0)
                throw new Exception("ClassTags only is concerned with methods that have type parameters");

            _methods.TryAdd(symbol, new ClassTagInfo(syntax));
        }

        public static bool NeedsClassTag(MethodSymbol methodSymbol, MethodDeclarationSyntax methodSyntax, string templateID)
        {
            return GetClassTagHashSet(methodSymbol, methodSyntax).Contains(templateID);
        }

        private static HashSet<string> GetClassTagHashSet(MethodSymbol methodSymbol, MethodDeclarationSyntax methodSyntax)
        {
            var info = _methods[methodSymbol];
            if (info.NeedsClassTagOpt != null)
                return info.NeedsClassTagOpt; //already have it cached

            return info.NeedsClassTagOpt = GetClassTagHashSetUncached(methodSymbol, methodSyntax);
        }

        private static HashSet<string> GetClassTagHashSetUncached(MethodSymbol methodSymbol, MethodDeclarationSyntax methodSyntax)
        {
            var model = Program.GetModel(methodSyntax);
            var ret = new HashSet<string>();

            foreach (var invoke in methodSyntax.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                var invokeMethod = model.GetSymbolInfo(invoke).Symbol.As<MethodSymbol>();

                if (invokeMethod.TypeParameters.Count == 0)
                    continue;
                var origInvoke = invokeMethod.OriginalDefinition.As<MethodSymbol>();

                HashSet<string> invokeTags;

                ClassTagInfo invokeInfo;
                if (_methods.TryGetValue(origInvoke, out invokeInfo))
                    invokeTags = GetClassTagHashSet(origInvoke, invokeInfo.Syntax);
                else
                {
                    //not a method we're writing out.  Check if it's a BCL method with known ClassTags.
                    invokeTags = TryGetBCLClassTags(origInvoke);

                    if (invokeTags == null)
                        continue; //not a BCL method we know of and not a method we're writing out. no tags needed
                }

                //invokeTags is now filled with the invoked methods tags.  See if those line up with our own tags.
                var tagIndexes = invokeTags.Select(o => invokeMethod.TypeParameters.IndexOf(invokeMethod.TypeParameters.Single(z => z.ToString() == o)));
                foreach (var retTag in tagIndexes.Select(o => invokeMethod.TypeArguments[o].ToString()))
                    ret.Add(retTag);
            }

            foreach(var objCreation in methodSyntax.DescendantNodes().OfType<ObjectCreationExpressionSyntax>())
            {
                var ctor = model.GetSymbolInfo(objCreation).Symbol.As<MethodSymbol>();
                //if (objCreation.ToString() == "new List<C>()")
                //    Debugger.Break();

                var tags = TryGetBCLClassTags(ctor);
                if (tags == null)
                    continue;
                
                var genName = objCreation.Type as GenericNameSyntax;
                if (genName == null)
                    continue;

                foreach (var arg in genName.TypeArgumentList.Arguments)
                    ret.Add(arg.ToString());
            }

            foreach (var arrayCreation in methodSyntax.DescendantNodes().OfType<ArrayCreationExpressionSyntax>())
            {
                var info = model.GetTypeInfo(arrayCreation.Type.ElementType).Type;
                if (info.TypeKind == TypeKind.TypeParameter)
                    ret.Add(info.ToString());
            }

            return ret;
        }

        private static HashSet<string> TryGetBCLClassTags(MethodSymbol method)
        {
            var methodStr = TypeProcessor.GenericTypeName(method.ContainingType) + "." + method.Name;

            var trans = NeedsClassTagTranslation.Get(methodStr);
            if (trans != null)
                return trans.TypeParametersOpt == null ? method.TypeParameters.ToArray().Select(o => o.ToString()).ToHashSet(true) : trans.TypeParametersOpt.ToHashSet(true);
            else
                return null;
        }
    }
}
