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

Namespace Shell.Commands
    Class WrapCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim CommandToBeWrapped As String = ListArgs(0).Split(" ")(0)
            If Shell.Commands.ContainsKey(CommandToBeWrapped) Then
                If Shell.Commands(CommandToBeWrapped).Wrappable Then
                    Dim WrapOutputPath As String = TempPath + "/wrapoutput.txt"
                    GetLine(ListArgs(0), False, WrapOutputPath)
                    Dim WrapOutputStream As New StreamReader(WrapOutputPath)
                    Dim WrapOutput As String = WrapOutputStream.ReadToEnd
                    WriteWrapped(WrapOutput, False, ColTypes.Neutral)
                    If Not WrapOutput.EndsWith(vbNewLine) Then Console.WriteLine()
                    WrapOutputStream.Close()
                    File.Delete(WrapOutputPath)
                Else
                    Dim WrappableCmds As New ArrayList
                    For Each CommandInfo As CommandInfo In Shell.Commands.Values
                        If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
                    Next
                    Write(DoTranslation("The command is not wrappable. These commands are wrappable:") + " {0}", True, ColTypes.Error, String.Join(", ", WrappableCmds.ToArray))
                End If
            Else
                Write(DoTranslation("The wrappable command is not found."), True, ColTypes.Error)
            End If
        End Sub

        Public Sub HelpHelper()
            'Get wrappable commands
            Dim WrappableCmds As New ArrayList
            For Each CommandInfo As CommandInfo In Shell.Commands.Values
                If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
            Next

            'Print them along with help description
            Write(DoTranslation("Wrappable commands:") + " {0}", True, ColTypes.Neutral, String.Join(", ", WrappableCmds.ToArray))
        End Sub

    End Class
End Namespace