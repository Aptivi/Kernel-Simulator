﻿using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

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
    /// You can log out of your account.
    /// </summary>
    /// <remarks>
    /// If there is a change that requires log-out and log-in for the changes to take effect, you must log off and log back in.
    /// <br></br>
    /// This command lets you off your account and sign in as somebody else. When you're finished with your account, and you want to use either the root account, or let someone else use their account, you must sign out.
    /// </remarks>
    class LogoutCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ShellStart.ShellStack.Count == 1)
            {
                Flags.LogoutRequested = true;
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Cannot log out from the subshell."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}