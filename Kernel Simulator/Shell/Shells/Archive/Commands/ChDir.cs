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

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Archive;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Archive.Commands
{
    /// <summary>
    /// Changes current local directory
    /// </summary>
    /// <remarks>
    /// If you want to interact with the ZIP file in another local directory, you can use this command to change the current local directory. This change isn't applied to the main shell.
    /// </remarks>
    class ArchiveShell_ChDirCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!ArchiveTools.ChangeWorkingArchiveLocalDirectory(ListArgsOnly[0]))
            {
                TextWriterColor.Write(Translate.DoTranslation("Directory {0} doesn't exist"), true, ColorTools.ColTypes.Error, ListArgsOnly[0]);
            }
        }

    }
}
