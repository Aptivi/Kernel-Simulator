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
using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;

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

namespace KS.Misc.Screensaver.Displays
{
    public static class ColorMixSettings
    {

        private static bool _colorMix255Colors;
        private static bool _colorMixTrueColor = true;
        private static int _colorMixDelay = 1;
        private static string _colorMixBackgroundColor = new Color(ConsoleColor.Red).PlainSequence;
        private static int _colorMixMinimumRedColorLevel = 0;
        private static int _colorMixMinimumGreenColorLevel = 0;
        private static int _colorMixMinimumBlueColorLevel = 0;
        private static int _colorMixMinimumColorLevel = 0;
        private static int _colorMixMaximumRedColorLevel = 255;
        private static int _colorMixMaximumGreenColorLevel = 255;
        private static int _colorMixMaximumBlueColorLevel = 255;
        private static int _colorMixMaximumColorLevel = 255;

        /// <summary>
        /// [ColorMix] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool ColorMix255Colors
        {
            get
            {
                return _colorMix255Colors;
            }
            set
            {
                _colorMix255Colors = value;
            }
        }
        /// <summary>
        /// [ColorMix] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ColorMixTrueColor
        {
            get
            {
                return _colorMixTrueColor;
            }
            set
            {
                _colorMixTrueColor = value;
            }
        }
        /// <summary>
        /// [ColorMix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ColorMixDelay
        {
            get
            {
                return _colorMixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                _colorMixDelay = value;
            }
        }
        /// <summary>
        /// [ColorMix] Screensaver background color
        /// </summary>
        public static string ColorMixBackgroundColor
        {
            get
            {
                return _colorMixBackgroundColor;
            }
            set
            {
                _colorMixBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum red color level (true color)
        /// </summary>
        public static int ColorMixMinimumRedColorLevel
        {
            get
            {
                return _colorMixMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _colorMixMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum green color level (true color)
        /// </summary>
        public static int ColorMixMinimumGreenColorLevel
        {
            get
            {
                return _colorMixMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _colorMixMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum blue color level (true color)
        /// </summary>
        public static int ColorMixMinimumBlueColorLevel
        {
            get
            {
                return _colorMixMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _colorMixMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorMixMinimumColorLevel
        {
            get
            {
                return _colorMixMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _colorMix255Colors | _colorMixTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _colorMixMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum red color level (true color)
        /// </summary>
        public static int ColorMixMaximumRedColorLevel
        {
            get
            {
                return _colorMixMaximumRedColorLevel;
            }
            set
            {
                if (value <= _colorMixMinimumRedColorLevel)
                    value = _colorMixMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _colorMixMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum green color level (true color)
        /// </summary>
        public static int ColorMixMaximumGreenColorLevel
        {
            get
            {
                return _colorMixMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _colorMixMinimumGreenColorLevel)
                    value = _colorMixMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _colorMixMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum blue color level (true color)
        /// </summary>
        public static int ColorMixMaximumBlueColorLevel
        {
            get
            {
                return _colorMixMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _colorMixMinimumBlueColorLevel)
                    value = _colorMixMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _colorMixMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorMixMaximumColorLevel
        {
            get
            {
                return _colorMixMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _colorMix255Colors | _colorMixTrueColor ? 255 : 15;
                if (value <= _colorMixMinimumColorLevel)
                    value = _colorMixMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _colorMixMaximumColorLevel = value;
            }
        }

    }

    public class ColorMixDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "ColorMix";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            KernelColorTools.SetConsoleColor(new Color(ColorMixSettings.ColorMixBackgroundColor), true);
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleWrapper.Clear();
        }

        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            // Set colors
            if (ColorMixSettings.ColorMixTrueColor)
            {
                int RedColorNum = RandomDriver.Next(ColorMixSettings.ColorMixMinimumRedColorLevel, ColorMixSettings.ColorMixMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Next(ColorMixSettings.ColorMixMinimumGreenColorLevel, ColorMixSettings.ColorMixMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Next(ColorMixSettings.ColorMixMinimumBlueColorLevel, ColorMixSettings.ColorMixMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    KernelColorTools.SetConsoleColor(ColorStorage, true);
                    TextWriterColor.WritePlain(" ", false);
                }
            }
            else if (ColorMixSettings.ColorMix255Colors)
            {
                int ColorNum = RandomDriver.Next(ColorMixSettings.ColorMixMinimumColorLevel, ColorMixSettings.ColorMixMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    KernelColorTools.SetConsoleColor(new Color(ColorNum), true);
                    TextWriterColor.WritePlain(" ", false);
                }
            }
            else
            {
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    Console.BackgroundColor = (ConsoleColor)RandomDriver.Next(ColorMixSettings.ColorMixMinimumColorLevel, ColorMixSettings.ColorMixMaximumColorLevel);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor);
                    TextWriterColor.WritePlain(" ", false);
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(ColorMixSettings.ColorMixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}