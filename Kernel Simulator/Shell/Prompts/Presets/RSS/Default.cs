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
using System.Text;
using KS.ConsoleBase.Colors;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.RSS;

namespace KS.Shell.Prompts.Presets.RSS
{
    public class RSSDefaultPreset : PromptPresetBase, IPromptPreset
    {

        public override string PresetName { get; } = "Default";

        public override string PresetPrompt
        {
            get
            {
                return PresetPromptBuilder();
            }
        }

        public override ShellType PresetShellType { get; } = ShellType.RSSShell;

        internal override string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append("[");

            // RSS site
            PresetStringBuilder.Append(ColorTools.UserNameShellColor.VTSequenceForeground);
            PresetStringBuilder.AppendFormat("{0}", new Uri(RSSShellCommon.RSSFeedLink).Host);

            // Closing
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append("] > ");

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}