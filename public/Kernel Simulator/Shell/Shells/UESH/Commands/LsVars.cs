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

using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using KS.Scripting;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists variables
    /// </summary>
    /// <remarks>
    /// This command lists all the defined UESH variables by either the set or the setrange commands, UESH commands that define and set a variable to a value (choice, ...), a UESH script, a mod, or your system's environment variables.
    /// </remarks>
    class LsVarsCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            foreach (string VarName in UESHVariables.GetVariables().Keys)
            {
                TextWriterColor.Write($"- {VarName}: ", false, ColorTools.ColTypes.ListEntry);
                TextWriterColor.Write(UESHVariables.GetVariables()[VarName], true, ColorTools.ColTypes.ListValue);
            }
        }

    }
}