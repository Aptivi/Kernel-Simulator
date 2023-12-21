﻿using System;
using System.Collections.Generic;
using KS.Misc.Threading;
using Terminaux.Base;

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

namespace KS.Misc.Screensaver
{
	public abstract class BaseScreensaver : IScreensaver
	{

		public virtual string ScreensaverName { get; set; } = "BaseScreensaver";

		public virtual Dictionary<string, object> ScreensaverSettings { get; set; }

		public virtual void ScreensaverPreparation()
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			ConsoleWrapper.Clear();
			ConsoleWrapper.CursorVisible = false;
		}

		public virtual void ScreensaverLogic()
		{
			ThreadManager.SleepNoBlock(10L, ScreensaverDisplayer.ScreensaverDisplayerThread);
		}
	}
}