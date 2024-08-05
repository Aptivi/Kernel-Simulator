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

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
namespace KS.Misc.ZipFile.Commands
{
    class ZipShell_GetCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string Where = "";
            var Absolute = default(bool);
            if ((ListArgs?.Length) is { } arg1 && arg1 > 1)
            {
                if (!(ListArgs[1] == "-absolute"))
                    Where = Filesystem.NeutralizePath(ListArgs[1]);
                if (ListArgs?.Contains("-absolute") == true)
                {
                    Absolute = true;
                }
            }
            ZipTools.ExtractZipFileEntry(ListArgs[0], Where, Absolute);
        }

        public override void HelpHelper()
        {
            TextWriters.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write("  -absolute: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Indicates that the target path is absolute"), true, KernelColorTools.ColTypes.ListValue);
        }

    }
}