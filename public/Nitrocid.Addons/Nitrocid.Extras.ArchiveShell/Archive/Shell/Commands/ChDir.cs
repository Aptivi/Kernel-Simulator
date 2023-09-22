﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;

namespace Nitrocid.Extras.ArchiveShell.Archive.Shell.Commands
{
    /// <summary>
    /// Changes current local directory
    /// </summary>
    /// <remarks>
    /// If you want to interact with the ZIP file in another local directory, you can use this command to change the current local directory. This change isn't applied to the main shell.
    /// </remarks>
    class ArchiveShell_ChDirCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            if (!ArchiveTools.ChangeWorkingArchiveLocalDirectory(ListArgsOnly[0]))
            {
                TextWriterColor.Write(Translate.DoTranslation("Directory {0} doesn't exist"), true, KernelColorType.Error, ListArgsOnly[0]);
                return 10000 + (int)KernelExceptionType.Archive;
            }
            return 0;
        }

    }
}
