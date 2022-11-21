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
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.Admin
{
    /// <summary>
    /// The admin shell
    /// </summary>
    public class AdminShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "AdminShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            while (!Bail)
            {
                try
                {
                    // Prompt for the command
                    Shell.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(Translate.DoTranslation("Error in administrative shell: {0}"), true, ColorTools.ColTypes.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Error: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

    }
}
