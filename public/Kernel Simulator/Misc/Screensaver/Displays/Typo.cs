﻿
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Typo
    /// </summary>
    public static class TypoSettings
    {

        private static int _Delay = 50;
        private static int _WriteAgainDelay = 3000;
        private static string _Write = "Kernel Simulator";
        private static int _WritingSpeedMin = 50;
        private static int _WritingSpeedMax = 80;
        private static int _MissStrikePossibility = 20;
        private static int _MissPossibility = 10;
        private static string _TextColor = new Color((int)ConsoleColor.White).PlainSequence;

        /// <summary>
        /// [Typo] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TypoDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _Delay = value;
            }
        }
        /// <summary>
        /// [Typo] How many milliseconds to wait before writing the text again?
        /// </summary>
        public static int TypoWriteAgainDelay
        {
            get
            {
                return _WriteAgainDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _WriteAgainDelay = value;
            }
        }
        /// <summary>
        /// [Typo] Text for Typo. Longer is better.
        /// </summary>
        public static string TypoWrite
        {
            get
            {
                return _Write;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Kernel Simulator";
                _Write = value;
            }
        }
        /// <summary>
        /// [Typo] Minimum writing speed in WPM
        /// </summary>
        public static int TypoWritingSpeedMin
        {
            get
            {
                return _WritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _WritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Typo] Maximum writing speed in WPM
        /// </summary>
        public static int TypoWritingSpeedMax
        {
            get
            {
                return _WritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                _WritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer made a typo in percent
        /// </summary>
        public static int TypoMissStrikePossibility
        {
            get
            {
                return _MissStrikePossibility;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                _MissStrikePossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer missed a character in percent
        /// </summary>
        public static int TypoMissPossibility
        {
            get
            {
                return _MissPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _MissPossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Text color
        /// </summary>
        public static string TypoTextColor
        {
            get
            {
                return _TextColor;
            }
            set
            {
                _TextColor = new Color(value).PlainSequence;
            }
        }

    }

    /// <summary>
    /// Display code for Typo
    /// </summary>
    public class TypoDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Typo";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.SetConsoleColor(new Color(TypoSettings.TypoTextColor));
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int CpmSpeedMin = TypoSettings.TypoWritingSpeedMin * 5;
            int CpmSpeedMax = TypoSettings.TypoWritingSpeedMax * 5;
            var Strikes = new List<string>() { "q`12wsa", "r43edfgt5", "u76yhjki8", @"p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa " };
            var CapStrikes = new List<string>() { "Q~!@WSA", "R$#EDFGT%", "U&^YHJKI*", "P)(OL:\"{_+}|", "?\":> ", "M<LKJN ", "VBHGFC ", "ZXDSA " };
            string CapSymbols = "~!@$#%&^*)(:\"{_+}|?><";

            ConsoleWrapper.CursorVisible = false;

            // Prepare display (make a paragraph indentation)
            ConsoleWrapper.WriteLine();
            ConsoleWrapper.Write("    ");
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);

            // Get struck character and write it
            var StrikeCharsIndex = default(int);
            char StruckCharAssigned = default(char);
            foreach (char StruckChar in TypoSettings.TypoWrite)
            {
                StruckCharAssigned = StruckChar;

                // Check to see if we can go ahead
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                int SelectedCpm = RandomDriver.Random(CpmSpeedMin, CpmSpeedMax);
                int WriteMs = (int)Math.Round(60d / SelectedCpm * 1000d);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", SelectedCpm, WriteMs);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckCharAssigned);

                // See if the typo is guaranteed
                double Probability = (TypoSettings.TypoMissStrikePossibility >= 80 ? 80 : TypoSettings.TypoMissStrikePossibility) / 100d;
                bool TypoGuaranteed = RandomDriver.RandomChance(Probability);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Probability: {0} Guarantee: {1}", Probability, TypoGuaranteed);
                if (TypoGuaranteed)
                {
                    // Sometimes, a typo is generated by missing a character.
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Made a typo!");
                    double MissProbability = (TypoSettings.TypoMissPossibility >= 10 ? 10 : TypoSettings.TypoMissPossibility) / 100d;
                    bool MissGuaranteed = RandomDriver.RandomChance(MissProbability);
                    if (MissGuaranteed)
                    {
                        // Miss is guaranteed. Simulate the missed character
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Missed a character!");
                        StruckCharAssigned = Convert.ToChar("\0");
                    }
                    // Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                    else
                    {
                        bool StruckFound = false;
                        bool CappedStrike = false;
                        string StrikesString = "";
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Bruteforcing...");
                        while (!StruckFound)
                        {
                            StrikeCharsIndex = RandomDriver.RandomIdx(0, Strikes.Count);
                            CappedStrike = char.IsUpper(StruckCharAssigned) | CapSymbols.Contains(Convert.ToString(StruckCharAssigned));
                            StrikesString = CappedStrike ? CapStrikes[StrikeCharsIndex] : Strikes[StrikeCharsIndex];
                            StruckFound = !string.IsNullOrEmpty(StrikesString) && StrikesString.Contains(Convert.ToString(StruckCharAssigned));
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Strike chars index: {0}", StrikeCharsIndex);
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Capped strike: {0}", CappedStrike);
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Strikes pattern: {0}", StrikesString);
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Found? {0}", StruckFound);
                        }
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Found!");

                        // Select a random character that is a typo from the selected strike index
                        int RandomStrikeIndex = RandomDriver.RandomIdx(0, StrikesString.Length);
                        char MistypedChar = StrikesString[RandomStrikeIndex];
                        if (@"`-=\][';/.,".Contains(Convert.ToString(MistypedChar)) & CappedStrike)
                        {
                            // The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Mistyped character is a symbol and the strike is capped.");
                            MistypedChar = CapStrikes[StrikeCharsIndex][RandomStrikeIndex];
                        }
                        StruckCharAssigned = MistypedChar;
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckCharAssigned);
                    }
                }

                // Write the final character to the console and wait
                if (!(StruckCharAssigned == Convert.ToChar(0)))
                    ConsoleWrapper.Write(StruckCharAssigned);
                ThreadManager.SleepNoBlock(WriteMs, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Wait until retry
            ConsoleWrapper.WriteLine();
            if (!ConsoleResizeListener.WasResized(false))
                ThreadManager.SleepNoBlock(TypoSettings.TypoWriteAgainDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(TypoSettings.TypoDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
