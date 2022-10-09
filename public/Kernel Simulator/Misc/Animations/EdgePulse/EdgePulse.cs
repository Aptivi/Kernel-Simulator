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
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;

namespace KS.Misc.Animations.EdgePulse
{
    /// <summary>
    /// Edge pulse animation module
    /// </summary>
    public static class EdgePulse
    {

        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static bool ResizeSyncing;

        /// <summary>
        /// Simulates the edge pulsing animation
        /// </summary>
        public static void Simulate(EdgePulseSettings Settings)
        {
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;

            // Now, do the rest
            int RedColorNum = RandomDriver.Random(Settings.EdgePulseMinimumRedColorLevel, Settings.EdgePulseMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.EdgePulseMinimumGreenColorLevel, Settings.EdgePulseMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.EdgePulseMinimumBlueColorLevel, Settings.EdgePulseMaximumBlueColorLevel);
            ConsoleBase.ConsoleWrapper.CursorVisible = false;

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.EdgePulseMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.EdgePulseMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.EdgePulseMaxSteps;
            DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.EdgePulseMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.EdgePulseMaxSteps);
                ThreadManager.SleepNoBlock(Settings.EdgePulseDelay, System.Threading.Thread.CurrentThread);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn), true, true);
                    FillIn();
                }
            }

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.EdgePulseMaxSteps; CurrentStep++)
            {
                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.EdgePulseMaxSteps);
                ThreadManager.SleepNoBlock(Settings.EdgePulseDelay, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut), true, true);
                    FillIn();
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(Settings.EdgePulseDelay, System.Threading.Thread.CurrentThread);
        }

        private static void FillIn()
        {
            int FloorTopLeftEdge = 0;
            int FloorBottomLeftEdge = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", FloorTopLeftEdge, FloorBottomLeftEdge);

            int FloorTopRightEdge = ConsoleBase.ConsoleWrapper.WindowWidth - 1;
            int FloorBottomRightEdge = ConsoleBase.ConsoleWrapper.WindowWidth - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", FloorTopRightEdge, FloorBottomRightEdge);

            int FloorTopEdge = 0;
            int FloorBottomEdge = ConsoleBase.ConsoleWrapper.WindowHeight - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", FloorTopEdge, FloorBottomEdge);

            int FloorLeftEdge = 0;
            int FloorRightEdge = ConsoleBase.ConsoleWrapper.WindowWidth - 2;
            DebugWriter.WriteDebug(DebugLevel.I, "Left edge: {0}, Right edge: {1}", FloorLeftEdge, FloorRightEdge);

            // First, draw the floor top edge
            for (int x = FloorTopLeftEdge; x <= FloorTopRightEdge; x++)
            {
                ConsoleBase.ConsoleWrapper.SetCursorPosition(x, 0);
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor top edge ({0}, {1})", x, 1);
                ConsoleBase.ConsoleWrapper.Write(" ");
            }

            // Second, draw the floor bottom edge
            for (int x = FloorBottomLeftEdge; x <= FloorBottomRightEdge; x++)
            {
                ConsoleBase.ConsoleWrapper.SetCursorPosition(x, FloorBottomEdge);
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", x, FloorBottomEdge);
                ConsoleBase.ConsoleWrapper.Write(" ");
            }

            // Third, draw the floor left edge
            for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
            {
                ConsoleBase.ConsoleWrapper.SetCursorPosition(FloorLeftEdge, y);
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor left edge ({0}, {1})", FloorLeftEdge, y);
                ConsoleBase.ConsoleWrapper.Write("  ");
            }

            // Finally, draw the floor right edge
            for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
            {
                ConsoleBase.ConsoleWrapper.SetCursorPosition(FloorRightEdge, y);
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor right edge ({0}, {1})", FloorRightEdge, y);
                ConsoleBase.ConsoleWrapper.Write("  ");
            }
        }

    }
}
