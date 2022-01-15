
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

Imports System.Threading

Public Module SFTPShellCommon

    Public ReadOnly SFTPCommands As New Dictionary(Of String, CommandInfo) From {{"connect", New CommandInfo("connect", ShellType.SFTPShell, "Connects to an SFTP server (it must start with ""sftp://"")", {"<server>"}, True, 1, New SFTP_ConnectCommand)},
                                                                                 {"cdl", New CommandInfo("cdl", ShellType.SFTPShell, "Changes local directory to download to or upload from", {"<directory>"}, True, 1, New SFTP_CdlCommand)},
                                                                                 {"cdr", New CommandInfo("cdr", ShellType.SFTPShell, "Changes remote directory to download from or upload to", {"<directory>"}, True, 1, New SFTP_CdrCommand)},
                                                                                 {"del", New CommandInfo("del", ShellType.SFTPShell, "Deletes remote file from server", {"<file>"}, True, 1, New SFTP_DelCommand)},
                                                                                 {"disconnect", New CommandInfo("disconnect", ShellType.SFTPShell, "Disconnects from server", {}, False, 0, New SFTP_DisconnectCommand)},
                                                                                 {"exit", New CommandInfo("exit", ShellType.SFTPShell, "Exits SFTP shell and returns to kernel", {}, False, 0, New SFTP_ExitCommand)},
                                                                                 {"get", New CommandInfo("get", ShellType.SFTPShell, "Downloads remote file to local directory using binary or text", {"<file>"}, True, 1, New SFTP_GetCommand)},
                                                                                 {"help", New CommandInfo("help", ShellType.SFTPShell, "Shows help screen", {}, False, 0, New SFTP_HelpCommand)},
                                                                                 {"lsl", New CommandInfo("lsl", ShellType.SFTPShell, "Lists local directory", {"[-showdetails|-suppressmessages] [dir]"}, False, 0, New SFTP_LslCommand, False, False, False, False, False, New Action(AddressOf (New SFTP_LslCommand).HelpHelper))},
                                                                                 {"lsr", New CommandInfo("lsr", ShellType.SFTPShell, "Lists remote directory", {"[-showdetails] [dir]"}, False, 0, New SFTP_LsrCommand, False, False, False, False, False, New Action(AddressOf (New SFTP_LsrCommand).HelpHelper))},
                                                                                 {"put", New CommandInfo("put", ShellType.SFTPShell, "Uploads local file to remote directory using binary or text", {"<file>"}, True, 1, New SFTP_PutCommand)},
                                                                                 {"pwdl", New CommandInfo("pwdl", ShellType.SFTPShell, "Gets current local directory", {}, False, 0, New SFTP_PwdlCommand)},
                                                                                 {"pwdr", New CommandInfo("pwdr", ShellType.SFTPShell, "Gets current remote directory", {}, False, 0, New SFTP_PwdrCommand)},
                                                                                 {"quickconnect", New CommandInfo("quickconnect", ShellType.SFTPShell, "Uses information from Speed Dial to connect to any network quickly", {}, False, 0, New SFTP_QuickConnectCommand)}}
    Public SFTPConnected As Boolean
    Public SFTPSite As String
    Public SFTPCurrDirect As String
    Public SFTPCurrentRemoteDir As String
    Public SFTPUser As String
    Public SFTPModCommands As New ArrayList
    Public SFTPShellPromptStyle As String = ""
    Public SFTPShowDetailsInList As Boolean = True
    Public SFTPUserPromptStyle As String = ""
    Public SFTPNewConnectionsToSpeedDial As Boolean = True
    Public ClientSFTP As SftpClient
    Friend SFTPPass As String

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub SFTPGetLine(SFTPStrCmd As String)
        Dim words As String() = SFTPStrCmd.SplitEncloseDoubleQuotes(" ")
        Wdbg(DebugLevel.I, "Command: {0}", SFTPStrCmd)
        Wdbg(DebugLevel.I, $"Is the command found? {SFTPCommands.ContainsKey(words(0))}")
        If SFTPCommands.ContainsKey(words(0)) Then
            Wdbg(DebugLevel.I, "Command found.")
            Dim Params As New ExecuteCommandThreadParameters(SFTPStrCmd, ShellType.SFTPShell, Nothing)
            SFTPStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "SFTP Command Thread"}
            SFTPStartCommandThread.Start(Params)
            SFTPStartCommandThread.Join()
        ElseIf SFTPModCommands.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "Mod command found.")
            ExecuteModCommand(SFTPStrCmd)
        ElseIf SFTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "Aliased command found.")
            SFTPStrCmd = SFTPStrCmd.Replace($"""{words(0)}""", words(0))
            ExecuteSFTPAlias(SFTPStrCmd)
        ElseIf Not SFTPStrCmd.StartsWith(" ") Then
            Wdbg(DebugLevel.E, "Command {0} not found.", SFTPStrCmd)
            Write(DoTranslation("SFTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on SFTP shell."), True, ColTypes.Error, words(0))
        End If
    End Sub

End Module
