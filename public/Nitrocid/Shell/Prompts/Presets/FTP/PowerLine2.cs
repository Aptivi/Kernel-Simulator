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

using System;
using System.Text;
using ColorSeq;
using KS.Shell.Shells.FTP;
using KS.ConsoleBase.Colors;
using System.Collections.Generic;
using KS.ConsoleBase.Writers.FancyWriters.Tools;

namespace KS.Shell.Prompts.Presets.FTP
{
    /// <summary>
    /// PowerLine 2 preset
    /// </summary>
    public class FtpPowerLine2Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLine2";

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "FTPShell";

        /// <inheritdoc/>
        public override string PresetPrompt =>
            PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetPromptCompletion =>
            PresetPromptCompletionBuilder();

        internal override string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char PadlockChar = Convert.ToChar(0xE0A2);

            // Segments
            List<PowerLineSegment> segments = new()
            {
                new PowerLineSegment(new Color(255, 85, 255), new Color(127, 43, 127), FTPShellCommon.FtpUser),
                new PowerLineSegment(new Color(0, 0, 0), new Color(85, 255, 255), FTPShellCommon.FtpSite, PadlockChar),
                new PowerLineSegment(new Color(0, 0, 0), new Color(255, 255, 255), FTPShellCommon.FtpCurrentRemoteDir),
            };

            // Builder
            var PresetStringBuilder = new StringBuilder();

            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        internal override string PresetPromptCompletionBuilder()
        {
            // Segments
            List<PowerLineSegment> segments = new()
            {
                new PowerLineSegment(new Color(255, 85, 255), new Color(127, 43, 127), "+"),
            };

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Use RenderSegments to render our segments
            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() =>
            PresetPromptBuilder();

        string IPromptPreset.PresetPromptCompletionBuilder() =>
            PresetPromptCompletionBuilder();

    }
}
