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

Namespace Network.FTP.Commands
    Class FTP_TypeCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs(0).ToLower = "a" Then
                ClientFTP.Config.DownloadDataType = FtpDataType.ASCII
                ClientFTP.Config.ListingDataType = FtpDataType.ASCII
                ClientFTP.Config.UploadDataType = FtpDataType.ASCII
                TextWriterColor.Write(DoTranslation("Data type set to ASCII!"), True, ColTypes.Success)
                TextWriterColor.Write(DoTranslation("Beware that most files won't download or upload properly using this mode, so we highly recommend using the Binary mode on most situations."), True, ColTypes.Warning)
            ElseIf ListArgs(0).ToLower = "b" Then
                ClientFTP.Config.DownloadDataType = FtpDataType.Binary
                ClientFTP.Config.ListingDataType = FtpDataType.Binary
                ClientFTP.Config.UploadDataType = FtpDataType.Binary
                TextWriterColor.Write(DoTranslation("Data type set to Binary!"), True, ColTypes.Success)
            Else
                TextWriterColor.Write(DoTranslation("Invalid data type."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace