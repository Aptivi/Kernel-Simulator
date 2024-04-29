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
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using StringMath;

namespace KS.Shell.Commands
{
    class CalcCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                string Res = ((MathExpr)StringArgs).Result.ToString();
                DebugWriter.Wdbg(DebugLevel.I, "Res = {0}", Res);
                TextWriterColor.Write(StringArgs + " = " + Res, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Error trying to calculate expression {0}: {1}", StringArgs, ex.Message);
                DebugWriter.WStkTrc(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error in calculation.") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
            }
        }

    }
}
