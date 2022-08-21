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
using KS.Network.RemoteDebug;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lets you unblock a blocked debug device
    /// </summary>
    /// <remarks>
    /// If you wanted to let a device whose IP address is blocked join the remote debugging again, you can unblock it using this command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class UnblockDbgDevCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (RemoteDebugger.RDebugBlocked.Contains(ListArgsOnly[0]))
            {
                if (RemoteDebugTools.TryRemoveFromBlockList(ListArgsOnly[0]))
                {
                    TextWriterColor.Write(Translate.DoTranslation("{0} can now join remote debug again."), true, ColorTools.ColTypes.Neutral, ListArgsOnly[0]);
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to unblock {0}."), true, ColorTools.ColTypes.Neutral, ListArgsOnly[0]);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("{0} is not blocked yet."), true, ColorTools.ColTypes.Neutral, ListArgsOnly[0]);
            }
        }

    }
}