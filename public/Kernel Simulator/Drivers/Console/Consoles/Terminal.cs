﻿
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

using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using con = System.Console;

namespace KS.Drivers.Console.Consoles
{
    internal class Terminal : IConsoleDriver
    {

        public string DriverName => "Default";

        public DriverTypes DriverType => DriverTypes.Console;

        private static bool _dumbSet = false;
        private static bool _dumb = true;

        /// <summary>
        /// Is the console a dumb console?
        /// </summary>
        public static bool IsDumb
        {
            get
            {
                try
                {
                    // Get terminal type
                    string TerminalType = KernelPlatform.GetTerminalType();

                    // Try to cache the value
                    if (!_dumbSet)
                    {
                        _dumbSet = true;
                        int _ = con.CursorLeft;

                        // If it doesn't get here without throwing exceptions, assume console is dumb. Now, check to see if terminal type is dumb
                        if (TerminalType != "dumb")
                            _dumb = false;
                    }
                }
                catch { }
                return _dumb;
            }
        }

        /// <inheritdoc/>
        public TextWriter Out => con.Out;

        /// <inheritdoc/>
        public int CursorLeft
        {
            get
            {
                if (IsDumb)
                    return 0;
                return con.CursorLeft;
            }
            set
            {
                if (!IsDumb)
                    con.CursorLeft = value;
            }
        }

        /// <inheritdoc/>
        public int CursorTop
        {
            get
            {
                if (IsDumb)
                    return 0;
                return con.CursorTop;
            }
            set
            {
                if (!IsDumb)
                    con.CursorTop = value;
            }
        }

        /// <inheritdoc/>
        public int WindowTop
        {
            get
            {
                if (IsDumb)
                    return 0;
                return con.WindowTop;
            }
        }

