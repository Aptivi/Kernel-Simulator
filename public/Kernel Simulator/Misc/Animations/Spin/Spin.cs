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

using ColorSeq;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using System.Text;
using System;
using KS.ConsoleBase;

namespace KS.Misc.Animations.Spin
{
    /// <summary>
    /// Spin animation module
    /// </summary>
    public static class Spin
    {

        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static int currentSpinStep = 0;
        private static readonly char[] spinSteps = new[] { '/', '|', '\\', '-' };

        /// <summary>
        /// Simulates the pulsing animation
        /// </summary>
        public static void Simulate(SpinSettings Settings)
        {
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ConsoleBase.ConsoleWrapper.CursorVisible = false;

            // Get spin character from current index
            char spinStep = spinSteps[currentSpinStep];

            StringBuilder spinBuffer = new();

            // Make a spin buffer
            for (int x = 0; x < CurrentWindowWidth; x++)
            {
                for (int y = 0; y < CurrentWindowHeight; y++)
                {
                    spinBuffer.Append(spinStep.ToString());
                }
            }

            // Spin!
            if (!ConsoleResizeListener.WasResized(false))
            {
                TextWriterWhereColor.WriteWhere(spinBuffer.ToString(), 0, 0, true, new Color(Convert.ToInt32(ConsoleColors.White)), new Color(Convert.ToInt32(ConsoleColors.Black)));
                ThreadManager.SleepNoBlock(Settings.SpinDelay, System.Threading.Thread.CurrentThread);
            }

            // Step the current spin step forward
            currentSpinStep++;
            if (currentSpinStep >= spinSteps.Length)
                currentSpinStep = 0;

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
        }

    }
}
