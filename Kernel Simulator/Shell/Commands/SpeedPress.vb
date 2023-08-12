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

Imports KS.Misc.Games
Imports KS.Misc.Reflection

Namespace Shell.Commands
    Class SpeedPressCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Difficulty As SpeedPressDifficulty = SpeedPressDifficulty.Medium
            Dim CustomTimeout As Integer = SpeedPressTimeout
            If ListSwitchesOnly.Contains("-e") Then Difficulty = SpeedPressDifficulty.Easy
            If ListSwitchesOnly.Contains("-m") Then Difficulty = SpeedPressDifficulty.Medium
            If ListSwitchesOnly.Contains("-h") Then Difficulty = SpeedPressDifficulty.Hard
            If ListSwitchesOnly.Contains("-v") Then Difficulty = SpeedPressDifficulty.VeryHard
            If ListSwitchesOnly.Contains("-c") And ListArgsOnly.Count > 0 AndAlso IsStringNumeric(ListArgsOnly(0)) Then
                Difficulty = SpeedPressDifficulty.Custom
                CustomTimeout = ListArgsOnly(0)
            End If
            InitializeSpeedPress(Difficulty, CustomTimeout)
        End Sub

        Public Overrides Sub HelpHelper()
            TextWriterColor.Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            TextWriterColor.Write("  -e: ", False, ColTypes.ListEntry) : TextWriterColor.Write(DoTranslation("Starts the game in easy difficulty"), True, ColTypes.ListValue)
            TextWriterColor.Write("  -m: ", False, ColTypes.ListEntry) : TextWriterColor.Write(DoTranslation("Starts the game in medium difficulty"), True, ColTypes.ListValue)
            TextWriterColor.Write("  -h: ", False, ColTypes.ListEntry) : TextWriterColor.Write(DoTranslation("Starts the game in hard difficulty"), True, ColTypes.ListValue)
            TextWriterColor.Write("  -v: ", False, ColTypes.ListEntry) : TextWriterColor.Write(DoTranslation("Starts the game in very hard difficulty"), True, ColTypes.ListValue)
            TextWriterColor.Write("  -c: ", False, ColTypes.ListEntry) : TextWriterColor.Write(DoTranslation("Starts the game in custom difficulty. Please note that the custom timeout in milliseconds should be written as argument."), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace