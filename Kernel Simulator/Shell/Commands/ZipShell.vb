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

Namespace Shell.Commands
    Class ZipShellCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            ListArgs(0) = NeutralizePath(ListArgs(0))
            Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", ListArgs(0), FileExists(ListArgs(0)))
            If FileExists(ListArgs(0)) Then
                StartShell(ShellType.ZIPShell, ListArgs(0))
            Else
                TextWriterColor.Write(DoTranslation("File doesn't exist."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace