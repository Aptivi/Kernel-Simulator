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

using System.Collections.Generic;
using KS.Shell.Prompts.Presets.Json;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Json.Commands;

namespace KS.Shell.Shells.Json
{
    /// <summary>
    /// Common JSON shell class
    /// </summary>
    internal class JsonShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// JSON commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "addarray", new CommandInfo("addarray", ShellType, /* Localizable */ "Adds a new property containing the array", new CommandArgumentInfo(new[] { "[-parentProperty=prop] <propertyName> <propertyValue1> [propertyValue2] [propertyValue3]..." }, true, 2), new JsonShell_AddArrayCommand()) },
            { "addproperty", new CommandInfo("addproperty", ShellType, /* Localizable */ "Adds a new property at the end of the JSON file", new CommandArgumentInfo(new[] { "[-parentProperty=prop] <propertyName> <propertyValue>" }, true, 2), new JsonShell_AddPropertyCommand()) },
            { "addobject", new CommandInfo("addobject", ShellType, /* Localizable */ "Adds a new object inside the array", new CommandArgumentInfo(new[] { "[-parentProperty=prop] <arrayName> <valueInArray>" }, true, 2), new JsonShell_AddObjectCommand()) },
            { "addobjectindexed", new CommandInfo("addobjectindexed", ShellType, /* Localizable */ "Adds a new object inside an object specified by index", new CommandArgumentInfo(new[] { "[-parentProperty=prop] <index> <valueInArray>" }, true, 2), new JsonShell_AddObjectIndexedCommand()) },
            { "clear", new CommandInfo("clear", ShellType, /* Localizable */ "Clears the JSON file", new CommandArgumentInfo(), new JsonShell_ClearCommand()) },
            { "delproperty", new CommandInfo("delproperty", ShellType, /* Localizable */ "Removes a property from the JSON file", new CommandArgumentInfo(new[] { "<propertyName>" }, true, 1), new JsonShell_DelPropertyCommand()) },
            { "exitnosave", new CommandInfo("exitnosave", ShellType, /* Localizable */ "Exits the JSON shell without saving the changes", new CommandArgumentInfo(), new JsonShell_ExitNoSaveCommand()) },
            { "jsoninfo", new CommandInfo("jsoninfo", ShellType, /* Localizable */ "Shows information about the JSON file", new CommandArgumentInfo(new[] { "[-simplified|-showvals]" }, false, 0), new JsonShell_JsonInfoCommand()) },
            { "print", new CommandInfo("print", ShellType, /* Localizable */ "Prints the JSON file", new CommandArgumentInfo(new[] { "[property]" }, false, 0), new JsonShell_PrintCommand()) },
            { "rmobject", new CommandInfo("rmobject", ShellType, /* Localizable */ "Removes an object", new CommandArgumentInfo(new[] { "[-parentProperty=prop] <objectName>" }, true, 1), new JsonShell_RmObjectCommand()) },
            { "rmobjectindexed", new CommandInfo("rmobjectindexed", ShellType, /* Localizable */ "Removes an object specified by index", new CommandArgumentInfo(new[] { "[-parentProperty=prop] <index>" }, true, 1), new JsonShell_RmObjectIndexedCommand()) },
            { "save", new CommandInfo("save", ShellType, /* Localizable */ "Saves the JSON file", new CommandArgumentInfo(new[] { "[-b|-m]" }, false, 0), new JsonShell_SaveCommand()) }
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new JsonDefaultPreset() },
            { "PowerLine1", new JsonPowerLine1Preset() },
            { "PowerLine2", new JsonPowerLine2Preset() },
            { "PowerLine3", new JsonPowerLine3Preset() },
            { "PowerLineBG1", new JsonPowerLineBG1Preset() },
            { "PowerLineBG2", new JsonPowerLineBG2Preset() },
            { "PowerLineBG3", new JsonPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new JsonShell();

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["JsonShell"];

    }
}
