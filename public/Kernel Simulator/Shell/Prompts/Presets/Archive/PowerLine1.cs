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
using System.IO;
using System.Text;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Misc.Text;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Archive;

namespace KS.Shell.Prompts.Presets.Archive
{
    /// <summary>
    /// PowerLine 1 preset
    /// </summary>
    public class ArchivePowerLine1Preset : PromptPresetBase, IPromptPreset
    {
        /// <inheritdoc/>

        public override string PresetName { get; } = "PowerLine1";

        /// <inheritdoc/>
        public override string PresetPrompt
        {
            get
            {
                return PresetPromptBuilder();
            }
        }

        /// <inheritdoc/>
        public override ShellType PresetShellType { get; } = ShellType.ArchiveShell;

        internal override string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char TransitionChar = Convert.ToChar(0xE0B0);

            // PowerLine preset colors
            var FirstColorSegmentForeground = new Color(85, 255, 255);
            var FirstColorSegmentBackground = new Color(43, 127, 127);
            var SecondColorSegmentForeground = new Color(0, 0, 0);
            var SecondColorSegmentBackground = new Color(85, 255, 255);
            var LastTransitionForeground = new Color(255, 255, 255);

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // File name
            PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat(" {0} ", Path.GetFileName(ArchiveShellCommon.ArchiveShell_FileStream.Name));

            // Transition
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat("{0}", TransitionChar);

            // Current archive directory
            PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat(" {0} ", ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory);

            // Transition
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceForeground);
            PresetStringBuilder.Append(Flags.SetBackground ? ColorTools.BackgroundColor.VTSequenceBackground : Convert.ToString(CharManager.GetEsc()) + $"[49m");
            PresetStringBuilder.AppendFormat("{0} ", TransitionChar);
            PresetStringBuilder.Append(ColorTools.InputColor.VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
