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
using Figletize.Utilities;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Colors;

namespace KS.Misc.Writers.FancyWriters
{
    public static class FigletColor
    {

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletPlain(string Text, FigletizeFont FigletFont, params object[] Vars)
        {
            try
            {
                // Format string as needed
                if (!(Vars.Length == 0))
                    Text = StringManipulate.FormatString(Text, Vars);

                // Write the font
                Text = FigletFont.Render(Text);
                TextWriterColor.WritePlain(Text, true, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WStkTrc(ex);
                KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletizeFont FigletFont, KernelColorTools.ColTypes ColTypes, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColor(ColTypes);

                // Actually write
                WriteFigletPlain(Text, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WStkTrc(ex);
                KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletizeFont FigletFont, KernelColorTools.ColTypes colorTypeForeground, KernelColorTools.ColTypes colorTypeBackground, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColor(colorTypeForeground);
                KernelColorTools.SetConsoleColor(colorTypeBackground, true);

                // Actually write
                WriteFigletPlain(Text, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WStkTrc(ex);
                KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletizeFont FigletFont, ConsoleColor Color, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColor(new Color(Color));

                // Actually write
                WriteFigletPlain(Text, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WStkTrc(ex);
                KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletizeFont FigletFont, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColor(new Color(ForegroundColor));
                KernelColorTools.SetConsoleColor(new Color(BackgroundColor), true);

                // Actually write
                WriteFigletPlain(Text, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WStkTrc(ex);
                KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletizeFont FigletFont, Color Color, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColor(Color);

                // Actually write
                WriteFigletPlain(Text, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WStkTrc(ex);
                KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletizeFont FigletFont, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColor(ForegroundColor);
                KernelColorTools.SetConsoleColor(BackgroundColor, true);

                // Actually write
                WriteFigletPlain(Text, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WStkTrc(ex);
                KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
            }
        }

    }
}
