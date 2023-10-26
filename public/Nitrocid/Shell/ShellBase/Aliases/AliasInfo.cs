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

using KS.Shell.ShellBase.Commands;
using Newtonsoft.Json;

namespace KS.Shell.ShellBase.Aliases
{
    /// <summary>
    /// Command alias information
    /// </summary>
    public class AliasInfo
    {
        [JsonProperty(PropertyName = "Alias")]
        internal string alias;
        [JsonProperty(PropertyName = "Command")]
        internal string command;
        [JsonProperty(PropertyName = "Type")]
        internal string type;

        /// <summary>
        /// Gets the alias that the shell resolves to the actual command
        /// </summary>
        [JsonIgnore]
        public string Alias =>
            alias;
        /// <summary>
        /// The actual command being resolved to
        /// </summary>
        [JsonIgnore]
        public string Command =>
            command;
        /// <summary>
        /// Type of the resolved command
        /// </summary>
        [JsonIgnore]
        public string Type =>
            type;
        /// <summary>
        /// Resolved target command info (for execution)
        /// </summary>
        [JsonIgnore]
        public CommandInfo TargetCommand =>
            CommandManager.GetCommand(Command, Type, false);

        [JsonConstructor]
        internal AliasInfo()
        { }
    }
}
