﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Parameters to pass to <see cref="CommandExecutor.ExecuteCommand(CommandExecutorParameters)"/>
    /// </summary>
    internal class CommandExecutorParameters
    {
        /// <summary>
        /// The requested command with arguments
        /// </summary>
        internal string RequestedCommand;
        /// <summary>
        /// The shell type
        /// </summary>
        internal string ShellType;
        /// <summary>
        /// Is the command the custom command?
        /// </summary>
        internal bool CustomCommand;
        /// <summary>
        /// Shell instance
        /// </summary>
        internal ShellExecuteInfo ShellInstance;

        internal CommandExecutorParameters(string RequestedCommand, ShellType ShellType, ShellExecuteInfo ShellInstance) :
            this(RequestedCommand, ShellManager.GetShellTypeName(ShellType), ShellInstance)
        { }

        internal CommandExecutorParameters(string RequestedCommand, string ShellType, ShellExecuteInfo ShellInstance)
        {
            this.RequestedCommand = RequestedCommand;
            this.ShellType = ShellType;
            this.ShellInstance = ShellInstance;
        }
    }
}
