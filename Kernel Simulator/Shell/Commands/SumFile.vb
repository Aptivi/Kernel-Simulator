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
Imports System.Text
Imports KS.Files.Querying
Imports KS.Misc.Encryption

Namespace Shell.Commands
    Class SumFileCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim file As String = NeutralizePath(ListArgsOnly(1))
            Dim out As String = ""
            Dim UseRelative As Boolean = ListSwitchesOnly.Contains("-relative")
            Dim FileBuilder As New StringBuilder
            If Not ListArgsOnly.Length < 3 Then
                out = NeutralizePath(ListArgsOnly(2))
            End If
            If FileExists(file) Then
                Dim AlgorithmEnum As Algorithms
                If ListArgsOnly(0) = "all" Then
                    For Each Algorithm As String In [Enum].GetNames(GetType(Algorithms))
                        AlgorithmEnum = [Enum].Parse(GetType(Algorithms), Algorithm)
                        Dim spent As New Stopwatch
                        spent.Start() 'Time when you're on a breakpoint is counted
                        Dim encrypted As String = GetEncryptedFile(file, AlgorithmEnum)
                        Write("{0} ({1})", True, color:=GetConsoleColor(ColTypes.Neutral), encrypted, AlgorithmEnum)
                        Write(DoTranslation("Time spent: {0} milliseconds"), True, color:=GetConsoleColor(ColTypes.Neutral), spent.ElapsedMilliseconds)
                        If UseRelative Then
                            FileBuilder.AppendLine($"- {ListArgsOnly(1)}: {encrypted} ({AlgorithmEnum})")
                        Else
                            FileBuilder.AppendLine($"- {file}: {encrypted} ({AlgorithmEnum})")
                        End If
                        spent.Stop()
                    Next
                ElseIf [Enum].TryParse(ListArgsOnly(0), AlgorithmEnum) Then
                    Dim spent As New Stopwatch
                    spent.Start() 'Time when you're on a breakpoint is counted
                    Dim encrypted As String = GetEncryptedFile(file, AlgorithmEnum)
                    Write(encrypted, True, GetConsoleColor(ColTypes.Neutral))
                    Write(DoTranslation("Time spent: {0} milliseconds"), True, color:=GetConsoleColor(ColTypes.Neutral), spent.ElapsedMilliseconds)
                    If UseRelative Then
                        FileBuilder.AppendLine($"- {ListArgsOnly(1)}: {encrypted} ({AlgorithmEnum})")
                    Else
                        FileBuilder.AppendLine($"- {file}: {encrypted} ({AlgorithmEnum})")
                    End If
                    spent.Stop()
                Else
                    Write(DoTranslation("Invalid encryption algorithm."), True, GetConsoleColor(ColTypes.Error))
                End If
                If Not out = "" Then
                    Dim FStream As New StreamWriter(out)
                    FStream.Write(FileBuilder.ToString)
                    FStream.Flush()
                End If
            Else
                Write(DoTranslation("{0} is not found."), True, color:=GetConsoleColor(ColTypes.Error), file)
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, GetConsoleColor(ColTypes.Neutral))
            Write("  -relative: ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(DoTranslation("Uses relative path instead of absolute"), True, GetConsoleColor(ColTypes.ListValue))
        End Sub

    End Class
End Namespace
