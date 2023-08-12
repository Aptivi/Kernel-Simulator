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

Imports KS.Network.Mail.Transfer
Imports KS.Network.Mail

Namespace Shell.Shells
    Public Class MailShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.MailShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            'Send ping to keep the connection alive
            Dim IMAP_NoOp As New KernelThread("IMAP Keep Connection", False, AddressOf IMAPKeepConnection)
            IMAP_NoOp.Start()
            Wdbg(DebugLevel.I, "Made new thread about IMAPKeepConnection()")
            If Not Mail_UsePop3 Then
                Dim SMTP_NoOp As New KernelThread("SMTP Keep Connection", False, AddressOf SMTPKeepConnection)
                SMTP_NoOp.Start()
                Wdbg(DebugLevel.I, "Made new thread about SMTPKeepConnection()")
            Else
#If POP3Feature Then
                Dim POP3_NoOp As New KernelThread("POP3 Keep Connection", False, AddressOf POP3KeepConnection)
                POP3_NoOp.Start()
                Wdbg(DebugLevel.I, "Made new thread about POP3KeepConnection()")
#End If
            End If

            'Add handler for IMAP and SMTP
            SwitchCancellationHandler(ShellType.MailShell)
            KernelEventManager.RaiseIMAPShellInitialized()

            While Not Bail
                SyncLock MailCancelSync
                    'Populate messages
                    PopulateMessages()
                    If Mail_NotifyNewMail Then InitializeHandlers()

                    'Initialize prompt
                    If DefConsoleOut IsNot Nothing Then
                        Console.SetOut(DefConsoleOut)
                    End If
                    Wdbg(DebugLevel.I, "MailShellPromptStyle = {0}", MailShellPromptStyle)
                    If MailShellPromptStyle = "" Then
                        TextWriterColor.Write("[", False, ColTypes.Gray) : TextWriterColor.Write("{0}", False, ColTypes.UserName, Mail_Authentication.UserName) : TextWriterColor.Write("|", False, ColTypes.Gray) : TextWriterColor.Write("{0}", False, ColTypes.HostName, Mail_Authentication.UserName) : TextWriterColor.Write("] ", False, ColTypes.Gray) : TextWriterColor.Write("{0} > ", False, ColTypes.Gray, IMAP_CurrentDirectory) : TextWriterColor.Write("", False, InputColor)
                    Else
                        Dim ParsedPromptStyle As String = ProbePlaces(MailShellPromptStyle)
                        ParsedPromptStyle.ConvertVTSequences
                        TextWriterColor.Write(ParsedPromptStyle, False, ColTypes.Gray) : TextWriterColor.Write("", False, InputColor)
                    End If

                    'Listen for a command
                    Dim cmd As String = Console.ReadLine
                    KernelEventManager.RaiseIMAPPreExecuteCommand(cmd)
                    GetLine(cmd, False, "", ShellType.MailShell)
                    KernelEventManager.RaiseIMAPPostExecuteCommand(cmd)
                End SyncLock
            End While

            'Disconnect the session
            IMAP_CurrentDirectory = "Inbox"
            If KeepAlive Then
                Wdbg(DebugLevel.W, "Exit requested, but not disconnecting.")
            Else
                Wdbg(DebugLevel.W, "Exit requested. Disconnecting host...")
                If Mail_NotifyNewMail Then ReleaseHandlers()
                IMAP_Client.Disconnect(True)
                SMTP_Client.Disconnect(True)
#If POP3Feature Then
                POP3_Client.Disconnect(True)
#End If
            End If

            'Restore handler
            SwitchCancellationHandler(LastShellType)
        End Sub

    End Class
End Namespace
