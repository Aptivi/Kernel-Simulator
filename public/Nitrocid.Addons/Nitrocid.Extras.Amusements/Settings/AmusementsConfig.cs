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

using KS.Kernel.Configuration;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Configuration.Settings;
using Newtonsoft.Json;

namespace Nitrocid.Extras.Amusements.Settings
{
    /// <summary>
    /// Configuration instance for amusements
    /// </summary>
    public class AmusementsConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(Resources.AddonResources.AmusementsSettings);

        /// <summary>
        /// What is the minimum number to choose?
        /// </summary>
        public int SolverMinimumNumber { get; set; } = 0;
        /// <summary>
        /// What is the maximum number to choose?
        /// </summary>
        public int SolverMaximumNumber { get; set; } = 1000;
        /// <summary>
        /// Whether to show what's written in the input prompt.
        /// </summary>
        public bool SolverShowInput { get; set; }
        /// <summary>
        /// Select your preferred difficulty
        /// </summary>
        public int SpeedPressCurrentDifficulty { get; set; } = 1;
        /// <summary>
        /// How many milliseconds to wait for the keypress before the timeout? (In custom difficulty)
        /// </summary>
        public int SpeedPressTimeout { get; set; } = 3000;
        /// <summary>
        /// Whether to use PowerLine to render the spaceship or to use the standard greater than character. If you want to use PowerLine with Meteor, you need to install an appropriate font with PowerLine support.
        /// </summary>
        public bool MeteorUsePowerLine { get; set; } = true;
        /// <summary>
        /// Specifies the game speed in milliseconds.
        /// </summary>
        public int MeteorSpeed { get; set; } = 10;
        /// <summary>
        /// Whether to use PowerLine to render the spaceship or to use the standard greater than character. If you want to use PowerLine with Meteor, you need to install an appropriate font with PowerLine support.
        /// </summary>
        public bool ShipDuetUsePowerLine { get; set; } = true;
        /// <summary>
        /// Specifies the game speed in milliseconds.
        /// </summary>
        public int ShipDuetSpeed { get; set; } = 10;
    }
}
