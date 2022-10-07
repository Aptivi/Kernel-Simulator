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

using System.IO;
using System.Text;
using KS.ConsoleBase.Colors;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Archive;

namespace KS.Shell.Prompts.Presets.Archive
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class ArchiveDefaultPreset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "Default";

        /// <inheritdoc/>
        public override string PresetPrompt => PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "ArchiveShell";

        internal override string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append("[");

            // File name
            PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat(Path.GetFileName(ArchiveShellCommon.ArchiveShell_FileStream.Name));

            // Current archive directory
            PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat("{0}", ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory);

            // Closing
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append("] > ");
            PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
