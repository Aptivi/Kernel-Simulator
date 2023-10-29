﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Debugging;
using KS.Languages;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using KS.Kernel.Exceptions;
using KS.Misc.Text;
using KS.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Sequences.Tools;
using TextEncoding = System.Text.Encoding;
using KS.Files.Operations.Querying;

namespace KS.Drivers.Console.Bases
{
    internal class File : BaseConsoleDriver, IConsoleDriver
    {

        public override string DriverName => "File";

        public override DriverTypes DriverType => DriverTypes.Console;

        public override bool DriverInternal => true;

        internal string PathToWrite { get; set; }
        internal bool FilterVT { get; set; }

        public override bool IsDumb => true;

        public override int CursorLeft { get => 0; set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override int CursorTop { get => 0; set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override int WindowWidth => 0;

        public override int WindowHeight => 0;

        public override int BufferWidth => 0;

        public override int BufferHeight => 0;

        public override ConsoleColor ForegroundColor { get => throw new KernelException(KernelExceptionType.NotImplementedYet); set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override ConsoleColor BackgroundColor { get => throw new KernelException(KernelExceptionType.NotImplementedYet); set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override bool CursorVisible { set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override TextEncoding OutputEncoding { get => throw new KernelException(KernelExceptionType.NotImplementedYet); set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override TextEncoding InputEncoding { get => throw new KernelException(KernelExceptionType.NotImplementedYet); set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override bool KeyAvailable =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void Beep() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void Clear(bool loadBack = false) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override Stream OpenStandardError() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override Stream OpenStandardInput() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override Stream OpenStandardOutput() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override ConsoleKeyInfo ReadKey(bool intercept = false) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void ResetColor() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetCursorPosition(int left, int top) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetOut(TextWriter newOut) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void Write(char value) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void Write(string text) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void Write(string text, params object[] args) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine() { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine(string text) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine(string text, params object[] args) { }

        /// <summary>
        /// Outputs text to file
        /// </summary>
        /// <inheritdoc/>
        public override void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (!Checking.FileExists(PathToWrite))
                    return;

                // If filtering, filter all VT sequences
                if (FilterVT)
                    Text = VtSequenceTools.FilterVTSequences(Text);

                // Open the stream
                StreamWriter fileWriter = new(PathToWrite, true) { AutoFlush = true };
                try
                {
                    if (Line)
                    {
                        if (!(vars.Length == 0))
                        {
                            fileWriter.WriteLine(Text, vars);
                        }
                        else
                        {
                            fileWriter.WriteLine(Text);
                        }
                    }
                    else if (!(vars.Length == 0))
                    {
                        fileWriter.Write(Text, vars);
                    }
                    else
                    {
                        fileWriter.Write(Text);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
                fileWriter.Close();
            }
        }

        /// <summary>
        /// Outputs new line to file
        /// </summary>
        /// <inheritdoc/>
        public override void WritePlain()
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (!Checking.FileExists(PathToWrite))
                    return;

                // Open the stream
                StreamWriter fileWriter = new(PathToWrite, true);
                try
                {
                    fileWriter.WriteLine();
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
                fileWriter.Close();
            }
        }

        /// <summary>
        /// Outputs text slowly to file
        /// </summary>
        /// <inheritdoc/>
        public override void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (!Checking.FileExists(PathToWrite))
                    return;

                // If filtering, filter all VT sequences
                if (FilterVT)
                    msg = VtSequenceTools.FilterVTSequences(msg);

                // Open the stream
                StreamWriter fileWriter = new(PathToWrite, true);
                try
                {
                    // Format string as needed
                    if (!(vars.Length == 0))
                        msg = TextTools.FormatString(msg, vars);

                    // Write text slowly
                    var chars = msg.ToCharArray().ToList();
                    foreach (char ch in chars)
                    {
                        Thread.Sleep((int)Math.Round(MsEachLetter));
                        fileWriter.Write(ch);
                    }
                    if (Line)
                    {
                        fileWriter.WriteLine();
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
                fileWriter.Close();
            }
        }

        /// <summary>
        /// Just writes text to file without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWherePlain(string msg, int Left, int Top, params object[] vars) =>
            WriteWherePlain(msg, Left, Top, false, vars);

        /// <summary>
        /// Just writes text to file without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars) =>
            WriteWherePlain(msg, Left, Top, Return, 0, vars);

        /// <summary>
        /// Just writes text to file without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (!Checking.FileExists(PathToWrite))
                    return;

                try
                {
                    // We can't do positioning on files, so change writing mode to WritePlain
                    WritePlain(msg, false, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Just writes text slowly to file, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, false, 0, vars);

        /// <summary>
        /// Just writes text slowly to file, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, 0, vars);

        /// <summary>
        /// Just writes text slowly to file, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (!Checking.FileExists(PathToWrite))
                    return;

                try
                {
                    // We can't do positioning on files, so change writing mode to WritePlain
                    WriteSlowlyPlain(msg, false, MsEachLetter, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Just writes text to file, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (!Checking.FileExists(PathToWrite))
                    return;

                try
                {
                    // We can't do positioning on files, so change writing mode to WritePlain
                    WritePlain(Text, Line, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }
    }
}
