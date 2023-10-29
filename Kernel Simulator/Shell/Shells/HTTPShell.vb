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

Imports KS.Network.HTTP

Namespace Shell.Shells
    Public Class HTTPShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.HTTPShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            While Not Bail
                SyncLock HTTPCancelSync
                    Try
                        'Prompt for command
                        If DefConsoleOut IsNot Nothing Then
                            Console.SetOut(DefConsoleOut)
                        End If
                        Wdbg(DebugLevel.I, "Preparing prompt...")
                        If HTTPConnected Then
                            Wdbg(DebugLevel.I, "HTTPShellPromptStyle = {0}", HTTPShellPromptStyle)
                            If HTTPShellPromptStyle = "" Then
                                TextWriterColor.Write("[", False, ColTypes.Gray) : TextWriterColor.Write("{0}", False, ColTypes.HostName, HTTPSite) : TextWriterColor.Write("]> ", False, ColTypes.Gray) : TextWriterColor.Write("", False, InputColor)
                            Else
                                Dim ParsedPromptStyle As String = ProbePlaces(HTTPShellPromptStyle)
                                TextWriterColor.Write(ParsedPromptStyle, False, ColTypes.Gray) : TextWriterColor.Write("", False, InputColor)
                            End If
                        Else
                            TextWriterColor.Write("> ", False, ColTypes.Gray) : TextWriterColor.Write("", False, InputColor)
                        End If

                        'Prompt for command
                        Wdbg(DebugLevel.I, "Normal shell")
                        Dim HttpCommand As String = Console.ReadLine()
                        KernelEventManager.RaiseHTTPPreExecuteCommand(HttpCommand)

                        'Parse command
                        If Not (HttpCommand = Nothing Or HttpCommand?.StartsWithAnyOf({" ", "#"})) Then
                            GetLine(HttpCommand, False, "", ShellType.HTTPShell)
                            KernelEventManager.RaiseHTTPPostExecuteCommand(HttpCommand)
                        End If
                    Catch ex As Exception
                        WStkTrc(ex)
                        Throw New Exceptions.HTTPShellException(DoTranslation("There was an error in the HTTP shell:") + " {0}", ex, ex.Message)
                    End Try
                End SyncLock
            End While

            'Exiting, so reset the site
            HTTPSite = ""
        End Sub

    End Class
End Namespace
