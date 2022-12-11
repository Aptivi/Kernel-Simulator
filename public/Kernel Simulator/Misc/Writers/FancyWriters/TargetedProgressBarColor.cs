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
using System.Threading;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Writers.FancyWriters
{
    /// <summary>
    /// Progress bar writer with color support (targeted width)
    /// </summary>
    public static class TargetedProgressBarColor
    {

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargetedPlain(double Progress, int Left, int Top, bool DrawBorder = true) =>
            WriteProgressTargetedPlain(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="ProgressWidth">Width of the progress bar</param>
        public static void WriteProgressTargetedPlain(double Progress, int Left, int Top, int ProgressWidth, bool DrawBorder = true)
        {
            try
            {
                // Draw the border
                if (DrawBorder)
                {
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressUpperLeftCornerChar + ProgressTools.ProgressUpperFrameChar.Repeat(ProgressWidth) + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLeftFrameChar + " ".Repeat(ProgressWidth) + ProgressTools.ProgressRightFrameChar, Left, Top + 1, true);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLowerLeftCornerChar + ProgressTools.ProgressLowerFrameChar.Repeat(ProgressWidth) + ProgressTools.ProgressLowerRightCornerChar, Left, Top + 2, true);
                }

                // Draw the progress bar
                TextWriterWhereColor.WriteWhere("*".Repeat(ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, ProgressWidth)), Left + 1, Top + 1, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        public static void WriteProgressTargeted(double Progress, int Left, int Top, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ColorTools.ColTypes.Progress, ColorTools.ColTypes.Gray, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressWidth">Width of the progress bar</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, int ProgressWidth, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ProgressWidth, ColorTools.ColTypes.Progress, ColorTools.ColTypes.Gray, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, ConsoleColors ProgressColor, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, new Color(Convert.ToInt32(ProgressColor)), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="ProgressWidth">Width of the progress bar</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, int ProgressWidth, ConsoleColors ProgressColor, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ProgressWidth, new Color(Convert.ToInt32(ProgressColor)), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="ProgressWidth">Width of the progress bar</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, int ProgressWidth, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true)
        {
            try
            {
                // Draw the border
                if (DrawBorder)
                {
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressUpperLeftCornerChar + ProgressTools.ProgressUpperFrameChar.Repeat(ProgressWidth) + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLeftFrameChar + " ".Repeat(ProgressWidth) + ProgressTools.ProgressRightFrameChar, Left, Top + 1, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLowerLeftCornerChar + ProgressTools.ProgressLowerFrameChar.Repeat(ProgressWidth) + ProgressTools.ProgressLowerRightCornerChar, Left, Top + 2, true, FrameColor);
                }

                // Draw the progress bar
                ColorTools.SetConsoleColor(new Color(Convert.ToInt32(ProgressColor)), true, true);
                TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, ProgressWidth)), Left + 1, Top + 1, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        public static void WriteProgressTargeted(double Progress, int Left, int Top, ColorTools.ColTypes ProgressColor, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, ColorTools.ColTypes.Gray, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="ProgressWidth">Width of the progress bar</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, int ProgressWidth, ColorTools.ColTypes ProgressColor, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ProgressWidth, ProgressColor, ColorTools.ColTypes.Gray, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, ColorTools.ColTypes ProgressColor, ColorTools.ColTypes FrameColor, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="ProgressWidth">Width of the progress bar</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, int ProgressWidth, ColorTools.ColTypes ProgressColor, ColorTools.ColTypes FrameColor, bool DrawBorder = true)
        {
            try
            {
                // Draw the border
                if (DrawBorder)
                {
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressUpperLeftCornerChar + ProgressTools.ProgressUpperFrameChar.Repeat(ProgressWidth) + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLeftFrameChar + " ".Repeat(ProgressWidth) + ProgressTools.ProgressRightFrameChar, Left, Top + 1, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLowerLeftCornerChar + ProgressTools.ProgressLowerFrameChar.Repeat(ProgressWidth) + ProgressTools.ProgressLowerRightCornerChar, Left, Top + 2, true, FrameColor);
                }

                // Draw the progress bar
                ColorTools.SetConsoleColor(ProgressColor, true, true);
                TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, ProgressWidth)), Left + 1, Top + 1, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        public static void WriteProgressTargeted(double Progress, int Left, int Top, Color ProgressColor, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressWidth">Width of the progress bar</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, int ProgressWidth, Color ProgressColor, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ProgressWidth, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            WriteProgressTargeted(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="ProgressWidth">Width of the progress bar</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressTargeted(double Progress, int Left, int Top, int ProgressWidth, Color ProgressColor, Color FrameColor, bool DrawBorder = true)
        {
            try
            {
                // Draw the border
                if (DrawBorder)
                {
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressUpperLeftCornerChar + ProgressTools.ProgressUpperFrameChar.Repeat(ProgressWidth) + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLeftFrameChar + " ".Repeat(ProgressWidth) + ProgressTools.ProgressRightFrameChar, Left, Top + 1, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLowerLeftCornerChar + ProgressTools.ProgressLowerFrameChar.Repeat(ProgressWidth) + ProgressTools.ProgressLowerRightCornerChar, Left, Top + 2, true, FrameColor);
                }

                // Draw the progress bar
                ColorTools.SetConsoleColor(ProgressColor, true, true);
                TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, ProgressWidth)), Left + 1, Top + 1, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

    }
}
