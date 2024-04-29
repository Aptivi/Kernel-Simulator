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
using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class CdbgLogCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (Flags.DebugMode)
            {
                try
                {
                    DebugWriter.DebugStreamWriter.Close();
                    DebugWriter.DebugStreamWriter = new StreamWriter(Paths.GetKernelPath(KernelPathType.Debugging)) { AutoFlush = true };
                    TextWriterColor.Write(Translate.DoTranslation("Debug log removed. All connected debugging devices may still view messages."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(Translate.DoTranslation("Debug log removal failed: {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
                    DebugWriter.WStkTrc(ex);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You must turn on debug mode before you can clear debug log."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            }
        }

    }
}