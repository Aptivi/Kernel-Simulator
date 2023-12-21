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

using KS.Misc.Games;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    public static class SnakerSettings
    {

        private static bool _snaker255Colors;
        private static bool _snakerTrueColor = true;
        private static int _snakerDelay = 100;
        private static int _snakerStageDelay = 5000;
        private static int _snakerMinimumRedColorLevel = 0;
        private static int _snakerMinimumGreenColorLevel = 0;
        private static int _snakerMinimumBlueColorLevel = 0;
        private static int _snakerMinimumColorLevel = 0;
        private static int _snakerMaximumRedColorLevel = 255;
        private static int _snakerMaximumGreenColorLevel = 255;
        private static int _snakerMaximumBlueColorLevel = 255;
        private static int _snakerMaximumColorLevel = 255;

        /// <summary>
        /// [Snaker] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool Snaker255Colors
        {
            get
            {
                return _snaker255Colors;
            }
            set
            {
                _snaker255Colors = value;
            }
        }
        /// <summary>
        /// [Snaker] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool SnakerTrueColor
        {
            get
            {
                return _snakerTrueColor;
            }
            set
            {
                _snakerTrueColor = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SnakerDelay
        {
            get
            {
                return _snakerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                _snakerDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next stage?
        /// </summary>
        public static int SnakerStageDelay
        {
            get
            {
                return _snakerStageDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                _snakerStageDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum red color level (true color)
        /// </summary>
        public static int SnakerMinimumRedColorLevel
        {
            get
            {
                return _snakerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _snakerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum green color level (true color)
        /// </summary>
        public static int SnakerMinimumGreenColorLevel
        {
            get
            {
                return _snakerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _snakerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum blue color level (true color)
        /// </summary>
        public static int SnakerMinimumBlueColorLevel
        {
            get
            {
                return _snakerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _snakerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakerMinimumColorLevel
        {
            get
            {
                return _snakerMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _snaker255Colors | _snakerTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _snakerMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum red color level (true color)
        /// </summary>
        public static int SnakerMaximumRedColorLevel
        {
            get
            {
                return _snakerMaximumRedColorLevel;
            }
            set
            {
                if (value <= _snakerMinimumRedColorLevel)
                    value = _snakerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _snakerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum green color level (true color)
        /// </summary>
        public static int SnakerMaximumGreenColorLevel
        {
            get
            {
                return _snakerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _snakerMinimumGreenColorLevel)
                    value = _snakerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _snakerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum blue color level (true color)
        /// </summary>
        public static int SnakerMaximumBlueColorLevel
        {
            get
            {
                return _snakerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _snakerMinimumBlueColorLevel)
                    value = _snakerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _snakerMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakerMaximumColorLevel
        {
            get
            {
                return _snakerMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _snaker255Colors | _snakerTrueColor ? 255 : 15;
                if (value <= _snakerMinimumColorLevel)
                    value = _snakerMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _snakerMaximumColorLevel = value;
            }
        }

    }

    public class SnakerDisplay : BaseScreensaver, IScreensaver
    {

        public override string ScreensaverName { get; set; } = "Snaker";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleWrapper.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            Snaker.InitializeSnaker(true);
            ThreadManager.SleepNoBlock(SnakerSettings.SnakerDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}