        /// <inheritdoc/>
        public int WindowWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return con.WindowWidth;
            }
        }

        /// <inheritdoc/>
        public int WindowHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return con.WindowHeight;
            }
        }

        /// <inheritdoc/>
        public int BufferWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return con.BufferWidth;
            }
        }

        /// <inheritdoc/>
        public int BufferHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return con.BufferHeight;
            }
        }

        /// <inheritdoc/>
        public ConsoleColor ForegroundColor
        {
            get
            {
                if (IsDumb)
                    return ConsoleColor.White;
                return con.ForegroundColor;
            }
            set
            {
                if (!IsDumb)
                    con.ForegroundColor = value;
                ColorTools.cachedForegroundColor = new Color(Convert.ToInt32(value)).VTSequenceForeground;
            }
        }

        /// <inheritdoc/>
        public ConsoleColor BackgroundColor
        {
            get
            {
                if (IsDumb)
                    return ConsoleColor.Black;
                return con.BackgroundColor;
            }
            set
            {
                if (!IsDumb)
                    con.BackgroundColor = value;
                ColorTools.cachedBackgroundColor = new Color(Convert.ToInt32(value)).VTSequenceBackground;
            }
        }

        /// <inheritdoc/>
        public bool CursorVisible
        {
            set
            {
                if (!IsDumb)
                    con.CursorVisible = value;
            }
        }

        /// <inheritdoc/>
        public Encoding OutputEncoding
        {
            get
            {
                if (IsDumb)
                    return Encoding.Default;
                return con.OutputEncoding;
            }
            set
            {
                if (!IsDumb)
                    con.OutputEncoding = value;
            }
        }

        /// <inheritdoc/>
        public Encoding InputEncoding
        {
            get
            {
                if (IsDumb)
                    return Encoding.Default;
                return con.InputEncoding;
            }
            set
            {
                if (!IsDumb)
                    con.InputEncoding = value;
            }
        }

        /// <inheritdoc/>
        public bool KeyAvailable
        {
            get
            {
                if (IsDumb)
                    return false;
                return con.KeyAvailable;
            }
        }

        /// <inheritdoc/>
        public void Beep() => 
            con.Beep();

        /// <inheritdoc/>
        public void Clear(bool loadBack = false)
        {
            if (!IsDumb)
            {
                if (loadBack)
                    ColorTools.LoadBack();
                con.Clear();
            }
        }

        /// <inheritdoc/>
        public Stream OpenStandardError() =>
            con.OpenStandardError();

        /// <inheritdoc/>
        public Stream OpenStandardInput() => 
            con.OpenStandardInput();

        /// <inheritdoc/>
        public Stream OpenStandardOutput() =>
            con.OpenStandardOutput();

        /// <inheritdoc/>
        public ConsoleKeyInfo ReadKey(bool intercept = false) => 
            con.ReadKey(intercept);

        /// <inheritdoc/>
        public void ResetColor()
        {
            if (!IsDumb)
                con.ResetColor();
        }

        /// <inheritdoc/>
        public void SetCursorPosition(int left, int top)
        {
            if (!IsDumb)
                con.SetCursorPosition(left, top);
        }

        /// <inheritdoc/>
        public void SetOut(TextWriter newOut)
        {
            // We need to reset dumb state because the new output may not support usual console features other then reading/writing.
            _dumbSet = false;
            _dumb = true;
            con.SetOut(newOut);
        }

        /// <inheritdoc/>
        public void Write(char value) =>
            con.Write(value);

        /// <inheritdoc/>
        public void Write(string text) =>
            con.Write(text);

        /// <inheritdoc/>
        public void Write(string text, params object[] args) => 
            con.Write(text, args);

        /// <inheritdoc/>
        public void WriteLine() =>
            con.WriteLine();

        /// <inheritdoc/>
        public void WriteLine(string text) => 
            con.WriteLine(text);

        /// <inheritdoc/>
        public void WriteLine(string text, params object[] args) =>
            con.WriteLine(text, args);

        /// <inheritdoc/>
        public void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Get the filtered positions first.
                    int FilteredLeft = default, FilteredTop = default;
                    if (!Line)
                    {
                        var pos = ConsoleExtensions.GetFilteredPositions(Text, vars);
                        FilteredLeft = pos.Item1;
                        FilteredTop = pos.Item2;
                    }

                    // Actually write
                    if (Line)
                    {
                        if (!(vars.Length == 0))
                        {
                            ConsoleWrapper.WriteLine(Text, vars);
                        }
                        else
                        {
                            ConsoleWrapper.WriteLine(Text);
                        }
                    }
                    else if (!(vars.Length == 0))
                    {
                        ConsoleWrapper.Write(Text, vars);
                    }
                    else
                    {
                        ConsoleWrapper.Write(Text);
                    }

                    // Return to the processed position
                    if (!Line)
                        ConsoleWrapper.SetCursorPosition(FilteredLeft, FilteredTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <inheritdoc/>
        public void WritePlain()
        {
            lock (TextWriterColor.WriteLock)
            {
                ConsoleWrapper.WriteLine();
            }
        }

        /// <inheritdoc/>
        public void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (!(vars.Length == 0))
                        msg = StringManipulate.FormatString(msg, vars);

                    // Write text slowly
                    var chars = msg.ToCharArray().ToList();
                    foreach (char ch in chars)
                    {
                        Thread.Sleep((int)Math.Round(MsEachLetter));
                        ConsoleWrapper.Write(ch);
                    }
                    if (Line)
                    {
                        ConsoleWrapper.WriteLine();
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <inheritdoc/>
        public void WriteWherePlain(string msg, int Left, int Top, params object[] vars) => 
            WriteWherePlain(msg, Left, Top, false, 0, vars);

        /// <inheritdoc/>
        public void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars) => 
            WriteWherePlain(msg, Left, Top, Return, 0, vars);

        /// <inheritdoc/>
        public void WriteWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format the message as necessary
                    if (!(vars.Length == 0))
                        msg = StringManipulate.FormatString(msg, vars);

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    var Paragraphs = msg.SplitNewLines();
                    ConsoleWrapper.SetCursorPosition(Left, Top);
                    for (int MessageParagraphIndex = 0; MessageParagraphIndex <= Paragraphs.Length - 1; MessageParagraphIndex++)
                    {
                        // We can now check to see if we're writing a letter past the console window width
                        string MessageParagraph = Paragraphs[MessageParagraphIndex];
                        foreach (char ParagraphChar in MessageParagraph)
                        {
                            if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth - RightMargin)
                            {
                                if (ConsoleWrapper.CursorTop == ConsoleWrapper.BufferHeight - 1)
                                {
                                    // We've reached the end of buffer. Write the line to scroll.
                                    ConsoleWrapper.WriteLine();
                                }
                                else
                                {
                                    ConsoleWrapper.CursorTop += 1;
                                }
                                ConsoleWrapper.CursorLeft = Left;
                            }
                            ConsoleWrapper.Write(ParagraphChar);
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (!(MessageParagraphIndex == Paragraphs.Length - 1))
                        {
                            if (ConsoleWrapper.CursorTop == ConsoleWrapper.BufferHeight - 1)
                            {
                                // We've reached the end of buffer. Write the line to scroll.
                                ConsoleWrapper.WriteLine();
                            }
                            else
                            {
                                ConsoleWrapper.CursorTop += 1;
                            }
                            ConsoleWrapper.CursorLeft = Left;
                        }
                    }
                    if (Return)
                        ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <inheritdoc/>
        public void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) => WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, false, vars);

        /// <inheritdoc/>
        public void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (!(vars.Length == 0))
                        msg = StringManipulate.FormatString(msg, vars);

                    // Write text in another place slowly
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    var Paragraphs = msg.SplitNewLines();
                    ConsoleWrapper.SetCursorPosition(Left, Top);
                    for (int MessageParagraphIndex = 0; MessageParagraphIndex <= Paragraphs.Length - 1; MessageParagraphIndex++)
                    {
                        // We can now check to see if we're writing a letter past the console window width
                        string MessageParagraph = Paragraphs[MessageParagraphIndex];
                        foreach (char ParagraphChar in MessageParagraph)
                        {
                            Thread.Sleep((int)Math.Round(MsEachLetter));
                            if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth)
                            {
                                ConsoleWrapper.CursorTop += 1;
                                ConsoleWrapper.CursorLeft = Left;
                            }
                            ConsoleWrapper.Write(ParagraphChar);
                            if (Line)
                                ConsoleWrapper.WriteLine();
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (!(MessageParagraphIndex == Paragraphs.Length - 1))
                        {
                            ConsoleWrapper.CursorTop += 1;
                            ConsoleWrapper.CursorLeft = Left;
                        }
                    }
                    if (Return)
                        ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <inheritdoc/>
        public void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                var LinesMade = default(int);
                int OldTop;
                try
                {
                    // Format string as needed
                    if (!(vars.Length == 0))
                        Text = StringManipulate.FormatString(Text, vars);

                    OldTop = ConsoleWrapper.CursorTop;
                    foreach (char TextChar in Text.ToString().ToCharArray())
                    {
                        ConsoleWrapper.Write(TextChar);
                        LinesMade += ConsoleWrapper.CursorTop - OldTop;
                        OldTop = ConsoleWrapper.CursorTop;
                        if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                        {
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                            LinesMade = 0;
                        }
                    }
                    if (Line)
                        ConsoleWrapper.WriteLine();
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }
    }
}
