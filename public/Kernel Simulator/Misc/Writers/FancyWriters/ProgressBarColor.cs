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

using System;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Misc.Writers.WriterBase;

namespace KS.Misc.Writers.FancyWriters
{
    /// <summary>
    /// Progress bar writer with color support
    /// </summary>
    public static class ProgressBarColor
    {

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressPlain(double Progress, int Left, int Top, bool DrawBorder = true)
        {
            try
            {
                // Draw the border
                if (DrawBorder)
                {
                    WriterPlainManager.CurrentPlain.WriteWherePlain(ProgressTools.ProgressUpperLeftCornerChar + ProgressTools.ProgressUpperFrameChar.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true);
                    WriterPlainManager.CurrentPlain.WriteWherePlain(ProgressTools.ProgressLeftFrameChar + " ".Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressTools.ProgressRightFrameChar, Left, Top + 1, true);
                    WriterPlainManager.CurrentPlain.WriteWherePlain(ProgressTools.ProgressLowerLeftCornerChar + ProgressTools.ProgressLowerFrameChar.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressTools.ProgressLowerRightCornerChar, Left, Top + 2, true);
                }

                // Draw the progress bar
                WriterPlainManager.CurrentPlain.WriteWherePlain("*".Repeat(ConsoleExtensions.PercentRepeat((int)Math.Round(Progress), 100, 10)), Left + 1, Top + 1, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, bool DrawBorder = true)
        {
            try
            {
                WriteProgress(Progress, Left, Top, ColorTools.ColTypes.Progress, ColorTools.ColTypes.Gray, DrawBorder);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, ColorTools.ColTypes ProgressColor, bool DrawBorder = true)
        {
            try
            {
                WriteProgress(Progress, Left, Top, ProgressColor, ColorTools.ColTypes.Gray, DrawBorder);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, ColorTools.ColTypes ProgressColor, ColorTools.ColTypes FrameColor, bool DrawBorder = true)
        {
            try
            {
                // Draw the border
                if (DrawBorder)
                {
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressUpperLeftCornerChar + ProgressTools.ProgressUpperFrameChar.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLeftFrameChar + " ".Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressTools.ProgressRightFrameChar, Left, Top + 1, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLowerLeftCornerChar + ProgressTools.ProgressLowerFrameChar.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressTools.ProgressLowerRightCornerChar, Left, Top + 2, true, FrameColor);
                }

                // Draw the progress bar
                ColorTools.SetConsoleColor(ProgressColor, true, true);
                WriterPlainManager.CurrentPlain.WriteWherePlain(" ".Repeat(ConsoleExtensions.PercentRepeat((int)Math.Round(Progress), 100, 10)), Left + 1, Top + 1, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor, bool DrawBorder = true)
        {
            try
            {
                WriteProgress(Progress, Left, Top, ProgressColor, ColorTools.GetGray(), DrawBorder);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor, bool DrawBorder = true)
        {
            try
            {
                // Draw the border
                if (DrawBorder)
                {
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressUpperLeftCornerChar + ProgressTools.ProgressUpperFrameChar.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLeftFrameChar + " ".Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressTools.ProgressRightFrameChar, Left, Top + 1, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLowerLeftCornerChar + ProgressTools.ProgressLowerFrameChar.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressTools.ProgressLowerRightCornerChar, Left, Top + 2, true, FrameColor);
                }

                // Draw the progress bar
                ColorTools.SetConsoleColor(ProgressColor, true, true);
                WriterPlainManager.CurrentPlain.WriteWherePlain(" ".Repeat(ConsoleExtensions.PercentRepeat((int)Math.Round(Progress), 100, 10)), Left + 1, Top + 1, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

    }
}
