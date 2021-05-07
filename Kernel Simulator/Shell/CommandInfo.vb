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

Public Class CommandInfo

    ''' <summary>
    ''' The command
    ''' </summary>
    Public ReadOnly Property Command As String
    ''' <summary>
    ''' The type of command
    ''' </summary>
    Public ReadOnly Property Type As ShellCommandType
    ''' <summary>
    ''' Does the command require arguments?
    ''' </summary>
    Public ReadOnly Property ArgumentsRequired As Boolean
    ''' <summary>
    ''' User must specify at least this number of arguments
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property MinimumArguments As Integer
    ''' <summary>
    ''' Is the command admin-only?
    ''' </summary>
    Public ReadOnly Property Strict As Boolean
    ''' <summary>
    ''' Is the command wrappable?
    ''' </summary>
    Public ReadOnly Property Wrappable As Boolean
    ''' <summary>
    ''' If true, the command can't be run in maintenance mode
    ''' </summary>
    Public ReadOnly Property NoMaintenance As Boolean
    ''' <summary>
    ''' Is the command obsolete?
    ''' </summary>
    Public ReadOnly Property Obsolete As Boolean

    ''' <summary>
    ''' Installs a new instance of command info class
    ''' </summary>
    ''' <param name="Command">Command</param>
    ''' <param name="Type">Shell command type</param>
    ''' <param name="ArgumentsRequired">Does the command require arguments?</param>
    ''' <param name="MinimumArguments">User must specify at least this number of arguments</param>
    ''' <param name="Strict">Is the command admin-only?</param>
    ''' <param name="Wrappable">Is the command wrappable?</param>
    ''' <param name="NoMaintenance">If true, the command can't be run in maintenance mode</param>
    ''' <param name="Obsolete">Is the command obsolete?</param>
    Public Sub New(ByVal Command As String, ByVal Type As ShellCommandType, ByVal ArgumentsRequired As Boolean, ByVal MinimumArguments As Integer, Strict As Boolean, Wrappable As Boolean, NoMaintenance As Boolean, Obsolete As Boolean)
        Me.Command = Command
        Me.Type = Type
        Me.ArgumentsRequired = ArgumentsRequired
        Me.MinimumArguments = MinimumArguments
        Me.Strict = Strict
        Me.Wrappable = Wrappable
        Me.NoMaintenance = NoMaintenance
        Me.Obsolete = Obsolete
    End Sub

End Class

Public Module CommandType

    ''' <summary>
    ''' Shell command type enumeration
    ''' </summary>
    Public Enum ShellCommandType
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
        ''' Remote debug shell
        ''' </summary>
        RemoteDebugShell
        ''' <summary>
        ''' ZIP shell
        ''' </summary>
        ZIPShell
    End Enum

End Module