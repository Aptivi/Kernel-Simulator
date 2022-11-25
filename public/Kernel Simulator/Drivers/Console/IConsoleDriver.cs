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

using Org.BouncyCastle.Asn1.Utilities;
using System.IO;
using System.Text;
using System;

namespace KS.Drivers.Console
{
    /// <summary>
    /// Console driver interface
    /// </summary>
    public interface IConsoleDriver : IDriver
    {
        /// <summary>
        /// Outputs the new line into the terminal prompt without colors
        /// </summary>
        void WritePlain();

        /// <summary>
        /// Outputs the text into the terminal prompt without colors
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        void WritePlain(string Text, bool Line, params object[] vars);

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with no color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        void WriteWherePlain(string msg, int Left, int Top, params object[] vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        void WriteWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, wraps the long terminal output if needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        void WriteWrappedPlain(string Text, bool Line, params object[] vars);

        /// <summary>
        /// The standard output stream that the console uses
        /// </summary>
        TextWriter Out { get; }

        /// <summary>
        /// The cursor left position
        /// </summary>
        int CursorLeft { get; set; }

        /// <summary>
        /// The cursor top position
        /// </summary>
        int CursorTop { get; set; }

        /// <summary>
        /// The console window top (rows)
        /// </summary>
        int WindowTop { get; }

        /// <summary>
        /// The console window width (columns)
        /// </summary>
        int WindowWidth { get; }

        /// <summary>
        /// The console window height (rows)
        /// </summary>
        int WindowHeight { get; }

        /// <summary>
        /// The console buffer width (columns)
        /// </summary>
        int BufferWidth { get; }

        /// <summary>
        /// The console buffer height (rows)
        /// </summary>
        int BufferHeight { get; }

        /// <summary>
        /// The foreground color
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// The background color
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        bool CursorVisible { set; }

        /// <summary>
        /// The output encoding
        /// </summary>
        Encoding OutputEncoding { get; set; }

        /// <summary>
        /// The input encoding
        /// </summary>
        Encoding InputEncoding { get; set; }

        /// <summary>
        /// Whether a key is pressed
        /// </summary>
        bool KeyAvailable { get; }

        /// <summary>
        /// Clears the console screen, filling it with spaces with the selected background color.
        /// </summary>
        void Clear(bool loadBack = false);

        /// <summary>
        /// Sets the cursor position
        /// </summary>
        /// <param name="left">The left to be set (from 0)</param>
        /// <param name="top">The top to be set (from 0)</param>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Resets console colors
        /// </summary>
        void ResetColor();

        /// <summary>
        /// Opens the standard input
        /// </summary>
        Stream OpenStandardInput();

        /// <summary>
        /// Opens the standard output
        /// </summary>
        Stream OpenStandardOutput();

        /// <summary>
        /// Opens the standard error
        /// </summary>
        Stream OpenStandardError();

        /// <summary>
        /// Sets console output
        /// </summary>
        /// <param name="newOut">New output</param>
        void SetOut(TextWriter newOut);

        /// <summary>
        /// Beeps the console
        /// </summary>
        void Beep();

        /// <summary>
        /// Reads a key
        /// </summary>
        /// <param name="intercept">Whether to intercept</param>
        ConsoleKeyInfo ReadKey(bool intercept = false);

        /// <summary>
        /// Writes a character to console
        /// </summary>
        /// <param name="value">A character</param>
        void Write(char value);

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        void Write(string text);

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        void Write(string text, params object[] args);

        /// <summary>
        /// Writes new line to console
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        void WriteLine(string text);

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        void WriteLine(string text, params object[] args);
    }
}
