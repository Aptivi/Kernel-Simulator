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

using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace KS.Misc.Writers.WriterBase.PlainWriters
{
    internal class FilePlainWriter : IWriterPlain
    {
        internal string PathToWrite { get; set; }
        internal bool AppendToFile { get; set; } = false;

        /// <summary>
        /// Outputs text to file
        /// </summary>
        /// <inheritdoc/>
        public void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (Checking.FileExists(PathToWrite))
                    return;

                // Open the stream
                StreamWriter fileWriter = new(PathToWrite, false);
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
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
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
        public void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (Checking.FileExists(PathToWrite))
                    return;

                // Open the stream
                StreamWriter fileWriter = new(PathToWrite, false);
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
                        fileWriter.Write(ch);
                    }
                    if (Line)
                    {
                        fileWriter.WriteLine();
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
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
        public void WriteWherePlain(string msg, int Left, int Top, params object[] vars) => WriteWherePlain(msg, Left, Top, false, vars);

        /// <summary>
        /// Just writes text to file without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (Checking.FileExists(PathToWrite))
                    return;

                try
                {
                    // We can't do positioning on files, so change writing mode to WritePlain
                    WritePlain(msg, false, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
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
        public void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) => WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, false, vars);

        /// <summary>
        /// Just writes text slowly to file, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (Checking.FileExists(PathToWrite))
                    return;

                try
                {
                    // We can't do positioning on files, so change writing mode to WritePlain
                    WriteSlowlyPlain(msg, false, MsEachLetter, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
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
        public void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                // If the file doesn't exist, don't do anything
                if (Checking.FileExists(PathToWrite))
                    return;

                try
                {
                    // We can't do positioning on files, so change writing mode to WritePlain
                    WritePlain(Text, Line, vars);
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
