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

using KS.Arguments.ArgumentBase;
using KS.Kernel;
using KS.Kernel.Debugging.Testing;
using System;

namespace KS.Arguments.CommandLineArguments
{
    class TestInteractiveArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            KernelTools.StageTimer.Stop();
            TestInteractive.Open();
            KernelTools.StageTimer.Start();
            if (TestInteractive.ShutdownFlag)
            {
                // Clear the console and reset the colors
                // TODO: We need a way to more appropriately handle this.
                ConsoleBase.ConsoleWrapper.ResetColor();
                ConsoleBase.ConsoleWrapper.Clear();
                ConsoleBase.ConsoleWrapper.CursorVisible = true;
                Environment.Exit(0);
            }
        }

    }
}
