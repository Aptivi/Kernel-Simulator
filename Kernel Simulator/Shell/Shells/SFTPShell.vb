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

Imports KS.Kernel
Imports KS.Network.SFTP

Namespace Shell.Shells
    Public Class SFTPShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.SFTPShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            'Parse shell arguments
            Dim Connects As Boolean = ShellArgs.Length > 0
            Dim Address As String = ""
            If Connects Then Address = ShellArgs(0)

            'Actual shell logic
            Dim SFTPStrCmd As String = ""
            Dim SFTPInitialized As Boolean
            While Not Bail
                SyncLock SFTPCancelSync
                    Try
                        'Complete initialization
                        If SFTPInitialized = False Then
                            Wdbg(DebugLevel.I, $"Completing initialization of SFTP: {SFTPInitialized}")
                            SFTPCurrDirect = HomePath
                            Kernel.KernelEventManager.RaiseSFTPShellInitialized()
                            SwitchCancellationHandler(ShellType.SFTPShell)
                            SFTPInitialized = True
                        End If

                        'Check if the shell is going to exit
                        If Bail Then
                            Wdbg(DebugLevel.W, "Exiting shell...")
                            SFTPConnected = False
                            ClientSFTP?.Disconnect()
                            SFTPSite = ""
                            SFTPCurrDirect = ""
                            SFTPCurrentRemoteDir = ""
                            SFTPUser = ""
                            SFTPPass = ""
                            SFTPInitialized = False
                            SwitchCancellationHandler(LastShellType)
                            Exit Sub
                        End If

                        'Prompt for command
                        If DefConsoleOut IsNot Nothing Then
                            Console.SetOut(DefConsoleOut)
                        End If
                        If Not Connects Then
                            Wdbg(DebugLevel.I, "Preparing prompt...")
                            If SFTPConnected Then
                                Wdbg(DebugLevel.I, "SFTPShellPromptStyle = {0}", SFTPShellPromptStyle)
                                If SFTPShellPromptStyle = "" Then
                                    Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, SFTPUser) : Write("@", False, ColTypes.Gray) : Write("{0}", False, ColTypes.HostName, SFTPSite) : Write("]{0}> ", False, ColTypes.Gray, SFTPCurrentRemoteDir)
                                Else
                                    Dim ParsedPromptStyle As String = ProbePlaces(SFTPShellPromptStyle)
                                    ParsedPromptStyle.ConvertVTSequences
                                    Write(ParsedPromptStyle, False, ColTypes.Gray)
                                End If
                            Else
                                Write("{0}> ", False, ColTypes.Gray, SFTPCurrDirect)
                            End If
                        End If

                        'Set input color
                        SetInputColor()

                        'Try to connect if IP address is specified.
                        If Connects Then
                            Wdbg(DebugLevel.I, $"Currently connecting to {Address} by ""sftp (address)""...")
                            SFTPStrCmd = $"connect {Address}"
                            Connects = False
                        Else
                            Wdbg(DebugLevel.I, "Normal shell")
                            SFTPStrCmd = Console.ReadLine()
                        End If
                        Kernel.KernelEventManager.RaiseSFTPPreExecuteCommand(SFTPStrCmd)

                        'Parse command
                        If Not (SFTPStrCmd = Nothing Or SFTPStrCmd?.StartsWithAnyOf({" ", "#"})) Then
                            GetLine(SFTPStrCmd, False, "", ShellType.SFTPShell)
                            Kernel.KernelEventManager.RaiseSFTPPostExecuteCommand(SFTPStrCmd)
                        End If
                    Catch ex As Exception
                        WStkTrc(ex)
                        Throw New Exceptions.SFTPShellException(DoTranslation("There was an error in the SFTP shell:") + " {0}", ex, ex.Message)
                    End Try
                End SyncLock
            End While
        End Sub

    End Class
End Namespace
