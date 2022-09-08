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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Misc.Animations.BeatEdgePulse;

namespace KS.Misc.Splash.Splashes
{
    class SplashBeatEdgePulse : ISplash
    {

        // Standalone splash information
        public string SplashName
        {
            get
            {
                return "BeatEdgePulse";
            }
        }

        private SplashInfo Info
        {
            get
            {
                return SplashManager.Splashes[SplashName];
            }
        }

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress
        {
            get
            {
                return Info.DisplaysProgress;
            }
        }

        // BeatEdgePulse-specific variables
        internal BeatEdgePulseSettings BeatEdgePulseSettings;
        internal Random RandomDriver;

        public SplashBeatEdgePulse() => BeatEdgePulseSettings = new BeatEdgePulseSettings()
        {
            BeatEdgePulse255Colors = false,
            BeatEdgePulseTrueColor = true,
            BeatEdgePulseBeatColor = 17.ToString(),
            BeatEdgePulseCycleColors = true,
            BeatEdgePulseDelay = 50,
            BeatEdgePulseMaxSteps = 30,
            BeatEdgePulseMinimumRedColorLevel = 0,
            BeatEdgePulseMinimumGreenColorLevel = 0,
            BeatEdgePulseMinimumBlueColorLevel = 0,
            BeatEdgePulseMinimumColorLevel = 0,
            BeatEdgePulseMaximumRedColorLevel = 255,
            BeatEdgePulseMaximumGreenColorLevel = 255,
            BeatEdgePulseMaximumBlueColorLevel = 255,
            BeatEdgePulseMaximumColorLevel = 255
        };

        // Actual logic
        public void Opening()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
            RandomDriver = new Random();
            BeatEdgePulseSettings.RandomDriver = RandomDriver;
        }

        public void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Loop until we got a closing notification
                while (!SplashClosing)
                    BeatEdgePulse.Simulate(BeatEdgePulseSettings);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            SplashClosing = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            ColorTools.SetConsoleColor(ColorTools.BackgroundColor, true);
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
        }

    }
}