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
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Queries a character in a specified line or all lines
    /// </summary>
    /// <remarks>
    /// You can use this command to query a character and get its number from the specified line or all lines. This is useful for some commands like delcharnum.
    /// </remarks>
    class TextEdit_QueryCharCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length == 2)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (Convert.ToInt32(ListArgsOnly[1]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        var QueriedChars = TextEditTools.TextEdit_QueryChar(Convert.ToChar(ListArgsOnly[0]), Convert.ToInt32(ListArgsOnly[1]));
                        foreach (int CharIndex in QueriedChars.Keys)
                        {
                            TextWriterColor.Write("- {0}: ", false, ColorTools.ColTypes.ListEntry, CharIndex);
                            TextWriterColor.Write("{0} ({1})", true, ColorTools.ColTypes.ListValue, ListArgsOnly[0], QueriedChars[CharIndex]);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, ColorTools.ColTypes.Error);
                    }
                }
                else if (ListArgsOnly[1].ToLower() == "all")
                {
                    var QueriedChars = TextEditTools.TextEdit_QueryChar(Convert.ToChar(ListArgsOnly[0]));
                    foreach (int LineIndex in QueriedChars.Keys)
                    {
                        foreach (int CharIndex in QueriedChars[LineIndex].Keys)
                        {
                            TextWriterColor.Write("- {0}:{1}: ", false, ColorTools.ColTypes.ListEntry, LineIndex, CharIndex);
                            TextWriterColor.Write("{0} ({1})", true, ColorTools.ColTypes.ListValue, ListArgsOnly[0], TextEditShellCommon.TextEdit_FileLines[LineIndex]);
                        }
                    }
                }
            }
            else if (ListArgsOnly.Length > 2)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[1]) & StringQuery.IsStringNumeric(ListArgsOnly[2]))
                {
                    if (Convert.ToInt32(ListArgsOnly[1]) <= TextEditShellCommon.TextEdit_FileLines.Count & Convert.ToInt32(ListArgsOnly[2]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(ListArgsOnly[1]);
                        int LineNumberEnd = Convert.ToInt32(ListArgsOnly[2]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            var QueriedChars = TextEditTools.TextEdit_QueryChar(Convert.ToChar(ListArgsOnly[0]), LineNumber);
                            foreach (int CharIndex in QueriedChars.Keys)
                            {
                                TextWriterColor.Write("- {0}:{1}: ", false, ColorTools.ColTypes.ListEntry, LineNumber, CharIndex);
                                TextWriterColor.Write("{0} ({1})", true, ColorTools.ColTypes.ListValue, ListArgsOnly[0], QueriedChars[CharIndex]);
                            }
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, ColorTools.ColTypes.Error);
                    }
                }
            }
        }

    }
}
