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
using KS.Kernel.Debugging.RemoteDebug;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists debugging devices connected
    /// </summary>
    /// <remarks>
    /// This command lists all the connected IP addresses that are currently receiving debug messages. This is useful for listing, identifying, and possibly disconnecting the address from the debugging server.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class LsDbgDevCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            foreach (RemoteDebugDevice DebugDevice in RemoteDebugger.DebugDevices)
            {
                TextWriterColor.Write($"- {DebugDevice.ClientIP}: ", false, ColorTools.ColTypes.ListEntry);
                TextWriterColor.Write(DebugDevice.ClientName, true, ColorTools.ColTypes.ListValue);
            }
        }

    }
}