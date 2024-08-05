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
using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Threading;
using Terminaux.Colors;
using Terminaux.Base;
using KS.Misc.Reflection;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BouncingBlock
    /// </summary>
    public static class BouncingBlockSettings
    {
        private static bool _bouncingBlock255Colors;
        private static bool _bouncingBlockTrueColor = true;
        private static int _bouncingBlockDelay = 10;
        private static string _bouncingBlockBackgroundColor = new Color(ConsoleColor.Black).PlainSequence;
        private static string _bouncingBlockForegroundColor = new Color(ConsoleColor.White).PlainSequence;
        private static int _bouncingBlockMinimumRedColorLevel = 0;
        private static int _bouncingBlockMinimumGreenColorLevel = 0;
        private static int _bouncingBlockMinimumBlueColorLevel = 0;
        private static int _bouncingBlockMinimumColorLevel = 0;
        private static int _bouncingBlockMaximumRedColorLevel = 255;
        private static int _bouncingBlockMaximumGreenColorLevel = 255;
        private static int _bouncingBlockMaximumBlueColorLevel = 255;
        private static int _bouncingBlockMaximumColorLevel = 255;

        /// <summary>
        /// [BouncingBlock] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool BouncingBlock255Colors
        {
            get
            {
                return _bouncingBlock255Colors;
            }
            set
            {
                _bouncingBlock255Colors = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool BouncingBlockTrueColor
        {
            get
            {
                return _bouncingBlockTrueColor;
            }
            set
            {
                _bouncingBlockTrueColor = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BouncingBlockDelay
        {
            get
            {
                return _bouncingBlockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _bouncingBlockDelay = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] Screensaver background color
        /// </summary>
        public static string BouncingBlockBackgroundColor
        {
            get
            {
                return _bouncingBlockBackgroundColor;
            }
            set
            {
                _bouncingBlockBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingBlock] Screensaver foreground color
        /// </summary>
        public static string BouncingBlockForegroundColor
        {
            get
            {
                return _bouncingBlockForegroundColor;
            }
            set
            {
                _bouncingBlockForegroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum red color level (true color)
        /// </summary>
        public static int BouncingBlockMinimumRedColorLevel
        {
            get
            {
                return _bouncingBlockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _bouncingBlockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum green color level (true color)
        /// </summary>
        public static int BouncingBlockMinimumGreenColorLevel
        {
            get
            {
                return _bouncingBlockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _bouncingBlockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum blue color level (true color)
        /// </summary>
        public static int BouncingBlockMinimumBlueColorLevel
        {
            get
            {
                return _bouncingBlockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _bouncingBlockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BouncingBlockMinimumColorLevel
        {
            get
            {
                return _bouncingBlockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _bouncingBlock255Colors | _bouncingBlockTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _bouncingBlockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum red color level (true color)
        /// </summary>
        public static int BouncingBlockMaximumRedColorLevel
        {
            get
            {
                return _bouncingBlockMaximumRedColorLevel;
            }
            set
            {
                if (value <= _bouncingBlockMinimumRedColorLevel)
                    value = _bouncingBlockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _bouncingBlockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum green color level (true color)
        /// </summary>
        public static int BouncingBlockMaximumGreenColorLevel
        {
            get
            {
                return _bouncingBlockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _bouncingBlockMinimumGreenColorLevel)
                    value = _bouncingBlockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _bouncingBlockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum blue color level (true color)
        /// </summary>
        public static int BouncingBlockMaximumBlueColorLevel
        {
            get
            {
                return _bouncingBlockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _bouncingBlockMinimumBlueColorLevel)
                    value = _bouncingBlockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _bouncingBlockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BouncingBlockMaximumColorLevel
        {
            get
            {
                return _bouncingBlockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _bouncingBlock255Colors | _bouncingBlockTrueColor ? 255 : 15;
                if (value <= _bouncingBlockMinimumColorLevel)
                    value = _bouncingBlockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _bouncingBlockMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for BouncingBlock
    /// </summary>
    public class BouncingBlockDisplay : BaseScreensaver, IScreensaver
    {

        private string Direction = "BottomRight";
        private int RowBlock, ColumnBlock;
        private Color blockColor = null;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BouncingBlock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RowBlock = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            ColumnBlock = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ColorTools.SetConsoleColor(new Color(BouncingBlockSettings.BouncingBlockForegroundColor));
            ColorTools.LoadBackDry(new Color(BouncingBlockSettings.BouncingBlockBackgroundColor));
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Row block: {0} | Column block: {1}", RowBlock, ColumnBlock);

            // Initialize the color
            blockColor ??= GetRandomBlockColor();

            // Render a block
            if (!ConsoleResizeHandler.WasResized(false))
            {
                TextWriterWhereColor.WriteWhereColorBack(" ", ColumnBlock, RowBlock, true, Color.Empty, blockColor);
            }
            else
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "We're resize-syncing! Setting RowBlock and ColumnBlock to its original position...");
                RowBlock = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                ColumnBlock = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
                ConsoleResizeHandler.WasResized();
                ThreadManager.SleepNoBlock(BouncingBlockSettings.BouncingBlockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                return;
            }

            if (RowBlock == ConsoleWrapper.WindowHeight - 1)
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the bottom.");
                Direction = Direction.Replace("Bottom", "Top");
                blockColor = GetRandomBlockColor();
            }
            else if (RowBlock == 0)
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the top.");
                Direction = Direction.Replace("Top", "Bottom");
                blockColor = GetRandomBlockColor();
            }

            if (ColumnBlock == ConsoleWrapper.WindowWidth - 1)
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the right.");
                Direction = Direction.Replace("Right", "Left");
                blockColor = GetRandomBlockColor();
            }
            else if (ColumnBlock == 0)
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the left.");
                Direction = Direction.Replace("Left", "Right");
                blockColor = GetRandomBlockColor();
            }

            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Block is facing {0}.", Direction);
            if (Direction == "BottomRight")
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Increasing row and column block position");
                RowBlock += 1;
                ColumnBlock += 1;
            }
            else if (Direction == "BottomLeft")
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Increasing row and decreasing column block position");
                RowBlock += 1;
                ColumnBlock -= 1;
            }
            else if (Direction == "TopRight")
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Decreasing row and increasing column block position");
                RowBlock -= 1;
                ColumnBlock += 1;
            }
            else if (Direction == "TopLeft")
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Decreasing row and column block position");
                RowBlock -= 1;
                ColumnBlock -= 1;
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(BouncingBlockSettings.BouncingBlockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        private static Color GetRandomBlockColor()
        {
            if (BouncingBlockSettings.BouncingBlockTrueColor)
            {
                int RedColorNum = RandomDriver.Random(BouncingBlockSettings.BouncingBlockMinimumRedColorLevel, BouncingBlockSettings.BouncingBlockMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(BouncingBlockSettings.BouncingBlockMinimumGreenColorLevel, BouncingBlockSettings.BouncingBlockMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(BouncingBlockSettings.BouncingBlockMinimumBlueColorLevel, BouncingBlockSettings.BouncingBlockMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                return new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(BouncingBlockSettings.BouncingBlockMinimumColorLevel, BouncingBlockSettings.BouncingBlockMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                return new Color(ColorNum);
            }
        }

    }
}
