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
using System.Threading;
using ColorSeq;
using KS.Kernel.Debugging;
using KS.Misc.Writers.ConsoleWriters;
using KS.ConsoleBase;
using VT.NET;
using VT.NET.Tools;

namespace KS.Misc.Splash.Splashes
{
    class SplashDots : ISplash
    {

        // Standalone splash information
        public string SplashName => "Dots";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        // Private variables
        readonly Color firstColor = new(ConsoleColors.White);
        readonly Color secondColor = new(ConsoleColors.Cyan);

        // Actual logic
        public void Opening()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleWrapper.Clear();
        }

        public void Display()
        {
            try
            {
                int dotStep = 0;
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                while (!SplashClosing)
                {
                    Color firstDotColor  = dotStep >= 1 ? secondColor : firstColor;
                    Color secondDotColor = dotStep >= 2 ? secondColor : firstColor;
                    Color thirdDotColor  = dotStep >= 3 ? secondColor : firstColor;

                    // Write the three dots
                    string dots = $"{firstDotColor.VTSequenceForeground}* {secondDotColor.VTSequenceForeground}* {thirdDotColor.VTSequenceForeground}*";
                    int dotsPosX = (ConsoleWrapper.WindowWidth / 2) - (VtSequenceTools.FilterVTSequences(dots).Length / 2);
                    int dotsPosY = ConsoleWrapper.WindowHeight - 2;
                    TextWriterWhereColor.WriteWhere(dots, dotsPosX, dotsPosY);
                    Thread.Sleep(500);
                    dotStep++;
                    if (dotStep > 3)
                        dotStep = 0;
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        { }

        public void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars)
        { }

    }
}
