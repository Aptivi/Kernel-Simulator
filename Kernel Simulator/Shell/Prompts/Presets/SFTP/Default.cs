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

using System.Text;
using KS.ConsoleBase.Colors;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.SFTP;

namespace KS.Shell.Prompts.Presets.SFTP
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class SFTPDefaultPreset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "Default";

        /// <inheritdoc/>
        public override string PresetPrompt
        {
            get
            {
                return PresetPromptBuilder();
            }
        }

        /// <inheritdoc/>
        public override ShellType PresetShellType { get; } = ShellType.SFTPShell;

        internal override string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            if (SFTPShellCommon.SFTPConnected)
            {
                // Opening
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append("[");

                // SFTP user
                PresetStringBuilder.Append(ColorTools.UserNameShellColor.VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", SFTPShellCommon.SFTPUser);

                // "at" sign
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append("@");

                // SFTP site
                PresetStringBuilder.Append(ColorTools.HostNameShellColor.VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", SFTPShellCommon.SFTPSite);

                // Closing
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("]{0}> ", SFTPShellCommon.SFTPCurrentRemoteDir);
            }
            else
            {
                // Current directory
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}> ", SFTPShellCommon.SFTPCurrDirect);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
