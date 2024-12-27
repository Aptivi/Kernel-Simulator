﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for HueBack
    /// </summary>
    public class HueBackDisplay : BaseScreensaver, IScreensaver
    {

        // Hue angle is in degrees and not radians
        private static int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "HueBack";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the color
            var color = new Color($"hsl:{currentHueAngle};{ScreensaverPackInit.SaversConfig.HueBackSaturation};{ScreensaverPackInit.SaversConfig.HueBackLuminance}");

            // Now, change the background color accordingly
            ColorTools.LoadBackDry(color);
            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.HueBackDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            currentHueAngle++;
            if (currentHueAngle > 360)
                currentHueAngle = 0;

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}
