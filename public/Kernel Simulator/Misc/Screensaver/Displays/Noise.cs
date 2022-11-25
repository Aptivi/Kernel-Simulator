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
using System.Collections;
using System.Collections.Generic;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Misc.Threading;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Noise
    /// </summary>
    public static class NoiseSettings
    {

        private static int _noiseNewScreenDelay = 5000;
        private static int _noiseDensity = 40;

        /// <summary>
        /// [Noise] How many milliseconds to wait before making the new screen?
        /// </summary>
        public static int NoiseNewScreenDelay
        {
            get
            {
                return _noiseNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                _noiseNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Noise] The noise density in percent
        /// </summary>
        public static int NoiseDensity
        {
            get
            {
                return _noiseDensity;
            }
            set
            {
                if (value < 0)
                    value = 40;
                if (value > 100)
                    value = 40;
                _noiseDensity = value;
            }
        }

    }

    /// <summary>
    /// Display code for Noise
    /// </summary>
    public class NoiseDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Noise";

        /// <inheritdoc/>
        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            double NoiseDense = (NoiseSettings.NoiseDensity > 100 ? 100 : NoiseSettings.NoiseDensity) / 100d;

            ConsoleWrapper.BackgroundColor = ConsoleColor.DarkGray;
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;

            // Select random positions to generate noise
            int AmountOfBlocks = ConsoleWrapper.WindowWidth * ConsoleWrapper.WindowHeight;
            int BlocksToCover = (int)Math.Round(AmountOfBlocks * NoiseDense);
            var CoveredBlocks = new ArrayList();
            while (!(CoveredBlocks.Count == BlocksToCover | ConsoleResizeListener.WasResized(false)))
            {
                if (!ConsoleResizeListener.WasResized(false))
                {
                    int CoverX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                    int CoverY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                    ConsoleWrapper.SetCursorPosition(CoverX, CoverY);
                    ConsoleWrapper.Write(" ");
                    if (!CoveredBlocks.Contains(CoverX.ToString() + ", " + CoverY.ToString()))
                        CoveredBlocks.Add(CoverX.ToString() + ", " + CoverY.ToString());
                }
                else
                {
                    // We're resizing.
                    break;
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(NoiseSettings.NoiseNewScreenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
