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
    public static class BeatFaderSettings
    {

        private static bool _beatFader255Colors;
        private static bool _beatFaderTrueColor = true;
        private static bool _beatFaderCycleColors = true;
        private static string _beatFaderBeatColor = 17.ToString();
        private static int _beatFaderDelay = 120;
        private static int _beatFaderMaxSteps = 25;
        private static int _beatFaderMinimumRedColorLevel = 0;
        private static int _beatFaderMinimumGreenColorLevel = 0;
        private static int _beatFaderMinimumBlueColorLevel = 0;
        private static int _beatFaderMinimumColorLevel = 0;
        private static int _beatFaderMaximumRedColorLevel = 255;
        private static int _beatFaderMaximumGreenColorLevel = 255;
        private static int _beatFaderMaximumBlueColorLevel = 255;
        private static int _beatFaderMaximumColorLevel = 255;

        /// <summary>
        /// [BeatFader] Enable 255 color support. Has a higher priority than 16 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool BeatFader255Colors
        {
            get
            {
                return _beatFader255Colors;
            }
            set
            {
                _beatFader255Colors = value;
            }
        }
        /// <summary>
        /// [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool BeatFaderTrueColor
        {
            get
            {
                return _beatFaderTrueColor;
            }
            set
            {
                _beatFaderTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        /// </summary>
        public static bool BeatFaderCycleColors
        {
            get
            {
                return _beatFaderCycleColors;
            }
            set
            {
                _beatFaderCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatFaderBeatColor
        {
            get
            {
                return _beatFaderBeatColor;
            }
            set
            {
                _beatFaderBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatFader] How many beats per minute to wait before making the next write?
        /// </summary>
        public static int BeatFaderDelay
        {
            get
            {
                return _beatFaderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 120;
                _beatFaderDelay = value;
            }
        }
        /// <summary>
        /// [BeatFader] How many fade steps to do?
        /// </summary>
        public static int BeatFaderMaxSteps
        {
            get
            {
                return _beatFaderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _beatFaderMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum red color level (true color)
        /// </summary>
        public static int BeatFaderMinimumRedColorLevel
        {
            get
            {
                return _beatFaderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatFaderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum green color level (true color)
        /// </summary>
        public static int BeatFaderMinimumGreenColorLevel
        {
            get
            {
                return _beatFaderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatFaderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum blue color level (true color)
        /// </summary>
        public static int BeatFaderMinimumBlueColorLevel
        {
            get
            {
                return _beatFaderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatFaderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatFaderMinimumColorLevel
        {
            get
            {
                return _beatFaderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _beatFader255Colors | _beatFaderTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _beatFaderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum red color level (true color)
        /// </summary>
        public static int BeatFaderMaximumRedColorLevel
        {
            get
            {
                return _beatFaderMaximumRedColorLevel;
            }
            set
            {
                if (value <= _beatFaderMinimumRedColorLevel)
                    value = _beatFaderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _beatFaderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum green color level (true color)
        /// </summary>
        public static int BeatFaderMaximumGreenColorLevel
        {
            get
            {
                return _beatFaderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _beatFaderMinimumGreenColorLevel)
                    value = _beatFaderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _beatFaderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum blue color level (true color)
        /// </summary>
        public static int BeatFaderMaximumBlueColorLevel
        {
            get
            {
                return _beatFaderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _beatFaderMinimumBlueColorLevel)
                    value = _beatFaderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _beatFaderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatFaderMaximumColorLevel
        {
            get
            {
                return _beatFaderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _beatFader255Colors | _beatFaderTrueColor ? 255 : 15;
                if (value <= _beatFaderMinimumColorLevel)
                    value = _beatFaderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _beatFaderMaximumColorLevel = value;
            }
        }

    }

    public class BeatFaderDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private Animations.BeatFader.BeatFaderSettings BeatFaderSettingsInstance;

        public override string ScreensaverName { get; set; } = "BeatFader";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            Console.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            BeatFaderSettingsInstance = new Animations.BeatFader.BeatFaderSettings()
            {
                BeatFader255Colors = BeatFaderSettings.BeatFader255Colors,
                BeatFaderTrueColor = BeatFaderSettings.BeatFaderTrueColor,
                BeatFaderBeatColor = BeatFaderSettings.BeatFaderBeatColor,
                BeatFaderDelay = BeatFaderSettings.BeatFaderDelay,
                BeatFaderMaxSteps = BeatFaderSettings.BeatFaderMaxSteps,
                BeatFaderCycleColors = BeatFaderSettings.BeatFaderCycleColors,
                BeatFaderMinimumRedColorLevel = BeatFaderSettings.BeatFaderMinimumRedColorLevel,
                BeatFaderMinimumGreenColorLevel = BeatFaderSettings.BeatFaderMinimumGreenColorLevel,
                BeatFaderMinimumBlueColorLevel = BeatFaderSettings.BeatFaderMinimumBlueColorLevel,
                BeatFaderMinimumColorLevel = BeatFaderSettings.BeatFaderMinimumColorLevel,
                BeatFaderMaximumRedColorLevel = BeatFaderSettings.BeatFaderMaximumRedColorLevel,
                BeatFaderMaximumGreenColorLevel = BeatFaderSettings.BeatFaderMaximumGreenColorLevel,
                BeatFaderMaximumBlueColorLevel = BeatFaderSettings.BeatFaderMaximumBlueColorLevel,
                BeatFaderMaximumColorLevel = BeatFaderSettings.BeatFaderMaximumColorLevel,
                RandomDriver = RandomDriver
            };
        }

        public override void ScreensaverLogic()
        {
            Animations.BeatFader.BeatFader.Simulate(BeatFaderSettingsInstance);
        }

    }
}
