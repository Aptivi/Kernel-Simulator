﻿using System;
using System.Text;
using System.Threading;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Kernel;

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

using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Misc.Splash.Splashes
{
    class SplashPowerLineProgress : ISplash
    {

        // Standalone splash information
        public string SplashName
        {
            get
            {
                return "PowerLineProgress";
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

        public int ProgressWritePositionY
        {
            get
            {
                switch (SplashSettings.PowerLineProgressProgressTextLocation)
                {
                    case TextLocation.Top:
                        {
                            return 1;
                        }
                    case TextLocation.Bottom:
                        {
                            return Console.WindowHeight - 6;
                        }

                    default:
                        {
                            return 1;
                        }
                }
            }
        }

        private readonly Color FirstColorSegmentForeground = new Color(85, 255, 255);
        private readonly Color FirstColorSegmentBackground = new Color(43, 127, 127);
        private readonly Color SecondColorSegmentForeground = new Color(0, 0, 0);
        private readonly Color SecondColorSegmentBackground = new Color(85, 255, 255);
        private readonly Color LastTransitionForeground = new Color(85, 255, 255);
        private readonly char TransitionChar = Convert.ToChar(0xE0B0);
        private readonly Random RandomDriver = new Random();

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

                // Display the progress bar
                UpdateProgressReport(SplashReport.Progress, SplashReport.ProgressText);

                // Loop until closing
                while (!SplashClosing)
                    Thread.Sleep(10);
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
            ColorTools.SetConsoleColor(ColorTools.ColTypes.Neutral);
            ColorTools.SetConsoleColor(ColorTools.BackgroundColor, true);
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
            // Variables
            var PresetStringBuilder = new StringBuilder();
            string RenderedText = ProgressReport.Truncate(Console.WindowWidth - 5);

            // Percentage
            PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat(" {0}% ", Progress.ToString().PadLeft(3));

            // Transition
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat("{0}", TransitionChar);

            // Progress text
            PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat(" {0} ", ProgressReport);

            // Transition
            PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground);
            PresetStringBuilder.Append(Flags.SetBackground ? ColorTools.BackgroundColor.VTSequenceBackground : Conversions.ToString(CharManager.GetEsc()) + $"[49m");
            PresetStringBuilder.AppendFormat("{0} ", TransitionChar);

            // Display the text and percentage
            TextWriterWhereColor.WriteWhere(PresetStringBuilder.ToString(), 0, ProgressWritePositionY, false, ColorTools.ColTypes.Progress, Vars);
            ConsoleExtensions.ClearLineToRight();

            // Display the progress bar
            if (!string.IsNullOrEmpty(SplashSettings.PowerLineProgressProgressColor) & ColorTools.TryParseColor(SplashSettings.PowerLineProgressProgressColor))
            {
                var ProgressColor = new Color(SplashSettings.PowerLineProgressProgressColor);
                ProgressBarColor.WriteProgress(Progress, 4, Console.WindowHeight - 4, ProgressColor);
            }
            else
            {
                ProgressBarColor.WriteProgress(Progress, 4, Console.WindowHeight - 4);
            }
        }

    }
}