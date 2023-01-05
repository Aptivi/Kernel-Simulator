﻿
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Lightspeed
    /// </summary>
    public static class LightspeedSettings
    {

        private static bool _CycleColors;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;

        /// <summary>
        /// [Lightspeed] Enable color cycling
        /// </summary>
        public static bool LightspeedCycleColors
        {
            get
            {
                return _CycleColors;
            }
            set
            {
                _CycleColors = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum red color level (true color)
        /// </summary>
        public static int LightspeedMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum green color level (true color)
        /// </summary>
        public static int LightspeedMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum blue color level (true color)
        /// </summary>
        public static int LightspeedMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LightspeedMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum red color level (true color)
        /// </summary>
        public static int LightspeedMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                if (value <= _MinimumRedColorLevel)
                    value = _MinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum green color level (true color)
        /// </summary>
        public static int LightspeedMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                if (value <= _MinimumGreenColorLevel)
                    value = _MinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum blue color level (true color)
        /// </summary>
        public static int LightspeedMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                if (value <= _MinimumBlueColorLevel)
                    value = _MinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LightspeedMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                if (value <= _MinimumColorLevel)
                    value = _MinimumColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Lightspeed
    /// </summary>
    public class LightspeedDisplay : BaseScreensaver, IScreensaver
    {

        private int CurrentColorR, CurrentColorG, CurrentColorB;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Lightspeed";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int MaximumColors = LightspeedSettings.LightspeedMaximumColorLevel;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", MaximumColors);
            int MaximumColorsR = LightspeedSettings.LightspeedMaximumRedColorLevel;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", MaximumColorsR);
            int MaximumColorsG = LightspeedSettings.LightspeedMaximumGreenColorLevel;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", MaximumColorsG);
            int MaximumColorsB = LightspeedSettings.LightspeedMaximumBlueColorLevel;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", MaximumColorsB);

            ConsoleBase.ConsoleWrapper.CursorVisible = false;

            // Select the background color
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors: {0}", LightspeedSettings.LightspeedCycleColors);
            if (!LightspeedSettings.LightspeedCycleColors)
            {
                int RedColorNum = RandomDriver.Random(255);
                int GreenColorNum = RandomDriver.Random(255);
                int BlueColorNum = RandomDriver.Random(255);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                ColorTools.SetConsoleColor(ColorStorage, true, true);
            }
            else
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB);
                var ColorStorage = new Color(CurrentColorR, CurrentColorG, CurrentColorB);
                ColorTools.SetConsoleColor(ColorStorage, true, true);
            }

            // Make the disco effect!
            ConsoleBase.ConsoleWrapper.Clear();

            // Switch to the next color
            if (LightspeedSettings.LightspeedCycleColors)
            {
                if (CurrentColorR >= MaximumColorsR)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Red level exceeded maximum color. {0} >= {1}", CurrentColorR, MaximumColorsR);
                    CurrentColorR = 0;
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one (R)...");
                    CurrentColorR += 1;
                }
                if (CurrentColorG >= MaximumColorsG)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Green level exceeded maximum color. {0} >= {1}", CurrentColorG, MaximumColorsG);
                    CurrentColorG = 0;
                }
                else if (CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one (G)...");
                    CurrentColorG += 1;
                }
                if (CurrentColorB >= MaximumColorsB)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Blue level exceeded maximum color. {0} >= {1}", CurrentColorB, MaximumColorsB);
                    CurrentColorB = 0;
                }
                else if (CurrentColorG == 0 & CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one (B)...");
                    CurrentColorB += 1;
                }
                if (CurrentColorB == 0 & CurrentColorG == 0 & CurrentColorR == 0)
                {
                    CurrentColorB = 0;
                    CurrentColorG = 0;
                    CurrentColorR = 0;
                }
            }

            // Check to see if we're dealing with beats per minute
            ThreadManager.SleepNoBlock(1, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
