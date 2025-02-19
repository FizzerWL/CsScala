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
    class LoopInfo
    {
        public LoopInfo(SyntaxNode loopSyntax)
        {
            if (!IsLoopSyntax(loopSyntax))
                throw new Exception("LoopInfo constructed on non-loop");

            RecurseForBreakContinue(loopSyntax, false);
            RecurseForReturnStatement(loopSyntax);
        }

        public bool HasContinue;
        public bool HasBreak;
        public bool HasReturnStatement;

        void RecurseForBreakContinue(SyntaxNode node, bool isInSwitch)
        {
            if (node is ContinueStatementSyntax)
                HasContinue = true;
            else if (node is BreakStatementSyntax && !isInSwitch) //ignore break statements in a switch, since they apply to breaking the switch and not the loop
                HasBreak = true;
            else
            {
                foreach (var child in node.ChildNodes().Where(o => !(o is LambdaExpressionSyntax)))
                {
                    if (!IsLoopSyntax(child)) //any breaks or continues in child loops will belong to that loop, so we can skip recusing into them.
                        RecurseForBreakContinue(child, isInSwitch || child is SwitchStatementSyntax);
                }
            }
        }

        void RecurseForReturnStatement(SyntaxNode node)
        {
            if (node is ReturnStatementSyntax)
                this.HasReturnStatement = true;
            else
                foreach (var child in node.ChildNodes().Where(o => !(o is LambdaExpressionSyntax)))
                {
                    RecurseForReturnStatement(child);
                    if (HasReturnStatement)
                        return; //can stop scanning, we found one, nothing else to find.  Just helps perf.
                }
        }

        public void WritePreLoop(ScalaWriter writer)
        {
            if (HasBreak)
            {
                writer.WriteLine("CsScala.csbreak.breakable {");
                writer.Indent++; //breakable can't have a newline after it
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
                writer.WriteLine("CsScala.cscontinue.breakable {");
                writer.Indent++; //breakable can't have a newline after it
            }
        }

        public void WriteLoopClosing(ScalaWriter writer)
        {
            if (HasContinue)
                writer.WriteCloseBrace();
        }

        public static bool IsLoopSyntax(SyntaxNode syntax)
        {
            return syntax is ForEachStatementSyntax
                || syntax is ForStatementSyntax
                || syntax is WhileStatementSyntax
                || syntax is DoStatementSyntax;
        }

    }
}