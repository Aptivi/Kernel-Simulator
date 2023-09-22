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

using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// The command executor class
    /// </summary>
    public abstract class BaseCommand : ICommand
    {

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="StringArgs">String of arguments</param>
        /// <param name="ListArgsOnly">List of all arguments</param>
        /// <param name="StringArgsOrig">Arguments in a string (original)</param>
        /// <param name="ListArgsOnlyOrig">List of provided arguments (original)</param>
        /// <param name="ListSwitchesOnly">List of all switches</param>
        /// <param name="variableValue">Variable value to provide to target variable while -set is passed</param>
        /// <returns>Error code for the command</returns>
        public virtual int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            DebugWriter.WriteDebug(DebugLevel.F, "We shouldn't be here!!!");
            throw new KernelException(KernelExceptionType.NotImplementedYet);
        }

        /// <summary>
        /// Executes a command on dumb consoles
        /// </summary>
        /// <param name="StringArgs">String of arguments</param>
        /// <param name="ListArgsOnly">List of all arguments</param>
        /// <param name="StringArgsOrig">Arguments in a string (original)</param>
        /// <param name="ListArgsOnlyOrig">List of provided arguments (original)</param>
        /// <param name="ListSwitchesOnly">List of all switches</param>
        /// <param name="variableValue">Variable value to provide to target variable while -set is passed</param>
        /// <returns>Error code for the command</returns>
        public virtual int ExecuteDumb(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue) =>
            Execute(StringArgs, ListArgsOnly, StringArgsOrig, ListArgsOnlyOrig, ListSwitchesOnly, ref variableValue);

        /// <summary>
        /// The help helper
        /// </summary>
        public virtual void HelpHelper() =>
            DebugWriter.WriteDebug(DebugLevel.I, "No additional information found.");

    }
}
