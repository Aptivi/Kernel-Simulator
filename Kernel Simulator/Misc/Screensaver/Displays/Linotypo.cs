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
using System.IO;
using System.Text;
using KS.ConsoleBase.Colors;
using KS.Files.Querying;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    public static class LinotypoSettings
    {

        private static int _linotypoDelay = 50;
        private static int _linotypoNewScreenDelay = 3000;
        private static string _linotypoWrite = "Kernel Simulator";
        private static int _linotypoWritingSpeedMin = 50;
        private static int _linotypoWritingSpeedMax = 80;
        private static int _linotypoMissStrikePossibility = 1;
        private static int _linotypoTextColumns = 3;
        private static int _linotypoEtaoinThreshold = 5;
        private static int _linotypoEtaoinCappingPossibility = 5;
        private static FillType _linotypoEtaoinType = FillType.EtaoinPattern;
        private static int _linotypoMissPossibility = 10;
        private static string _linotypoTextColor = new Color(ConsoleColor.White).PlainSequence;

        /// <summary>
        /// [Linotypo] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LinotypoDelay
        {
            get
            {
                return _linotypoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _linotypoDelay = value;
            }
        }
        /// <summary>
        /// [Linotypo] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public static int LinotypoNewScreenDelay
        {
            get
            {
                return _linotypoNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _linotypoNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Linotypo] Text for Linotypo. Longer is better.
        /// </summary>
        public static string LinotypoWrite
        {
            get
            {
                return _linotypoWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Kernel Simulator";
                _linotypoWrite = value;
            }
        }
        /// <summary>
        /// [Linotypo] Minimum writing speed in WPM
        /// </summary>
        public static int LinotypoWritingSpeedMin
        {
            get
            {
                return _linotypoWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _linotypoWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Linotypo] Maximum writing speed in WPM
        /// </summary>
        public static int LinotypoWritingSpeedMax
        {
            get
            {
                return _linotypoWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                _linotypoWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Linotypo] Possibility that the writer made a typo in percent
        /// </summary>
        public static int LinotypoMissStrikePossibility
        {
            get
            {
                return _linotypoMissStrikePossibility;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                _linotypoMissStrikePossibility = value;
            }
        }
        /// <summary>
        /// [Linotypo] The text columns to be printed.
        /// </summary>
        public static int LinotypoTextColumns
        {
            get
            {
                return _linotypoTextColumns;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                if (value > 3)
                    value = 3;
                _linotypoTextColumns = value;
            }
        }
        /// <summary>
        /// [Linotypo] How many characters to write before triggering the "line fill"?
        /// </summary>
        public static int LinotypoEtaoinThreshold
        {
            get
            {
                return _linotypoEtaoinThreshold;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                if (value > 8)
                    value = 8;
                _linotypoEtaoinThreshold = value;
            }
        }
        /// <summary>
        /// [Linotypo] Possibility that the Etaoin pattern will be printed in all caps in percent
        /// </summary>
        public static int LinotypoEtaoinCappingPossibility
        {
            get
            {
                return _linotypoEtaoinCappingPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                _linotypoEtaoinCappingPossibility = value;
            }
        }
        /// <summary>
        /// [Linotypo] Line fill pattern type
        /// </summary>
        public static FillType LinotypoEtaoinType
        {
            get
            {
                return _linotypoEtaoinType;
            }
            set
            {
                _linotypoEtaoinType = value;
            }
        }
        /// <summary>
        /// [Linotypo] Possibility that the writer missed a character in percent
        /// </summary>
        public static int LinotypoMissPossibility
        {
            get
            {
                return _linotypoMissPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _linotypoMissPossibility = value;
            }
        }
        /// <summary>
        /// [Linotypo] Text color
        /// </summary>
        public static string LinotypoTextColor
        {
            get
            {
                return _linotypoTextColor;
            }
            set
            {
                _linotypoTextColor = new Color(value).PlainSequence;
            }
        }

        public enum FillType
        {
            /// <summary>
            /// Prints the known pattern of etaoin characters, such as: etaoin shrdlu
            /// </summary>
            EtaoinPattern,
            /// <summary>
            /// Prints the complete pattern of etaoin characters, such as: etaoin shrdlu cmfwyp vbgkqj xz
            /// </summary>
            EtaoinComplete,
            /// <summary>
            /// Prints the random set of characters to rapidly fill in the line
            /// </summary>
            RandomChars
        }

    }

    internal class LinotypoDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "Linotypo";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            KernelColorTools.SetConsoleColor(new Color(LinotypoSettings.LinotypoTextColor));
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            int CpmSpeedMin = LinotypoSettings.LinotypoWritingSpeedMin * 5;
            int CpmSpeedMax = LinotypoSettings.LinotypoWritingSpeedMax * 5;
            int MaxCharacters = (int)Math.Round((ConsoleWrapper.WindowWidth - 2) / (double)LinotypoSettings.LinotypoTextColumns - 3d);
            int CurrentColumn = 1;
            int CurrentColumnRowConsole = ConsoleWrapper.CursorLeft;
            int ColumnRowConsoleThreshold = (int)Math.Round(ConsoleWrapper.WindowWidth / (double)LinotypoSettings.LinotypoTextColumns);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Minimum speed from {0} WPM: {1} CPM", LinotypoSettings.LinotypoWritingSpeedMin, CpmSpeedMin);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum speed from {0} WPM: {1} CPM", LinotypoSettings.LinotypoWritingSpeedMax, CpmSpeedMax);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum characters: {0} (satisfying {1} columns)", MaxCharacters, LinotypoSettings.LinotypoTextColumns);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Width threshold: {0}", ColumnRowConsoleThreshold);

            // Strikes
            var Strikes = new List<string>() { "q`12wsa", "r43edfgt5", "u76yhjki8", @"p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa " };
            var CapStrikes = new List<string>() { "Q~!@WSA", "R$#EDFGT%", "U&^YHJKI*", "P)(OL:\"{_+}|", "?\":> ", "M<LKJN ", "VBHGFC ", "ZXDSA " };
            string CapSymbols = "~!@$#%&^*)(:\"{_+}|?><";
            string[,] LinotypeLayout = new string[,] { { "e", "t", "a", "o", "i", "n", " " }, { "s", "h", "r", "d", "l", "u", " " }, { "c", "m", "f", "w", "y", "p", " " }, { "v", "b", "g", "k", "q", "j", " " }, { "x", "z", " ", " ", " ", " ", " " }, { " ", " ", " ", " ", " ", " ", " " } };

            // Other variables
            var CountingCharacters = default(bool);
            var CharacterCounter = default(int);
            var EtaoinMode = default(bool);
            var CappedEtaoin = default(bool);
            string LinotypeWrite = LinotypoSettings.LinotypoWrite;

            // Linotypo can also deal with files written on the field that is used for storing text, so check to see if the path exists.
            DebugWriter.Wdbg(DebugLevel.I, "Checking \"{0}\" to see if it's a file path", LinotypoSettings.LinotypoWrite);
            if (Parsing.TryParsePath(LinotypoSettings.LinotypoWrite) && Checking.FileExists(LinotypoSettings.LinotypoWrite))
            {
                // File found! Now, write the contents of it to the local variable that stores the actual written text.
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Opening file {0} to write...", LinotypoSettings.LinotypoWrite);
                LinotypeWrite = File.ReadAllText(LinotypoSettings.LinotypoWrite);
            }

            // For each line, write four spaces, and extra two spaces if paragraph starts.
            var StrikeCharsIndex = default(int);
            foreach (string Paragraph in LinotypeWrite.SplitNewLines())
            {
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "New paragraph: {0}", Paragraph);

                // Sometimes, a paragraph could consist of nothing, but prints its new line, so honor this by checking to see if we need to
                // clear screen or advance to the next column so that we don't mess up the display by them
                HandleNextColumn(ref CurrentColumn, ref CurrentColumnRowConsole, ColumnRowConsoleThreshold);

                // We need to make sure that we indent spaces for each new paragraph.
                if (CurrentColumn == 1)
                {
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column 1. Printing newline...");
                    TextWriterColor.WritePlain("", true);
                }
                else
                {
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column {0}. Setting left to {1}...", CurrentColumn, CurrentColumnRowConsole);
                    ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, ConsoleWrapper.CursorTop + 1);
                }
                TextWriterColor.WritePlain("    ", false);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                bool NewLineDone = true;

                // Split the paragraph into sentences that have the length of maximum characters that can be printed for each column
                // in various terminal sizes. This enables us to easily tell if we're going to re-write the line after a typo and the
                // line completion that consists of "Etaoin shrdlu" and other nonsense written sometimes on newspapers or ads back in
                // the early 20th century.
                var IncompleteSentences = new List<string>();
                var IncompleteSentenceBuilder = new StringBuilder();
                int CharactersParsed = 0;

                // This reserved characters count tells us how many spaces are used for indenting the paragraph. This is only four for
                // the first time and will be reverted back to zero after the incomplete sentence is formed.
                int ReservedCharacters = 4;
                foreach (char ParagraphChar in Paragraph)
                {
                    if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                        ResizeSyncing = true;
                    if (ResizeSyncing)
                        break;

                    // Append the character into the incomplete sentence builder.
                    IncompleteSentenceBuilder.Append(ParagraphChar);
                    CharactersParsed += 1;

                    // Check to see if we're at the maximum character number
                    if (IncompleteSentenceBuilder.Length == MaxCharacters - ReservedCharacters | Paragraph.Length == CharactersParsed)
                    {
                        // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Adding {0} to the list... Incomplete sentences: {1}", IncompleteSentenceBuilder.ToString(), IncompleteSentences.Count);
                        IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());

                        // Clean everything up
                        IncompleteSentenceBuilder.Clear();
                        ReservedCharacters = 0;
                    }
                }

                // Get struck character and write it
                for (int IncompleteSentenceIndex = 0, loopTo = IncompleteSentences.Count - 1; IncompleteSentenceIndex <= loopTo; IncompleteSentenceIndex++)
                {
                    string IncompleteSentence = IncompleteSentences[IncompleteSentenceIndex];
                    if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                        ResizeSyncing = true;
                    if (ResizeSyncing)
                        break;

                    // Check if we need to indent a sentence
                    if (!NewLineDone)
                    {
                        if (CurrentColumn == 1)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column 1. Printing newline...");
                            TextWriterColor.WritePlain("", true);
                        }
                        else
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column {0}. Setting left to {1}...", CurrentColumn, CurrentColumnRowConsole);
                            ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, ConsoleWrapper.CursorTop + 1);
                        }
                    }
                    TextWriterColor.WritePlain("  ", false);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);

                    // We need to store which column and which key from the linotype keyboard layout is taken.
                    int LinotypeColumnIndex = 0;
                    int LinotypeKeyIndex = 0;
                    int LinotypeMaxColumnIndex = 5;

                    // Process the incomplete sentences
                    for (int StruckCharIndex = 0, loopTo1 = IncompleteSentence.Length - 1; StruckCharIndex <= loopTo1; StruckCharIndex++)
                    {
                        if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                            ResizeSyncing = true;
                        if (ResizeSyncing)
                            break;

                        // Sometimes, typing error can be made in the last line and the line is repeated on the first line in the different
                        // column, but it ruins the overall beautiful look of the paragraphs, considering how it is split in columns. We
                        // need to re-indent the sentence.
                        if (ConsoleWrapper.CursorTop == 0)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Line repeat in first line in new column. Indenting...");
                            if (CurrentColumn == 1)
                            {
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column 1. Printing newline...");
                                TextWriterColor.WritePlain("", true);
                            }
                            else
                            {
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column {0}. Setting left to {1}...", CurrentColumn, CurrentColumnRowConsole);
                                ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, ConsoleWrapper.CursorTop + 1);
                            }
                            TextWriterColor.WritePlain("  ", false);
                            if (IncompleteSentenceIndex == 0)
                                TextWriterColor.WritePlain("    ", false);
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                        }

                        // Select a character
                        char StruckChar = IncompleteSentence[StruckCharIndex];
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckChar);

                        // Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                        int SelectedCpm = RandomDriver.Next(CpmSpeedMin, CpmSpeedMax);
                        int WriteMs = (int)Math.Round(60d / SelectedCpm * 1000d);
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", SelectedCpm, WriteMs);

                        // Choose a character depending on the current mode
                        if (EtaoinMode)
                        {
                            // Doing this in linotype machines after spotting an error usually triggers a speed boost, because the authors
                            // that used this machine back then considered it as a quick way to fill the faulty line.
                            WriteMs = (int)Math.Round(WriteMs / (1d + RandomDriver.NextDouble()));
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Etaoin mode on. Delaying {0} ms...", WriteMs);

                            // Get the character
                            StruckChar = Convert.ToChar(LinotypeLayout[LinotypeColumnIndex, LinotypeKeyIndex]);
                            if (CappedEtaoin)
                            {
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Capped Etaoin.");
                                StruckChar = char.ToUpper(StruckChar);
                            }

                            // Advance the indexes of column and key, depending on their values, and get the character
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Etaoin type: {0}", LinotypoSettings.LinotypoEtaoinType);
                            switch (LinotypoSettings.LinotypoEtaoinType)
                            {
                                case LinotypoSettings.FillType.EtaoinComplete:
                                case LinotypoSettings.FillType.EtaoinPattern:
                                    {
                                        if (LinotypoSettings.LinotypoEtaoinType == LinotypoSettings.FillType.EtaoinPattern)
                                            LinotypeMaxColumnIndex = 1;

                                        // Increment the key (and optionally column) index. If both exceed the max limit, reset both to zero.
                                        LinotypeKeyIndex += 1;
                                        if (LinotypeKeyIndex == 7)
                                        {
                                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Key index exceeded 7. Advancing to next column...");
                                            LinotypeKeyIndex = 0;
                                            LinotypeColumnIndex += 1;
                                        }
                                        if (LinotypeColumnIndex == LinotypeMaxColumnIndex + 1)
                                        {
                                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column indexes exceeded. Resetting...");
                                            LinotypeColumnIndex = 0;
                                            LinotypeKeyIndex = 0;
                                        }

                                        break;
                                    }
                                case LinotypoSettings.FillType.RandomChars:
                                    {
                                        // Randomly select the linotype indexes
                                        LinotypeColumnIndex = RandomDriver.Next(0, 5);
                                        LinotypeKeyIndex = RandomDriver.Next(0, 6);
                                        break;
                                    }
                            }
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Key index: {0} | Column index: {1}", LinotypeKeyIndex, LinotypeColumnIndex);
                        }
                        else
                        {
                            // See if the typo is guaranteed
                            double Probability = (LinotypoSettings.LinotypoMissStrikePossibility >= 5 ? 5 : LinotypoSettings.LinotypoMissStrikePossibility) / 100d;
                            bool LinotypoGuaranteed = RandomDriver.NextDouble() < Probability;
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Probability: {0} Guarantee: {1}", Probability, LinotypoGuaranteed);
                            if (LinotypoGuaranteed)
                            {
                                // Sometimes, a typo is generated by missing a character.
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Made a typo!");
                                double MissProbability = (LinotypoSettings.LinotypoMissPossibility >= 10 ? 10 : LinotypoSettings.LinotypoMissPossibility) / 100d;
                                bool MissGuaranteed = RandomDriver.NextDouble() < MissProbability;
                                if (MissGuaranteed)
                                {
                                    // Miss is guaranteed. Simulate the missed character
                                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Missed a character!");
                                    StruckChar = '\0';
                                }
                                // Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                                else
                                {
                                    bool StruckFound = false;
                                    bool CappedStrike = false;
                                    string StrikesString = "";
                                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Bruteforcing...");
                                    while (!StruckFound)
                                    {
                                        StrikeCharsIndex = RandomDriver.Next(0, Strikes.Count - 1);
                                        CappedStrike = char.IsUpper(StruckChar) | CapSymbols.Contains(Convert.ToString(StruckChar));
                                        StrikesString = CappedStrike ? CapStrikes[StrikeCharsIndex] : Strikes[StrikeCharsIndex];
                                        StruckFound = !string.IsNullOrEmpty(StrikesString) && StrikesString.Contains(Convert.ToString(StruckChar));
                                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Strike chars index: {0}", StrikeCharsIndex);
                                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Capped strike: {0}", CappedStrike);
                                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Strikes pattern: {0}", StrikesString);
                                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Found? {0}", StruckFound);
                                    }
                                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Found!");

                                    // Select a random character that is a typo from the selected strike index
                                    int RandomStrikeIndex = RandomDriver.Next(0, StrikesString.Length - 1);
                                    char MistypedChar = StrikesString[RandomStrikeIndex];
                                    if (@"`-=\][';/.,".Contains(Convert.ToString(MistypedChar)) & CappedStrike)
                                    {
                                        // The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Mistyped character is a symbol and the strike is capped.");
                                        MistypedChar = CapStrikes[StrikeCharsIndex][RandomStrikeIndex];
                                    }
                                    StruckChar = MistypedChar;
                                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckChar);
                                }

                                // Randomly select whether or not to turn on the capped Etaoin
                                double CappingProbability = (LinotypoSettings.LinotypoEtaoinCappingPossibility >= 10 ? 10 : LinotypoSettings.LinotypoEtaoinCappingPossibility) / 100d;
                                CappedEtaoin = RandomDriver.NextDouble() < CappingProbability;
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Capped Etaoin: {0}", CappedEtaoin);

                                // Trigger character counter mode
                                CountingCharacters = true;
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Counting...");
                            }
                        }

                        // Write the final character to the console and wait
                        if (!(Convert.ToString(StruckChar) == "\0"))
                            TextWriterColor.WritePlain(Convert.ToString(StruckChar), false);
                        ThreadManager.SleepNoBlock(WriteMs, ScreensaverDisplayer.ScreensaverDisplayerThread);

                        // If we're on the character counter mode, increment this for every character until the "line fill" mode starts
                        if (CountingCharacters)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Counter increased. {0}", CharacterCounter);
                            CharacterCounter += 1;
                            if (CharacterCounter > LinotypoSettings.LinotypoEtaoinThreshold)
                            {
                                // We've reached the Etaoin threshold. Turn on that mode and stop counting characters.
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Etaoin mode on because threshold reached.");
                                EtaoinMode = true;
                                CountingCharacters = false;
                                CharacterCounter = 0;
                            }
                        }

                        // If we're on the Etaoin mode and we've reached the end of incomplete sentence, reset the index to 0 and do the
                        // necessary changes.
                        if (EtaoinMode & (StruckCharIndex == MaxCharacters - 1 | StruckCharIndex == IncompleteSentence.Length - 1))
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Etaoin mode off because end of sentence.");
                            StruckCharIndex = -1;
                            EtaoinMode = false;
                            if (ConsoleWrapper.CursorTop >= ConsoleWrapper.WindowHeight - 2)
                            {
                                HandleNextColumn(ref CurrentColumn, ref CurrentColumnRowConsole, ColumnRowConsoleThreshold);
                            }
                            else
                            {
                                if (CurrentColumn == 1)
                                {
                                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column 1. Printing newline...");
                                    TextWriterColor.WritePlain("", true);
                                }
                                else
                                {
                                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column {0}. Setting left to {1}...", CurrentColumn, CurrentColumnRowConsole);
                                    ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, ConsoleWrapper.CursorTop + 1);
                                }
                                TextWriterColor.WritePlain("  ", false);
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                            }
                        }
                    }

                    // Let the next sentence generate a new line
                    NewLineDone = false;

                    // It's possible that the writer might have made an error on writing a line on the very end of it where the threshold is
                    // lower than the partial sentence being written, so don't do the Etaoin pattern in this case, but re-write the text as
                    // if the error is being made.
                    if (CountingCharacters)
                    {
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Sentence ended before Etaoin mode is activated. Resetting counter...");
                        CountingCharacters = false;
                        CharacterCounter = 0;
                        IncompleteSentenceIndex -= 1;
                    }

                    // See if the current cursor is on the bottom so we can make the second column, if we have more than a column assigned
                    HandleNextColumn(ref CurrentColumn, ref CurrentColumnRowConsole, ColumnRowConsoleThreshold);
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(LinotypoSettings.LinotypoDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Instructs the Linotypo screensaver to go to the next column
        /// </summary>
        /// <param name="CurrentColumn"></param>
        /// <param name="CurrentColumnRowConsole"></param>
        /// <param name="ColumnRowConsoleThreshold"></param>
        public void HandleNextColumn(ref int CurrentColumn, ref int CurrentColumnRowConsole, int ColumnRowConsoleThreshold)
        {
            if (LinotypoSettings.LinotypoTextColumns > 1)
            {
                if (ConsoleWrapper.CursorTop >= ConsoleWrapper.WindowHeight - 2)
                {
                    // We're on the bottom, so...
                    if (CurrentColumn >= LinotypoSettings.LinotypoTextColumns)
                    {
                        // ...wait until retry
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn all columns. Waiting {0} ms...", LinotypoSettings.LinotypoNewScreenDelay);
                        TextWriterColor.WritePlain("", true);
                        ThreadManager.SleepNoBlock(LinotypoSettings.LinotypoNewScreenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

                        // ...and make a new screen
                        ConsoleWrapper.Clear();
                        CurrentColumn = 1;
                        CurrentColumnRowConsole = ConsoleWrapper.CursorLeft;
                    }
                    else
                    {
                        // ...we're moving to the next column
                        CurrentColumn += 1;
                        CurrentColumnRowConsole += ColumnRowConsoleThreshold;
                        ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, 0);
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "New column. Moving to {0}...", CurrentColumnRowConsole);
                    }
                }
            }
            else if (LinotypoSettings.LinotypoTextColumns == 1 & ConsoleWrapper.CursorTop >= ConsoleWrapper.WindowHeight - 2)
            {
                // We're on the bottom, so wait until retry...
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn all text. Waiting {0} ms...", LinotypoSettings.LinotypoNewScreenDelay);
                TextWriterColor.WritePlain("", true);
                ThreadManager.SleepNoBlock(LinotypoSettings.LinotypoNewScreenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

                // ...and make a new screen
                ConsoleWrapper.Clear();
                CurrentColumn = 1;
                CurrentColumnRowConsole = ConsoleWrapper.CursorLeft;
            }
        }

    }
}