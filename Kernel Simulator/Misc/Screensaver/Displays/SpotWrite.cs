﻿using System;
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
using Microsoft.VisualBasic.CompilerServices;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
	public static class SpotWriteSettings
	{

		private static int _spotWriteDelay = 100;
		private static string _spotWriteWrite = "Kernel Simulator";
		private static int _spotWriteNewScreenDelay = 3000;
		private static string _spotWriteTextColor = new Color(ConsoleColor.White).PlainSequence;

		/// <summary>
        /// [SpotWrite] How many milliseconds to wait before making the next write?
        /// </summary>
		public static int SpotWriteDelay
		{
			get
			{
				return _spotWriteDelay;
			}
			set
			{
				if (value <= 0)
					value = 100;
				_spotWriteDelay = value;
			}
		}
		/// <summary>
        /// [SpotWrite] Text for SpotWrite. Longer is better.
        /// </summary>
		public static string SpotWriteWrite
		{
			get
			{
				return _spotWriteWrite;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
					value = "Kernel Simulator";
				_spotWriteWrite = value;
			}
		}
		/// <summary>
        /// [SpotWrite] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
		public static int SpotWriteNewScreenDelay
		{
			get
			{
				return _spotWriteNewScreenDelay;
			}
			set
			{
				if (value <= 0)
					value = 3000;
				_spotWriteNewScreenDelay = value;
			}
		}
		/// <summary>
        /// [SpotWrite] Text color
        /// </summary>
		public static string SpotWriteTextColor
		{
			get
			{
				return _spotWriteTextColor;
			}
			set
			{
				_spotWriteTextColor = new Color(value).PlainSequence;
			}
		}

	}

	public class SpotWriteDisplay : BaseScreensaver, IScreensaver
	{

		private int CurrentWindowWidth;
		private int CurrentWindowHeight;
		private bool ResizeSyncing;

		public override string ScreensaverName { get; set; } = "SpotWrite";

		public override Dictionary<string, object> ScreensaverSettings { get; set; }

		public override void ScreensaverPreparation()
		{
			// Variable preparations
			CurrentWindowWidth = ConsoleWrapper.WindowWidth;
			CurrentWindowHeight = ConsoleWrapper.WindowHeight;
			KernelColorTools.SetConsoleColor(new Color(SpotWriteSettings.SpotWriteTextColor));
			ConsoleWrapper.Clear();
		}

		public override void ScreensaverLogic()
		{
			string TypeWrite = SpotWriteSettings.SpotWriteWrite;
			ConsoleWrapper.CursorVisible = false;

			// SpotWrite can also deal with files written on the field that is used for storing text, so check to see if the path exists.
			DebugWriter.Wdbg(DebugLevel.I, "Checking \"{0}\" to see if it's a file path", SpotWriteSettings.SpotWriteWrite);
			if (Parsing.TryParsePath(SpotWriteSettings.SpotWriteWrite) && Checking.FileExists(SpotWriteSettings.SpotWriteWrite))
			{
				// File found! Now, write the contents of it to the local variable that stores the actual written text.
				DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Opening file {0} to write...", SpotWriteSettings.SpotWriteWrite);
				TypeWrite = File.ReadAllText(SpotWriteSettings.SpotWriteWrite);
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
					ConsoleWrapper.SetCursorPosition(0, ConsoleWrapper.CursorTop + 1);
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

						// If we're at the end of the page, clear the screen
						if (ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 2)
						{
							DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're at the end of the page! {0} = {1}", ConsoleWrapper.CursorTop, ConsoleWrapper.WindowHeight - 2);
							ThreadManager.SleepNoBlock(SpotWriteSettings.SpotWriteNewScreenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
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

						// Write the final character to the console and wait
						TextWriterColor.WritePlain(Conversions.ToString(Color255.GetEsc()) + "[1K" + Conversions.ToString(StruckChar) + Conversions.ToString(Color255.GetEsc()) + "[K", false);
						ThreadManager.SleepNoBlock(SpotWriteSettings.SpotWriteDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
					}
					TextWriterColor.WritePlain(Conversions.ToString(Color255.GetEsc()) + "[1K", false);
				}
			}

			// Reset resize sync
			ResizeSyncing = false;
			CurrentWindowWidth = ConsoleWrapper.WindowWidth;
			CurrentWindowHeight = ConsoleWrapper.WindowHeight;
			ThreadManager.SleepNoBlock(SpotWriteSettings.SpotWriteDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
		}

	}
}