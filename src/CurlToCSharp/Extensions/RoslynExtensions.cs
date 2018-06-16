﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CurlToCSharp.Extensions
{
    public class RoslynExtensions
    {
        public static InvocationExpressionSyntax CreateInvocationExpression(
            string leftPart,
            string rightPart,
            params ArgumentSyntax[] argument)
        {
            return CreateInvocationExpression(SyntaxFactory.IdentifierName(leftPart), rightPart, argument);
        }

        public static InvocationExpressionSyntax CreateInvocationExpression(
            string firstPart,
            string secondPart,
            string thirdPart,
            params ArgumentSyntax[] arguments)
        {
            var memberAccessExpression = CreateMemberAccessExpression(firstPart, secondPart);

            return CreateInvocationExpression(memberAccessExpression, thirdPart, arguments);
        }

        public static InvocationExpressionSyntax CreateInvocationExpression(
            ExpressionSyntax leftPart,
            string rightPart,
            params ArgumentSyntax[] argument)
        {
            var expression = CreateMemberAccessExpression(leftPart, rightPart);
            var separatedSyntaxList = new SeparatedSyntaxList<ArgumentSyntax>();
            separatedSyntaxList = separatedSyntaxList.AddRange(argument);

            return SyntaxFactory.InvocationExpression(expression, SyntaxFactory.ArgumentList(separatedSyntaxList));
        }

        public static MemberAccessExpressionSyntax CreateMemberAccessExpression(string leftPart, string rightPart)
        {
            return CreateMemberAccessExpression(SyntaxFactory.IdentifierName(leftPart), rightPart);
        }

        public static ObjectCreationExpressionSyntax CreateObjectCreationExpression(
            string objectName,
            params ArgumentSyntax[] arguments)
        {
            var methodArgumentList = new SeparatedSyntaxList<ArgumentSyntax>().AddRange(arguments);
            return SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.IdentifierName(objectName),
                SyntaxFactory.ArgumentList(methodArgumentList),
                null);
        }

        public static ArgumentSyntax CreateStringLiteralArgument(string argumentName)
        {
            return SyntaxFactory.Argument(
                SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal(argumentName)));
        }

        public static VariableDeclarationSyntax CreateVariableInitializationExpression(
            string variableName,
            ExpressionSyntax expression)
        {
            return SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("var"))
                .AddVariables(
                    SyntaxFactory.VariableDeclarator(
                        SyntaxFactory.Identifier(variableName),
                        null,
                        SyntaxFactory.EqualsValueClause(expression)));
        }

        public static UsingStatementSyntax CreateUsingStatement(string variableName, string disposableName, params ArgumentSyntax[] arguments)
        {
            var variableDeclaration = CreateVariableFromNewObjectExpression(variableName, disposableName, arguments);

            return SyntaxFactory.UsingStatement(variableDeclaration, null, SyntaxFactory.Block());
        }

        public static VariableDeclarationSyntax CreateVariableFromNewObjectExpression(string variableName, string newObjectName, params ArgumentSyntax[] arguments)
        {
            var objectCreationExpression = CreateObjectCreationExpression(newObjectName, arguments);
            return CreateVariableInitializationExpression(variableName, objectCreationExpression);
        }

        private static MemberAccessExpressionSyntax CreateMemberAccessExpression(
            ExpressionSyntax leftPart,
            string rightPart)
        {
            return SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                leftPart,
                SyntaxFactory.IdentifierName(rightPart));
        }
    }
}
