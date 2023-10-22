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

using KS.ConsoleBase.Colors;
using KS.Misc.Text;
using Syndian.Instance;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Writers.ConsoleWriters;

namespace KS.Shell.Shells.RSS.Commands
{
    /// <summary>
    /// Lists articles
    /// </summary>
    /// <remarks>
    /// If you want to get articles found in the current RSS feed, you can use this command.
    /// </remarks>
    class ListCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            foreach (RSSArticle Article in RSSShellCommon.RSSFeedInstance.FeedArticles)
            {
                TextWriterColor.WriteKernelColor("- {0}: ", false, KernelColorType.ListEntry, Article.ArticleTitle);
                TextWriterColor.WriteKernelColor(Article.ArticleLink, true, KernelColorType.ListValue);
                TextWriterColor.Write("    {0}", Article.ArticleDescription.SplitNewLines()[0].Truncate(200));
            }
            return 0;
        }

    }
}
