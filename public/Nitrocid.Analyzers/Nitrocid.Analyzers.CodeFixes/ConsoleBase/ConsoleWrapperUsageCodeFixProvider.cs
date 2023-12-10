﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nitrocid.Analyzers.Resources;

namespace Nitrocid.Analyzers.ConsoleBase
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConsoleWrapperUsageCodeFixProvider)), Shared]
    public class ConsoleWrapperUsageCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(ConsoleWrapperUsageAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MemberAccessExpressionSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.ConsoleWrapperUsageCodeFixTitle,
                    createChangedSolution: c => UseTextToolsFormatStringAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.ConsoleWrapperUsageCodeFixTitle)),
                diagnostic);
        }

        private async Task<Solution> UseTextToolsFormatStringAsync(Document document, MemberAccessExpressionSyntax typeDecl, CancellationToken cancellationToken)
        {
            if (typeDecl.Expression is IdentifierNameSyntax identifier)
            {
                // Get the method
                var idName = ((IdentifierNameSyntax)typeDecl.Name).Identifier.Text;

                // We need to have a syntax that calls ConsoleWrapper.idName
                var classSyntax = SyntaxFactory.IdentifierName("ConsoleWrapper");
                var methodSyntax = SyntaxFactory.IdentifierName(idName);
                var resultSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                var replacedSyntax = resultSyntax
                    .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                    .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

                // Actually replace
                var node = await document.GetSyntaxRootAsync(cancellationToken);
                var finalNode = node.ReplaceNode(typeDecl, replacedSyntax);

                // Check the imports
                var compilation = finalNode as CompilationUnitSyntax;
                if (compilation?.Usings.Any(u => u.Name.ToString() == "KS.ConsoleBase") == false)
                {
                    var name = SyntaxFactory.QualifiedName(
                        SyntaxFactory.IdentifierName("KS"),
                        SyntaxFactory.IdentifierName("ConsoleBase"));
                    compilation = compilation
                        .AddUsings(SyntaxFactory.UsingDirective(name));
                }

                var finalDoc = document.WithSyntaxRoot(compilation);
                return finalDoc.Project.Solution;
            }
            return document.Project.Solution;
        }
    }
}
