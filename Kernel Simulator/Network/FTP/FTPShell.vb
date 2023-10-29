
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

Imports System.Threading

Public Module FTPShell

    Public ReadOnly FTPCommands As New Dictionary(Of String, CommandInfo) From {{"connect", New CommandInfo("connect", ShellCommandType.FTPShell, DoTranslation("Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")"), True, 1)},
                                                                                {"cdl", New CommandInfo("cdl", ShellCommandType.FTPShell, DoTranslation("Changes local directory to download to or upload from"), True, 1)},
                                                                                {"cdr", New CommandInfo("cdr", ShellCommandType.FTPShell, DoTranslation("Changes remote directory to download from or upload to"), True, 1)},
                                                                                {"cp", New CommandInfo("cp", ShellCommandType.FTPShell, DoTranslation("Copies file or directory to another file or directory."), True, 2)},
                                                                                {"del", New CommandInfo("del", ShellCommandType.FTPShell, DoTranslation("Deletes remote file from server"), True, 1)},
                                                                                {"disconnect", New CommandInfo("disconnect", ShellCommandType.FTPShell, DoTranslation("Disconnects from server"), False, 0)},
                                                                                {"exit", New CommandInfo("exit", ShellCommandType.FTPShell, DoTranslation("Exits FTP shell and returns to kernel"), False, 0)},
                                                                                {"get", New CommandInfo("get", ShellCommandType.FTPShell, DoTranslation("Downloads remote file to local directory using binary or text"), True, 1)},
                                                                                {"help", New CommandInfo("help", ShellCommandType.FTPShell, DoTranslation("Shows help screen"), False, 0)},
                                                                                {"lsl", New CommandInfo("lsl", ShellCommandType.FTPShell, DoTranslation("Lists local directory"), False, 0)},
                                                                                {"lsr", New CommandInfo("lsr", ShellCommandType.FTPShell, DoTranslation("Lists remote directory"), False, 0)},
                                                                                {"put", New CommandInfo("put", ShellCommandType.FTPShell, DoTranslation("Uploads local file to remote directory using binary or text"), True, 1)},
                                                                                {"pwdl", New CommandInfo("pwdl", ShellCommandType.FTPShell, DoTranslation("Gets current local directory"), False, 0)},
                                                                                {"pwdr", New CommandInfo("pwdr", ShellCommandType.FTPShell, DoTranslation("Gets current remote directory"), False, 0)},
                                                                                {"mv", New CommandInfo("mv", ShellCommandType.FTPShell, DoTranslation("Moves file or directory to another file or directory. You can also use that to rename files."), True, 2)},
                                                                                {"perm", New CommandInfo("perm", ShellCommandType.FTPShell, DoTranslation("Sets file permissions. This is supported only on FTP servers that run Unix."), True, 2)},
                                                                                {"type", New CommandInfo("type", ShellCommandType.FTPShell, DoTranslation("Sets the type for this session"), True, 1)},
                                                                                {"quickconnect", New CommandInfo("quickconnect", ShellCommandType.FTPShell, DoTranslation("Uses information from Speed Dial to connect to any network quickly"), False, 0)}}
    Public connected As Boolean = False
    Private initialized As Boolean = False
    Public ftpsite As String
    Public currDirect As String 'Current Local Directory
    Public currentremoteDir As String 'Current Remote Directory
    Public user As String
    Friend pass As String
    Private strcmd As String
    Public ftpexit As Boolean = False
    Public FTPModCommands As New ArrayList
    Public FTPShellPromptStyle As String = ""

    ''' <summary>
    ''' Initializes the FTP shell
    ''' </summary>
    ''' <param name="Connects">Specifies whether the FTP client is currently connecting</param>
    ''' <param name="Address">An IP address</param>
    Public Sub InitiateShell(Optional Connects As Boolean = False, Optional Address As String = "")
        While True
            Try
                'Complete initialization
                If initialized = False Then
                    Wdbg("I", $"Completing initialization of FTP: {initialized}")
                    currDirect = paths("Home")
                    EventManager.RaiseFTPShellInitialized()

                    'This is the workaround for a bug in .NET Framework regarding Console.CancelKeyPress event. More info can be found below:
                    'https://stackoverflow.com/a/22717063/6688914
                    AddHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    initialized = True
                End If

                'Check if the shell is going to exit
                If ftpexit = True Then
                    Wdbg("W", "Exiting shell...")
                    connected = False
                    ClientFTP?.Disconnect()
                    ftpsite = ""
                    currDirect = ""
                    currentremoteDir = ""
                    user = ""
                    pass = ""
                    strcmd = ""
                    ftpexit = False
                    initialized = False
                    AddHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    Exit Sub
                End If

                'Prompt for command
                If DefConsoleOut IsNot Nothing Then
                    Console.SetOut(DefConsoleOut)
                End If
                If Not Connects Then
                    Wdbg("I", "Preparing prompt...")
                    If connected Then
                        Wdbg("I", "FTPShellPromptStyle = {0}", FTPShellPromptStyle)
                        If FTPShellPromptStyle = "" Then
                            Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, user) : Write("@", False, ColTypes.Gray) : Write("{0}", False, ColTypes.HostName, ftpsite) : Write("]{0}> ", False, ColTypes.Gray, currentremoteDir) : Write("", False, ColTypes.Input)
                        Else
                            Dim ParsedPromptStyle As String = ProbePlaces(FTPShellPromptStyle)
                            Write(ParsedPromptStyle, False, ColTypes.Gray) : Write("", False, ColTypes.Input)
                        End If
                    Else
                        Write("{0}> ", False, ColTypes.Gray, currDirect) : Write("", False, ColTypes.Input)
                    End If
                End If

                'Try to connect if IP address is specified.
                If Connects Then
                    Wdbg("I", $"Currently connecting to {Address} by ""ftp (address)""...")
                    strcmd = $"connect {Address}"
                    Connects = False
                Else
                    Wdbg("I", "Normal shell")
                    strcmd = Console.ReadLine()
                End If
                EventManager.RaiseFTPPreExecuteCommand(strcmd)

                'Parse command
                If Not (strcmd = Nothing Or strcmd?.StartsWithAnyOf({" ", "#"})) Then
                    FTPGetLine()
                    EventManager.RaiseFTPPostExecuteCommand(strcmd)
                End If

                'This is to fix race condition between FTP shell initialization and starting the event handler thread
                If strcmd Is Nothing Then
                    Thread.Sleep(30)
                End If
            Catch ex As Exception
                WStkTrc(ex)
                Throw New Exceptions.FTPShellException(DoTranslation("There was an error in the FTP shell:") + " {0}", ex, ex.Message)
            End Try
        End While
    End Sub

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub FTPGetLine()
        Dim words As String() = strcmd.SplitEncloseDoubleQuotes()
        Wdbg("I", $"Is the command found? {FTPCommands.ContainsKey(words(0))}")
        If FTPCommands.ContainsKey(words(0)) Then
            Wdbg("I", "Command found.")
            FTPStartCommandThread = New Thread(AddressOf FTPGetCommand.ExecuteCommand) With {.Name = "FTP Command Thread"}
            FTPStartCommandThread.Start(strcmd)
            FTPStartCommandThread.Join()
        ElseIf FTPModCommands.Contains(words(0)) Then
            Wdbg("I", "Mod command found.")
            ExecuteModCommand(strcmd)
        ElseIf FTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg("I", "FTP shell alias command found.")
            strcmd = strcmd.Replace($"""{words(0)}""", words(0))
            ExecuteFTPAlias(strcmd)
        ElseIf Not strcmd.StartsWith(" ") Then
            Wdbg("E", "Command {0} not found.", strcmd)
            Write(DoTranslation("FTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on FTP shell."), True, ColTypes.Error, words(0))
        End If
    End Sub

    ''' <summary>
    ''' Executes the FTP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteFTPAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes()(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, FTPShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        FTPStartCommandThread = New Thread(AddressOf FTPGetCommand.ExecuteCommand) With {.Name = "FTP Command Thread"}
        FTPStartCommandThread.Start(actualCmd)
        FTPStartCommandThread.Join()
    End Sub

End Module
