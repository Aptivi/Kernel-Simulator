﻿using System;
using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;

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

using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Misc.Writers.ConsoleWriters
{
    public static class TextWriterColor
    {

        internal static object WriteLock = new object();

        /// <summary>
        /// Outputs the text into the terminal prompt without colors
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Get the filtered positions first.
                    int FilteredLeft = default, FilteredTop = default;
                    if (!Line & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out)))
                        ConsoleExtensions.GetFilteredPositions(Text, ref FilteredLeft, ref FilteredTop, vars);

                    // Actually write
                    if (Line)
                    {
                        if (!(vars.Length == 0))
                        {
                            Console.WriteLine(Text, vars);
                        }
                        else
                        {
                            Console.WriteLine(Text);
                        }
                    }
                    else if (!(vars.Length == 0))
                    {
                        Console.Write(Text, vars);
                    }
                    else
                    {
                        Console.Write(Text);
                    }

                    // Return to the processed position
                    if (!Line & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out)))
                        Console.SetCursorPosition(FilteredLeft, FilteredTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ColorTools.ColTypes colorType, params object[] vars)
        {
            Write(Text, Line, false, colorType, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ColorTools.ColTypes colorType, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    ColorTools.SetConsoleColor(colorType, Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(colorType);
                        ColorTools.SetConsoleColor(ColorTools.BackgroundColor, true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }

                    // Reset the colors
                    if (colorType == ColorTools.ColTypes.Input & Shell.Shell.ColoredShell & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out)))
                        ColorTools.SetInputColor();
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ColorTools.ColTypes colorTypeForeground, ColorTools.ColTypes colorTypeBackground, params object[] vars)
        {
            Write(Text, Line, false, colorTypeForeground, colorTypeBackground, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ColorTools.ColTypes colorTypeForeground, ColorTools.ColTypes colorTypeBackground, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    ColorTools.SetConsoleColor(colorTypeForeground, Highlight);
                    ColorTools.SetConsoleColor(colorTypeBackground, !Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(colorTypeForeground);
                        ColorTools.SetConsoleColor(colorTypeBackground, true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }

                    // Reset the colors
                    if (colorTypeForeground == ColorTools.ColTypes.Input & Shell.Shell.ColoredShell & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out)))
                        ColorTools.SetInputColor();
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ConsoleColor color, params object[] vars)
        {
            Write(Text, Line, false, color, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ConsoleColor color, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    if (Highlight)
                    {
                        Console.ForegroundColor = (ConsoleColor)Conversions.ToInteger(StringQuery.IsStringNumeric(ColorTools.BackgroundColor.PlainSequence) && Conversions.ToDouble(ColorTools.BackgroundColor.PlainSequence) <= 15d ? Enum.Parse(typeof(ConsoleColor), ColorTools.BackgroundColor.PlainSequence) : ConsoleColor.Black);
                        Console.BackgroundColor = color;
                    }
                    else
                    {
                        Console.BackgroundColor = (ConsoleColor)Conversions.ToInteger(StringQuery.IsStringNumeric(ColorTools.BackgroundColor.PlainSequence) && Conversions.ToDouble(ColorTools.BackgroundColor.PlainSequence) <= 15d ? Enum.Parse(typeof(ConsoleColor), ColorTools.BackgroundColor.PlainSequence) : ConsoleColor.Black);
                        Console.ForegroundColor = color;
                    }

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        Console.BackgroundColor = (ConsoleColor)Conversions.ToInteger(StringQuery.IsStringNumeric(ColorTools.BackgroundColor.PlainSequence) && Conversions.ToDouble(ColorTools.BackgroundColor.PlainSequence) <= 15d ? Enum.Parse(typeof(ConsoleColor), ColorTools.BackgroundColor.PlainSequence) : ConsoleColor.Black);
                        Console.ForegroundColor = color;
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }

                    // Reset the colors
                    if (Shell.Shell.ColoredShell & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out)))
                        ColorTools.SetInputColor();
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] vars)
        {
            Write(Text, Line, false, ForegroundColor, BackgroundColor, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    if (Highlight)
                    {
                        Console.BackgroundColor = ForegroundColor;
                        Console.ForegroundColor = BackgroundColor;
                    }
                    else
                    {
                        Console.BackgroundColor = BackgroundColor;
                        Console.ForegroundColor = ForegroundColor;
                    }

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        Console.BackgroundColor = BackgroundColor;
                        Console.ForegroundColor = ForegroundColor;
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }

                    // Reset the colors
                    if (Shell.Shell.ColoredShell & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out)))
                        ColorTools.SetInputColor();
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, Color color, params object[] vars)
        {
            Write(Text, Line, false, color, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, Color color, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
                    {
                        ColorTools.SetConsoleColor(color, Highlight, Highlight);
                        ColorTools.SetConsoleColor(ColorTools.BackgroundColor, !Highlight, !Highlight);
                    }

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(color);
                        ColorTools.SetConsoleColor(ColorTools.BackgroundColor, true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }

                    // Reset the colors
                    if (Shell.Shell.ColoredShell & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out)))
                        ColorTools.SetInputColor();
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            Write(Text, Line, false, ForegroundColor, BackgroundColor, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
                    {
                        ColorTools.SetConsoleColor(ForegroundColor, Highlight, Highlight);
                        ColorTools.SetConsoleColor(BackgroundColor, !Highlight, !Highlight);
                    }

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(ForegroundColor);
                        ColorTools.SetConsoleColor(BackgroundColor, true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }

                    // Reset the colors
                    if (Shell.Shell.ColoredShell & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out)))
                        ColorTools.SetInputColor();
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

    }
}