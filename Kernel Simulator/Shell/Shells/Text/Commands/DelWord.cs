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

using Extensification.IntegerExts;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Editors.TextEdit;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Deletes a word or phrase from line number
    /// </summary>
    /// <remarks>
    /// You can use this command to remove an extraneous word or phrase enclosed in double quotes in a specified line number. You can use the print command to review the changes and line numbers.
    /// </remarks>
    class TextEdit_DelWordCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length == 2)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (Conversions.ToInteger(ListArgsOnly[1]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        TextEditTools.TextEdit_DeleteWord(ListArgsOnly[0], Conversions.ToInteger(ListArgsOnly[1]));
                        TextWriterColor.Write(Translate.DoTranslation("Word deleted."), true, ColorTools.ColTypes.Success);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, ColorTools.ColTypes.Error);
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, ColorTools.ColTypes.Error, ListArgsOnly[1]);
                    DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[1]);
                }
            }
            else if (ListArgsOnly.Length > 2)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[1]) & StringQuery.IsStringNumeric(ListArgsOnly[2]))
                {
                    if (Conversions.ToInteger(ListArgsOnly[1]) <= TextEditShellCommon.TextEdit_FileLines.Count & Conversions.ToInteger(ListArgsOnly[2]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        int LineNumberStart = Conversions.ToInteger(ListArgsOnly[1]);
                        int LineNumberEnd = Conversions.ToInteger(ListArgsOnly[2]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart, loopTo = LineNumberEnd; LineNumber <= loopTo; LineNumber++)
                        {
                            TextEditTools.TextEdit_DeleteWord(ListArgsOnly[0], LineNumber);
                            TextWriterColor.Write(Translate.DoTranslation("Word deleted in line {0}."), true, ColorTools.ColTypes.Success, LineNumber);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, ColorTools.ColTypes.Error);
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, ColorTools.ColTypes.Error, ListArgsOnly[1]);
                    DebugWriter.Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[1]);
                }
            }
        }

    }
}