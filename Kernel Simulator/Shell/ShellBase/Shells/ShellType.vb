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

Namespace Shell.ShellBase.Shells
    ''' <summary>
    ''' Shell type enumeration
    ''' </summary>
    Public Enum ShellType
        ''' <summary>
        ''' Normal UESH shell
        ''' </summary>
        Shell
        ''' <summary>
        ''' FTP shell
        ''' </summary>
        FTPShell
        ''' <summary>
        ''' Mail shell
        ''' </summary>
        MailShell
        ''' <summary>
        ''' SFTP shell
        ''' </summary>
        SFTPShell
        ''' <summary>
        ''' Text shell
        ''' </summary>
        TextShell
        ''' <summary>
        ''' Test shell
        ''' </summary>
        TestShell
        ''' <summary>
        ''' Remote debug shell
        ''' </summary>
        RemoteDebugShell
        ''' <summary>
        ''' ZIP shell
        ''' </summary>
        ZIPShell
        ''' <summary>
        ''' RSS Shell
        ''' </summary>
        RSSShell
        ''' <summary>
        ''' JSON Shell
        ''' </summary>
        JsonShell
        ''' <summary>
        ''' HTTP shell
        ''' </summary>
        HTTPShell
        ''' <summary>
        ''' Hex shell
        ''' </summary>
        HexShell
        ''' <summary>
        ''' RAR shell
        ''' </summary>
        RARShell
    End Enum
End Namespace
