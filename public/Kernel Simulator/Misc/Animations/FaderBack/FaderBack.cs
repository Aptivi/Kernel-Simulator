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
using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Screensaver;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Animations.FaderBack
{
    /// <summary>
    /// Background fader animation module
    /// </summary>
    public static class FaderBack
    {

        /// <summary>
        /// Simulates the background fading animation
        /// </summary>
        public static void Simulate(FaderBackSettings Settings)
        {
            int RedColorNum = RandomDriver.Random(Settings.FaderBackMinimumRedColorLevel, Settings.FaderBackMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.FaderBackMinimumGreenColorLevel, Settings.FaderBackMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.FaderBackMinimumBlueColorLevel, Settings.FaderBackMaximumBlueColorLevel);
            ConsoleBase.ConsoleWrapper.CursorVisible = false;

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.FaderBackMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.FaderBackMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.FaderBackMaxSteps;
            DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.FaderBackMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.FaderBackMaxSteps);
                ThreadManager.SleepNoBlock(Settings.FaderBackDelay, System.Threading.Thread.CurrentThread);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                ColorTools.LoadBack(new Color($"{CurrentColorRedIn};{CurrentColorGreenIn};{CurrentColorBlueIn}"), true);
            }

            // Wait until fade out
            if (!ConsoleResizeListener.WasResized(false))
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", Settings.FaderBackFadeOutDelay);
                ThreadManager.SleepNoBlock(Settings.FaderBackFadeOutDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.FaderBackMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.FaderBackMaxSteps);
                ThreadManager.SleepNoBlock(Settings.FaderBackDelay, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                ColorTools.LoadBack(new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), true);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(Settings.FaderBackDelay, System.Threading.Thread.CurrentThread);
        }

    }
}
