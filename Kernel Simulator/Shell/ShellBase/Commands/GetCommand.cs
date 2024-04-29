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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Editors.HexEdit;
using KS.Misc.Editors.JsonShell;
using KS.Misc.Editors.TextEdit;
using KS.Misc.RarFile;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.MiscWriters;
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
    public static class GetCommand
    {

        /// <summary>
        /// Thread parameters for ExecuteCommand()
        /// </summary>
        internal class ExecuteCommandThreadParameters
        {
            /// <summary>
            /// The requested command with arguments
            /// </summary>
            internal string RequestedCommand;
            /// <summary>
            /// The shell type
            /// </summary>
            internal ShellType ShellType;
            /// <summary>
            /// The debug device stream writer
            /// </summary>
            internal StreamWriter DebugDeviceSocket;

            internal ExecuteCommandThreadParameters(string RequestedCommand, ShellType ShellType, StreamWriter DebugDeviceSocket)
            {
                this.RequestedCommand = RequestedCommand;
                this.ShellType = ShellType;
                this.DebugDeviceSocket = DebugDeviceSocket;
            }
        }

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="ThreadParams">Thread parameters for ExecuteCommand.</param>
        internal static void ExecuteCommand(ExecuteCommandThreadParameters ThreadParams)
        {
            string RequestedCommand = ThreadParams.RequestedCommand;
            var ShellType = ThreadParams.ShellType;
            var DebugDeviceSocket = ThreadParams.DebugDeviceSocket;
            try
            {
                // Variables
                var ArgumentInfo = new ProvidedCommandArgumentsInfo(RequestedCommand, ShellType);
                string Command = ArgumentInfo.Command;
                string[] FullArgs = ArgumentInfo.FullArgumentsList;
                string[] Args = ArgumentInfo.ArgumentsList;
                string[] Switches = ArgumentInfo.SwitchesList;
                string StrArgs = ArgumentInfo.ArgumentsText;
                bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;
                var TargetCommands = Shell.Commands;

                // Set TargetCommands according to the shell type
                TargetCommands = GetCommands(ShellType);

                // Check to see if a requested command is obsolete
                if (TargetCommands[Command].Obsolete)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "The command requested {0} is obsolete", Command);
                    Decisive.DecisiveWrite(ShellType, DebugDeviceSocket, Translate.DoTranslation("This command is obsolete and will be removed in a future release."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                }

                // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                if (TargetCommands[Command].CommandArgumentInfo is not null)
                {
                    var ArgInfo = TargetCommands[Command].CommandArgumentInfo;
                    if (ArgInfo.ArgumentsRequired & RequiredArgumentsProvided | !ArgInfo.ArgumentsRequired)
                    {
                        var CommandBase = TargetCommands[Command].CommandBase;
                        CommandBase.Execute(StrArgs, FullArgs, Args, Switches);
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "User hasn't provided enough arguments for {0}", Command);
                        Decisive.DecisiveWrite(ShellType, DebugDeviceSocket, Translate.DoTranslation("There was not enough arguments. See below for usage:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                        HelpSystem.ShowHelp(Command, ShellType);
                    }
                }
                else
                {
                    var CommandBase = TargetCommands[Command].CommandBase;
                    CommandBase.Execute(StrArgs, FullArgs, Args, Switches);
                }
            }
            catch (ThreadInterruptedException)
            {
                Flags.CancelRequested = false;
                return;
            }
            catch (Exception ex)
            {
                Kernel.Kernel.KernelEventManager.RaiseCommandError(RequestedCommand, ex);
                DebugWriter.WStkTrc(ex);
                Decisive.DecisiveWrite(ShellType, DebugDeviceSocket, Translate.DoTranslation("Error trying to execute command") + " {2}." + Kernel.Kernel.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.GetType().FullName, ex.Message, RequestedCommand);
            }
        }

        /// <summary>
        /// Gets the command dictionary according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, CommandInfo> GetCommands(ShellType ShellType)
        {
            // Individual shells
            Dictionary<string, CommandInfo> FinalCommands;
            switch (ShellType)
            {
                case ShellType.FTPShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(FTPShellCommon.FTPCommands);
                        break;
                    }
                case ShellType.MailShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(MailShellCommon.MailCommands);
                        break;
                    }
                case ShellType.RemoteDebugShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(RemoteDebugCmd.DebugCommands);
                        break;
                    }
                case ShellType.RSSShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(RSSShellCommon.RSSCommands);
                        break;
                    }
                case ShellType.SFTPShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(SFTPShellCommon.SFTPCommands);
                        break;
                    }
                case ShellType.TestShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(TestShellCommon.Test_Commands);
                        break;
                    }
                case ShellType.TextShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(TextEditShellCommon.TextEdit_Commands);
                        break;
                    }
                case ShellType.ZIPShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(ZipShellCommon.ZipShell_Commands);
                        break;
                    }
                case ShellType.JsonShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(JsonShellCommon.JsonShell_Commands);
                        break;
                    }
                case ShellType.HTTPShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(HTTPShellCommon.HTTPCommands);
                        break;
                    }
                case ShellType.HexShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(HexEditShellCommon.HexEdit_Commands);
                        break;
                    }
                case ShellType.RARShell:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(RarShellCommon.RarShell_Commands);
                        break;
                    }

                default:
                    {
                        FinalCommands = new Dictionary<string, CommandInfo>(Shell.Commands);
                        break;
                    }
            }

            // Unified commands
            foreach (string UnifiedCommand in Shell.UnifiedCommandDict.Keys)
            {
                if (FinalCommands.ContainsKey(UnifiedCommand))
                {
                    FinalCommands[UnifiedCommand] = Shell.UnifiedCommandDict[UnifiedCommand];
                }
                else
                {
                    FinalCommands.Add(UnifiedCommand, Shell.UnifiedCommandDict[UnifiedCommand]);
                }
            }

            return FinalCommands;
        }

    }
}