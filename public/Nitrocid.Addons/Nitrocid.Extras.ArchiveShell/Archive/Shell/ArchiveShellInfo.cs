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
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.ArchiveShell.Archive.Shell.Commands;
using Nitrocid.Extras.ArchiveShell.Archive.Shell.Presets;
using KS.Shell.ShellBase.Switches;
using KS.Shell.ShellBase.Arguments;

namespace Nitrocid.Extras.ArchiveShell.Archive.Shell
{
    /// <summary>
    /// Common archive shell class
    /// </summary>
    internal class ArchiveShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Archive commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "cdir",
                new CommandInfo("cdir", ShellType, /* Localizable */ "Gets current local directory",
                    new[] {
                        new CommandArgumentInfo()
                    }, new CDirCommand())
            },

            { "chdir",
                new CommandInfo("chdir", ShellType, /* Localizable */ "Changes directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory")
                        })
                    }, new ChDirCommand())
            },

            { "chadir",
                new CommandInfo("chadir", ShellType, /* Localizable */ "Changes archive directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "archivedirectory")
                        })
                    }, new ChADirCommand())
            },

            { "get",
                new CommandInfo("get", ShellType, /* Localizable */ "Extracts a file to a specified directory or a current directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "entry"),
                            new CommandArgumentPart(false, "where")
                        }, new[] {
                            new SwitchInfo("absolute", /* Localizable */ "Indicates that the target path is absolute")
                        })
                    }, new GetCommand())
            },

            { "list",
                new CommandInfo("list", ShellType, /* Localizable */ "Lists all files inside the archive",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "directory")
                        })
                    }, new ListCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "pack",
                new CommandInfo("pack", ShellType, /* Localizable */ "Packs a local file to the archive",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "localfile"),
                            new CommandArgumentPart(false, "where")
                        })
                    }, new PackCommand())
            },
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new ArchiveDefaultPreset() },
            { "PowerLine1", new ArchivePowerLine1Preset() },
            { "PowerLine2", new ArchivePowerLine2Preset() },
            { "PowerLine3", new ArchivePowerLine3Preset() },
            { "PowerLineBG1", new ArchivePowerLineBG1Preset() },
            { "PowerLineBG2", new ArchivePowerLineBG2Preset() },
            { "PowerLineBG3", new ArchivePowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new ArchiveShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}
