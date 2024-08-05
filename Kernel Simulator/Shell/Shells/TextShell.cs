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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Editors.TextEdit;
using KS.Misc.Text;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells
{
    public class TextShell : ShellExecutor, IShell
    {

        public override ShellType ShellType => ShellType.TextShell;

        public override bool Bail { get; set; }

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
                TextWriters.Write(Translate.DoTranslation("File not specified. Exiting shell..."), true, KernelColorTools.ColTypes.Error);
                Bail = true;
            }

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    // Open file if not open
                    if (TextEditShellCommon.TextEdit_FileStream is null)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "File not open yet. Trying to open {0}...", FilePath);
                        if (!TextEditTools.TextEdit_OpenTextFile(FilePath))
                        {
                            TextWriters.Write(Translate.DoTranslation("Failed to open file. Exiting shell..."), true, KernelColorTools.ColTypes.Error);
                            Bail = true;
                            break;
                        }
                        TextEditShellCommon.TextEdit_AutoSave.Start();
                    }

                    // See UESHShell.vb for more info
                    lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                    {
                        // Prepare for prompt
                        if (Kernel.Kernel.DefConsoleOut is not null)
                        {
                            Console.SetOut(Kernel.Kernel.DefConsoleOut);
                        }
                        PromptPresetManager.WriteShellPrompt(ShellType);

                        // Raise the event
                        Kernel.Kernel.KernelEventManager.RaiseTextShellInitialized();
                    }

                    // Prompt for command
                    string WrittenCommand = Input.ReadLine();
                    if ((string.IsNullOrEmpty(WrittenCommand) | (WrittenCommand?.StartsWithAnyOf([" ", "#"]))) == false)
                    {
                        Kernel.Kernel.KernelEventManager.RaiseTextPreExecuteCommand(WrittenCommand);
                        Shell.GetLine(WrittenCommand, false, "", ShellType.TextShell);
                        Kernel.Kernel.KernelEventManager.RaiseTextPostExecuteCommand(WrittenCommand);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    TextWriters.Write(Translate.DoTranslation("There was an error in the shell.") + Kernel.Kernel.NewLine + "Error {0}: {1}", true, KernelColorTools.ColTypes.Error, ex.GetType().FullName, ex.Message);
                    continue;
                }
            }

            // Close file
            TextEditTools.TextEdit_CloseTextFile();
            TextEditShellCommon.TextEdit_AutoSave.Stop();
        }

    }
}