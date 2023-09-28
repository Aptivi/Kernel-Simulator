﻿
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.RSS.Commands
{
    /// <summary>
    /// Gets article information
    /// </summary>
    /// <remarks>
    /// If you want to know more about the article, you can use this command to get extensive information about the article. Some feeds provide extra arguments to the article to make getting information even more vital to those who need it.
    /// <br></br>
    /// It shows you the article title, the link, the description, and the extra arguments and their values, if available.
    /// </remarks>
    class RSS_ArticleInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            int ArticleIndex = (int)Math.Round(Convert.ToDouble(parameters.ArgumentsList[0]) - 1d);
            if (ArticleIndex > RSSShellCommon.RSSFeedInstance.FeedArticles.Count - 1)
            {
                TextWriterColor.Write(Translate.DoTranslation("Article number couldn't be bigger than the available articles."), true, KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "Tried to access article number {0}, but count is {1}.", ArticleIndex, RSSShellCommon.RSSFeedInstance.FeedArticles.Count - 1);
                return 10000 + (int)KernelExceptionType.RSSShell;
            }
            else
            {
                var Article = RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex];
                TextWriterColor.Write("- " + Translate.DoTranslation("Title:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(Article.ArticleTitle, true, KernelColorType.ListValue);
                TextWriterColor.Write("- " + Translate.DoTranslation("Link:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(Article.ArticleLink, true, KernelColorType.ListValue);
                foreach (string Variable in Article.ArticleVariables.Keys)
                {
                    if (!(Variable == "title") & !(Variable == "link") & !(Variable == "summary") & !(Variable == "description") & !(Variable == "content"))
                    {
                        TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, Variable);
                        TextWriterColor.Write(Article.ArticleVariables[Variable].InnerText, true, KernelColorType.ListValue);
                    }
                }
                TextWriterColor.Write(CharManager.NewLine + Article.ArticleDescription);
                return 0;
            }
        }

    }
}
