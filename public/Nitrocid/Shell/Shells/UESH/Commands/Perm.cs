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

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Users.Permissions;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages permissions for your user or other users
    /// </summary>
    /// <remarks>
    /// If you want to manage permissions for your user or other users, or if you want to prevent the user from being logged on, use this command.
    /// <br></br>
    /// This command lets you manage permissions whether the administrative privileges are on or off, or if the user is disabled or not.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class PermCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageUsers);
            string target = ListArgsOnly[0];
            string mode = ListArgsOnly[1];
            string perm = ListArgsOnly[2];

            if (!Enum.TryParse(typeof(PermissionTypes), perm, out object permission))
            {
                // Permission not found
                TextWriterColor.Write(Translate.DoTranslation("No such permission"), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.PermissionManagement;
            }

            if (mode == "allow")
                // Granting permission.
                PermissionsTools.GrantPermission(target, (PermissionTypes)permission);
            else if (mode == "revoke")
                // Revoking permission.
                PermissionsTools.RevokePermission(target, (PermissionTypes)permission);
            else
            {
                // No mode
                TextWriterColor.Write(Translate.DoTranslation("No such permission mode"), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.PermissionManagement;
            }
            return 0;
        }

    }
}
