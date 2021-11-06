﻿
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.Text.RegularExpressions

Class SearchCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        Try
            Dim Matches As List(Of String) = SearchFileForStringRegexp(ListArgs(1), New Regex(ListArgs(0), RegexOptions.IgnoreCase))
            For Each Match As String In Matches
                Write(Match, True, ColTypes.Neutral)
            Next
        Catch ex As Exception
            Wdbg(DebugLevel.E, "Error trying to search {0} for {1}", ListArgs(0), ListArgs(1))
            WStkTrc(ex)
            Write(DoTranslation("Searching {0} for {1} failed.") + " {2}", True, ColTypes.Error, ListArgs(0), ListArgs(1), ex.Message)
        End Try
    End Sub

End Class