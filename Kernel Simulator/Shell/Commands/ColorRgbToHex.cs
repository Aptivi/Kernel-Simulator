﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
namespace KS.Shell.Commands
{
    class ColorRgbToHexCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string Hex;

            // Check to see if we have the numeric arguments
            if (!int.TryParse(ListArgsOnly[0], out int R))
            {
                TextWriters.Write(Translate.DoTranslation("The red color level must be numeric."), true, KernelColorTools.ColTypes.Error);
                return;
            }
            if (!int.TryParse(ListArgsOnly[1], out int G))
            {
                TextWriters.Write(Translate.DoTranslation("The green color level must be numeric."), true, KernelColorTools.ColTypes.Error);
                return;
            }
            if (!int.TryParse(ListArgsOnly[2], out int B))
            {
                TextWriters.Write(Translate.DoTranslation("The blue color level must be numeric."), true, KernelColorTools.ColTypes.Error);
                return;
            }

            // Do the job
            Hex = KernelColorTools.ConvertFromRGBToHex(R, G, B);
            TextWriters.Write("- " + Translate.DoTranslation("Color hexadecimal representation:") + " ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Hex, true, KernelColorTools.ColTypes.ListValue);
        }

    }
}