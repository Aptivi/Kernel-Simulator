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
using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;

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

namespace KS.Misc.Screensaver.Displays
{
    public static class IndeterminateSettings
    {

        private static bool _indeterminate255Colors;
        private static bool _indeterminateTrueColor = true;
        private static int _indeterminateDelay = 20;
        private static string _indeterminateUpperLeftCornerChar = "╔";
        private static string _indeterminateUpperRightCornerChar = "╗";
        private static string _indeterminateLowerLeftCornerChar = "╚";
        private static string _indeterminateLowerRightCornerChar = "╝";
        private static string _indeterminateUpperFrameChar = "═";
        private static string _indeterminateLowerFrameChar = "═";
        private static string _indeterminateLeftFrameChar = "║";
        private static string _indeterminateRightFrameChar = "║";
        private static int _indeterminateMinimumRedColorLevel = 0;
        private static int _indeterminateMinimumGreenColorLevel = 0;
        private static int _indeterminateMinimumBlueColorLevel = 0;
        private static int _indeterminateMinimumColorLevel = 0;
        private static int _indeterminateMaximumRedColorLevel = 255;
        private static int _indeterminateMaximumGreenColorLevel = 255;
        private static int _indeterminateMaximumBlueColorLevel = 255;
        private static int _indeterminateMaximumColorLevel = 255;
        private static string _indeterminateUpperLeftCornerColor = 7.ToString();
        private static string _indeterminateUpperRightCornerColor = 7.ToString();
        private static string _indeterminateLowerLeftCornerColor = 7.ToString();
        private static string _indeterminateLowerRightCornerColor = 7.ToString();
        private static string _indeterminateUpperFrameColor = 7.ToString();
        private static string _indeterminateLowerFrameColor = 7.ToString();
        private static string _indeterminateLeftFrameColor = 7.ToString();
        private static string _indeterminateRightFrameColor = 7.ToString();
        private static bool _indeterminateUseBorderColors;

