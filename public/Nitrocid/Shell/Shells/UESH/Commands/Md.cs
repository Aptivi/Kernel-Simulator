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

using KS.Files.Operations;
using KS.Shell.ShellBase.Commands;
using KS.Users.Permissions;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can make your directory
    /// </summary>
    /// <remarks>
    /// If you want to organize your files into a folder, you must create a folder.
    /// <br></br>
    /// This command lets you create a folder in the current working directory.
    /// </remarks>
    class MdCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            Making.MakeDirectory(ListArgsOnly[0]);
            variableValue = ListArgsOnly[0];
            return 0;
        }
    }
}
