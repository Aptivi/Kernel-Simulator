﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Extras.Mods.Commands;
using Nitrocid.Extras.Mods.Modifications;
using Nitrocid.Extras.Mods.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Mods
{
    internal class ModsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("modman", /* Localizable */ "Manage your mods",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "start/stop/info/reload/install/uninstall", new()
                        {
                            ExactWording = ["start", "stop", "info", "reload", "install", "uninstall"]
                        }),
                        new CommandArgumentPart(true, "modfilename"),
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "list/reloadall/stopall/startall/tui", new()
                        {
                            ExactWording = ["list", "reloadall", "stopall", "startall", "tui"]
                        }),
                    }),
                ], new ModManCommand(), CommandFlags.Strict),

            new CommandInfo("modmanual", /* Localizable */ "Mod manual",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "modname"),
                    })
                ], new ModManualCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMods);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Important;

        internal static ModsConfig ModsConfig =>
            (ModsConfig)Config.baseConfigurations[nameof(ModsConfig)];

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            var config = new ModsConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ModsConfig));
        }
    }
}
