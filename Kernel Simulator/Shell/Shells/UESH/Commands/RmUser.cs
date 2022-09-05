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
using KS.Shell.ShellBase.Commands;
using KS.Users;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Removes uninvited or redundant user
    /// </summary>
    /// <remarks>
    /// This command lets you remove the uninvited or redundant user from the user dictionary that is initialized at the start of the kernel. It also removes password from the removed user if it has one.
    /// <br></br>
    /// However you can't remove your own user that is signed in.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class RmUserCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            UserManagement.RemoveUser(ListArgsOnly[0]);
            TextWriterColor.Write(Translate.DoTranslation("User {0} removed."), true, ColorTools.ColTypes.Neutral, ListArgsOnly[0]);
        }

    }
}