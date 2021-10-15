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

Public Module CancellationInstallers

    ''' <summary>
    ''' Switches the command cancellation handler for the shell
    ''' </summary>
    ''' <param name="ShellType">Target shell type</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <remarks>This is the workaround for a bug in .NET Framework regarding <see cref="Console.CancelKeyPress"/> event. More info can be found below:<br>
    ''' </br><see href="https://stackoverflow.com/a/22717063/6688914">Deep explanation of the bug</see></remarks>
    Public Function SwitchCancellationHandler(ShellType As ShellCommandType) As Boolean
        LastShellType = CurrentShellType
        Select Case ShellType
            Case ShellCommandType.FTPShell
                AddHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
            Case ShellCommandType.JsonShell
                AddHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
            Case ShellCommandType.MailShell
                AddHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
            Case ShellCommandType.RSSShell
                AddHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
            Case ShellCommandType.SFTPShell
                AddHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
            Case ShellCommandType.Shell
                AddHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
            Case ShellCommandType.TestShell
                AddHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
            Case ShellCommandType.TextShell
                AddHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
            Case ShellCommandType.ZIPShell
                AddHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
            Case ShellCommandType.HTTPShell
                AddHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
            Case Else
                Return False
        End Select
        CurrentShellType = ShellType
        Return True
    End Function

End Module
