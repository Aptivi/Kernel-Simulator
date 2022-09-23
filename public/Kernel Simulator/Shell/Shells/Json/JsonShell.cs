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
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Editors.JsonShell;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.Json
{
    /// <summary>
    /// The JSON editor shell
    /// </summary>
    public class JsonShell : ShellExecutor, IShell
    {

        /// <inheritdoc/>
        public override ShellType ShellType => ShellType.JsonShell;

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Get file path
            string FilePath = "";
            if (ShellArgs.Length > 0)
            {
                FilePath = Convert.ToString(ShellArgs[0]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File not specified. Exiting shell..."), true, ColorTools.ColTypes.Error);
                Bail = true;
            }

            while (!Bail)
            {
                // Open file if not open
                if (JsonShellCommon.JsonShell_FileStream is null)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", FilePath);
                    if (!JsonTools.JsonShell_OpenJsonFile(FilePath))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Failed to open file. Exiting shell..."), true, ColorTools.ColTypes.Error);
                        break;
                    }
                    JsonShellCommon.JsonShell_AutoSave.Start();
                }

                // See UESHShell.cs for more info
                lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                {
                    // Prepare for prompt
                    PromptPresetManager.WriteShellPrompt(ShellType);

                    // Raise the event
                    Kernel.Kernel.KernelEventManager.RaiseJsonShellInitialized();
                }

                // Prompt for command
                string WrittenCommand = Input.ReadLine();
                if ((string.IsNullOrEmpty(WrittenCommand) | (WrittenCommand?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                {
                    Kernel.Kernel.KernelEventManager.RaiseJsonPreExecuteCommand(WrittenCommand);
                    Shell.GetLine(WrittenCommand, "", ShellType.JsonShell);
                    Kernel.Kernel.KernelEventManager.RaiseJsonPostExecuteCommand(WrittenCommand);
                }
            }

            // Close file
            JsonTools.JsonShell_CloseTextFile();
            JsonShellCommon.JsonShell_AutoSave.Stop();
        }

    }
}