        /// <summary>
        /// [Indeterminate] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool Indeterminate255Colors
        {
            get
            {
                return _indeterminate255Colors;
            }
            set
            {
                _indeterminate255Colors = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool IndeterminateTrueColor
        {
            get
            {
                return _indeterminateTrueColor;
            }
            set
            {
                _indeterminateTrueColor = value;
            }
        }
        /// <summary>
        /// [Indeterminate] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int IndeterminateDelay
        {
            get
            {
                return _indeterminateDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                _indeterminateDelay = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner character 
        /// </summary>
        public static string IndeterminateUpperLeftCornerChar
        {
            get
            {
                return _indeterminateUpperLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _indeterminateUpperLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper right corner character 
        /// </summary>
        public static string IndeterminateUpperRightCornerChar
        {
            get
            {
                return _indeterminateUpperRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _indeterminateUpperRightCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower left corner character 
        /// </summary>
        public static string IndeterminateLowerLeftCornerChar
        {
            get
            {
                return _indeterminateLowerLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _indeterminateLowerLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower right corner character 
        /// </summary>
        public static string IndeterminateLowerRightCornerChar
        {
            get
            {
                return _indeterminateLowerRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _indeterminateLowerRightCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper frame character 
        /// </summary>
        public static string IndeterminateUpperFrameChar
        {
            get
            {
                return _indeterminateUpperFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _indeterminateUpperFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower frame character 
        /// </summary>
        public static string IndeterminateLowerFrameChar
        {
            get
            {
                return _indeterminateLowerFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _indeterminateLowerFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Left frame character 
        /// </summary>
        public static string IndeterminateLeftFrameChar
        {
            get
            {
                return _indeterminateLeftFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _indeterminateLeftFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Right frame character 
        /// </summary>
        public static string IndeterminateRightFrameChar
        {
            get
            {
                return _indeterminateRightFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _indeterminateRightFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum red color level (true color)
        /// </summary>
        public static int IndeterminateMinimumRedColorLevel
        {
            get
            {
                return _indeterminateMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _indeterminateMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum green color level (true color)
        /// </summary>
        public static int IndeterminateMinimumGreenColorLevel
        {
            get
            {
                return _indeterminateMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _indeterminateMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum blue color level (true color)
        /// </summary>
        public static int IndeterminateMinimumBlueColorLevel
        {
            get
            {
                return _indeterminateMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _indeterminateMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int IndeterminateMinimumColorLevel
        {
            get
            {
                return _indeterminateMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _indeterminate255Colors | _indeterminateTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _indeterminateMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum red color level (true color)
        /// </summary>
        public static int IndeterminateMaximumRedColorLevel
        {
            get
            {
                return _indeterminateMaximumRedColorLevel;
            }
            set
            {
                if (value <= _indeterminateMinimumRedColorLevel)
                    value = _indeterminateMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _indeterminateMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum green color level (true color)
        /// </summary>
        public static int IndeterminateMaximumGreenColorLevel
        {
            get
            {
                return _indeterminateMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _indeterminateMinimumGreenColorLevel)
                    value = _indeterminateMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _indeterminateMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum blue color level (true color)
        /// </summary>
        public static int IndeterminateMaximumBlueColorLevel
        {
            get
            {
                return _indeterminateMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _indeterminateMinimumBlueColorLevel)
                    value = _indeterminateMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _indeterminateMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int IndeterminateMaximumColorLevel
        {
            get
            {
                return _indeterminateMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _indeterminate255Colors | _indeterminateTrueColor ? 255 : 15;
                if (value <= _indeterminateMinimumColorLevel)
                    value = _indeterminateMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _indeterminateMaximumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner color.
        /// </summary>
        public static string IndeterminateUpperLeftCornerColor
        {
            get
            {
                return _indeterminateUpperLeftCornerColor;
            }
            set
            {
                _indeterminateUpperLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper right corner color.
        /// </summary>
        public static string IndeterminateUpperRightCornerColor
        {
            get
            {
                return _indeterminateUpperRightCornerColor;
            }
            set
            {
                _indeterminateUpperRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower left corner color.
        /// </summary>
        public static string IndeterminateLowerLeftCornerColor
        {
            get
            {
                return _indeterminateLowerLeftCornerColor;
            }
            set
            {
                _indeterminateLowerLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower right corner color.
        /// </summary>
        public static string IndeterminateLowerRightCornerColor
        {
            get
            {
                return _indeterminateLowerRightCornerColor;
            }
            set
            {
                _indeterminateLowerRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper frame color.
        /// </summary>
        public static string IndeterminateUpperFrameColor
        {
            get
            {
                return _indeterminateUpperFrameColor;
            }
            set
            {
                _indeterminateUpperFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower frame color.
        /// </summary>
        public static string IndeterminateLowerFrameColor
        {
            get
            {
                return _indeterminateLowerFrameColor;
            }
            set
            {
                _indeterminateLowerFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Left frame color.
        /// </summary>
        public static string IndeterminateLeftFrameColor
        {
            get
            {
                return _indeterminateLeftFrameColor;
            }
            set
            {
                _indeterminateLeftFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Right frame color.
        /// </summary>
        public static string IndeterminateRightFrameColor
        {
            get
            {
                return _indeterminateRightFrameColor;
            }
            set
            {
                _indeterminateRightFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Use the border colors.
        /// </summary>
        public static bool IndeterminateUseBorderColors
        {
            get
            {
                return _indeterminateUseBorderColors;
            }
            set
            {
                _indeterminateUseBorderColors = value;
            }
        }

    }
    public class IndeterminateDisplay : BaseScreensaver, IScreensaver
    {

        private readonly int RampFrameBlockStartWidth = 5;
        private readonly int RampFrameBlockWidth = 3;
        private int IndeterminateCurrentBlockStart;
        private int IndeterminateCurrentBlockEnd;
        private IndeterminateDirection IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight;
        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "Indeterminate";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public IndeterminateDisplay()
        {
            IndeterminateCurrentBlockStart = RampFrameBlockStartWidth;
            IndeterminateCurrentBlockEnd = IndeterminateCurrentBlockStart + RampFrameBlockWidth;
        }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            Console.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            int RedColorNum = RandomDriver.Next(IndeterminateSettings.IndeterminateMinimumRedColorLevel, IndeterminateSettings.IndeterminateMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Next(IndeterminateSettings.IndeterminateMinimumGreenColorLevel, IndeterminateSettings.IndeterminateMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Next(IndeterminateSettings.IndeterminateMinimumBlueColorLevel, IndeterminateSettings.IndeterminateMaximumBlueColorLevel);
            int ColorNum = RandomDriver.Next(IndeterminateSettings.IndeterminateMinimumColorLevel, IndeterminateSettings.IndeterminateMaximumColorLevel);

            // Console resizing can sometimes cause the cursor to remain visible. This happens on Windows 10's terminal.
            ConsoleWrapper.CursorVisible = false;
            if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                ResizeSyncing = true;

            // Set start and end widths for the ramp frame
            int RampFrameStartWidth = 4;
            int RampFrameEndWidth = ConsoleWrapper.WindowWidth - RampFrameStartWidth;
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition);

            // Draw the frame
            if (!ResizeSyncing)
            {
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperLeftCornerColor) : new Color(ConsoleColor.Gray));
                TextWriterColor.Write(IndeterminateSettings.IndeterminateUpperFrameChar.Repeat(RampFrameSpaces), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperFrameColor) : new Color(ConsoleColor.Gray));
                TextWriterColor.Write(IndeterminateSettings.IndeterminateUpperRightCornerChar, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperRightCornerColor) : new Color(ConsoleColor.Gray));
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : new Color(ConsoleColor.Gray));
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : new Color(ConsoleColor.Gray));
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : new Color(ConsoleColor.Gray));
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : new Color(ConsoleColor.Gray));
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : new Color(ConsoleColor.Gray));
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : new Color(ConsoleColor.Gray));
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerLeftCornerColor) : new Color(ConsoleColor.Gray));
                TextWriterColor.Write(IndeterminateSettings.IndeterminateLowerFrameChar.Repeat(RampFrameSpaces), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerFrameColor) : new Color(ConsoleColor.Gray));
                TextWriterColor.Write(IndeterminateSettings.IndeterminateLowerRightCornerChar, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerRightCornerColor) : new Color(ConsoleColor.Gray));
            }

            // Draw the ramp
            int RampFrameBlockEndWidth = RampFrameEndWidth;
            Color RampCurrentColorInstance;
            if (IndeterminateSettings.IndeterminateTrueColor)
            {
                // Set the current colors
                RampCurrentColorInstance = new Color($"{Convert.ToInt32(RedColorNum)};{Convert.ToInt32(GreenColorNum)};{Convert.ToInt32(BlueColorNum)}");
            }
            else
            {
                // Set the current colors
                RampCurrentColorInstance = new Color(Convert.ToInt32(ColorNum));
            }

            // Fill the ramp!
            while (!(IndeterminateCurrentBlockEnd == RampFrameBlockEndWidth & IndeterminateCurrentBlockDirection == IndeterminateDirection.LeftToRight | IndeterminateCurrentBlockStart == RampFrameBlockStartWidth & IndeterminateCurrentBlockDirection == IndeterminateDirection.RightToLeft))
            {
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;

                // Clear the ramp
                Console.BackgroundColor = ConsoleColor.Black;
                if (IndeterminateCurrentBlockDirection == IndeterminateDirection.LeftToRight)
                {
                    for (int BlockPos = RampFrameBlockStartWidth, loopTo = IndeterminateCurrentBlockStart; BlockPos <= loopTo; BlockPos++)
                    {
                        TextWriterWhereColor.WriteWherePlain(" ", BlockPos, RampCenterPosition - 1, true);
                        TextWriterWhereColor.WriteWherePlain(" ", BlockPos, RampCenterPosition, true);
                        TextWriterWhereColor.WriteWherePlain(" ", BlockPos, RampCenterPosition + 1, true);
                    }
                }
                else
                {
                    for (int BlockPos = IndeterminateCurrentBlockEnd, loopTo1 = RampFrameBlockEndWidth; BlockPos <= loopTo1; BlockPos++)
                    {
                        TextWriterWhereColor.WriteWherePlain(" ", BlockPos, RampCenterPosition - 1, true);
                        TextWriterWhereColor.WriteWherePlain(" ", BlockPos, RampCenterPosition, true);
                        TextWriterWhereColor.WriteWherePlain(" ", BlockPos, RampCenterPosition + 1, true);
                    }
                }

                // Fill the ramp
                KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true);
                for (int BlockPos = IndeterminateCurrentBlockStart, loopTo2 = IndeterminateCurrentBlockEnd; BlockPos <= loopTo2; BlockPos++)
                {
                    TextWriterWhereColor.WriteWherePlain(" ", BlockPos, RampCenterPosition - 1, true);
                    TextWriterWhereColor.WriteWherePlain(" ", BlockPos, RampCenterPosition, true);
                    TextWriterWhereColor.WriteWherePlain(" ", BlockPos, RampCenterPosition + 1, true);
                }

                // Change the start and end positions
                switch (IndeterminateCurrentBlockDirection)
                {
                    case IndeterminateDirection.LeftToRight:
                        {
                            IndeterminateCurrentBlockStart += 1;
                            IndeterminateCurrentBlockEnd += 1;
                            break;
                        }
                    case IndeterminateDirection.RightToLeft:
                        {
                            IndeterminateCurrentBlockStart -= 1;
                            IndeterminateCurrentBlockEnd -= 1;
                            break;
                        }
                }

                // Delay writing
                ThreadManager.SleepNoBlock(IndeterminateSettings.IndeterminateDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Change the direction enumeration
            switch (IndeterminateCurrentBlockDirection)
            {
                case IndeterminateDirection.LeftToRight:
                    {
                        IndeterminateCurrentBlockDirection = IndeterminateDirection.RightToLeft;
                        break;
                    }
                case IndeterminateDirection.RightToLeft:
                    {
                        IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight;
                        break;
                    }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(IndeterminateSettings.IndeterminateDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        private enum IndeterminateDirection
        {
            LeftToRight,
            RightToLeft
        }

    }
}