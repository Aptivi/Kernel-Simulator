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
using KS.Misc.ZipFile;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Zip.Commands
{
    /// <summary>
    /// Changes current archive directory
    /// </summary>
    /// <remarks>
    /// If you want to go to a folder inside the ZIP archive, you can use this command to change the working archive directory.
    /// </remarks>
    class ZipShell_ChADirCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!ZipTools.ChangeWorkingArchiveDirectory(ListArgsOnly[0]))
            {
                TextWriterColor.Write(Translate.DoTranslation("Archive directory {0} doesn't exist"), true, ColorTools.ColTypes.Error, ListArgsOnly[0]);
            }
        }

    }
}