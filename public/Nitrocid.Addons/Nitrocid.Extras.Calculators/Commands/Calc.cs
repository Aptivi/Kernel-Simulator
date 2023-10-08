﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using StringMath;

namespace Nitrocid.Extras.Calculators.Commands
{
    /// <summary>
    /// Calculates expressions.
    /// </summary>
    /// <remarks>
    /// This command lets you calculate expressions and return the results.
    /// </remarks>
    class CalcCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                double Res = ((MathExpr)parameters.ArgumentsText).Result;
                DebugWriter.WriteDebug(DebugLevel.I, "Res = {0}", Res);
                TextWriterColor.Write(parameters.ArgumentsText + " = " + Res.ToString());
                variableValue = Res.ToString();
                return 0;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Error trying to calculate expression {0}: {1}", parameters.ArgumentsText, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Error in calculation.") + " {0}", true, KernelColorType.Error, ex.Message);
                return ex.GetHashCode();
            }
        }

    }
}
