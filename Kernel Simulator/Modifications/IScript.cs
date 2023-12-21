﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using KS.Kernel;
using KS.Shell.ShellBase.Commands;

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

namespace KS.Modifications
{
    /// <summary>
    /// Interface for mods
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// List of commands for mod
        /// </summary>
        Dictionary<string, CommandInfo> Commands { get; set; }
        /// <summary>
        /// Mod name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Name of part of mod
        /// </summary>
        string ModPart { get; set; }
        /// <summary>
        /// Mod version
        /// </summary>
        string Version { get; set; }
        /// <summary>
        /// Code executed when starting mod
        /// </summary>
        void StartMod();
        /// <summary>
        /// Code executed when stopping mod
        /// </summary>
        void StopMod();
        /// <summary>
        /// Code executed when initializing events
        /// </summary>
        /// <param name="ev">Event name. Look it up on <see cref="Events"/></param>
        void InitEvents(string ev);
        /// <summary>
        /// Code executed when initializing events
        /// </summary>
        /// <param name="ev">Event name. Look it up on <see cref="Events"/></param>
        /// <param name="Args">Arguments.</param>
        void InitEvents(string ev, params object[] Args);
    }
}