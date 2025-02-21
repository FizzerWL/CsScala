﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsScala
{
    static class Core
    {
        public static void Write(ScalaWriter writer, SyntaxNode node, bool isConst = false)
        {
            TriviaProcessor.ProcessNode(writer, node);

            if (Program.DoNotWrite.ContainsKey(node))
                return;

            var exprOpt = node as ExpressionSyntax;
            string postFactory = null;

            if (exprOpt != null)
            {
                var type = Program.GetModel(node).GetTypeInfo(exprOpt);

                if (type.Type != null && type.Type.SpecialType == SpecialType.System_Byte && type.ConvertedType.SpecialType != SpecialType.System_Byte && Utility.IsNumeric(type.ConvertedType))
                {
                    //Bytes are signed in the JVM, so we have to take care when up-converting them
                    writer.Write("System.CsScala.ByteTo");
                    writer.Write(TypeProcessor.ConvertType(type.ConvertedType));
                    writer.Write("(");
                    postFactory = ")";
                }

                if (type.Type != null && (type.ConvertedType.SpecialType == SpecialType.System_Double || type.ConvertedType.ToString() == "double?") && Utility.IsNumeric(type.Type) && type.Type.SpecialType != SpecialType.System_Double)
                {
                    /* Starting in scala 3, there are cases where we have to specifically call toDouble to convert an int to a double, for example:
import java.util.ArrayList

def TakesDouble(d:Double):Unit = {}
def ReturnsT[T](a:ArrayList[T]):T = a.get(0);
val list = new ArrayList[Int]();
TakesDouble(ReturnsT(list));
                    TODO: There are likely other places that a similar conversion is needed, but I'm not sure how to write this in a fully generic way yet until I encounter more cases
                    */

                    postFactory = ".toDouble";


                }
            }
            

            Factory(writer, node, isConst);
            writer.Write(postFactory);
        }

        private static void Factory(ScalaWriter writer, SyntaxNode node, bool isConst)
        {
            if (node is MethodDeclarationSyntax)
                WriteMethod.Go(writer, node.As<MethodDeclarationSyntax>());
            else if (node is PropertyDeclarationSyntax)
                WriteProperty.Go(writer, node.As<PropertyDeclarationSyntax>());
            else if (node is FieldDeclarationSyntax)
                WriteField.Go(writer, node.As<FieldDeclarationSyntax>());
            else if (node is ConstructorDeclarationSyntax)
                WriteConstructorBody.Go(writer, node.As<ConstructorDeclarationSyntax>());
            else if (node is ExpressionStatementSyntax)
                WriteStatement(writer, node.As<ExpressionStatementSyntax>());
            else if (node is LocalDeclarationStatementSyntax)
                WriteLocalDeclaration.Go(writer, node.As<LocalDeclarationStatementSyntax>());
            else if (node is BlockSyntax)
                WriteBlock(writer, node.As<BlockSyntax>());
            else if (node is InvocationExpressionSyntax)
                WriteInvocationExpression.Go(writer, node.As<InvocationExpressionSyntax>());
            else if (node is LiteralExpressionSyntax)
                WriteLiteralExpression.Go(writer, node.As<LiteralExpressionSyntax>(), isConst);
            else if (node is IdentifierNameSyntax)
                WriteIdentifierName.Go(writer, node.As<IdentifierNameSyntax>());
            else if (node is ImplicitArrayCreationExpressionSyntax)
                WriteArrayCreationExpression.Go(writer, node.As<ImplicitArrayCreationExpressionSyntax>());
            else if (node is ArrayCreationExpressionSyntax)
                WriteArrayCreationExpression.Go(writer, node.As<ArrayCreationExpressionSyntax>());
            else if (node is MemberAccessExpressionSyntax)
                WriteMemberAccessExpression.Go(writer, node.As<MemberAccessExpressionSyntax>());
            else if (node is ParenthesizedLambdaExpressionSyntax)
                WriteLambdaExpression.Go(writer, node.As<ParenthesizedLambdaExpressionSyntax>());
            else if (node is SimpleLambdaExpressionSyntax)
                WriteLambdaExpression.Go(writer, node.As<SimpleLambdaExpressionSyntax>());
            else if (node is ReturnStatementSyntax)
                WriteReturnStatement.Go(writer, node.As<ReturnStatementSyntax>());
            else if (node is ObjectCreationExpressionSyntax)
                WriteObjectCreationExpression.Go(writer, node.As<ObjectCreationExpressionSyntax>());
            else if (node is ElementAccessExpressionSyntax)
                WriteElementAccessExpression.Go(writer, node.As<ElementAccessExpressionSyntax>());
            else if (node is ForEachStatementSyntax)
                WriteForEachStatement.Go(writer, node.As<ForEachStatementSyntax>());
            else if (node is IfStatementSyntax)
                WriteIfStatement.Go(writer, node.As<IfStatementSyntax>());
            else if (node is BinaryExpressionSyntax)
                WriteBinaryExpression.Go(writer, node.As<BinaryExpressionSyntax>());
            else if (node is AssignmentExpressionSyntax)
                WriteBinaryExpression.Go(writer, node.As<AssignmentExpressionSyntax>());
            else if (node is ConditionalExpressionSyntax)
                WriteConditionalExpression.Go(writer, node.As<ConditionalExpressionSyntax>());
            else if (node is BaseExpressionSyntax)
                WriteBaseExpression.Go(writer, node.As<BaseExpressionSyntax>());
            else if (node is ThisExpressionSyntax)
                WriteThisExpression.Go(writer, node.As<ThisExpressionSyntax>());
            else if (node is CastExpressionSyntax)
                WriteCastExpression.Go(writer, node.As<CastExpressionSyntax>());
            else if (node is ThrowStatementSyntax)
                WriteThrowStatement.Go(writer, node.As<ThrowStatementSyntax>());
            else if (node is ThrowExpressionSyntax)
                WriteThrowStatement.WriteThrowExpression(writer, node.As<ThrowExpressionSyntax>());
            else if (node is EqualsValueClauseSyntax)
                WriteEqualsValueClause.Go(writer, node.As<EqualsValueClauseSyntax>());
            else if (node is ForStatementSyntax)
                WriteForStatement.Go(writer, node.As<ForStatementSyntax>());
            else if (node is WhileStatementSyntax)
                WriteWhileStatement.Go(writer, node.As<WhileStatementSyntax>());
            else if (node is BreakStatementSyntax)
                WriteBreakStatement.Go(writer, node.As<BreakStatementSyntax>());
            else if (node is ContinueStatementSyntax)
                WriteContinueStatement.Go(writer, node.As<ContinueStatementSyntax>());
            else if (node is DoStatementSyntax)
                WriteDoStatement.Go(writer, node.As<DoStatementSyntax>());
            else if (node is SwitchStatementSyntax)
                WriteSwitchStatement.Go(writer, node.As<SwitchStatementSyntax>());
            else if (node is TryStatementSyntax)
                WriteTryStatement.Go(writer, node.As<TryStatementSyntax>());
            else if (node is UsingStatementSyntax)
                WriteUsingStatement.Go(writer, node.As<UsingStatementSyntax>());
            else if (node is ParenthesizedExpressionSyntax)
                WriteParenthesizedExpression.Go(writer, node.As<ParenthesizedExpressionSyntax>());
            else if (node is LockStatementSyntax)
                WriteLockStatement.Go(writer, node.As<LockStatementSyntax>());
            else if (node is TypeOfExpressionSyntax)
                WriteTypeOfExpression.Go(writer, node.As<TypeOfExpressionSyntax>());
            else if (node is AnonymousObjectCreationExpressionSyntax)
                WriteAnonymousObjectCreationExpression.Go(writer, node.As<AnonymousObjectCreationExpressionSyntax>());
            else if (node is EmptyStatementSyntax)
                return; //ignore empty statements
            else if (node is DelegateDeclarationSyntax)
                return; //don't write delegates - TypeProcessor converts them to function types directly
            else if (node is DefaultExpressionSyntax)
                WriteDefaultExpression.Go(writer, node.As<DefaultExpressionSyntax>());
            else if (node is GenericNameSyntax)
                WriteGenericName.Go(writer, node.As<GenericNameSyntax>());
            else if (node is ConversionOperatorDeclarationSyntax)
                WriteConversionOperatorDeclaration.Go(writer, node.As<ConversionOperatorDeclarationSyntax>());
            else if (node is PrefixUnaryExpressionSyntax)
                WriteUnaryExpression.WritePrefix(writer, node.As<PrefixUnaryExpressionSyntax>());
            else if (node is PostfixUnaryExpressionSyntax)
                WriteUnaryExpression.WritePostfix(writer, node.As<PostfixUnaryExpressionSyntax>());
            else if (node is SizeOfExpressionSyntax)
                WriteSizeOfExpression.Go(writer, node.As<SizeOfExpressionSyntax>());
            else
                throw new NotImplementedException(node.GetType().Name + " is not supported. " + Utility.Descriptor(node));
        }

        public static void WriteStatement(ScalaWriter writer, ExpressionStatementSyntax statement)
        {
            writer.WriteIndent();
            Write(writer, statement.Expression);
            writer.Write(";\r\n");
        }


        public static void WriteBlock(ScalaWriter writer, BlockSyntax block, bool writeBraces = true)
        {
            if (writeBraces)
                writer.WriteOpenBrace();

            foreach (var statement in block.Statements)
                Write(writer, statement);

            TriviaProcessor.ProcessTrivias(writer, block.DescendantTrivia());

            if (writeBraces)
                writer.WriteCloseBrace();
        }

        public static void WriteStatementAsBlock(ScalaWriter writer, StatementSyntax statement, bool writeBraces = true)
        {
            if (statement is BlockSyntax)
                WriteBlock(writer, statement.As<BlockSyntax>(), writeBraces);
            else
            {
                if (writeBraces)
                    writer.WriteOpenBrace();

                Core.Write(writer, statement);
                TriviaProcessor.ProcessTrivias(writer, statement.DescendantTrivia());

                if (writeBraces)
                    writer.WriteCloseBrace();
            }

        }


    }
}
