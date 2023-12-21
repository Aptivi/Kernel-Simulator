﻿using System;
using System.Collections.Generic;

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

using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using FluentFTP.Helpers;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Editors.TextEdit
{
	public static class TextEditTools
	{

		/// <summary>
		/// Opens the text file
		/// </summary>
		/// <param name="File">Target file. We recommend you to use <see cref="NeutralizePath(string, bool)"></see> to neutralize path.</param>
		/// <returns>True if successful; False if unsuccessful</returns>
		public static bool TextEdit_OpenTextFile(string File)
		{
			try
			{
				DebugWriter.Wdbg(DebugLevel.I, "Trying to open file {0}...", File);
				TextEditShellCommon.TextEdit_FileStream = new FileStream(File, FileMode.Open);
				if (TextEditShellCommon.TextEdit_FileLines is null)
					TextEditShellCommon.TextEdit_FileLines = [];
				if (TextEditShellCommon.TextEdit_FileLinesOrig is null)
					TextEditShellCommon.TextEdit_FileLinesOrig = [];
				DebugWriter.Wdbg(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", File, TextEditShellCommon.TextEdit_FileStream.Length, TextEditShellCommon.TextEdit_FileStream.Position);
				var TextFileStreamReader = new StreamReader(TextEditShellCommon.TextEdit_FileStream);
				while (!TextFileStreamReader.EndOfStream)
				{
					string StreamLine = TextFileStreamReader.ReadLine();
					TextEditShellCommon.TextEdit_FileLines.Add(StreamLine);
					TextEditShellCommon.TextEdit_FileLinesOrig.Add(StreamLine);
				}
				TextEditShellCommon.TextEdit_FileStream.Seek(0L, SeekOrigin.Begin);
				return true;
			}
			catch (Exception ex)
			{
				DebugWriter.Wdbg(DebugLevel.E, "Open file {0} failed: {1}", File, ex.Message);
				DebugWriter.WStkTrc(ex);
				return false;
			}
		}

		/// <summary>
		/// Closes text file
		/// </summary>
		/// <returns>True if successful; False if unsuccessful</returns>
		public static bool TextEdit_CloseTextFile()
		{
			try
			{
				DebugWriter.Wdbg(DebugLevel.I, "Trying to close file...");
				TextEditShellCommon.TextEdit_FileStream.Close();
				TextEditShellCommon.TextEdit_FileStream = null;
				DebugWriter.Wdbg(DebugLevel.I, "File is no longer open.");
				TextEditShellCommon.TextEdit_FileLines.Clear();
				TextEditShellCommon.TextEdit_FileLinesOrig.Clear();
				return true;
			}
			catch (Exception ex)
			{
				DebugWriter.Wdbg(DebugLevel.E, "Closing file failed: {0}", ex.Message);
				DebugWriter.WStkTrc(ex);
				return false;
			}
		}

		/// <summary>
		/// Saves text file
		/// </summary>
		/// <returns>True if successful; False if unsuccessful</returns>
		public static bool TextEdit_SaveTextFile(bool ClearLines)
		{
			try
			{
				DebugWriter.Wdbg(DebugLevel.I, "Trying to save file...");
				TextEditShellCommon.TextEdit_FileStream.SetLength(0L);
				DebugWriter.Wdbg(DebugLevel.I, "Length set to 0.");
				byte[] FileLinesByte = Encoding.Default.GetBytes(TextEditShellCommon.TextEdit_FileLines.ToArray().Join(Kernel.Kernel.NewLine));
				DebugWriter.Wdbg(DebugLevel.I, "Converted lines to bytes. Length: {0}", FileLinesByte.Length);
				TextEditShellCommon.TextEdit_FileStream.Write(FileLinesByte, 0, FileLinesByte.Length);
				TextEditShellCommon.TextEdit_FileStream.Flush();
				DebugWriter.Wdbg(DebugLevel.I, "File is saved.");
				if (ClearLines)
				{
					TextEditShellCommon.TextEdit_FileLines.Clear();
				}
				TextEditShellCommon.TextEdit_FileLinesOrig.Clear();
				TextEditShellCommon.TextEdit_FileLinesOrig.AddRange(TextEditShellCommon.TextEdit_FileLines);
				return true;
			}
			catch (Exception ex)
			{
				DebugWriter.Wdbg(DebugLevel.E, "Saving file failed: {0}", ex.Message);
				DebugWriter.WStkTrc(ex);
				return false;
			}
		}

		/// <summary>
		/// Handles autosave
		/// </summary>
		public static void TextEdit_HandleAutoSaveTextFile()
		{
			if (TextEditShellCommon.TextEdit_AutoSaveFlag)
			{
				try
				{
					Thread.Sleep(TextEditShellCommon.TextEdit_AutoSaveInterval * 1000);
					if (TextEditShellCommon.TextEdit_FileStream is not null)
					{
						TextEdit_SaveTextFile(false);
					}
				}
				catch (Exception ex)
				{
					DebugWriter.WStkTrc(ex);
				}
			}
		}

		/// <summary>
		/// Was text edited?
		/// </summary>
		public static bool TextEdit_WasTextEdited()
		{
			if (TextEditShellCommon.TextEdit_FileLines is not null & TextEditShellCommon.TextEdit_FileLinesOrig is not null)
			{
				return !TextEditShellCommon.TextEdit_FileLines.SequenceEqual(TextEditShellCommon.TextEdit_FileLinesOrig);
			}
			return false;
		}

		/// <summary>
		/// Adds a new line to the current text
		/// </summary>
		/// <param name="Content">New line content</param>
		public static void TextEdit_AddNewLine(string Content)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				TextEditShellCommon.TextEdit_FileLines.Add(Content);
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Adds the new lines to the current text
		/// </summary>
		/// <param name="Lines">New lines</param>
		public static void TextEdit_AddNewLines(string[] Lines)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				foreach (string Line in Lines)
					TextEditShellCommon.TextEdit_FileLines.Add(Line);
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Removes a line from the current text
		/// </summary>
		/// <param name="LineNumber">The line number to remove</param>
		public static void TextEdit_RemoveLine(int LineNumber)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				int LineIndex = LineNumber - 1;
				DebugWriter.Wdbg(DebugLevel.I, "Got line index: {0}", LineIndex);
				DebugWriter.Wdbg(DebugLevel.I, "Old file lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				if (LineNumber <= TextEditShellCommon.TextEdit_FileLines.Count)
				{
					TextEditShellCommon.TextEdit_FileLines.RemoveAt(LineIndex);
					DebugWriter.Wdbg(DebugLevel.I, "New file lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, Translate.DoTranslation("The specified line number may not be larger than the last file line number."));
				}
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Replaces every occurence of a string with the replacement using regular expressions
		/// </summary>
		/// <param name="From">Regular expression to be replaced</param>
		/// <param name="[With]">String to replace with</param>
		public static void TextEdit_ReplaceRegex(string From, string With)
		{
			if (string.IsNullOrEmpty(From))
				throw new ArgumentNullException(nameof(From));
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				DebugWriter.Wdbg(DebugLevel.I, "Source: {0}, Target: {1}", From, With);
				for (int LineIndex = 0, loopTo = TextEditShellCommon.TextEdit_FileLines.Count - 1; LineIndex <= loopTo; LineIndex++)
				{
					DebugWriter.Wdbg(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", From, With, LineIndex + 1);
					TextEditShellCommon.TextEdit_FileLines[LineIndex] = Regex.Replace(TextEditShellCommon.TextEdit_FileLines[LineIndex], From, With);
				}
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Replaces every occurence of a string with the replacement using regular expressions
		/// </summary>
		/// <param name="From">Regular expression to be replaced</param>
		/// <param name="[With]">String to replace with</param>
		/// <param name="LineNumber">The line number</param>
		public static void TextEdit_ReplaceRegex(string From, string With, int LineNumber)
		{
			if (string.IsNullOrEmpty(From))
				throw new ArgumentNullException(nameof(From));
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				DebugWriter.Wdbg(DebugLevel.I, "Source: {0}, Target: {1}, Line Number: {2}", From, With, LineNumber);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				long LineIndex = LineNumber - 1;
				if (LineNumber <= TextEditShellCommon.TextEdit_FileLines.Count)
				{
					DebugWriter.Wdbg(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", From, With, LineIndex + 1L);
					TextEditShellCommon.TextEdit_FileLines[(int)LineIndex] = Regex.Replace(TextEditShellCommon.TextEdit_FileLines[(int)LineIndex], From, With);
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, Translate.DoTranslation("The specified line number may not be larger than the last file line number."));
				}
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Replaces every occurence of a string with the replacement
		/// </summary>
		/// <param name="From">String to be replaced</param>
		/// <param name="[With]">String to replace with</param>
		public static void TextEdit_Replace(string From, string With)
		{
			if (string.IsNullOrEmpty(From))
				throw new ArgumentNullException(nameof(From));
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				DebugWriter.Wdbg(DebugLevel.I, "Source: {0}, Target: {1}", From, With);
				for (int LineIndex = 0, loopTo = TextEditShellCommon.TextEdit_FileLines.Count - 1; LineIndex <= loopTo; LineIndex++)
				{
					DebugWriter.Wdbg(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", From, With, LineIndex + 1);
					TextEditShellCommon.TextEdit_FileLines[LineIndex] = TextEditShellCommon.TextEdit_FileLines[LineIndex].Replace(From, With);
				}
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Replaces every occurence of a string with the replacement
		/// </summary>
		/// <param name="From">String to be replaced</param>
		/// <param name="[With]">String to replace with</param>
		/// <param name="LineNumber">The line number</param>
		public static void TextEdit_Replace(string From, string With, int LineNumber)
		{
			if (string.IsNullOrEmpty(From))
				throw new ArgumentNullException(nameof(From));
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				DebugWriter.Wdbg(DebugLevel.I, "Source: {0}, Target: {1}, Line Number: {2}", From, With, LineNumber);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				long LineIndex = LineNumber - 1;
				if (LineNumber <= TextEditShellCommon.TextEdit_FileLines.Count)
				{
					DebugWriter.Wdbg(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", From, With, LineIndex + 1L);
					TextEditShellCommon.TextEdit_FileLines[(int)LineIndex] = TextEditShellCommon.TextEdit_FileLines[(int)LineIndex].Replace(From, With);
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, Translate.DoTranslation("The specified line number may not be larger than the last file line number."));
				}
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Deletes a word or a phrase from the line
		/// </summary>
		/// <param name="Word">The word or phrase</param>
		/// <param name="LineNumber">The line number</param>
		public static void TextEdit_DeleteWord(string Word, int LineNumber)
		{
			if (string.IsNullOrEmpty(Word))
				throw new ArgumentNullException(nameof(Word));
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				int LineIndex = LineNumber - 1;
				DebugWriter.Wdbg(DebugLevel.I, "Word/Phrase: {0}, Line: {1}", Word, LineNumber);
				DebugWriter.Wdbg(DebugLevel.I, "Got line index: {0}", LineIndex);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				if (LineNumber <= TextEditShellCommon.TextEdit_FileLines.Count)
				{
					TextEditShellCommon.TextEdit_FileLines[LineIndex] = TextEditShellCommon.TextEdit_FileLines[LineIndex].Replace(Word, "");
					DebugWriter.Wdbg(DebugLevel.I, "Removed {0}. Result: {1}", LineIndex, TextEditShellCommon.TextEdit_FileLines.Count);
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, Translate.DoTranslation("The specified line number may not be larger than the last file line number."));
				}
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Deletes a character from the line
		/// </summary>
		/// <param name="CharNumber">The character number</param>
		/// <param name="LineNumber">The line number</param>
		public static void TextEdit_DeleteChar(int CharNumber, int LineNumber)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				int LineIndex = LineNumber - 1;
				int CharIndex = CharNumber - 1;
				DebugWriter.Wdbg(DebugLevel.I, "Char number: {0}, Line: {1}", CharNumber, LineNumber);
				DebugWriter.Wdbg(DebugLevel.I, "Got line index: {0}", LineIndex);
				DebugWriter.Wdbg(DebugLevel.I, "Got char index: {0}", CharIndex);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				if (LineNumber <= TextEditShellCommon.TextEdit_FileLines.Count)
				{
					TextEditShellCommon.TextEdit_FileLines[LineIndex] = TextEditShellCommon.TextEdit_FileLines[LineIndex].Remove(CharIndex, 1);
					DebugWriter.Wdbg(DebugLevel.I, "Removed {0}. Result: {1}", LineIndex, TextEditShellCommon.TextEdit_FileLines[LineIndex]);
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, Translate.DoTranslation("The specified line number may not be larger than the last file line number."));
				}
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Queries a character in all lines.
		/// </summary>
		/// <param name="Char">The character to query</param>
		public static Dictionary<int, Dictionary<int, string>> TextEdit_QueryChar(char Char)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				var Lines = new Dictionary<int, Dictionary<int, string>>();
				var Results = new Dictionary<int, string>();
				DebugWriter.Wdbg(DebugLevel.I, "Char: {0}", Char);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				for (int LineIndex = 0, loopTo = TextEditShellCommon.TextEdit_FileLines.Count - 1; LineIndex <= loopTo; LineIndex++)
				{
					for (int CharIndex = 0, loopTo1 = TextEditShellCommon.TextEdit_FileLines[LineIndex].Length - 1; CharIndex <= loopTo1; CharIndex++)
					{
						if (TextEditShellCommon.TextEdit_FileLines[LineIndex][CharIndex] == Char)
						{
							Results.Add(CharIndex, TextEditShellCommon.TextEdit_FileLines[LineIndex]);
						}
					}
					Lines.Add(LineIndex, new Dictionary<int, string>(Results));
					Results.Clear();
				}
				return Lines;
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Queries a character in specific line.
		/// </summary>
		/// <param name="Char">The character to query</param>
		/// <param name="LineNumber">The line number</param>
		public static Dictionary<int, string> TextEdit_QueryChar(char Char, int LineNumber)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				int LineIndex = LineNumber - 1;
				var Results = new Dictionary<int, string>();
				DebugWriter.Wdbg(DebugLevel.I, "Char: {0}, Line: {1}", Char, LineNumber);
				DebugWriter.Wdbg(DebugLevel.I, "Got line index: {0}", LineIndex);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				if (LineNumber <= TextEditShellCommon.TextEdit_FileLines.Count)
				{
					for (int CharIndex = 0, loopTo = TextEditShellCommon.TextEdit_FileLines[LineIndex].Length - 1; CharIndex <= loopTo; CharIndex++)
					{
						if (TextEditShellCommon.TextEdit_FileLines[LineIndex][CharIndex] == Char)
						{
							Results.Add(CharIndex, TextEditShellCommon.TextEdit_FileLines[LineIndex]);
						}
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, Translate.DoTranslation("The specified line number may not be larger than the last file line number."));
				}
				return Results;
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Queries a word in all lines.
		/// </summary>
		/// <param name="Word">The word to query</param>
		public static Dictionary<int, Dictionary<int, string>> TextEdit_QueryWord(string Word)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				var Lines = new Dictionary<int, Dictionary<int, string>>();
				var Results = new Dictionary<int, string>();
				DebugWriter.Wdbg(DebugLevel.I, "Word: {0}", Word);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				for (int LineIndex = 0, loopTo = TextEditShellCommon.TextEdit_FileLines.Count - 1; LineIndex <= loopTo; LineIndex++)
				{
					string[] Words = TextEditShellCommon.TextEdit_FileLines[LineIndex].Split(' ');
					for (int WordIndex = 0, loopTo1 = Words.Length - 1; WordIndex <= loopTo1; WordIndex++)
					{
						if (Words[WordIndex].ToLower().Contains(Word.ToLower()))
						{
							Results.Add(WordIndex, TextEditShellCommon.TextEdit_FileLines[LineIndex]);
						}
					}
					Lines.Add(LineIndex, new Dictionary<int, string>(Results));
					Results.Clear();
				}
				return Lines;
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Queries a word in specific line.
		/// </summary>
		/// <param name="Word">The word to query</param>
		/// <param name="LineNumber">The line number</param>
		public static Dictionary<int, string> TextEdit_QueryWord(string Word, int LineNumber)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				int LineIndex = LineNumber - 1;
				var Results = new Dictionary<int, string>();
				DebugWriter.Wdbg(DebugLevel.I, "Word: {0}, Line: {1}", Word, LineNumber);
				DebugWriter.Wdbg(DebugLevel.I, "Got line index: {0}", LineIndex);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				if (LineNumber <= TextEditShellCommon.TextEdit_FileLines.Count)
				{
					string[] Words = TextEditShellCommon.TextEdit_FileLines[LineIndex].Split(' ');
					for (int WordIndex = 0, loopTo = Words.Length - 1; WordIndex <= loopTo; WordIndex++)
					{
						if (Words[WordIndex].ToLower().Contains(Word.ToLower()))
						{
							Results.Add(WordIndex, TextEditShellCommon.TextEdit_FileLines[LineIndex]);
						}
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, Translate.DoTranslation("The specified line number may not be larger than the last file line number."));
				}
				return Results;
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Queries a word in all lines using regular expressions
		/// </summary>
		/// <param name="Word">The regular expression to query</param>
		public static Dictionary<int, Dictionary<int, string>> TextEdit_QueryWordRegex(string Word)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				var Lines = new Dictionary<int, Dictionary<int, string>>();
				var Results = new Dictionary<int, string>();
				DebugWriter.Wdbg(DebugLevel.I, "Word: {0}", Word);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				for (int LineIndex = 0, loopTo = TextEditShellCommon.TextEdit_FileLines.Count - 1; LineIndex <= loopTo; LineIndex++)
				{
					var LineMatches = Regex.Matches(TextEditShellCommon.TextEdit_FileLines[LineIndex], Word);
					for (int MatchIndex = 0, loopTo1 = LineMatches.Count - 1; MatchIndex <= loopTo1; MatchIndex++)
					{
						_ = LineMatches[MatchIndex];
						Results.Add(MatchIndex, TextEditShellCommon.TextEdit_FileLines[LineIndex]);
					}
					Lines.Add(LineIndex, new Dictionary<int, string>(Results));
					Results.Clear();
				}
				return Lines;
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

		/// <summary>
		/// Queries a word in specific line using regular expressions
		/// </summary>
		/// <param name="Word">The regular expression to query</param>
		/// <param name="LineNumber">The line number</param>
		public static Dictionary<int, string> TextEdit_QueryWordRegex(string Word, int LineNumber)
		{
			if (TextEditShellCommon.TextEdit_FileStream is not null)
			{
				int LineIndex = LineNumber - 1;
				var Results = new Dictionary<int, string>();
				DebugWriter.Wdbg(DebugLevel.I, "Word: {0}, Line: {1}", Word, LineNumber);
				DebugWriter.Wdbg(DebugLevel.I, "Got line index: {0}", LineIndex);
				DebugWriter.Wdbg(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
				if (LineNumber <= TextEditShellCommon.TextEdit_FileLines.Count)
				{
					var LineMatches = Regex.Matches(TextEditShellCommon.TextEdit_FileLines[LineIndex], Word);
					for (int MatchIndex = 0, loopTo = LineMatches.Count - 1; MatchIndex <= loopTo; MatchIndex++)
					{
						_ = LineMatches[MatchIndex];
						Results.Add(MatchIndex, TextEditShellCommon.TextEdit_FileLines[LineIndex]);
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, Translate.DoTranslation("The specified line number may not be larger than the last file line number."));
				}
				return Results;
			}
			else
			{
				throw new InvalidOperationException(Translate.DoTranslation("The text editor hasn't opened a file stream yet."));
			}
		}

	}
}