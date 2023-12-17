﻿
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports KS.TimeDate

Namespace Shell.Commands
    Class ShowTdZoneCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ShowAll As Boolean
            If ListSwitchesOnly.Contains("-all") Then ShowAll = True
            If ShowAll Then
                ShowAllTimeZones()
            Else
                If Not ShowTimeZones(ListArgsOnly(0)) Then Write(DoTranslation("Timezone is specified incorrectly."), True, GetConsoleColor(ColTypes.Error))
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, GetConsoleColor(ColTypes.Neutral))
            Write("  -all: ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(DoTranslation("Shows all the time zones"), True, GetConsoleColor(ColTypes.ListValue))
        End Sub

    End Class
End Namespace