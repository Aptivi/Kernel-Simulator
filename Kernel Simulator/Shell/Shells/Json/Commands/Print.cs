﻿using KS.ConsoleBase.Colors;

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

using KS.Misc.Editors.JsonShell;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Json.Commands
{
    /// <summary>
    /// Prints a property or the whole file
    /// </summary>
    /// <remarks>
    /// You can use this command to print the contents of either the full JSON file or a property.
    /// </remarks>
    class JsonShell_PrintCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length > 0)
            {
                TextWriterColor.Write(JsonTools.JsonShell_SerializeToString(StringArgs), true, ColorTools.ColTypes.Neutral);
            }
            else
            {
                TextWriterColor.Write(JsonTools.JsonShell_SerializeToString("$"), true, ColorTools.ColTypes.Neutral);
            }
        }

    }
}