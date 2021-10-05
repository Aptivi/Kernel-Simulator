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

Class FTP_TypeCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String) Implements ICommand.Execute
        If ListArgs(0).ToLower = "a" Then
            ClientFTP.DownloadDataType = FtpDataType.ASCII
            ClientFTP.ListingDataType = FtpDataType.ASCII
            ClientFTP.UploadDataType = FtpDataType.ASCII
            W(DoTranslation("Data type set to ASCII!"), True, ColTypes.Success)
            W(DoTranslation("Beware that most files won't download or upload properly using this mode, so we highly recommend using the Binary mode on most situations."), True, ColTypes.Warning)
        ElseIf ListArgs(0).ToLower = "b" Then
            ClientFTP.DownloadDataType = FtpDataType.Binary
            ClientFTP.ListingDataType = FtpDataType.Binary
            ClientFTP.UploadDataType = FtpDataType.Binary
            W(DoTranslation("Data type set to Binary!"), True, ColTypes.Success)
        Else
            W(DoTranslation("Invalid data type."), True, ColTypes.Error)
        End If
    End Sub

End Class