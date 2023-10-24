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

Imports System.IO
Imports System.IO.Compression
Imports System.Threading

Public Module ZipGetCommand

    'Variables
    Public ZipShell_CommandThread As New Thread(AddressOf ZipShell_ParseCommand) With {.Name = "ZIP Shell Command Thread"}

    Sub ZipShell_ParseCommand(CommandText As String)
        Try
            'Indicator if required arguments are provided
            Dim RequiredArgumentsProvided As Boolean = True

            'Get the index of the first space
            Dim index As Integer = CommandText.IndexOf(" ")
            If index = -1 Then index = CommandText.Length
            Wdbg("I", "Index: {0}", index)

            'Get the String Of arguments
            Dim strArgs As String = CommandText.Substring(index)
            Wdbg("I", "Prototype strArgs: {0}", strArgs)
            If Not index = CommandText.Length Then strArgs = strArgs.Substring(1)
            Wdbg("I", "Finished strArgs: {0}", strArgs)

            'Separate between command and arguments specified
            Dim Command As String = CommandText.Split(" ")(0)
            Dim Arguments() As String = strArgs.SplitEncloseDoubleQuotes(" ")
            If Arguments IsNot Nothing Then
                RequiredArgumentsProvided = Arguments?.Length >= ZipShell_Commands(Command).MinimumArguments
            ElseIf ZipShell_Commands(Command).ArgumentsRequired And Arguments Is Nothing Then
                RequiredArgumentsProvided = False
            End If

            'Try to parse command
            Select Case Command
                Case "list"
                    Dim Entries As List(Of ZipArchiveEntry)
                    If Arguments?.Length > 0 Then
                        Wdbg("I", "Listing entries with {0} as target directory", Arguments(0))
                        Entries = ListZipEntries(Arguments(0))
                    Else
                        Wdbg("I", "Listing entries with current directory as target directory")
                        Entries = ListZipEntries(ZipShell_CurrentArchiveDirectory)
                    End If
                    For Each Entry As ZipArchiveEntry In Entries
                        Write("- {0}: ", False, ColTypes.ListEntry, Entry.FullName)
                        If Not Entry.Name = "" Then 'Entry is a file
                            Write("{0} ({1})", True, ColTypes.ListValue, Entry.CompressedLength.FileSizeToString, Entry.Length.FileSizeToString)
                        Else
                            Console.WriteLine()
                        End If
                    Next
                Case "get"
                    If RequiredArgumentsProvided Then
                        Dim Where As String = ""
                        Dim Absolute As Boolean
                        If Arguments?.Length > 1 Then
                            If Not Arguments(1) = "-absolute" Then Where = NeutralizePath(Arguments(1))
                            If Arguments?.Contains("-absolute") Then
                                Absolute = True
                            End If
                        End If
                        ExtractZipFileEntry(Arguments(0), Where, Absolute)
                    End If
                Case "chdir"
                    If RequiredArgumentsProvided Then
                        If Not ChangeWorkingZipLocalDirectory(Arguments(0)) Then
                            Write(DoTranslation("Directory {0} doesn't exist"), True, ColTypes.Error, Arguments(0))
                        End If
                    End If
                Case "chadir"
                    If RequiredArgumentsProvided Then
                        If Not ChangeWorkingArchiveDirectory(Arguments(0)) Then
                            Write(DoTranslation("Archive directory {0} doesn't exist"), True, ColTypes.Error, Arguments(0))
                        End If
                    End If
                Case "cdir"
                    Write(ZipShell_CurrentDirectory, True, ColTypes.Neutral)
                Case "exit"
                    ZipShell_Exiting = True
                Case "help"
                    If Arguments?.Length > 0 Then
                        Wdbg("I", "Requested help for {0}", Arguments(0))
                        ZipShell_GetHelp(Arguments(0))
                    Else
                        Wdbg("I", "Requested help for all commands")
                        ZipShell_GetHelp()
                    End If
            End Select

            'See if the command is done (passed all required arguments)
            If ZipShell_Commands(Command).ArgumentsRequired And Not RequiredArgumentsProvided Then
                Write(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Error, Command)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", Command, Arguments?.Length)
                ZipShell_GetHelp(Command)
            End If
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception
            Write(DoTranslation("Error trying to run command: {0}"), True, ColTypes.Error, ex.Message)
            Wdbg("E", "Error running command {0}: {1}", CommandText.Split(" ")(0), ex.Message)
            WStkTrc(ex)
            EventManager.RaiseZipCommandError(CommandText, ex)
        End Try
    End Sub

    Sub ZipShellCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            ZipShell_CommandThread.Abort()
        End If
    End Sub

End Module
