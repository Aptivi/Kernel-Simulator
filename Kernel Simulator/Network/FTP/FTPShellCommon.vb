
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

'TODO: Unify GetLine as we're now moving to IShell to handle shells
Public Module FTPShellCommon

    Public ReadOnly FTPCommands As New Dictionary(Of String, CommandInfo) From {{"connect", New CommandInfo("connect", ShellType.FTPShell, "Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")", {"<server>"}, True, 1, New FTP_ConnectCommand)},
                                                                                {"cdl", New CommandInfo("cdl", ShellType.FTPShell, "Changes local directory to download to or upload from", {"<directory>"}, True, 1, New FTP_CdlCommand)},
                                                                                {"cdr", New CommandInfo("cdr", ShellType.FTPShell, "Changes remote directory to download from or upload to", {"<directory>"}, True, 1, New FTP_CdrCommand)},
                                                                                {"cp", New CommandInfo("cp", ShellType.FTPShell, "Copies file or directory to another file or directory.", {"<sourcefileordir> <targetfileordir>"}, True, 2, New FTP_CpCommand)},
                                                                                {"del", New CommandInfo("del", ShellType.FTPShell, "Deletes remote file from server", {"<file>"}, True, 1, New FTP_DelCommand)},
                                                                                {"disconnect", New CommandInfo("disconnect", ShellType.FTPShell, "Disconnects from server", {"[-f]"}, False, 0, New FTP_DisconnectCommand, False, False, False, False, False, New Action(AddressOf (New FTP_DisconnectCommand).HelpHelper))},
                                                                                {"execute", New CommandInfo("execute", ShellType.FTPShell, "Executes an FTP server command", {"<command>"}, True, 1, New FTP_ExecuteCommand)},
                                                                                {"exit", New CommandInfo("exit", ShellType.FTPShell, "Exits FTP shell and returns to kernel", {}, False, 0, New FTP_ExitCommand)},
                                                                                {"get", New CommandInfo("get", ShellType.FTPShell, "Downloads remote file to local directory using binary or text", {"<file> [output]"}, True, 1, New ZipShell_GetCommand)},
                                                                                {"getfolder", New CommandInfo("getfolder", ShellType.FTPShell, "Downloads remote folder to local directory using binary or text", {"<folder> [outputfolder]"}, True, 1, New FTP_GetFolderCommand)},
                                                                                {"help", New CommandInfo("help", ShellType.FTPShell, "Shows help screen", {"[command]"}, False, 0, New FTP_HelpCommand)},
                                                                                {"info", New CommandInfo("info", ShellType.FTPShell, "FTP server information", {}, False, 0, New FTP_InfoCommand)},
                                                                                {"lsl", New CommandInfo("lsl", ShellType.FTPShell, "Lists local directory", {"[-showdetails|-suppressmessages] [dir]"}, False, 0, New FTP_LslCommand, False, False, False, False, False, New Action(AddressOf (New FTP_LslCommand).HelpHelper))},
                                                                                {"lsr", New CommandInfo("lsr", ShellType.FTPShell, "Lists remote directory", {"[-showdetails] [dir]"}, False, 0, New FTP_LsrCommand, False, False, False, False, False, New Action(AddressOf (New FTP_LsrCommand).HelpHelper))},
                                                                                {"mv", New CommandInfo("mv", ShellType.FTPShell, "Moves file or directory to another file or directory. You can also use that to rename files.", {"<sourcefileordir> <targetfileordir>"}, True, 2, New FTP_MvCommand)},
                                                                                {"put", New CommandInfo("put", ShellType.FTPShell, "Uploads local file to remote directory using binary or text", {"<file> [output]"}, True, 1, New FTP_PutCommand)},
                                                                                {"putfolder", New CommandInfo("putfolder", ShellType.FTPShell, "Uploads local folder to remote directory using binary or text", {"<folder> [outputfolder]"}, True, 1, New FTP_PutFolderCommand)},
                                                                                {"pwdl", New CommandInfo("pwdl", ShellType.FTPShell, "Gets current local directory", {}, False, 0, New FTP_PwdlCommand)},
                                                                                {"pwdr", New CommandInfo("pwdr", ShellType.FTPShell, "Gets current remote directory", {}, False, 0, New FTP_PwdrCommand)},
                                                                                {"perm", New CommandInfo("perm", ShellType.FTPShell, "Sets file permissions. This is supported only on FTP servers that run Unix.", {"<file> <permnumber>"}, True, 2, New FTP_PermCommand)},
                                                                                {"quickconnect", New CommandInfo("quickconnect", ShellType.FTPShell, "Uses information from Speed Dial to connect to any network quickly", {}, False, 0, New FTP_QuickConnectCommand)},
                                                                                {"sumfile", New CommandInfo("sumfile", ShellType.FTPShell, "Calculates file sums.", {"<file> <MD5/SHA1/SHA256/SHA512/CRC>"}, True, 2, New FTP_SumFileCommand)},
                                                                                {"sumfiles", New CommandInfo("sumfiles", ShellType.FTPShell, "Calculates sums of files in specified directory.", {"<file> <MD5/SHA1/SHA256/SHA512/CRC>"}, True, 2, New FTP_SumFilesCommand)},
                                                                                {"type", New CommandInfo("type", ShellType.FTPShell, "Sets the type for this session", {"<a/b>"}, True, 1, New FTP_TypeCommand)}}
    Public FtpConnected As Boolean
    Public FtpSite As String
    Public FtpCurrentDirectory As String
    Public FtpCurrentRemoteDir As String
    Public FtpUser As String
    Public FTPModCommands As New ArrayList
    Public FTPShellPromptStyle As String = ""
    Public ClientFTP As FtpClient
    Public FtpShowDetailsInList As Boolean = True
    Public FtpUserPromptStyle As String = ""
    Public FtpPassPromptStyle As String = ""
    Public FtpUseFirstProfile As Boolean
    Public FtpNewConnectionsToSpeedDial As Boolean = True
    Public FtpTryToValidateCertificate As Boolean = True
    Public FtpRecursiveHashing As Boolean
    Public FtpShowMotd As Boolean = True
    Public FtpAlwaysAcceptInvalidCerts As Boolean
    Public FtpVerifyRetryAttempts As Integer = 3
    Public FtpConnectTimeout As Integer = 15000
    Public FtpDataConnectTimeout As Integer = 15000
    Public FtpProtocolVersions As FtpIpVersion = FtpIpVersion.ANY
    Friend FtpPass As String

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub FTPGetLine(FtpCommand As String)
        Dim words As String() = FtpCommand.SplitEncloseDoubleQuotes(" ")
        Wdbg(DebugLevel.I, $"Is the command found? {FTPCommands.ContainsKey(words(0))}")
        If FTPCommands.ContainsKey(words(0)) Then
            Wdbg(DebugLevel.I, "Command found.")
            Dim Params As New ExecuteCommandThreadParameters(FtpCommand, ShellType.FTPShell, Nothing)
            FTPStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "FTP Command Thread"}
            FTPStartCommandThread.Start(Params)
            FTPStartCommandThread.Join()
        ElseIf FTPModCommands.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "Mod command found.")
            ExecuteModCommand(FtpCommand)
        ElseIf FTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "FTP shell alias command found.")
            FtpCommand = FtpCommand.Replace($"""{words(0)}""", words(0))
            ExecuteFTPAlias(FtpCommand)
        ElseIf Not FtpCommand.StartsWith(" ") Then
            Wdbg(DebugLevel.E, "Command {0} not found.", FtpCommand)
            Write(DoTranslation("FTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on FTP shell."), True, ColTypes.Error, words(0))
        End If
    End Sub

End Module
