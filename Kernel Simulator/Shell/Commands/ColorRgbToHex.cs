﻿using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

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

namespace KS.Shell.Commands
{
	class ColorRgbToHexCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			int R, G, B;
			string Hex;

			// Check to see if we have the numeric arguments
			if (!int.TryParse(ListArgsOnly[0], out R))
			{
				TextWriterColor.Write(Translate.DoTranslation("The red color level must be numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				return;
			}
			if (!int.TryParse(ListArgsOnly[1], out G))
			{
				TextWriterColor.Write(Translate.DoTranslation("The green color level must be numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				return;
			}
			if (!int.TryParse(ListArgsOnly[2], out B))
			{
				TextWriterColor.Write(Translate.DoTranslation("The blue color level must be numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				return;
			}

			// Do the job
			Hex = KernelColorTools.ConvertFromRGBToHex(R, G, B);
			TextWriterColor.Write("- " + Translate.DoTranslation("Color hexadecimal representation:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
			TextWriterColor.Write(Hex, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
		}

	}
}