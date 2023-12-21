﻿using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
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

namespace KS.Misc.Editors.TextEdit.Commands
{
	class TextEdit_AddLinesCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			var FinalLines = new List<string>();
			string FinalLine = "";

			// Keep prompting for lines until the user finishes
			TextWriterColor.Write(Translate.DoTranslation("Enter the text that you want to append to the end of the file. When you're done, write \"EOF\" on its own line."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			while (FinalLine != "EOF")
			{
				TextWriterColor.Write(">> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
				FinalLine = Input.ReadLine(false);
				if (!(FinalLine == "EOF"))
				{
					FinalLines.Add(FinalLine);
				}
			}

			// Add the new lines
			TextEditTools.TextEdit_AddNewLines([.. FinalLines]);
		}

	}
}