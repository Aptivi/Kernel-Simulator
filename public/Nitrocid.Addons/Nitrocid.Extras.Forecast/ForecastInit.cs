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

using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.Forecast.Forecast.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Forecast
{
    internal class ForecastInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "weather",
                new CommandInfo("weather", ShellType.Shell, /* Localizable */ "Shows weather info for specified city. Uses OpenWeatherMap.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "CityID/CityName"),
                            new CommandArgumentPart(false, "apikey"),
                        }, new[] {
                            new SwitchInfo("list", /* Localizable */ "Shows all the available cities", false, false, null, 2, false)
                        })
                    }, new WeatherCommand())
            },
        };

        string IAddon.AddonName => "Extras - Forecast";

        AddonType IAddon.AddonType => AddonType.Optional;

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
    }
}
