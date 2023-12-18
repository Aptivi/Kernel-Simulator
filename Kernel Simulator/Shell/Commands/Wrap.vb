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
                    Dim AltThreads As List(Of KernelThread) = ShellStack(ShellStack.Count - 1).AltCommandThreads
                    If AltThreads.Count = 0 OrElse AltThreads(AltThreads.Count - 1).IsAlive Then
                        Dim WrappedCommand As New KernelThread($"Wrapped Shell Command Thread", False, AddressOf ExecuteCommand)
                        ShellStack(ShellStack.Count - 1).AltCommandThreads.Add(WrappedCommand)
                    End If
                    GetLine(ListArgs(0), False, WrapOutputPath)
                    Dim WrapOutputStream As New StreamReader(WrapOutputPath)
                    Dim WrapOutput As String = WrapOutputStream.ReadToEnd
                    WriteWrapped(WrapOutput, False, GetConsoleColor(ColTypes.Neutral))
                    If Not WrapOutput.EndsWith(NewLine) Then WritePlain("", True)
                    WrapOutputStream.Close()
                    File.Delete(WrapOutputPath)
                Else
                    Dim WrappableCmds As New ArrayList
                    For Each CommandInfo As CommandInfo In Shell.Commands.Values
                        If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
                    Next
                    Write(DoTranslation("The command is not wrappable. These commands are wrappable:") + " {0}", True, color:=GetConsoleColor(ColTypes.Error), String.Join(", ", WrappableCmds.ToArray))
                End If
            Else
                Write(DoTranslation("The wrappable command is not found."), True, GetConsoleColor(ColTypes.Error))
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            'Get wrappable commands
            Dim WrappableCmds As New ArrayList
            For Each CommandInfo As CommandInfo In Shell.Commands.Values
                If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
            Next

            'Print them along with help description
            Write(DoTranslation("Wrappable commands:") + " {0}", True, color:=GetConsoleColor(ColTypes.Neutral), String.Join(", ", WrappableCmds.ToArray))
        End Sub

    End Class
End Namespace
