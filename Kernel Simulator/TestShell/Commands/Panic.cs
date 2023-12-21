﻿using System;
using KS.Kernel;
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
	class Test_PanicCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			KernelErrorLevel EType = (KernelErrorLevel)Convert.ToInt32(Enum.Parse(typeof(KernelErrorLevel), ListArgs[0]));
			bool Reboot = Convert.ToBoolean(ListArgs[1]);
			long RTime = Convert.ToInt64(ListArgs[2]);
			var Exc = new Exception();
			string Message = ListArgs[3];
			KernelTools.KernelError(EType, Reboot, RTime, Message, Exc);
		}

	}
}