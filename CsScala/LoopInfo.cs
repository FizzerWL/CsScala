using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CsScala
{
    class LoopInfo
    {
        public LoopInfo(SyntaxNode loopSyntax)
        {
            if (!IsLoopSyntax(loopSyntax))
                throw new Exception("LoopInfo constructed on non-loop");

            Recurse(loopSyntax, false);
        }

        private bool HasContinue;
        private bool HasBreak;

        void Recurse(SyntaxNode node, bool isInSwitch)
        {
            if (node is ContinueStatementSyntax)
                HasContinue = true;
            else if (node is BreakStatementSyntax && !isInSwitch) //ignore break statements in a switch, since they apply to breaking the switch and not the loop
                HasBreak = true;
            else
            {
                foreach (var child in node.ChildNodes())
                {
                    if (!IsLoopSyntax(child)) //any breaks or continues in child loops will belong to that loop, so we can skip recusing into them.
                        Recurse(child, isInSwitch || child is SwitchStatementSyntax);
                }
            }
        }

        public void WritePreLoop(ScalaWriter writer)
        {
            if (HasBreak)
            {
                writer.WriteLine("CsScala.csbreak.breakable");
                writer.WriteOpenBrace();
            }
        }

        public void WritePostLoop(ScalaWriter writer)
        {
            if (HasBreak)
            {
                writer.WriteCloseBrace();
            }
        }

        public void WriteLoopOpening(ScalaWriter writer)
        {
            if (HasContinue)
            {
                writer.WriteLine("CsScala.cscontinue.breakable");
                writer.WriteOpenBrace();
            }
        }

        public void WriteLoopClosing(ScalaWriter writer)
        {
            if (HasContinue)
                writer.WriteCloseBrace();
        }

        static bool IsLoopSyntax(SyntaxNode syntax)
        {
            return syntax is ForEachStatementSyntax
                || syntax is ForStatementSyntax
                || syntax is WhileStatementSyntax
                || syntax is DoStatementSyntax;
        }

    }
}