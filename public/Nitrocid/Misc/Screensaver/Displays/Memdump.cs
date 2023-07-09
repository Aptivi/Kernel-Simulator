﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Memdump
    /// </summary>
    public static class MemdumpSettings
    {

        /// <summary>
        /// [Memdump] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int MemdumpDelay
        {
            get
            {
                return Config.SaverConfig.MemdumpDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                Config.SaverConfig.MemdumpDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Memdump
    /// </summary>
    public class MemdumpDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Memdump";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Two boxes, one for the initial color, and one for the bit-shifted color
            BorderColor.WriteBorder(2, 1, 8, 3);
            BorderColor.WriteBorder(13, 1, 8, 3);

            // Some positions
            int infoTop = 6;
            int initialInfoMargin = 4;
            int shiftedInfoMargin = 15;

            // Get the initial color values
            int initialR = RandomDriver.Random(255);
            int initialG = RandomDriver.Random(255);
            int initialB = RandomDriver.Random(255);
            var initialColor = new Color(initialR, initialG, initialB);

            // Get the hexadecimal value for display and decimal value for shifting
            string initialHex = $"{initialR:X2}{initialG:X2}{initialB:X2}";
            long initialValue = Convert.ToInt64($"{initialR}{initialG}{initialB}");

            // Get the colors from the decimal value by shifting the bits from it and then the hex
            int shiftedR = (int)(initialValue & 0xFF0000) >> 16;
            int shiftedG = (int)(initialValue & 0xFF00) >> 8;
            int shiftedB = (int) initialValue & 0xFF;
            string shiftedHex = $"{shiftedR:X2}{shiftedG:X2}{shiftedB:X2}";
            var shiftedColor = new Color(shiftedR, shiftedG, shiftedB);

            // Now, fill the colors!
            for (int i = 0; i < 3; i++)
                TextWriterWhereColor.WriteWhere(new string(' ', 8), 3, 2 + i, Color.Empty, initialColor);
            for (int i = 0; i < 3; i++)
                TextWriterWhereColor.WriteWhere(new string(' ', 8), 14, 2 + i, Color.Empty, shiftedColor);

            // Print the hexes
            TextWriterWhereColor.WriteWhere(initialHex, initialInfoMargin, infoTop, initialColor);
            TextWriterWhereColor.WriteWhere(shiftedHex, shiftedInfoMargin, infoTop, shiftedColor);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(MemdumpSettings.MemdumpDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
