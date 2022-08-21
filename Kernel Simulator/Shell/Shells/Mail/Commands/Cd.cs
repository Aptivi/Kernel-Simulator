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

using KS.Network.Mail.Directory;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Mail.Commands
{
    /// <summary>
    /// Changes your directory
    /// </summary>
    /// <remarks>
    /// This command lets you change your directory in your mail folders to another directory that exists in the subdirectory.
    /// </remarks>
    class Mail_CdCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            MailDirectory.MailChangeDirectory(ListArgsOnly[0]);
        }

    }
}