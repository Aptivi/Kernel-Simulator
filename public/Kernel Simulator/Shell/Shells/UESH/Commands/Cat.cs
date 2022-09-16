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
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files.Print;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Prints files to console.
    /// </summary>
    /// <remarks>
    /// This command lets you print the contents of a text file to the console.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-lines</term>
    /// <description>Prints the line numbers alongside the contents</description>
    /// </item>
    /// <item>
    /// <term>-nolines</term>
    /// <description>Prints only the contents</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class CatCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                bool PrintLines = Flags.PrintLineNumbers;
                bool ForcePlain = false;
                if (ListSwitchesOnly.Contains("-lines"))
                    PrintLines = true;
                if (ListSwitchesOnly.Contains("-nolines"))
                    // -lines and -nolines cancel together.
                    PrintLines = false; 
                if (ListSwitchesOnly.Contains("-plain"))
                    ForcePlain = true;
                FileContentPrinter.PrintContents(ListArgsOnly[0], PrintLines, ForcePlain);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(ex.Message, true, ColorTools.ColTypes.Error);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, ColorTools.ColTypes.NeutralText);
            TextWriterColor.Write("  -lines: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Prints the line numbers that follow the line being printed"), true, ColorTools.ColTypes.ListValue);
            TextWriterColor.Write("  -nolines: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Prevents printing the line numbers"), true, ColorTools.ColTypes.ListValue);
        }

    }
}
