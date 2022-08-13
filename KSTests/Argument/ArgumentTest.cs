﻿using System.Diagnostics;
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

using KS.Arguments.ArgumentBase;

namespace KSTests
{

    class ArgumentTest : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            Debug.WriteLine("We're on ArgumentTest with:");
            Debug.WriteLine(format: "- StringArgs: {0}", StringArgs);
            Debug.WriteLine(format: "- ListArgs: {0}", string.Join(", ", ListArgs));
            Debug.WriteLine(format: "- ListArgsOnly: {0}", string.Join(", ", ListArgsOnly));
            Debug.WriteLine(format: "- ListSwitchesOnly: {0}", string.Join(", ", ListSwitchesOnly));
        }

    }
}