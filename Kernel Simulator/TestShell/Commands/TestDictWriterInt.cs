﻿using System.Collections.Generic;
using KS.ConsoleBase.Colors;
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

namespace KS.TestShell.Commands
{
	class Test_TestDictWriterIntCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			var NormalIntegerDict = new Dictionary<string, int>() { { "One", 1 }, { "Two", 2 }, { "Three", 3 } };
			var ArrayIntegerDict = new Dictionary<string, int[]>() { { "One", new int[] { 1, 2, 3 } }, { "Two", new int[] { 1, 2, 3 } }, { "Three", new int[] { 1, 2, 3 } } };
			TextWriterColor.Write(Translate.DoTranslation("Normal integer dictionary:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			ListWriterColor.WriteList(NormalIntegerDict);
			TextWriterColor.Write(Translate.DoTranslation("Array integer dictionary:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
			ListWriterColor.WriteList(ArrayIntegerDict);
		}

	}
}
