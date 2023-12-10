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

namespace Nitrocid.Analyzers.Misc.Text
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NewLineSplitEnvironmentNewLineUsageCodeFixProvider)), Shared]
    public class NewLineSplitEnvironmentNewLineUsageCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(NewLineSplitEnvironmentNewLineUsageAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.NewLineSplitEnvironmentNewLineUsageCodeFixTitle,
                    createChangedSolution: c => UseTextToolsFormatStringAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.NewLineSplitEnvironmentNewLineUsageCodeFixTitle)),
                diagnostic);
        }

        private async Task<Solution> UseTextToolsFormatStringAsync(Document document, InvocationExpressionSyntax typeDecl, CancellationToken cancellationToken)
        {
            // We need to have a syntax that calls myvar.SplitNewLines
            var maes = (MemberAccessExpressionSyntax)typeDecl.Expression;
            var varName = maes.Expression;
            var classSyntax = varName;
            var methodSyntax = SyntaxFactory.IdentifierName("SplitNewLines");
            var maesSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
            var argsSyntax = SyntaxFactory.ArgumentList();
            var resultSyntax = SyntaxFactory.InvocationExpression(maesSyntax, argsSyntax);
            var replacedSyntax = resultSyntax
                .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

            // Actually replace
            var node = await document.GetSyntaxRootAsync(cancellationToken);
            var finalNode = node.ReplaceNode(typeDecl, replacedSyntax);

            // Check the imports
            var compilation = finalNode as CompilationUnitSyntax;
            if (compilation?.Usings.Any(u => u.Name.ToString() == "KS.Misc.Text") == false)
            {
                var name = SyntaxFactory.QualifiedName(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.IdentifierName("KS"),
                        SyntaxFactory.IdentifierName("Misc")),
                    SyntaxFactory.IdentifierName("Text"));
                compilation = compilation
                    .AddUsings(SyntaxFactory.UsingDirective(name));
            }

            var finalDoc = document.WithSyntaxRoot(compilation);
            return finalDoc.Project.Solution;
        }
    }
}
