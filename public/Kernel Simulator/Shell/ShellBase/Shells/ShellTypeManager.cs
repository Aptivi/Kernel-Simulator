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
using Extensification.DictionaryExts;

namespace KS.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell type management module
    /// </summary>
    public static class ShellTypeManager
    {
        /// <summary>
        /// Registers the custom shell. Should be called when the mods start up.
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellTypeInfo">The shell type information</param>
        public static void RegisterShell(string ShellType, BaseShellInfo ShellTypeInfo) => 
            Shell.AvailableShells.AddIfNotFound(ShellType, ShellTypeInfo);

        /// <summary>
        /// Unregisters the custom shell. Should be called when the mods shut down.
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static void UnregisterShell(string ShellType)
        {
            if (!Enum.IsDefined(typeof(ShellType), ShellType))
                Shell.AvailableShells.Remove(ShellType);
        }

        /// <summary>
        /// Is the shell pre-defined in Kernel Simulator?
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        /// <returns>If available in ShellType, then it's a built-in shell, thus returning true. Otherwise, false for custom shells.</returns>
        public static bool IsShellBuiltin(string ShellType) =>
            Enum.IsDefined(typeof(ShellType), ShellType);
    }
}
