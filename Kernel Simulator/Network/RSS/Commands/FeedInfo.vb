﻿
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Namespace Network.RSS.Commands
    Class RSS_FeedInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            TextWriterColor.Write("- " + DoTranslation("Title:") + " ", False, ColTypes.ListEntry)
            TextWriterColor.Write(RSSFeedInstance.FeedTitle, True, ColTypes.ListValue)
            TextWriterColor.Write("- " + DoTranslation("Link:") + " ", False, ColTypes.ListEntry)
            TextWriterColor.Write(RSSFeedInstance.FeedUrl, True, ColTypes.ListValue)
            TextWriterColor.Write("- " + DoTranslation("Description:") + " ", False, ColTypes.ListEntry)
            TextWriterColor.Write(RSSFeedInstance.FeedDescription, True, ColTypes.ListValue)
            TextWriterColor.Write("- " + DoTranslation("Feed type:") + " ", False, ColTypes.ListEntry)
            TextWriterColor.Write(RSSFeedInstance.FeedType.ToString, True, ColTypes.ListValue)
            TextWriterColor.Write("- " + DoTranslation("Number of articles:") + " ", False, ColTypes.ListEntry)
            TextWriterColor.Write(RSSFeedInstance.FeedArticles.Count.ToString, True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace