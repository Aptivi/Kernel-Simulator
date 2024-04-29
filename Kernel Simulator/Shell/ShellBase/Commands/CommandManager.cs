﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Misc.Editors.HexEdit;
using KS.Misc.Editors.JsonShell;
using KS.Misc.Editors.TextEdit;
using KS.Misc.RarFile;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.ZipFile;
using KS.Network.FTP;
using KS.Network.HTTP;
using KS.Network.Mail;
using KS.Network.RemoteDebug;
using KS.Network.RSS;
using KS.Network.SFTP;
using KS.Shell.ShellBase.Shells;
using KS.TestShell;

namespace KS.Shell.ShellBase.Commands
{
    public static class CommandManager
    {

        /// <summary>
        /// Checks to see if the command is found in selected shell command type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type</param>
        /// <returns>True if found; False if not found or shell type is invalid.</returns>
        public static bool IsCommandFound(string Command, ShellType ShellType)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Command: {0}, ShellType: {1}", Command, ShellType);
            switch (ShellType)
            {
                case ShellType.FTPShell:
                    {
                        return FTPShellCommon.FTPCommands.ContainsKey(Command);
                    }
                case ShellType.JsonShell:
                    {
                        return JsonShellCommon.JsonShell_Commands.ContainsKey(Command);
                    }
                case ShellType.MailShell:
                    {
                        return MailShellCommon.MailCommands.ContainsKey(Command);
                    }
                case ShellType.RemoteDebugShell:
                    {
                        return RemoteDebugCmd.DebugCommands.ContainsKey(Command);
                    }
                case ShellType.RSSShell:
                    {
                        return RSSShellCommon.RSSCommands.ContainsKey(Command);
                    }
                case ShellType.SFTPShell:
                    {
                        return SFTPShellCommon.SFTPCommands.ContainsKey(Command);
                    }
                case ShellType.Shell:
                    {
                        return Shell.Commands.ContainsKey(Command);
                    }
                case ShellType.TestShell:
                    {
                        return TestShellCommon.Test_Commands.ContainsKey(Command);
                    }
                case ShellType.TextShell:
                    {
                        return TextEditShellCommon.TextEdit_Commands.ContainsKey(Command);
                    }
                case ShellType.ZIPShell:
                    {
                        return ZipShellCommon.ZipShell_Commands.ContainsKey(Command);
                    }
                case ShellType.HTTPShell:
                    {
                        return HTTPShellCommon.HTTPCommands.ContainsKey(Command);
                    }
                case ShellType.HexShell:
                    {
                        return HexEditShellCommon.HexEdit_Commands.ContainsKey(Command);
                    }
                case ShellType.RARShell:
                    {
                        return RarShellCommon.RarShell_Commands.ContainsKey(Command);
                    }

                default:
                    {
                        return false;
                    }
            }
        }

        /// <summary>
        /// Checks to see if the command is found in all the shells
        /// </summary>
        /// <param name="Command">A command</param>
        /// <returns>True if found; False if not found.</returns>
        public static bool IsCommandFound(string Command)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Command: {0}", Command);
            return FTPShellCommon.FTPCommands.ContainsKey(Command) | JsonShellCommon.JsonShell_Commands.ContainsKey(Command) | MailShellCommon.MailCommands.ContainsKey(Command) | RemoteDebugCmd.DebugCommands.ContainsKey(Command) | RSSShellCommon.RSSCommands.ContainsKey(Command) | SFTPShellCommon.SFTPCommands.ContainsKey(Command) | Shell.Commands.ContainsKey(Command) | TestShellCommon.Test_Commands.ContainsKey(Command) | TextEditShellCommon.TextEdit_Commands.ContainsKey(Command) | ZipShellCommon.ZipShell_Commands.ContainsKey(Command) | HTTPShellCommon.HTTPCommands.ContainsKey(Command) | HexEditShellCommon.HexEdit_Commands.ContainsKey(Command) | RarShellCommon.RarShell_Commands.ContainsKey(Command);
        }

    }
}