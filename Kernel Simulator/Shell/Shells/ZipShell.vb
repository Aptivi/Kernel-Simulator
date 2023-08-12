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
Imports System.IO.Compression
Imports KS.Misc.ZipFile

Namespace Shell.Shells
    Public Class ZipShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.ZIPShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            'Add handler for ZIP shell
            SwitchCancellationHandler(ShellType.ZIPShell)
            ZipShell_CurrentDirectory = CurrDir

            'Get file path
            Dim ZipFile As String = ""
            If ShellArgs.Length > 0 Then
                ZipFile = ShellArgs(0)
            Else
                TextWriterColor.Write(DoTranslation("File not specified. Exiting shell..."), True, ColTypes.Error)
                Bail = True
            End If

            While Not Bail
                SyncLock ZipShellCancelSync
                    'Open file if not open
                    If ZipShell_FileStream Is Nothing Then ZipShell_FileStream = New FileStream(ZipFile, FileMode.Open)
                    If ZipShell_ZipArchive Is Nothing Then ZipShell_ZipArchive = New ZipArchive(ZipShell_FileStream, ZipArchiveMode.Update, False)

                    'Prepare for prompt
                    If DefConsoleOut IsNot Nothing Then
                        Console.SetOut(DefConsoleOut)
                    End If
                    Wdbg(DebugLevel.I, "ZipShell_PromptStyle = {0}", ZipShell_PromptStyle)
                    If ZipShell_PromptStyle = "" Then
                        TextWriterColor.Write("[", False, ColTypes.Gray) : TextWriterColor.Write("{0}@{1}", False, ColTypes.UserName, ZipShell_CurrentArchiveDirectory, Path.GetFileName(ZipFile)) : TextWriterColor.Write("] > ", False, ColTypes.Gray) : TextWriterColor.Write("", False, InputColor)
                    Else
                        Dim ParsedPromptStyle As String = ProbePlaces(ZipShell_PromptStyle)
                        ParsedPromptStyle.ConvertVTSequences
                        TextWriterColor.Write(ParsedPromptStyle, False, ColTypes.Gray) : TextWriterColor.Write("", False, InputColor)
                    End If

                    'Prompt for command
                    KernelEventManager.RaiseZipShellInitialized()
                    Dim WrittenCommand As String = Console.ReadLine
                    KernelEventManager.RaiseZipPreExecuteCommand(WrittenCommand)
                    GetLine(WrittenCommand, False, "", ShellType.ZIPShell)
                    KernelEventManager.RaiseZipPostExecuteCommand(WrittenCommand)
                End SyncLock
            End While

            'Close file stream
            ZipShell_ZipArchive.Dispose()
            ZipShell_CurrentDirectory = ""
            ZipShell_CurrentArchiveDirectory = ""
            ZipShell_ZipArchive = Nothing
            ZipShell_FileStream = Nothing

            'Remove handler for ZIP shell
            SwitchCancellationHandler(LastShellType)
        End Sub

    End Class
End Namespace
