﻿using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

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

using KS.Network.FTP.Transfer;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.FTP.Commands
{
	class FTP_GetFolderCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			string RemoteFolder = ListArgs[0];
			string LocalFolder = ListArgs.Count() > 1 ? ListArgs[1] : "";
			TextWriterColor.Write(Translate.DoTranslation("Downloading folder {0}..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Progress), RemoteFolder);
			bool Result = !string.IsNullOrWhiteSpace(LocalFolder) ? FTPTransfer.FTPGetFolder(RemoteFolder, LocalFolder) : FTPTransfer.FTPGetFolder(RemoteFolder);
			if (Result)
			{
				TextWriterColor.WritePlain("", true);
				TextWriterColor.Write(Translate.DoTranslation("Downloaded folder {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success), RemoteFolder);
			}
			else
			{
				TextWriterColor.WritePlain("", true);
				TextWriterColor.Write(Translate.DoTranslation("Download failed for folder {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), RemoteFolder);
			}
		}

	}
}