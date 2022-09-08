﻿
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using FluentFTP;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.FTP.Commands;

namespace KS.Shell.Shells.FTP
{
    /// <summary>
    /// Common FTP shell class
    /// </summary>
    public static class FTPShellCommon
    {

        /// <summary>
        /// FTP commands
        /// </summary>
        public readonly static Dictionary<string, CommandInfo> FTPCommands = new()
        {
            { "connect", new CommandInfo("connect", ShellType.FTPShell, "Connects to an FTP server (it must start with \"ftp://\" or \"ftps://\")", new CommandArgumentInfo(new[] { "<server>" }, true, 1), new FTP_ConnectCommand()) },
            { "cdl", new CommandInfo("cdl", ShellType.FTPShell, "Changes local directory to download to or upload from", new CommandArgumentInfo(new[] { "<directory>" }, true, 1), new FTP_CdlCommand()) },
            { "cdr", new CommandInfo("cdr", ShellType.FTPShell, "Changes remote directory to download from or upload to", new CommandArgumentInfo(new[] { "<directory>" }, true, 1), new FTP_CdrCommand()) },
            { "cp", new CommandInfo("cp", ShellType.FTPShell, "Copies file or directory to another file or directory.", new CommandArgumentInfo(new[] { "<sourcefileordir> <targetfileordir>" }, true, 2), new FTP_CpCommand()) },
            { "del", new CommandInfo("del", ShellType.FTPShell, "Deletes remote file from server", new CommandArgumentInfo(new[] { "<file>" }, true, 1), new FTP_DelCommand()) },
            { "disconnect", new CommandInfo("disconnect", ShellType.FTPShell, "Disconnects from server", new CommandArgumentInfo(new[] { "[-f]" }, false, 0), new FTP_DisconnectCommand()) },
            { "execute", new CommandInfo("execute", ShellType.FTPShell, "Executes an FTP server command", new CommandArgumentInfo(new[] { "<command>" }, true, 1), new FTP_ExecuteCommand()) },
            { "get", new CommandInfo("get", ShellType.FTPShell, "Downloads remote file to local directory using binary or text", new CommandArgumentInfo(new[] { "<file> [output]" }, true, 1), new FTP_GetCommand()) },
            { "getfolder", new CommandInfo("getfolder", ShellType.FTPShell, "Downloads remote folder to local directory using binary or text", new CommandArgumentInfo(new[] { "<folder> [outputfolder]" }, true, 1), new FTP_GetFolderCommand()) },
            { "info", new CommandInfo("info", ShellType.FTPShell, "FTP server information", new CommandArgumentInfo(), new FTP_InfoCommand()) },
            { "lsl", new CommandInfo("lsl", ShellType.FTPShell, "Lists local directory", new CommandArgumentInfo(new[] { "[-showdetails|-suppressmessages] [dir]" }, false, 0), new FTP_LslCommand()) },
            { "lsr", new CommandInfo("lsr", ShellType.FTPShell, "Lists remote directory", new CommandArgumentInfo(new[] { "[-showdetails] [dir]" }, false, 0), new FTP_LsrCommand()) },
            { "mv", new CommandInfo("mv", ShellType.FTPShell, "Moves file or directory to another file or directory. You can also use that to rename files.", new CommandArgumentInfo(new[] { "<sourcefileordir> <targetfileordir>" }, true, 2), new FTP_MvCommand()) },
            { "put", new CommandInfo("put", ShellType.FTPShell, "Uploads local file to remote directory using binary or text", new CommandArgumentInfo(new[] { "<file> [output]" }, true, 1), new FTP_PutCommand()) },
            { "putfolder", new CommandInfo("putfolder", ShellType.FTPShell, "Uploads local folder to remote directory using binary or text", new CommandArgumentInfo(new[] { "<folder> [outputfolder]" }, true, 1), new FTP_PutFolderCommand()) },
            { "pwdl", new CommandInfo("pwdl", ShellType.FTPShell, "Gets current local directory", new CommandArgumentInfo(), new FTP_PwdlCommand()) },
            { "pwdr", new CommandInfo("pwdr", ShellType.FTPShell, "Gets current remote directory", new CommandArgumentInfo(), new FTP_PwdrCommand()) },
            { "perm", new CommandInfo("perm", ShellType.FTPShell, "Sets file permissions. This is supported only on FTP servers that run Unix.", new CommandArgumentInfo(new[] { "<file> <permnumber>" }, true, 2), new FTP_PermCommand()) },
            { "quickconnect", new CommandInfo("quickconnect", ShellType.FTPShell, "Uses information from Speed Dial to connect to any network quickly", new CommandArgumentInfo(), new FTP_QuickConnectCommand()) },
            { "sumfile", new CommandInfo("sumfile", ShellType.FTPShell, "Calculates file sums.", new CommandArgumentInfo(new[] { "<file> <MD5/SHA1/SHA256/SHA512/CRC>" }, true, 2), new FTP_SumFileCommand()) },
            { "sumfiles", new CommandInfo("sumfiles", ShellType.FTPShell, "Calculates sums of files in specified directory.", new CommandArgumentInfo(new[] { "<file> <MD5/SHA1/SHA256/SHA512/CRC>" }, true, 2), new FTP_SumFilesCommand()) },
            { "type", new CommandInfo("type", ShellType.FTPShell, "Sets the type for this session", new CommandArgumentInfo(new[] { "<a/b>" }, true, 1), new FTP_TypeCommand()) }
        };
        /// <summary>
        /// FTP current local directory
        /// </summary>
        public static string FtpCurrentDirectory;
        /// <summary>
        /// FTP current remote directory
        /// </summary>
        public static string FtpCurrentRemoteDir;
        /// <summary>
        /// FTP show details in list
        /// </summary>
        public static bool FtpShowDetailsInList = true;
        /// <summary>
        /// FTP user prompt style
        /// </summary>
        public static string FtpUserPromptStyle = "";
        /// <summary>
        /// FTP password prompt style
        /// </summary>
        public static string FtpPassPromptStyle = "";
        /// <summary>
        /// FTP always use first profile
        /// </summary>
        public static bool FtpUseFirstProfile;
        /// <summary>
        /// FTP add new connections to speed dial
        /// </summary>
        public static bool FtpNewConnectionsToSpeedDial = true;
        /// <summary>
        /// FTP try to validate the certificate
        /// </summary>
        public static bool FtpTryToValidateCertificate = true;
        /// <summary>
        /// FTP recursive hashing
        /// </summary>
        public static bool FtpRecursiveHashing;
        /// <summary>
        /// FTP show MOTD
        /// </summary>
        public static bool FtpShowMotd = true;
        /// <summary>
        /// FTP always accept invalid certificates. Turning it on is not recommended.
        /// </summary>
        public static bool FtpAlwaysAcceptInvalidCerts;
        /// <summary>
        /// FTP verify retry attempts
        /// </summary>
        public static int FtpVerifyRetryAttempts = 3;
        /// <summary>
        /// FTP connection timeout in milliseconds
        /// </summary>
        public static int FtpConnectTimeout = 15000;
        /// <summary>
        /// FTP data connection timeout in milliseconds
        /// </summary>
        public static int FtpDataConnectTimeout = 15000;
        /// <summary>
        /// FTP protocol versions
        /// </summary>
        public static FtpIpVersion FtpProtocolVersions = FtpIpVersion.ANY;
        internal static FtpClient _clientFTP;
        internal static bool FtpConnected;
        internal static string FtpSite;
        internal static string FtpPass;
        internal static string FtpUser;
        internal readonly static Dictionary<string, CommandInfo> FTPModCommands = new();

        /// <summary>
        /// The FTP client used to connect to the FTP server
        /// </summary>
        public static FtpClient ClientFTP
        {
            get
            {
                return _clientFTP;
            }
        }

    }
}
