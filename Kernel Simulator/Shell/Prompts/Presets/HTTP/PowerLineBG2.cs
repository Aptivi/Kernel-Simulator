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
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Misc.Text;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.HTTP;

namespace KS.Shell.Prompts.Presets.HTTP
{
    /// <summary>
    /// PowerLine BG 2 preset
    /// </summary>
    public class HTTPPowerLineBG2Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLineBG2";

        /// <inheritdoc/>
        public override string PresetPrompt
        {
            get
            {
                return PresetPromptBuilder();
            }
        }

        /// <inheritdoc/>
        public override ShellType PresetShellType { get; } = ShellType.HTTPShell;

        internal override string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char TransitionChar = Convert.ToChar(0xE0B0);
            char TransitionPartChar = Convert.ToChar(0xE0B1);
            char PadlockChar = Convert.ToChar(0xE0A2);

            // PowerLine preset colors
            var FirstColorSegmentForeground = new Color(255, 85, 255);
            var FirstColorSegmentBackground = new Color(25, 25, 25);
            var SecondColorSegmentForeground = new Color(255, 85, 255);
            var SecondColorSegmentBackground = new Color(25, 25, 25);
            var ThirdColorSegmentForeground = new Color(255, 85, 255);
            var ThirdColorSegmentBackground = new Color(25, 25, 25);
            var LastTransitionForeground = new Color(25, 25, 25);

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (HTTPShellCommon.HTTPConnected)
            {
                // Current username
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat(" {0} {1} ", PadlockChar, HTTPShellCommon.HTTPSite);

                // Transition
                PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground);
                PresetStringBuilder.Append(Flags.SetBackground ? ColorTools.BackgroundColor.VTSequenceBackground : Convert.ToString(CharManager.GetEsc()) + $"[49m");
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar);
            }
            else
            {
                // HTTP current directory
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat(" Not connected ");

                // Transition
                PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground);
                PresetStringBuilder.Append(Flags.SetBackground ? ColorTools.BackgroundColor.VTSequenceBackground : Convert.ToString(CharManager.GetEsc()) + $"[49m");
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
