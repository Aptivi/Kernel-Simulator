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

using System;
using System.Threading;
using Extensification.StringExts;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.FTP
{
    /// <summary>
    /// The FTP shell
    /// </summary>
    public class FTPShell : ShellExecutor, IShell
    {

        /// <inheritdoc/>
        public override ShellType ShellType
        {
            get
            {
                return ShellType.FTPShell;
            }
        }

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            string FtpCommand;
            bool Connects = ShellArgs.Length > 0;
            string Address = "";
            if (Connects)
                Address = Convert.ToString(ShellArgs[0]);

            // Populate FTP current directory
            FTPShellCommon.FtpCurrentDirectory = Paths.HomePath;

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    // Check if the shell is going to exit
                    if (Bail)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Exiting shell...");
                        FTPShellCommon.FtpConnected = false;
                        FTPShellCommon.ClientFTP?.Disconnect();
                        FTPShellCommon.FtpSite = "";
                        FTPShellCommon.FtpCurrentDirectory = "";
                        FTPShellCommon.FtpCurrentRemoteDir = "";
                        FTPShellCommon.FtpUser = "";
                        FTPShellCommon.FtpPass = "";
                        return;
                    }

                    // Raise event
                    Kernel.Kernel.KernelEventManager.RaiseFTPShellInitialized();

                    // See UESHShell.cs for more info
                    lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                    {
                        // Prompt for command
                        if (!Connects)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Preparing prompt...");
                            PromptPresetManager.WriteShellPrompt(ShellType);
                        }
                    }

                    // Try to connect if IP address is specified.
                    if (Connects)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, $"Currently connecting to {Address} by \"ftp (address)\"...");
                        FtpCommand = $"connect {Address}";
                        Connects = false;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Normal shell");
                        FtpCommand = Input.ReadLine();
                    }

                    // Parse command
                    if ((string.IsNullOrEmpty(FtpCommand) | (FtpCommand?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                    {
                        Kernel.Kernel.KernelEventManager.RaiseFTPPreExecuteCommand(FtpCommand);
                        Shell.GetLine(FtpCommand, "", ShellType.FTPShell);
                        Kernel.Kernel.KernelEventManager.RaiseFTPPostExecuteCommand(FtpCommand);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new Kernel.Exceptions.FTPShellException(Translate.DoTranslation("There was an error in the FTP shell:") + " {0}", ex, ex.Message);
                }
            }
        }

    }
}
