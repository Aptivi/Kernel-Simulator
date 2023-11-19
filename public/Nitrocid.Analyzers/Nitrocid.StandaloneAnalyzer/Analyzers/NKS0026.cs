﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.MiscWriters;

namespace Nitrocid.StandaloneAnalyzer.Analyzers
{
    internal class NKS0026 : IAnalyzer
    {
        public bool Analyze(Document document)
        {
            var tree = document.GetSyntaxTreeAsync().Result;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            bool found = false;
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                if (syntaxNode is not MemberAccessExpressionSyntax exp)
                    continue;

                // Check the ToString() call
                if (exp.Parent is InvocationExpressionSyntax parentIes)
                {
                    var parentExp = (MemberAccessExpressionSyntax)parentIes.Expression;
                    if (parentExp.Name.ToString() != nameof(ToString))
                        continue;
                }
                else if (exp.Parent is MemberAccessExpressionSyntax parentMaes)
                {
                    if (parentMaes.Name.ToString() != nameof(ToString))
                        continue;
                }

                // Analyze!
                if (exp.Expression is IdentifierNameSyntax identifier)
                {
                    var location = exp.Parent.GetLocation();
                    if (identifier.Identifier.Text == "TimeDateTools")
                    {
                        // Let's see if the caller tries to access TimeDateTools.KernelDateTime.ToString.
                        var name = (IdentifierNameSyntax)exp.Name;
                        var idName = name.Identifier.Text;
                        if (idName == "KernelDateTime")
                        {
                            var inName = (IdentifierNameSyntax)exp.Name;
                            var inIdName = inName.Identifier.Text;
                            if (inIdName == "ToString")
                            {
                                var lineSpan = location.GetLineSpan();
                                TextWriterColor.Write($"{GetType().Name}: {document.FilePath} ({lineSpan.StartLinePosition} -> {lineSpan.EndLinePosition}): Caller uses Console.ResetColor instead of ResetColor()", true, ConsoleColors.Yellow);
                                if (!string.IsNullOrEmpty(document.FilePath))
                                    LineHandleRangedWriter.PrintLineWithHandle(document.FilePath, lineSpan.StartLinePosition.Line + 1, lineSpan.StartLinePosition.Character + 1, lineSpan.EndLinePosition.Character);
                                found = true;
                            }
                        }
                    }
                }
            }
            return found;
        }

        public async Task SuggestAsync(Document document, CancellationToken cancellationToken = default)
        {
            var tree = document.GetSyntaxTreeAsync().Result;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                if (syntaxNode is not MemberAccessExpressionSyntax exp)
                    continue;
                if (exp.Expression is IdentifierNameSyntax identifier && exp.Name is IdentifierNameSyntax idName)
                {
                    // Get the method
                    if (identifier.Identifier.Text != "TimeDateTools")
                        continue;
                    if (idName.Identifier.Text != "KernelDateTime")
                        continue;

                    // We need to have a syntax that calls TimeDateRenderers.RenderDate
                    var classSyntax = SyntaxFactory.IdentifierName("TimeDateRenderers");
                    var methodSyntax = SyntaxFactory.IdentifierName("RenderDate");
                    var maeSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                    var valuesSyntax = SyntaxFactory.ArgumentList().AddArguments();
                    var resultSyntax = SyntaxFactory.InvocationExpression(maeSyntax, valuesSyntax);
                    var replacedSyntax = resultSyntax
                        .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                        .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

                    // Actually replace
                    var node = await document.GetSyntaxRootAsync(cancellationToken);
                    var finalNode = node.ReplaceNode(exp, replacedSyntax);
                    TextWriterColor.Write("Here's what the replacement would look like (with no Roslyn trivia):", true, ConsoleColors.Yellow);
                    TextWriterColor.Write($"  - {exp}", true, ConsoleColors.Red);
                    TextWriterColor.Write($"  + {replacedSyntax.ToFullString()}", true, ConsoleColors.Green);

                    // Check the imports
                    var compilation = finalNode as CompilationUnitSyntax;
                    if (compilation?.Usings.Any(u => u.Name.ToString() == "KS.Kernel.Time.Renderers") == false)
                    {
                        var name = SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(
                                SyntaxFactory.QualifiedName(
                                    SyntaxFactory.IdentifierName("KS"),
                                    SyntaxFactory.IdentifierName("Kernel")),
                                SyntaxFactory.IdentifierName("Time")),
                            SyntaxFactory.IdentifierName("Renderers"));
                        var directive = SyntaxFactory.UsingDirective(name).NormalizeWhitespace();
                        TextWriterColor.Write("Additionally, the suggested fix will add the following using statement:", true, ConsoleColors.Yellow);
                        TextWriterColor.Write($"  + {directive.ToFullString()}", true, ConsoleColors.Green);
                    }
                }
            }
        }
    }
}
