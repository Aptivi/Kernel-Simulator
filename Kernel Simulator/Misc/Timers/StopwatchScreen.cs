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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.ConsoleBase.Writers;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace KS.Misc.Timers
{
    public static class StopwatchScreen
    {

        internal static List<LapDisplayInfo> Laps = [];
        internal static KernelThread StopwatchUpdate = new("Stopwatch ETA Updater", true, UpdateStopwatchElapsedDisplay);
        internal static Color LapColor = KernelColorTools.NeutralTextColor;
        internal static Stopwatch Stopwatch = new();
        internal static Stopwatch LappedStopwatch = new();
        internal static bool NewLapAcknowledged;
        internal static bool stopwatchStopping;

        /// <summary>
        /// Opens the stopwatch screen
        /// </summary>
        public static void OpenStopwatch()
        {
            // Clear for cleanliness
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;

            // Populate the positions for time
            string LapsText = Translate.DoTranslation("Lap");
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            int TimeLeftPosition = (int)Math.Round(HalfWidth * 1.5d - Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult).Length / 2d);
            int TimeTopPosition = HalfHeight - 2;
            int LapsCurrentLapLeftPosition = 4;
            int LapsCurrentLapTopPosition = ConsoleWrapper.WindowHeight - 6;

            // Populate the keys text variable
            string KeysText = "[ENTER] " + Translate.DoTranslation("Start or stop") + " | [L] " + Translate.DoTranslation("Lap") + " | [R] " + Translate.DoTranslation("Reset") + " | [ESC] " + Translate.DoTranslation("Exit");
            int KeysTextLeftPosition = (int)Math.Round(HalfWidth - KeysText.Length / 2d);
            int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 2;
            var KeysKeypress = default(ConsoleKey);

            // Print the keys text
            TextWriters.WriteWhere(KeysText, KeysTextLeftPosition, KeysTextTopPosition, true, KernelColorTools.ColTypes.Tip);

            // Print the time interval and the current lap
            TextWriterWhereColor.WriteWhereColor(Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult), TimeLeftPosition, TimeTopPosition, true, LapColor);
            TextWriterWhereColor.WriteWhereColor(LapsText + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, true, LapColor, vars: [Laps.Count + 1, LappedStopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult)]);

            // Print the border
            MakeBorder();

            while (KeysKeypress != ConsoleKey.Escape)
            {
                // Wait for a keypress
                KeysKeypress = Input.DetectKeypress().Key;

                // Check for a keypress
                switch (KeysKeypress)
                {
                    case ConsoleKey.Enter:
                        {
                            if (!StopwatchUpdate.IsAlive)
                            {
                                stopwatchStopping = false;
                                StopwatchUpdate.Start();
                            }
                            else
                            {
                                stopwatchStopping = true;
                                StopwatchUpdate.Stop();
                            }
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Stop();
                            else
                                LappedStopwatch.Start();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Stop();
                            else
                                Stopwatch.Start();
                            break;
                        }
                    case ConsoleKey.L:
                        {
                            if (LappedStopwatch.IsRunning)
                            {
                                var Lap = new LapDisplayInfo(LapColor, LappedStopwatch.Elapsed);
                                Laps.Add(Lap);
                                LappedStopwatch.Restart();
                                NewLapAcknowledged = true;

                                // Select random color
                                var Randomizer = new Random();
                                int RedValue = Randomizer.Next(255);
                                int GreenValue = Randomizer.Next(255);
                                int BlueValue = Randomizer.Next(255);
                                LapColor = new Color(RedValue, GreenValue, BlueValue);
                            }

                            break;
                        }
                    case ConsoleKey.R:
                        {
                            if (StopwatchUpdate.IsAlive)
                            {
                                stopwatchStopping = true;
                                StopwatchUpdate.Stop();
                            }
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Reset();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Reset();

                            // Clear the laps and the laps list
                            Laps.Clear();
                            for (int Y = 1, loopTo = LapsCurrentLapTopPosition - 1; Y <= loopTo; Y++)
                            {
                                ConsoleWrapper.SetCursorPosition(LapsCurrentLapLeftPosition, Y);
                                ConsoleBase.ConsoleExtensions.ClearLineToRight();
                            }

                            // Reset the indicators
                            LapColor = KernelColorTools.NeutralTextColor;
                            TextWriterWhereColor.WriteWhereColor(Translate.DoTranslation("Lap") + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, false, LapColor, vars: [Laps.Count + 1, LappedStopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult)]);
                            ConsoleBase.ConsoleExtensions.ClearLineToRight();
                            ConsoleWrapper.SetCursorPosition(0, TimeTopPosition);
                            ConsoleBase.ConsoleExtensions.ClearLineToRight();
                            TextWriterWhereColor.WriteWhereColor(Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult), TimeLeftPosition, TimeTopPosition, false, LapColor);
                            MakeBorder();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Reset();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Reset();
                            LapColor = KernelColorTools.NeutralTextColor;
                            if (StopwatchUpdate.IsAlive)
                            {
                                stopwatchStopping = true;
                                StopwatchUpdate.Stop();
                            }
                            break;
                        }
                }
            }

            // Clear for cleanliness
            Laps.Clear();
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = true;
            stopwatchStopping = false;
        }

        /// <summary>
        /// Updates the elapsed display for stopwatch
        /// </summary>
        private static void UpdateStopwatchElapsedDisplay()
        {
            // Populate the positions for time and for lap list
            string LapsText = Translate.DoTranslation("Lap");
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            int TimeLeftPosition = (int)Math.Round(HalfWidth * 1.5d - Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult).Length / 2d);
            int TimeTopPosition = HalfHeight - 2;
            int LapsCurrentLapLeftPosition = 4;
            int LapsCurrentLapTopPosition = ConsoleWrapper.WindowHeight - 6;
            int LapsLapsListLeftPosition = 4;
            int LapsLapsListTopPosition = 3;

            while (!stopwatchStopping)
            {
                try
                {
                    // Update the elapsed display
                    TextWriterWhereColor.WriteWhereColor(Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult), TimeLeftPosition, TimeTopPosition, true, LapColor);
                    TextWriterWhereColor.WriteWhereColor(LapsText + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, true, LapColor, vars: [Laps.Count + 1, LappedStopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult)]);

                    // Update the laps list if new lap is acknowledged
                    if (NewLapAcknowledged)
                    {
                        int LapsListEndBorder = ConsoleWrapper.WindowHeight - 10;
                        var LapsListBuilder = new StringBuilder();
                        int BorderDifference = Laps.Count - LapsListEndBorder;
                        if (BorderDifference < 0)
                            BorderDifference = 0;
                        for (int LapIndex = BorderDifference, loopTo = Laps.Count - 1; LapIndex <= loopTo; LapIndex++)
                        {
                            var Lap = Laps[LapIndex];
                            LapsListBuilder.AppendLine(Lap.LapColor.VTSequenceForeground + Translate.DoTranslation("Lap") + $" {LapIndex + 1}: {Lap.LapInterval.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult)}");
                        }
                        TextWriterWhereColor.WriteWhereColor(LapsListBuilder.ToString(), LapsLapsListLeftPosition, LapsLapsListTopPosition, true, LapColor);
                        NewLapAcknowledged = false;
                    }
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Makes the display border
        /// </summary>
        public static void MakeBorder()
        {
            int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 2;
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            TextWriters.WriteWhere("═".Repeat(ConsoleWrapper.WindowWidth), 0, KeysTextTopPosition - 2, true, KernelColorTools.ColTypes.Gray);
            TextWriters.WriteWhere("═".Repeat(ConsoleWrapper.WindowWidth), 0, 1, true, KernelColorTools.ColTypes.Gray);
            for (int Height = 2, loopTo = KeysTextTopPosition - 2; Height <= loopTo; Height++)
                TextWriters.WriteWhere("║", HalfWidth, Height, true, KernelColorTools.ColTypes.Gray);
            TextWriters.WriteWhere("╩", HalfWidth, KeysTextTopPosition - 2, true, KernelColorTools.ColTypes.Gray);
            TextWriters.WriteWhere("╦", HalfWidth, 1, true, KernelColorTools.ColTypes.Gray);
        }

    }
}
