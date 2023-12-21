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
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.Login;

namespace KS.Shell.Prompts.Presets.UESH
{
	public class DefaultPreset : PromptPresetBase, IPromptPreset
	{

		public override string PresetName { get; } = "Default";

		public override string PresetPrompt
		{
			get
			{
				return PresetPromptBuilder();
			}
		}

		internal override string PresetPromptBuilder()
		{
			var PresetStringBuilder = new StringBuilder();
			char UserDollarSign = PermissionManagement.HasPermission(Login.Login.CurrentUser.Username, PermissionManagement.PermissionType.Administrator) ? '#' : '$';

			// Build the preset
			if (!Flags.Maintenance)
			{
				// Opening
				PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
				PresetStringBuilder.Append("[");

				// Current username
				PresetStringBuilder.Append(KernelColorTools.UserNameShellColor.VTSequenceForeground);
				PresetStringBuilder.AppendFormat("{0}", Login.Login.CurrentUser.Username);

				// "At" sign
				PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
				PresetStringBuilder.Append("@");

				// Current hostname
				PresetStringBuilder.Append(KernelColorTools.HostNameShellColor.VTSequenceForeground);
				PresetStringBuilder.AppendFormat("{0}", Kernel.Kernel.HostName);

				// Current directory
				PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
				PresetStringBuilder.AppendFormat("]{0}", CurrentDirectory.CurrentDir);

				// User dollar sign
				PresetStringBuilder.Append(KernelColorTools.UserDollarColor.VTSequenceForeground);
				PresetStringBuilder.AppendFormat(" {0} ", UserDollarSign);
			}
			else
			{
				// Maintenance mode
				PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
				PresetStringBuilder.Append(Translate.DoTranslation("Maintenance Mode") + "> ");
			}

			// Present final string
			return PresetStringBuilder.ToString();
		}

		string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

	}
}