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

Imports KS.Network

Namespace Shell.Commands
    Class PutCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim RetryCount As Integer = 1
            Dim FileName As String = NeutralizePath(ListArgs(0))
            Dim URL As String = ListArgs(1)
            Wdbg(DebugLevel.I, "URL: {0}", URL)
            While Not RetryCount > UploadRetries
                Try
                    If Not (URL.StartsWith("ftp://") Or URL.StartsWith("ftps://") Or URL.StartsWith("ftpes://")) Then
                        If Not URL.StartsWith(" ") Then
                            Dim Credentials As New NetworkCredential
                            If ListArgs.Length > 2 Then 'Username specified
                                Credentials.UserName = ListArgs(2)
                                TextWriterColor.Write(DoTranslation("Enter password: "), False, ColTypes.Input)
                                Credentials.Password = ReadLineNoInput()
                                Console.WriteLine()
                            End If
                            TextWriterColor.Write(DoTranslation("Uploading {0} to {1}..."), True, ColTypes.Neutral, FileName, URL)
                            If UploadFile(FileName, URL, Credentials) Then
                                TextWriterColor.Write(DoTranslation("Upload has completed."), True, ColTypes.Neutral)
                            End If
                        Else
                            TextWriterColor.Write(DoTranslation("Specify the address"), True, ColTypes.Error)
                        End If
                    Else
                        TextWriterColor.Write(DoTranslation("Please use ""ftp"" if you are going to upload files to the FTP server."), True, ColTypes.Error)
                    End If
                    Exit Sub
                Catch ex As Exception
                    UFinish = False
                    TextWriterColor.Write(DoTranslation("Upload failed in try {0}: {1}"), True, ColTypes.Error, RetryCount, ex.Message)
                    RetryCount += 1
                    Wdbg(DebugLevel.I, "Try count: {0}", RetryCount)
                    WStkTrc(ex)
                End Try
            End While
        End Sub

    End Class
End Namespace