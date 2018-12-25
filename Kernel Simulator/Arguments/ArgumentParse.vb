﻿
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module ArgumentParse

    'Variables
    Public arguser As String                    'A username
    Public argword As String                    'A password
    Public argcommands As String                'Commands entered
    Public argcmds() As String

    Public Sub ParseArguments()

        'Check for the arguments written by the user
        Try
            BootArgs = answerargs.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
            For i As Integer = 0 To BootArgs.Count - 1
                Dim indexArg As Integer = BootArgs(i).IndexOf(" ")
                If (indexArg = -1) Then
                    indexArg = BootArgs(i).Count
                    BootArgs(i) = BootArgs(i).Substring(0, indexArg)
                End If
                If (AvailableArgs.Contains(BootArgs(i).Substring(0, indexArg))) Then
                    If (BootArgs(i) = "nohwprobe") Then

                        'Disables automatic hardware probing.
                        ProbeFlag = False

                    ElseIf (BootArgs(i) = "quiet") Then

                        Quiet = True

                    ElseIf (BootArgs(i).Contains("cmdinject")) Then

                        'Command Injector argument
                        If (BootArgs(i) = "cmdinject") Then
                            W(DoTranslation("Available commands: {0}", currentLang) + vbNewLine +
                              DoTranslation("Write command:", currentLang) + " ", "input", String.Join(", ", availableCommands)) 'TODO: The extra + " " will be fixed in the final release
                            argcmds = System.Console.ReadLine().Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
                            argcommands = String.Join(", ", argcmds)
                            If (argcommands <> "q") Then
                                CommandFlag = True
                            Else
                                Wln(DoTranslation("Command injection has been cancelled.", currentLang), "neutralText")
                            End If
                        Else
                            argcmds = BootArgs(i).Substring(10).Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
                            argcommands = String.Join(", ", argcmds)
                            CommandFlag = True
                        End If

                    ElseIf (BootArgs(i) = "debug") Then

                        DebugMode = True

                    ElseIf (BootArgs(i) = "maintenance") Then

                        maintenance = True

                    ElseIf (BootArgs(i) = "help") Then

                        Wln(DoTranslation("Separate boot arguments with commas without spaces, for example, 'motd,gpuprobe'", currentLang) + vbNewLine +
                            DoTranslation("Separate commands on 'cmdinject' with colons with spaces, for example, 'cmdinject setthemes Hacker : beep 1024 0.5'", currentLang) + vbNewLine +
                            DoTranslation("Note that the 'debug' argument does not fully cover the kernel.", currentLang), "neutralText")
                        answerargs = "" : argsFlag = False : argsInjected = False
                        ArgumentPrompt.PromptArgs()
                        If (argsFlag = True) Then
                            ArgumentParse.ParseArguments()
                        End If

                    End If
                Else
                    Wln(DoTranslation("bargs: The requested argument {0} is not found.", currentLang), "neutralText", BootArgs(i).Substring(0, indexArg))
                End If
            Next
        Catch ex As Exception
            KernelError(CChar("U"), True, 5, DoTranslation("bargs: Unrecoverable error in argument: ", currentLang) + Err.Description)
        End Try

    End Sub

End Module
