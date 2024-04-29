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
using System.Data;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
namespace KS.Misc.Games
{
    public static class Solver
    {

        public static int SolverMinimumNumber = 0;
        public static int SolverMaximumNumber = 1000;
        public static bool SolverShowInput;

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeSolver()
        {
            var RandomDriver = new Random();
            string RandomExpression;
            string UserEvaluated;
            string[] Operations = ["+", "-", "*", "/"];

            // Show tip to exit
            TextWriterColor.Write(Translate.DoTranslation("Press \"q\" to exit."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            DebugWriter.Wdbg(DebugLevel.I, "Initialized expressions.");
            while (true)
            {
                // Populate the numbers
                int FirstNumber = RandomDriver.Next(SolverMinimumNumber, SolverMaximumNumber);
                int OperationIndex = RandomDriver.Next(Operations.Length);
                int SecondNumber = RandomDriver.Next(SolverMinimumNumber, SolverMaximumNumber);

                // Generate the expression
                RandomExpression = FirstNumber.ToString() + Operations.ElementAt(OperationIndex) + SecondNumber.ToString();
                DebugWriter.Wdbg(DebugLevel.I, "Expression to be solved: {0}", RandomExpression);
                TextWriterColor.Write(RandomExpression, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));

                // Wait for response
                UserEvaluated = SolverShowInput ? Input.ReadLine() : Input.ReadLineNoInput('\0');
                DebugWriter.Wdbg(DebugLevel.I, "Evaluated: {0}", UserEvaluated);

                // Check to see if the user has entered the correct answer
                double EvaluatedNumber = Convert.ToDouble(new DataTable().Compute(RandomExpression, null));
                if (double.TryParse(UserEvaluated, out double UserEvaluatedNumber))
                {
                    if (UserEvaluatedNumber == EvaluatedNumber)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, EvaluatedNumber);
                        TextWriterColor.Write(Translate.DoTranslation("Solved perfectly!"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, EvaluatedNumber);
                        TextWriterColor.Write(Translate.DoTranslation("Solved incorrectly."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                    }
                }
                else if (UserEvaluated == "q")
                {
                    DebugWriter.Wdbg(DebugLevel.W, "User requested exit.");
                    break;
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.E, "User evaluated \"{0}\". However, it's not numeric.", UserEvaluated);
                    TextWriterColor.Write(Translate.DoTranslation("You can only write the numbers."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                }
            }
        }

    }
}
