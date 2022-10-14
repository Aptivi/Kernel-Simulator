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
using KS.Network.Mail;
using KS.Shell.Shells.Mail;

namespace KS.Shell.Prompts.Presets.Mail
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class MailDefaultPreset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "Default";

        /// <inheritdoc/>
        public override string PresetPrompt => PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "MailShell";

        internal override string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append("[");

            // Mail username
            PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat("{0}", MailLogin.Mail_Authentication.UserName);

            // Closing
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append("] ");

            // Closing
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.AppendFormat("{0} > ", MailShellCommon.IMAP_CurrentDirectory);
            PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
