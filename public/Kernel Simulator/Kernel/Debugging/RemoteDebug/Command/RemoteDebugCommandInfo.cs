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

using KS.Languages;

namespace KS.Kernel.Debugging.RemoteDebug.Command
{
    /// <summary>
    /// Remote debug command information class
    /// </summary>
    public class RemoteDebugCommandInfo
    {

        /// <summary>
        /// The command
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// The untranslated help definition of command. Translated by <see cref="GetTranslatedHelpEntry()"/>
        /// </summary>
        public string HelpDefinition { get; set; }
        /// <summary>
        /// Command argument info
        /// </summary>
        public RemoteDebugCommandArgumentInfo CommandArgumentInfo { get; private set; }
        /// <summary>
        /// Command base for execution
        /// </summary>
        public RemoteDebugBaseCommand CommandBase { get; private set; }

        /// <summary>
        /// Installs a new instance of remote debug command info class
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="HelpDefinition">Command help definition</param>
        /// <param name="CommandArgumentInfo">Command argument info</param>
        /// <param name="CommandBase">Command base for execution</param>
        public RemoteDebugCommandInfo(string Command, string HelpDefinition, RemoteDebugCommandArgumentInfo CommandArgumentInfo, RemoteDebugBaseCommand CommandBase)
        {
            this.Command = Command;
            this.HelpDefinition = HelpDefinition;
            this.CommandArgumentInfo = CommandArgumentInfo;
            this.CommandBase = CommandBase;
        }

        /// <summary>
        /// Gets the translated version of help entry (KS built-in remote debug commands only)
        /// </summary>
        public string GetTranslatedHelpEntry() => Translate.DoTranslation(HelpDefinition);

    }
}
