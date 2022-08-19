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

Imports KS.Network.FTP.Transfer

Namespace Shell.Shells.FTP.Commands
    ''' <summary>
    ''' Uploads the file to the server
    ''' </summary>
    ''' <remarks>
    ''' If you need to add your local files in your current working directory to the current working server directory, you must have administrative privileges to add them.
    ''' <br></br>
    ''' For example, if you're adding the picture of the New Delhi city using the PNG format, you need to upload it to the server for everyone to see. Assuming that it's "NewDelhi.PNG", use "put NewDelhi.PNG."
    ''' <br></br>
    ''' The authenticated user must have at least the administrative privileges before they can run the below commands.
    ''' </remarks>
    Class FTP_PutCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim LocalFile As String = ListArgsOnly(0)
            Dim RemoteFile As String = If(ListArgsOnly.Length > 1, ListArgsOnly(1), "")
            Write(DoTranslation("Uploading file {0}..."), False, ColTypes.Progress, ListArgsOnly(0))
            Dim Result As Boolean = If(Not String.IsNullOrWhiteSpace(LocalFile), FTPUploadFile(RemoteFile, LocalFile), FTPUploadFile(RemoteFile))
            If Result Then
                Console.WriteLine()
                Write(DoTranslation("Uploaded file {0}"), True, ColTypes.Success, LocalFile)
            Else
                Console.WriteLine()
                Write(DoTranslation("Failed to upload {0}"), True, ColTypes.Error, LocalFile)
            End If
        End Sub

    End Class
End Namespace
