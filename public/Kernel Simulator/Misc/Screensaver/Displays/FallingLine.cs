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
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for FallingLine
    /// </summary>
    public static class FallingLineSettings
    {

        private static bool _fallingLineTrueColor = true;
        private static int _fallingLineDelay = 10;
        private static int _fallingLineMaxSteps = 25;
        private static int _fallingLineMinimumRedColorLevel = 0;
        private static int _fallingLineMinimumGreenColorLevel = 0;
        private static int _fallingLineMinimumBlueColorLevel = 0;
        private static int _fallingLineMinimumColorLevel = 0;
        private static int _fallingLineMaximumRedColorLevel = 255;
        private static int _fallingLineMaximumGreenColorLevel = 255;
        private static int _fallingLineMaximumBlueColorLevel = 255;
        private static int _fallingLineMaximumColorLevel = 255;

        /// <summary>
        /// [FallingLine] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FallingLineTrueColor
        {
            get
            {
                return _fallingLineTrueColor;
            }
            set
            {
                _fallingLineTrueColor = value;
            }
        }
        /// <summary>
        /// [FallingLine] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FallingLineDelay
        {
            get
            {
                return _fallingLineDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _fallingLineDelay = value;
            }
        }
        /// <summary>
        /// [FallingLine] How many fade steps to do?
        /// </summary>
        public static int FallingLineMaxSteps
        {
            get
            {
                return _fallingLineMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _fallingLineMaxSteps = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum red color level (true color)
        /// </summary>
        public static int FallingLineMinimumRedColorLevel
        {
            get
            {
                return _fallingLineMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _fallingLineMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum green color level (true color)
        /// </summary>
        public static int FallingLineMinimumGreenColorLevel
        {
            get
            {
                return _fallingLineMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _fallingLineMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum blue color level (true color)
        /// </summary>
        public static int FallingLineMinimumBlueColorLevel
        {
            get
            {
                return _fallingLineMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _fallingLineMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FallingLineMinimumColorLevel
        {
            get
            {
                return _fallingLineMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _fallingLineMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum red color level (true color)
        /// </summary>
        public static int FallingLineMaximumRedColorLevel
        {
            get
            {
                return _fallingLineMaximumRedColorLevel;
            }
            set
            {
                if (value <= _fallingLineMinimumRedColorLevel)
                    value = _fallingLineMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _fallingLineMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum green color level (true color)
        /// </summary>
        public static int FallingLineMaximumGreenColorLevel
        {
            get
            {
                return _fallingLineMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _fallingLineMinimumGreenColorLevel)
                    value = _fallingLineMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _fallingLineMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum blue color level (true color)
        /// </summary>
        public static int FallingLineMaximumBlueColorLevel
        {
            get
            {
                return _fallingLineMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _fallingLineMinimumBlueColorLevel)
                    value = _fallingLineMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _fallingLineMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FallingLineMaximumColorLevel
        {
            get
            {
                return _fallingLineMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _fallingLineMinimumColorLevel)
                    value = _fallingLineMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _fallingLineMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for FallingLine
    /// </summary>
    public class FallingLineDisplay : BaseScreensaver, IScreensaver
    {

        private int ColumnLine;
        private readonly List<Tuple<int, int>> CoveredPositions = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FallingLine";

        /// <inheritdoc/>
        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleBase.ConsoleWrapper.Clear();
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Choose the column for the falling line
            ColumnLine = RandomDriver.RandomIdx(ConsoleBase.ConsoleWrapper.WindowWidth);

            // Now, determine the fall start and end position
            int FallStart = 0;
            int FallEnd = ConsoleBase.ConsoleWrapper.WindowHeight - 1;

            // Select the color
            Color ColorStorage;
            if (FallingLineSettings.FallingLineTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FallingLineSettings.FallingLineMinimumRedColorLevel, FallingLineSettings.FallingLineMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FallingLineSettings.FallingLineMinimumGreenColorLevel, FallingLineSettings.FallingLineMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FallingLineSettings.FallingLineMinimumBlueColorLevel, FallingLineSettings.FallingLineMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                ColorTools.SetConsoleColor(ColorStorage, true, true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(FallingLineSettings.FallingLineMinimumColorLevel, FallingLineSettings.FallingLineMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                ColorStorage = new Color(ColorNum);
                ColorTools.SetConsoleColor(ColorStorage, true, true);
            }

            // Make the line fall down
            for (int Fall = FallStart; Fall <= FallEnd; Fall++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Print a block and add the covered position to the list so fading down can be done
                TextWriterWhereColor.WriteWhere(" ", ColumnLine, Fall, false);
                var PositionTuple = new Tuple<int, int>(ColumnLine, Fall);
                CoveredPositions.Add(PositionTuple);

                // Delay
                ThreadManager.SleepNoBlock(FallingLineSettings.FallingLineDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Fade the line down. Please note that this requires true-color support in the terminal to work properly.
            for (int StepNum = 0; StepNum <= FallingLineSettings.FallingLineMaxSteps; StepNum++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Set thresholds
                double ThresholdRed = ColorStorage.R / (double)FallingLineSettings.FallingLineMaxSteps;
                double ThresholdGreen = ColorStorage.G / (double)FallingLineSettings.FallingLineMaxSteps;
                double ThresholdBlue = ColorStorage.B / (double)FallingLineSettings.FallingLineMaxSteps;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

                // Set color fade steps
                int CurrentColorRedOut = (int)Math.Round(ColorStorage.R - ThresholdRed * StepNum);
                int CurrentColorGreenOut = (int)Math.Round(ColorStorage.G - ThresholdGreen * StepNum);
                int CurrentColorBlueOut = (int)Math.Round(ColorStorage.B - ThresholdBlue * StepNum);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);

                // Get the positions and write the block with new color
                var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                foreach (Tuple<int, int> PositionTuple in CoveredPositions)
                {
                    // Check to see if user decided to resize
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    // Actually fade the line out
                    int PositionLeft = PositionTuple.Item1;
                    int PositionTop = PositionTuple.Item2;
                    TextWriterWhereColor.WriteWhere(" ", PositionLeft, PositionTop, false, Color.Empty, CurrentFadeColor);
                }

                // Delay
                ThreadManager.SleepNoBlock(FallingLineSettings.FallingLineDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(FallingLineSettings.FallingLineDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
