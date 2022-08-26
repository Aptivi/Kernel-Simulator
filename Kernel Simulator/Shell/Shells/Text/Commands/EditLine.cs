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

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Edits a line
    /// </summary>
    /// <remarks>
    /// You can use this command to edit a line seamlessly.
    /// </remarks>
    class TextEdit_EditLineCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (StringQuery.IsStringNumeric(ListArgsOnly[0]))
            {
                if (Convert.ToInt32(ListArgsOnly[0]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                {
                    string OriginalLine = TextEditShellCommon.TextEdit_FileLines[(int)Math.Round(Convert.ToDouble(ListArgsOnly[0]) - 1d)];
                    TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
                    string EditedLine = Input.ReadLine("", OriginalLine, false);
                    TextEditShellCommon.TextEdit_FileLines[(int)Math.Round(Convert.ToDouble(ListArgsOnly[0]) - 1d)] = EditedLine;
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, ColorTools.ColTypes.Error);
                DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[0]);
            }
        }

    }
}