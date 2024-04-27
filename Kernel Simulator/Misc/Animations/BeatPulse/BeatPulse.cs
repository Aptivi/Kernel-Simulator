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

using KS.Misc.Screensaver;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace KS.Misc.Animations.BeatPulse
{
    public static class BeatPulse
    {

        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static bool ResizeSyncing;

        /// <summary>
        /// Simulates the beat pulsing animation
        /// </summary>
        public static void Simulate(BeatPulseSettings Settings)
        {
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            var RandomDriver = Settings.RandomDriver;
            ConsoleWrapper.CursorVisible = false;
            int BeatInterval = (int)Math.Round(60000d / Settings.BeatPulseDelay);
            int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)Settings.BeatPulseMaxSteps);
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", Settings.BeatPulseDelay, BeatInterval);
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", Settings.BeatPulseDelay, BeatIntervalStep);
            ThreadManager.SleepNoBlock(BeatIntervalStep, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // If we're cycling colors, set them. Else, use the user-provided color
            int RedColorNum = default, GreenColorNum = default, BlueColorNum = default;
            if (Settings.BeatPulseCycleColors)
            {
                // We're cycling. Select the color mode, starting from true color
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (Settings.BeatPulseTrueColor)
                {
                    RedColorNum = RandomDriver.Next(Settings.BeatPulseMinimumRedColorLevel, Settings.BeatPulseMinimumRedColorLevel);
                    GreenColorNum = RandomDriver.Next(Settings.BeatPulseMinimumGreenColorLevel, Settings.BeatPulseMaximumGreenColorLevel);
                    BlueColorNum = RandomDriver.Next(Settings.BeatPulseMinimumBlueColorLevel, Settings.BeatPulseMaximumBlueColorLevel);
                }
                else if (Settings.BeatPulse255Colors)
                {
                    var ConsoleColor = new Color((ConsoleColors)RandomDriver.Next(Settings.BeatPulseMinimumColorLevel, Settings.BeatPulseMaximumColorLevel));
                    RedColorNum = ConsoleColor.RGB.R;
                    GreenColorNum = ConsoleColor.RGB.G;
                    BlueColorNum = ConsoleColor.RGB.B;
                }
                else
                {
                    var ConsoleColor = new Color((ConsoleColors)RandomDriver.Next(Settings.BeatPulseMinimumColorLevel, Settings.BeatPulseMaximumColorLevel));
                    RedColorNum = ConsoleColor.RGB.R;
                    GreenColorNum = ConsoleColor.RGB.G;
                    BlueColorNum = ConsoleColor.RGB.B;
                }
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                // We're not cycling. Parse the color and then select the color mode, starting from true color
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", Settings.BeatPulseBeatColor);
                var UserColor = new Color(Settings.BeatPulseBeatColor);
                if (UserColor.Type == ColorType.TrueColor)
                {
                    RedColorNum = UserColor.RGB.R;
                    GreenColorNum = UserColor.RGB.G;
                    BlueColorNum = UserColor.RGB.B;
                }
                else if (UserColor.Type == ColorType.EightBitColor)
                {
                    var ConsoleColor = new Color((ConsoleColors)Convert.ToInt32(UserColor.PlainSequence));
                    RedColorNum = ConsoleColor.RGB.R;
                    GreenColorNum = ConsoleColor.RGB.G;
                    BlueColorNum = ConsoleColor.RGB.B;
                }
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.BeatPulseMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.BeatPulseMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.BeatPulseMaxSteps;
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.BeatPulseMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, BeatIntervalStep);
                ThreadManager.SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    KernelColorTools.SetConsoleColor(new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn), true);
                    ConsoleWrapper.Clear();
                }
            }

            // Fade out
            for (int CurrentStep = 1, loopTo = Settings.BeatPulseMaxSteps; CurrentStep <= loopTo; CurrentStep++)
            {
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", CurrentStep, Settings.BeatPulseMaxSteps, BeatIntervalStep);
                ThreadManager.SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    KernelColorTools.SetConsoleColor(new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), true);
                    ConsoleWrapper.Clear();
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(Settings.BeatPulseDelay, System.Threading.Thread.CurrentThread);
        }

    }
}
