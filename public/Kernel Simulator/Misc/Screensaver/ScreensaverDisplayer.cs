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
using System.Threading;
using KS.Kernel.Debugging;
using KS.Kernel;
using KS.Misc.Threading;

namespace KS.Misc.Screensaver
{
    /// <summary>
    /// Screensaver display module
    /// </summary>
    public static class ScreensaverDisplayer
    {

        internal readonly static KernelThread ScreensaverDisplayerThread = new("Screensaver display thread", false, (ss) => DisplayScreensaver((BaseScreensaver)ss));
        internal static bool OutOfRandom;

        /// <summary>
        /// Displays the screensaver from the screensaver base
        /// </summary>
        /// <param name="Screensaver">Screensaver base containing information about the screensaver</param>
        public static void DisplayScreensaver(BaseScreensaver Screensaver)
        {
            try
            {
                // Preparations
                OutOfRandom = false;
                Screensaver.ScreensaverPreparation();

                // Execute the actual screensaver logic
                while (!OutOfRandom)
                    Screensaver.ScreensaverLogic();
            }
            catch (ThreadInterruptedException)
            {
                Misc.Screensaver.Screensaver.HandleSaverCancel();
            }
            catch (Exception ex)
            {
                Misc.Screensaver.Screensaver.HandleSaverError(ex);
            }
            finally
            {
                OutOfRandom = true;
                Screensaver.ScreensaverOutro();
            }
        }

        internal static void BailFromScreensaver()
        {
            if (Screensaver.InSaver)
            {
                ScreensaverDisplayerThread.Stop();
                Screensaver.SaverAutoReset.WaitOne();

                // Raise event
                DebugWriter.WriteDebug(DebugLevel.I, "Screensaver really stopped.");
                Kernel.Events.EventsManager.FireEvent("PostShowScreensaver");
                Screensaver.inSaver = false;
                Flags.ScrnTimeReached = false;
            }
        }

    }
}
