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

using System;
using System.Collections;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Animations.Glitch
{
    /// <summary>
    /// Glitch animation module
    /// </summary>
    public static class Glitch
    {

        /// <summary>
        /// Simulates the glitching animation
        /// </summary>
        public static void Simulate(GlitchSettings Settings)
        {
            // Select random positions to generate the glitch
            double GlitchDense = (Settings.GlitchDensity > 100 ? 100 : Settings.GlitchDensity) / 100d;
            int AmountOfBlocks = ConsoleWrapper.WindowWidth * ConsoleWrapper.WindowHeight - 1;
            int BlocksToCover = (int)Math.Round(AmountOfBlocks * GlitchDense);
            var CoveredBlocks = new ArrayList();
            while (CoveredBlocks.Count != BlocksToCover || !ConsoleResizeListener.WasResized(false))
            {
                if (!ConsoleResizeListener.WasResized(false))
                {
                    // Select random position to cover
                    int CoverX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                    int CoverY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

                    // Glitch!
                    GlitchAt(CoverX, CoverY);

                    // Add to covered position
                    if (!CoveredBlocks.Contains(CoverX.ToString() + ", " + CoverY.ToString()))
                        CoveredBlocks.Add(CoverX.ToString() + ", " + CoverY.ToString());
                }
                else
                {
                    // We're resizing.
                    ConsoleWrapper.CursorVisible = false;
                    break;
                }
                ThreadManager.SleepNoBlock(Settings.GlitchDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

        /// <summary>
        /// Glitch at specified position
        /// </summary>
        public static void GlitchAt() =>
            GlitchAt(RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth), RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight));

        /// <summary>
        /// Glitch at specified position
        /// </summary>
        public static void GlitchAt(int CoverX, int CoverY)
        {
            ConsoleWrapper.SetCursorPosition(CoverX, CoverY);

            // Select random glitch type
            GlitchType GlitchType = (GlitchType)Convert.ToInt32(Enum.Parse(typeof(GlitchType), RandomDriver.Random(4).ToString()));

            // Select random letter
            bool LetterCapitalized = Convert.ToBoolean(RandomDriver.Random(1));
            int LetterRangeStart = LetterCapitalized ? 65 : 97;
            int LetterRangeEnd = LetterCapitalized ? 90 : 122;
            char Letter = Convert.ToChar(RandomDriver.Random(LetterRangeStart, LetterRangeEnd));

            // Select random symbol
            bool UseExtendedAscii = Convert.ToBoolean(RandomDriver.Random(1));
            int SymbolRangeStart = UseExtendedAscii ? 128 : 33;
            int SymbolRangeEnd = UseExtendedAscii ? 256 : 64;
            char Symbol = Convert.ToChar(RandomDriver.Random(SymbolRangeStart, SymbolRangeEnd));

            // Select red, green, or blue background and foreground
            GlitchColorType GlitchBlockColorType = (GlitchColorType)Convert.ToInt32(Enum.Parse(typeof(GlitchColorType), RandomDriver.Random(2).ToString()));
            GlitchColorType GlitchLetterColorType = (GlitchColorType)Convert.ToInt32(Enum.Parse(typeof(GlitchColorType), RandomDriver.Random(2).ToString()));
            bool ColorLetter = Convert.ToBoolean(RandomDriver.Random(1));
            int ColorBlockNumber = RandomDriver.Random(0, 255);
            int ColorLetterNumber = RandomDriver.Random(0, 255);
            var ColorBlockInstance = Color.Empty;
            var ColorLetterInstance = Color.Empty;

            // ...for the block
            switch (GlitchBlockColorType)
            {
                case GlitchColorType.Red:
                    {
                        ColorBlockInstance = new Color(ColorBlockNumber, 0, 0);
                        break;
                    }
                case GlitchColorType.Green:
                    {
                        ColorBlockInstance = new Color(0, ColorBlockNumber, 0);
                        break;
                    }
                case GlitchColorType.Blue:
                    {
                        ColorBlockInstance = new Color(0, 0, ColorBlockNumber);
                        break;
                    }
            }

            // ...and for the letter
            switch (GlitchLetterColorType)
            {
                case GlitchColorType.Red:
                    {
                        ColorLetterInstance = new Color(ColorLetterNumber, 0, 0);
                        break;
                    }
                case GlitchColorType.Green:
                    {
                        ColorLetterInstance = new Color(0, ColorLetterNumber, 0);
                        break;
                    }
                case GlitchColorType.Blue:
                    {
                        ColorLetterInstance = new Color(0, 0, ColorLetterNumber);
                        break;
                    }
            }

            // Now, print based on the glitch type
            switch (GlitchType)
            {
                case GlitchType.RandomLetter:
                    {
                        if (ColorLetter)
                            KernelColorTools.SetConsoleColor(ColorLetterInstance);
                        else
                            ConsoleWrapper.ForegroundColor = ConsoleColor.White;
                        ConsoleWrapper.Write(Letter);
                        break;
                    }
                case GlitchType.RandomSymbol:
                    {
                        if (ColorLetter)
                            KernelColorTools.SetConsoleColor(ColorLetterInstance);
                        else
                            ConsoleWrapper.ForegroundColor = ConsoleColor.White;
                        ConsoleWrapper.Write(Symbol);
                        break;
                    }
                case GlitchType.RedGreenBlueColor:
                    {
                        KernelColorTools.SetConsoleColor(ColorBlockInstance, true);
                        ConsoleWrapper.Write(" ");
                        break;
                    }
                case GlitchType.RedGreenBlueColorWithRandomLetter:
                    {
                        if (ColorLetter)
                            KernelColorTools.SetConsoleColor(ColorLetterInstance);
                        else
                            ConsoleWrapper.ForegroundColor = ConsoleColor.White;
                        KernelColorTools.SetConsoleColor(ColorBlockInstance, true);
                        ConsoleWrapper.Write(Letter);
                        break;
                    }
                case GlitchType.RedGreenBlueColorWithRandomSymbol:
                    {
                        if (ColorLetter)
                            KernelColorTools.SetConsoleColor(ColorLetterInstance);
                        else
                            ConsoleWrapper.ForegroundColor = ConsoleColor.White;
                        KernelColorTools.SetConsoleColor(ColorBlockInstance, true);
                        ConsoleWrapper.Write(Symbol);
                        break;
                    }
            }
        }

    }

    enum GlitchType
    {
        /// <summary>
        /// A block with either red, green, or blue as color, and can be darkened
        /// </summary>
        RedGreenBlueColor,
        /// <summary>
        /// A block with either red, green, or blue as color, and can be darkened and feature a random letter printed in white
        /// </summary>
        RedGreenBlueColorWithRandomLetter,
        /// <summary>
        /// A block with either red, green, or blue as color, and can be darkened and feature a random symbol printed in white
        /// </summary>
        RedGreenBlueColorWithRandomSymbol,
        /// <summary>
        /// A random letter printed in white, red, green, or blue, and can be darkened
        /// </summary>
        RandomLetter,
        /// <summary>
        /// A random symbol printed in white, red, green, or blue, and can be darkened
        /// </summary>
        RandomSymbol
    }

    enum GlitchColorType
    {
        /// <summary>
        /// The red color
        /// </summary>
        Red,
        /// <summary>
        /// The green color
        /// </summary>
        Green,
        /// <summary>
        /// The blue color
        /// </summary>
        Blue
    }
}
