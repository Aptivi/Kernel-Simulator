﻿
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using KS.Kernel.Debugging;

namespace KS.Kernel.Debugging.RemoteDebug.Command
{
    /// <summary>
    /// The command executor class
    /// </summary>
    public abstract class RemoteDebugBaseCommand : IRemoteDebugCommand
    {

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="StringArgs">String of arguments</param>
        /// <param name="ListArgsOnly">List of all arguments</param>
        /// <param name="ListSwitchesOnly">List of all switches</param>
        /// <param name="Address">Device address that executed the command</param>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, string Address)
        {
            DebugWriter.WriteDebug(DebugLevel.F, "We shouldn't be here!!!");
            throw new InvalidOperationException();
        }

        /// <summary>
        /// The help helper
        /// </summary>
        public virtual void HelpHelper() => DebugWriter.WriteDebug(DebugLevel.I, "No additional information found.");

    }
}
