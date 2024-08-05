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

using Terminaux.Writer.ConsoleWriters;
using KS.Misc.Reflection;
using KS.Misc.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Text
    /// </summary>
    public static class TextSettings
    {
        private static bool textTrueColor = true;
        private static int textDelay = 1000;
        private static string textWrite = "Nitrocid KS";
        private static bool textRainbowMode;
        private static int textMinimumRedColorLevel = 0;
        private static int textMinimumGreenColorLevel = 0;
        private static int textMinimumBlueColorLevel = 0;
        private static int textMinimumColorLevel = 0;
        private static int textMaximumRedColorLevel = 255;
        private static int textMaximumGreenColorLevel = 255;
        private static int textMaximumBlueColorLevel = 255;
        private static int textMaximumColorLevel = 255;

        /// <summary>
        /// [Text] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool TextTrueColor
        {
            get
            {
                return textTrueColor;
            }
            set
            {
                textTrueColor = value;
            }
        }
        /// <summary>
        /// [Text] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TextDelay
        {
            get
            {
                return textDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                textDelay = value;
            }
        }
        /// <summary>
        /// [Text] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public static string TextWrite
        {
            get
            {
                return textWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textWrite = value;
            }
        }
        /// <summary>
        /// [Text] Enables the rainbow colors mode
        /// </summary>
        public static bool TextRainbowMode
        {
            get
            {
                return textRainbowMode;
            }
            set
            {
                textRainbowMode = value;
            }
        }
        /// <summary>
        /// [Text] The minimum red color level (true color)
        /// </summary>
        public static int TextMinimumRedColorLevel
        {
            get
            {
                return textMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum green color level (true color)
        /// </summary>
        public static int TextMinimumGreenColorLevel
        {
            get
            {
                return textMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum blue color level (true color)
        /// </summary>
        public static int TextMinimumBlueColorLevel
        {
            get
            {
                return textMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextMinimumColorLevel
        {
            get
            {
                return textMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                textMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum red color level (true color)
        /// </summary>
        public static int TextMaximumRedColorLevel
        {
            get
            {
                return textMaximumRedColorLevel;
            }
            set
            {
                if (value <= textMinimumRedColorLevel)
                    value = textMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum green color level (true color)
        /// </summary>
        public static int TextMaximumGreenColorLevel
        {
            get
            {
                return textMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textMinimumGreenColorLevel)
                    value = textMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum blue color level (true color)
        /// </summary>
        public static int TextMaximumBlueColorLevel
        {
            get
            {
                return textMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textMinimumBlueColorLevel)
                    value = textMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextMaximumColorLevel
        {
            get
            {
                return textMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= textMinimumColorLevel)
                    value = textMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                textMaximumColorLevel = value;
            }
        }
    }

    /// <summary>
    /// Display code for Text
    /// </summary>
    public class TextDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Text";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            currentHueAngle = 0;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextColor();
            string renderedText = TextSettings.TextWrite;
            int halfConsoleY = (int)(ConsoleWrapper.WindowHeight / 2d);
            int textPosX = ConsoleWrapper.WindowWidth / 2 - renderedText.Length / 2;

            // Write the text
            if (TextSettings.TextRainbowMode)
            {
                color = new($"hsl:{currentHueAngle};100;50");
                currentHueAngle++;
                if (currentHueAngle > 360)
                    currentHueAngle = 0;
            }
            TextWriterWhereColor.WriteWhereColor(renderedText, textPosX, halfConsoleY, color);

            // Delay
            int delay = TextSettings.TextRainbowMode ? 16 : TextSettings.TextDelay;
            ThreadManager.SleepNoBlock(delay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        private Color ChangeTextColor()
        {
            Color ColorInstance;
            if (TextSettings.TextTrueColor)
            {
                int RedColorNum = RandomDriver.Random(TextSettings.TextMinimumRedColorLevel, TextSettings.TextMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(TextSettings.TextMinimumGreenColorLevel, TextSettings.TextMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(TextSettings.TextMinimumBlueColorLevel, TextSettings.TextMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(TextSettings.TextMinimumColorLevel, TextSettings.TextMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
