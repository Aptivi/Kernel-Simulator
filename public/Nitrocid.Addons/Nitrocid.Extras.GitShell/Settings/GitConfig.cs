﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Newtonsoft.Json;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection.Internal;
using Nitrocid.Shell.Prompts;

namespace Nitrocid.Extras.GitShell.Settings
{
    /// <summary>
    /// Configuration instance for Git
    /// </summary>
    public class GitConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(ResourcesManager.GetData("GitSettings.json", ResourcesType.Misc, typeof(GitConfig).Assembly) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain settings entries.")));

        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string GitPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("GitShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "GitShell", false);
        }
    }
}
