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

using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files.Operations;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Combines the two text files or more into the console.
    /// </summary>
    /// <remarks>
    /// If you have two or more fragments of a complete text file, you can combine them using this command to print the complete text file to toe console.
    /// </remarks>
    class CombineStrCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string InputPath = ListArgsOnly[0];
            var CombineInputPaths = ListArgsOnly.Skip(1).ToArray();

            // Make a combined content array
            var CombinedContents = Combination.CombineFiles(InputPath, CombineInputPaths);
            TextWriterColor.Write(string.Join("\n", CombinedContents), true, ColorTools.ColTypes.NeutralText);
        }

    }
}
