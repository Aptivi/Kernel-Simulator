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
Imports System.Threading
Imports MimeKit

Module MailGetCommand

    Public MailStartCommandThread As New Thread(AddressOf Mail_ExecuteCommand) With {.Name = "Mail Command Thread"}

    ''' <summary>
    ''' Parses and executes the specified mail command
    ''' </summary>
    ''' <param name="cmd">A command. It may come with arguments</param>
    Sub Mail_ExecuteCommand(cmd As String)
        'Variables
        Dim RequiredArgsProvided As Boolean = True

        'Get command and arguments
        Dim index As Integer = cmd.IndexOf(" ")
        If index = -1 Then index = cmd.Length
        Dim words = cmd.Split({" "c})
        Dim strArgs As String = cmd.Substring(index)
        If Not index = cmd.Length Then strArgs = strArgs.Substring(1)

        'Parse arguments
        Dim FullArgsLQ() As String = strArgs.SplitEncloseDoubleQuotes(" ")
        If FullArgsLQ IsNot Nothing Then
            RequiredArgsProvided = FullArgsLQ?.Length >= FTPCommands(words(0)).MinimumArguments
        ElseIf FTPCommands(words(0)).ArgumentsRequired And FullArgsLQ Is Nothing Then
            RequiredArgsProvided = False
        End If

        Try
            Select Case words(0)
                Case "cd"
                    If RequiredArgsProvided Then
                        MailChangeDirectory(FullArgsLQ(0))
                    End If
                Case "list"
                    If FullArgsLQ?.Count > 0 Then
                        Wdbg("I", "Page is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            Write(MailListMessages(FullArgsLQ(0)), False, ColTypes.Neutral)
                        Else
                            Write(DoTranslation("Page is not a numeric value."), True, ColTypes.Error)
                        End If
                    Else
                        Write(MailListMessages(1), False, ColTypes.Neutral)
                    End If
                Case "lsdirs"
                    Write(MailListDirectories, False, ColTypes.Neutral)
                Case "read"
                    If RequiredArgsProvided Then
                        Wdbg("I", "Message number is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            MailPrintMessage(FullArgsLQ(0))
                        Else
                            Write(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "readenc"
                    If RequiredArgsProvided Then
                        Wdbg("I", "Message number is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            MailPrintMessage(FullArgsLQ(0), True)
                        Else
                            Write(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "send", "sendenc"
                    Dim Receiver, Subject As String
                    Dim Body As New BodyBuilder

                    'Prompt for receiver e-mail address
                    Write(DoTranslation("Enter recipient mail address:") + " ", False, ColTypes.Input)
                    Receiver = Console.ReadLine
                    Wdbg("I", "Recipient: {0}", Receiver)

                    'Check for mail format
                    If Receiver.Contains("@") And Receiver.Substring(Receiver.IndexOf("@")).Contains(".") Then
                        Wdbg("I", "Mail format satisfied. Contains ""@"" and contains ""."" in the second part after the ""@"" symbol.")

                        'Prompt for subject
                        Write(DoTranslation("Enter the subject:") + " ", False, ColTypes.Input)
                        Subject = Console.ReadLine
                        Wdbg("I", "Subject: {0} ({1} chars)", Subject, Subject.Length)

                        'Prompt for body
                        Write(DoTranslation("Enter your message below. Write ""EOF"" to confirm."), True, ColTypes.Input)
                        Dim BodyLine As String = ""
                        While Not BodyLine.ToUpper = "EOF"
                            BodyLine = Console.ReadLine
                            If Not BodyLine.ToUpper = "EOF" Then
                                Wdbg("I", "Body line: {0} ({1} chars)", BodyLine, BodyLine.Length)
                                Body.TextBody += BodyLine + vbNewLine
                                Wdbg("I", "Body length: {0} chars", Body.TextBody.Length)
                            End If
                        End While

                        Write(DoTranslation("Enter file paths to attachments. Press ENTER on a blank path to confirm."), True, ColTypes.Neutral)
                        Dim PathLine As String = " "
                        While Not PathLine = ""
                            Write("> ", False, ColTypes.Input)
                            PathLine = Console.ReadLine
                            If Not PathLine = "" Then
                                PathLine = NeutralizePath(PathLine)
                                Wdbg("I", "Path line: {0} ({1} chars)", PathLine, PathLine.Length)
                                If File.Exists(PathLine) Then
                                    Body.Attachments.Add(PathLine)
                                End If
                            End If
                        End While

                        'Send the message
                        Write(DoTranslation("Sending message..."), True, ColTypes.Neutral)
                        If cmd = "sendenc" Then
                            If MailSendEncryptedMessage(Receiver, Subject, Body.ToMessageBody) Then
                                Wdbg("I", "Message sent.")
                                Write(DoTranslation("Message sent."), True, ColTypes.Neutral)
                            Else
                                Wdbg("E", "See debug output to find what's wrong.")
                                Write(DoTranslation("Error sending message."), True, ColTypes.Error)
                            End If
                        Else
                            If MailSendMessage(Receiver, Subject, Body.ToMessageBody) Then
                                Wdbg("I", "Message sent.")
                                Write(DoTranslation("Message sent."), True, ColTypes.Neutral)
                            Else
                                Wdbg("E", "See debug output to find what's wrong.")
                                Write(DoTranslation("Error sending message."), True, ColTypes.Error)
                            End If
                        End If
                    Else
                        Wdbg("E", "Mail format unsatisfied. " + Receiver)
                        Write(DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:") + " john.s@example.com", True, ColTypes.Error)
                    End If
                Case "rm"
                    If RequiredArgsProvided Then
                        Wdbg("I", "Message number is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            MailRemoveMessage(FullArgsLQ(0))
                        Else
                            Write(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "rmall"
                    If RequiredArgsProvided Then
                        If MailRemoveAllBySender(FullArgsLQ(0)) Then
                            Write(DoTranslation("All mail made by {0} are removed successfully."), True, ColTypes.Neutral, FullArgsLQ(0))
                        Else
                            Write(DoTranslation("Failed to remove all mail made by {0}."), True, ColTypes.Neutral, FullArgsLQ(0))
                        End If
                    End If
                Case "mv"
                    If RequiredArgsProvided Then
                        Wdbg("I", "Message number is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            MailMoveMessage(FullArgsLQ(0), FullArgsLQ(1))
                        Else
                            Write(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "mvall"
                    If RequiredArgsProvided Then
                        If MailMoveAllBySender(FullArgsLQ(0), FullArgsLQ(1)) Then
                            Write(DoTranslation("All mail made by {0} are moved successfully."), True, ColTypes.Neutral, FullArgsLQ(0))
                        Else
                            Write(DoTranslation("Failed to move all mail made by {0}."), True, ColTypes.Neutral, FullArgsLQ(0))
                        End If
                    End If
                Case "mkdir"
                    If RequiredArgsProvided Then
                        CreateMailDirectory(FullArgsLQ(0))
                    End If
                Case "rmdir"
                    If RequiredArgsProvided Then
                        DeleteMailDirectory(FullArgsLQ(0))
                    End If
                Case "ren"
                    If RequiredArgsProvided Then
                        RenameMailDirectory(FullArgsLQ(0), FullArgsLQ(1))
                    End If
                Case "help"
                    If FullArgsLQ?.Length > 0 Then
                        IMAPShowHelp(FullArgsLQ(0))
                    Else
                        IMAPShowHelp()
                    End If
                Case "exit"
                    ExitRequested = True
                    Write(DoTranslation("Do you want to keep connected?") + " <y/n> ", False, ColTypes.Input)
                    Dim Answer As Char = Console.ReadKey.KeyChar
                    Console.WriteLine()
                    If Answer = "y" Then
                        KeepAlive = True
                    ElseIf Answer = "n" Then
                        KeepAlive = False
                    Else
                        Write(DoTranslation("Invalid choice. Assuming no..."), True, ColTypes.Input)
                    End If
            End Select

            If MailCommands(words(0)).ArgumentsRequired And Not RequiredArgsProvided Then
                Write(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Error, words(0))
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", words(0), FullArgsLQ?.Count)
                IMAPShowHelp(words(0))
            End If
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception
            EventManager.RaiseIMAPCommandError(cmd, ex)
            Write(DoTranslation("Error executing mail command: {0}"), True, ColTypes.Error, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

    Sub MailCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            MailStartCommandThread.Abort()
        End If
    End Sub

End Module
