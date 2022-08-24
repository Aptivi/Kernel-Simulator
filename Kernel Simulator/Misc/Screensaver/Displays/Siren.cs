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
using KS.Misc.Threading;
using KS.ConsoleBase.Colors;

namespace KS.Misc.Screensaver.Displays
{
    public static class SirenSettings
    {

        private static int _sirenDelay = 500;
        private static string _sirenStyle = "Cop";

        /// <summary>
        /// [Siren] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SirenDelay
        {
            get
            {
                return _sirenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                _sirenDelay = value;
            }
        }

        /// <summary>
        /// [Siren] The siren style
        /// </summary>
        public static string SirenStyle
        {
            get
            {
                return _sirenStyle;
            }
            set
            {
                _sirenStyle = SirenDisplay.sirens.ContainsKey(value) ? value : "Cop";
            }
        }

    }

    public class SirenDisplay : BaseScreensaver, IScreensaver
    {

        internal readonly static Dictionary<string, Color[]> sirens = new()
        {
            { "Cop", new Color[] { new(255, 0, 0), new(0, 0, 255) } },
            { "Ambulance", new Color[] { new(255, 0, 0), new(128, 0, 0) } }
        };
        private int step = 0;

        public override string ScreensaverName { get; set; } = "Siren";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }

        public override void ScreensaverLogic()
        {
            Console.CursorVisible = false;

            // Get step color array from siren type
            Color[] sirenColors = sirens[SirenSettings.SirenStyle];

            // Step through the color
            step += 1;
            if (step >= sirenColors.Length)
                step = 0;

            // Set color
            ColorTools.SetConsoleColor(sirenColors[step], true, true);

            // Clear screen and delay
            Console.Clear();
            ThreadManager.SleepNoBlock(SirenSettings.SirenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
