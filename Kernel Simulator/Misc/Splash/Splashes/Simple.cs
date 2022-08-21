﻿using System;

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

using System.Threading;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Splash.Splashes
{
    class SplashSimple : ISplash
    {

        // Standalone splash information
        public string SplashName
        {
            get
            {
                return "Simple";
            }
        }

        private SplashInfo Info
        {
            get
            {
                return SplashManager.Splashes[SplashName];
            }
        }

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress
        {
            get
            {
                return Info.DisplaysProgress;
            }
        }

        public int ProgressWritePositionX
        {
            get
            {
                return 3;
            }
        }

        public int ProgressWritePositionY
        {
            get
            {
                switch (SplashSettings.SimpleProgressTextLocation)
                {
                    case TextLocation.Top:
                        {
                            return 1;
                        }
                    case TextLocation.Bottom:
                        {
                            return Console.WindowHeight - 2;
                        }

                    default:
                        {
                            return 1;
                        }
                }
            }
        }

        public int ProgressReportWritePositionX
        {
            get
            {
                return 9;
            }
        }

        public int ProgressReportWritePositionY
        {
            get
            {
                switch (SplashSettings.SimpleProgressTextLocation)
                {
                    case TextLocation.Top:
                        {
                            return 1;
                        }
                    case TextLocation.Bottom:
                        {
                            return Console.WindowHeight - 2;
                        }

                    default:
                        {
                            return 1;
                        }
                }
            }
        }

        // Actual logic
        public void Opening()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Splash opening. Clearing console...");
            Console.Clear();
        }

        public void Display()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash displaying.");

                // Display the progress text
                UpdateProgressReport(SplashReport.Progress, SplashReport.ProgressText, ProgressWritePositionX, ProgressWritePositionY, ProgressReportWritePositionX, ProgressReportWritePositionY);

                // Loop until closing
                while (!SplashClosing)
                    Thread.Sleep(1);
            }
            catch (ThreadInterruptedException ex)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            SplashClosing = true;
            DebugWriter.Wdbg(DebugLevel.I, "Splash closing. Clearing console...");
            Console.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
            UpdateProgressReport(Progress, ProgressReport, Vars);
        }

        /// <summary>
        /// Updates the splash progress
        /// </summary>
        /// <param name="Progress">Progress percentage from 0 to 100</param>
        /// <param name="ProgressReport">The progress text</param>
        public void UpdateProgressReport(int Progress, string ProgressReport, params object[] Vars)
        {
            string RenderedText = ProgressReport.Truncate(Console.WindowWidth - ProgressReportWritePositionX - ProgressWritePositionX - 3);
            TextWriterWhereColor.WriteWhere("{0}%", ProgressWritePositionX, ProgressWritePositionY, true, ColorTools.ColTypes.Progress, Progress.ToString().PadLeft(3));
            TextWriterWhereColor.WriteWhere(RenderedText, ProgressReportWritePositionX, ProgressReportWritePositionY, false, ColorTools.ColTypes.Neutral, Vars);
            ConsoleExtensions.ClearLineToRight();
        }

    }
}