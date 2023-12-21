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

using KS.Files.Querying;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    public static class TypewriterSettings
    {

        private static int _typewriterDelay = 50;
        private static int _typewriterNewScreenDelay = 3000;
        private static string _typewriterWrite = "Kernel Simulator";
        private static int _typewriterWritingSpeedMin = 50;
        private static int _typewriterWritingSpeedMax = 80;
        private static bool _typewriterShowArrowPos = true;
        private static string _typewriterTextColor = new Color(ConsoleColor.White).PlainSequence;

        /// <summary>
        /// [Typewriter] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TypewriterDelay
        {
            get
            {
                return _typewriterDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _typewriterDelay = value;
            }
        }
        /// <summary>
        /// [Typewriter] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public static int TypewriterNewScreenDelay
        {
            get
            {
                return _typewriterNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _typewriterNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Typewriter] Text for Typewriter. Longer is better.
        /// </summary>
        public static string TypewriterWrite
        {
            get
            {
                return _typewriterWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Kernel Simulator";
                _typewriterWrite = value;
            }
        }
        /// <summary>
        /// [Typewriter] Minimum writing speed in WPM
        /// </summary>
        public static int TypewriterWritingSpeedMin
        {
            get
            {
                return _typewriterWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _typewriterWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Typewriter] Maximum writing speed in WPM
        /// </summary>
        public static int TypewriterWritingSpeedMax
        {
            get
            {
                return _typewriterWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                _typewriterWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Typewriter] Shows the typewriter letter column position by showing this key on the bottom of the screen: <code>^</code>
        /// </summary>
        public static bool TypewriterShowArrowPos
        {
            get
            {
                return _typewriterShowArrowPos;
            }
            set
            {
                _typewriterShowArrowPos = value;
            }
        }
        /// <summary>
        /// [Typewriter] Text color
        /// </summary>
        public static string TypewriterTextColor
        {
            get
            {
                return _typewriterTextColor;
            }
            set
            {
                _typewriterTextColor = new Color(value).PlainSequence;
            }
        }

    }
    public class TypewriterDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "Typewriter";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            KernelColorTools.SetConsoleColor(new Color(TypewriterSettings.TypewriterTextColor));
            ConsoleWrapper.Clear();
        }

        public override void ScreensaverLogic()
        {
            int CpmSpeedMin = TypewriterSettings.TypewriterWritingSpeedMin * 5;
            int CpmSpeedMax = TypewriterSettings.TypewriterWritingSpeedMax * 5;
            string TypeWrite = TypewriterSettings.TypewriterWrite;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Minimum speed from {0} WPM: {1} CPM", TypewriterSettings.TypewriterWritingSpeedMin, CpmSpeedMin);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum speed from {0} WPM: {1} CPM", TypewriterSettings.TypewriterWritingSpeedMax, CpmSpeedMax);
            ConsoleWrapper.CursorVisible = false;
            // Typewriter can also deal with files written on the field that is used for storing text, so check to see if the path exists.
            DebugWriter.Wdbg(DebugLevel.I, "Checking \"{0}\" to see if it's a file path", TypewriterSettings.TypewriterWrite);
            if (Parsing.TryParsePath(TypewriterSettings.TypewriterWrite) && Checking.FileExists(TypewriterSettings.TypewriterWrite))
            {
                // File found! Now, write the contents of it to the local variable that stores the actual written text.
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Opening file {0} to write...", TypewriterSettings.TypewriterWrite);
                TypeWrite = File.ReadAllText(TypewriterSettings.TypewriterWrite);
            }

            // For each line, write four spaces, and extra two spaces if paragraph starts.
            foreach (string Paragraph in TypeWrite.SplitNewLines())
            {
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "New paragraph: {0}", Paragraph);

                // Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
                // sizes.
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
                    if (IncompleteSentenceBuilder.Length == ConsoleWrapper.WindowWidth - 2 - ReservedCharacters | Paragraph.Length == CharactersParsed)
                    {
                        // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Adding {0} to the list... Incomplete sentences: {1}", IncompleteSentenceBuilder.ToString(), IncompleteSentences.Count);
                        IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());

                        // Clean everything up
                        IncompleteSentenceBuilder.Clear();
                        ReservedCharacters = 0;
                    }
                }

                // Prepare display (make a paragraph indentation)
                if (!(ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 2))
                {
                    TextWriterColor.WritePlain("", true);
                    TextWriterColor.WritePlain("    ", false);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                }

                // Get struck character and write it
                for (int SentenceIndex = 0, loopTo = IncompleteSentences.Count - 1; SentenceIndex <= loopTo; SentenceIndex++)
                {
                    string Sentence = IncompleteSentences[SentenceIndex];
                    if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                        ResizeSyncing = true;
                    if (ResizeSyncing)
                        break;
                    foreach (char StruckChar in Sentence)
                    {
                        if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                            ResizeSyncing = true;
                        if (ResizeSyncing)
                            break;

                        // Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                        int SelectedCpm = RandomDriver.Next(CpmSpeedMin, CpmSpeedMax);
                        int WriteMs = (int)Math.Round(60d / SelectedCpm * 1000d);
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", SelectedCpm, WriteMs);

                        // If we're at the end of the page, clear the screen
                        if (ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 2)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're at the end of the page! {0} = {1}", ConsoleWrapper.CursorTop, ConsoleWrapper.WindowHeight - 2);
                            ThreadManager.SleepNoBlock(TypewriterSettings.TypewriterNewScreenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            ConsoleWrapper.Clear();
                            TextWriterColor.WritePlain("", true);
                            if (SentenceIndex == 0)
                            {
                                TextWriterColor.WritePlain("    ", false);
                            }
                            else
                            {
                                TextWriterColor.WritePlain(" ", false);
                            }
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                        }

                        // If we need to show the arrow indicator, update its position
                        if (TypewriterSettings.TypewriterShowArrowPos)
                        {
                            int OldTop = ConsoleWrapper.CursorTop;
                            int OldLeft = ConsoleWrapper.CursorLeft;
                            ConsoleWrapper.SetCursorPosition(OldLeft, ConsoleWrapper.WindowHeight - 1);
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Arrow drawn in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                            TextWriterColor.WritePlain(Convert.ToString(Color255.GetEsc()) + "[1K^" + Convert.ToString(Color255.GetEsc()) + "[K", false);
                            ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Returned to {0}, {1}", OldLeft, OldTop);
                        }

                        // Write the final character to the console and wait
                        TextWriterColor.WritePlain(Convert.ToString(StruckChar), false);
                        ThreadManager.SleepNoBlock(WriteMs, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    }
                    TextWriterColor.WritePlain("", true);
                    TextWriterColor.WritePlain(" ", false);
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(TypewriterSettings.TypewriterDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}