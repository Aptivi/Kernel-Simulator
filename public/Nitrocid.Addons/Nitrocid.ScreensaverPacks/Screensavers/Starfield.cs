﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Starfield
    /// </summary>
    public static class StarfieldSettings
    {

        /// <summary>
        /// [Starfield] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int StarfieldDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StarfieldDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                ScreensaverPackInit.SaversConfig.StarfieldDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Starfield
    /// </summary>
    public class StarfieldDisplay : BaseScreensaver, IScreensaver
    {

        private readonly List<Tuple<int, int>> Stars = [];

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Starfield";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Move the stars left
            for (int Star = 0; Star <= Stars.Count - 1; Star++)
            {
                int StarX = Stars[Star].Item1 - 1;
                int StarY = Stars[Star].Item2;
                Stars[Star] = new Tuple<int, int>(StarX, StarY);
            }

            // If any star is out of X range, delete it
            for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
            {
                var Star = Stars[StarIndex];
                if (Star.Item1 < 0)
                {
                    // The star went beyond. Remove it.
                    Stars.RemoveAt(StarIndex);
                }
            }

            // Add new star if guaranteed
            bool StarShowGuaranteed = RandomDriver.RandomChance(10);
            if (StarShowGuaranteed)
            {
                int StarX = ConsoleWrapper.WindowWidth - 1;
                int StarY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                Stars.Add(new Tuple<int, int>(StarX, StarY));
            }

            // Draw stars
            var starsBuffer = new StringBuilder();
            for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                var Star = Stars[StarIndex];
                char StarSymbol = '*';
                int StarX = Star.Item1;
                int StarY = Star.Item2;
                starsBuffer.Append(TextWriterWhereColor.RenderWhere(Convert.ToString(StarSymbol), StarX, StarY, false, ConsoleColors.White, ConsoleColors.Black));
            }
            if (ConsoleResizeHandler.WasResized(false))
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                Stars.Clear();
            }
            else
                TextWriterColor.WritePlain(starsBuffer.ToString(), false);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(StarfieldSettings.StarfieldDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            Stars.Clear();
        }

    }
}
