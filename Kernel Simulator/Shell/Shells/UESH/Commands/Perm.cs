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
using KS.Login;
using KS.Shell.ShellBase.Commands;

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
    class PermCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            PermissionManagement.Permission((PermissionManagement.PermissionType)Convert.ToInt32(Enum.Parse(typeof(PermissionManagement.PermissionType), ListArgsOnly[1])), ListArgsOnly[0], (PermissionManagement.PermissionManagementMode)Convert.ToInt32(Enum.Parse(typeof(PermissionManagement.PermissionManagementMode), ListArgsOnly[2])));
        }

    }
}