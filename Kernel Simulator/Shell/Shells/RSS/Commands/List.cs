﻿
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.RSS.Instance;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.RSS.Commands
{
    /// <summary>
    /// Lists articles
    /// </summary>
    /// <remarks>
    /// If you want to get articles found in the current RSS feed, you can use this command.
    /// </remarks>
    class RSS_ListCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            foreach (RSSArticle Article in RSSShellCommon.RSSFeedInstance.FeedArticles)
            {
                TextWriterColor.Write("- {0}: ", false, ColorTools.ColTypes.ListEntry, Article.ArticleTitle);
                TextWriterColor.Write(Article.ArticleLink, true, ColorTools.ColTypes.ListValue);
                TextWriterColor.Write("    {0}", true, ColorTools.ColTypes.Neutral, Article.ArticleDescription.SplitNewLines()[0].Truncate(200));
            }
        }

    }
}
