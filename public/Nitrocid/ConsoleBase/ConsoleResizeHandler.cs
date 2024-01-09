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

using Terminaux.Base.Buffered;
using Nitrocid.Drivers;
using Nitrocid.Drivers.Console;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Events;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.ResizeListener;

namespace Nitrocid.ConsoleBase
{
    /// <summary>
    /// The console resize listener module
    /// </summary>
    public static class ConsoleResizeHandler
    {
        internal static bool usesSigWinch;
        internal static bool ResizeDetected;

        internal static void HandleResize(int oldX, int oldY, int newX, int newY)
        {
            ResizeDetected = true;

            // We need to call the WindowHeight and WindowWidth properties on the Terminal console driver, because
            // this polling works for all the terminals. Other drivers that don't use the terminal may not even
            // implement these two properties.
            var termDriver = DriverHandler.GetFallbackDriver<IConsoleDriver>();
            DebugWriter.WriteDebug(DebugLevel.W, "Console resize detected! Terminaux reported old width x height: {0}x{1} | New width x height: {2}x{3}", oldX, oldY, newX, newY);
            newX = termDriver.WindowWidth;
            newY = termDriver.WindowHeight;
            DebugWriter.WriteDebug(DebugLevel.W, "Final: Old width x height: {0}x{1} | New width x height: {2}x{3}", oldX, oldY, newX, newY);
            DebugWriter.WriteDebug(DebugLevel.W, $"Userspace application will have to call {nameof(ConsoleResizeHandler.ResizeDetected)} to reset the state.");
            EventsManager.FireEvent(EventType.ResizeDetected, oldX, oldY, newX, newY);

            // Also, tell the screen-based apps to refresh themselves
            if (ScreenTools.CurrentScreen is not null && !ScreensaverManager.InSaver)
                ScreenTools.Render();

            // Also, tell the screensaver application to refresh itself
            if (ScreensaverManager.InSaver)
                ScreensaverDisplayer.displayingSaver.ScreensaverResizeSync();
        }
    }
}
