﻿using System.Diagnostics;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

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

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Reports a bug
    /// </summary>
    /// <remarks>
    /// You can use this command to prepare your bug report to be sent to GitHub. You must be signed in to GitHub to be able to use this feature.
    /// </remarks>
    class ReportBugCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            TextWriterColor.Write(Translate.DoTranslation("Thank you for reporting a bug to us! Please follow the instructions on the screen."), true, ColorTools.ColTypes.Neutral);
            Process.Start("https://github.com/Aptivi/Kernel-Simulator/issues/new/choose");
        }

    }
}