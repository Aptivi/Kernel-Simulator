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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace KS.Misc.Splash.Splashes
{
    class SplashOpenRC : ISplash
    {

        // Standalone splash information
        public string SplashName => "openrc";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        // Private variables
        private int IndicatorLeft;
        private int IndicatorTop;
        private bool Beginning = true;
        private readonly Color OpenRCVersionColor = new(85, 255, 255);
        private readonly Color OpenRCIndicatorColor = new(ConsoleColor.Green);
        private readonly Color OpenRCPlaceholderColor = new(85, 85, 255);

        // Actual logic
        public void Opening()
        {
            Beginning = true;
            DebugWriter.Wdbg(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleWrapper.Clear();
            TextWriters.Write(Kernel.Kernel.NewLine + $"   {OpenRCIndicatorColor.VTSequenceForeground}OpenRC {OpenRCVersionColor.VTSequenceForeground}0.13.11 {KernelColorTools.NeutralTextColor.VTSequenceForeground}is starting up {OpenRCPlaceholderColor.VTSequenceForeground}Kernel Simulator {Kernel.Kernel.KernelVersion}" + Kernel.Kernel.NewLine, true, KernelColorTools.ColTypes.Neutral);
        }

        public void Display()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash displaying.");
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop;
                while (!SplashClosing)
                    Thread.Sleep(1);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            SplashClosing = true;
            DebugWriter.Wdbg(DebugLevel.I, "Splash closing. Clearing console...");
            ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
            if (!Beginning)
            {
                TextWriterWhereColor.WriteWhereColor("[    ]", IndicatorLeft, IndicatorTop, true, OpenRCPlaceholderColor);
                TextWriterWhereColor.WriteWhereColor(" ok ", IndicatorLeft + 1, IndicatorTop, true, OpenRCIndicatorColor);
            }
            TextWriterColor.WriteColor($" * ", false, OpenRCIndicatorColor);
            TextWriters.Write(ProgressReport, true, KernelColorTools.ColTypes.Neutral, Vars);
            if (!Beginning)
            {
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
        }

    }
}
