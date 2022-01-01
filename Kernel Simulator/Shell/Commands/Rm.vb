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

Imports System.IO

Class RmCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        For Each Path As String In ListArgs
            Dim NeutPath As String = NeutralizePath(Path)
            If FileExists(NeutPath) Then
                Wdbg(DebugLevel.I, "{0} is a file. Removing...", Path)
                RemoveFile(Path)
            ElseIf FolderExists(NeutPath) Then
                Wdbg(DebugLevel.I, "{0} is a folder. Removing...", Path)
                RemoveDirectory(Path)
            Else
                Wdbg(DebugLevel.W, "Trying to remove {0} which is not found.", Path)
                Write(DoTranslation("Can't remove {0} because it doesn't exist."), True, ColTypes.Error, Path)
            End If
        Next
    End Sub

End Class