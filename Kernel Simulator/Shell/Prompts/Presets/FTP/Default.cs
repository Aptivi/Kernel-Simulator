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
using KS.Network.FTP;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Prompts.Presets.FTP
{
	public class FTPDefaultPreset : PromptPresetBase, IPromptPreset
	{

		public override string PresetName { get; } = "Default";

		public override string PresetPrompt
		{
			get
			{
				return PresetPromptBuilder();
			}
		}

		public override ShellType PresetShellType { get; } = ShellType.FTPShell;

		internal override string PresetPromptBuilder()
		{
			// Build the preset
			var PresetStringBuilder = new StringBuilder();

			if (FTPShellCommon.FtpConnected)
			{
				// Opening
				PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
				PresetStringBuilder.Append("[");

				// SFTP user
				PresetStringBuilder.Append(KernelColorTools.UserNameShellColor.VTSequenceForeground);
				PresetStringBuilder.AppendFormat("{0}", FTPShellCommon.FtpUser);

				// "at" sign
				PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
				PresetStringBuilder.Append("@");

				// SFTP site
				PresetStringBuilder.Append(KernelColorTools.HostNameShellColor.VTSequenceForeground);
				PresetStringBuilder.AppendFormat("{0}", FTPShellCommon.FtpSite);

				// Closing
				PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
				PresetStringBuilder.AppendFormat("]{0}> ", FTPShellCommon.FtpCurrentRemoteDir);
			}
			else
			{
				// Current directory
				PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
				PresetStringBuilder.AppendFormat("{0}> ", FTPShellCommon.FtpCurrentDirectory);
			}

			// Present final string
			return PresetStringBuilder.ToString();
		}

		string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

	}
}