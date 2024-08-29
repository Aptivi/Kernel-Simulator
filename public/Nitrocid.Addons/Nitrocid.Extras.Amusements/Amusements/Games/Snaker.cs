﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using System.Collections.Generic;
using System.Threading;
using Terminaux.Colors;
using Nitrocid.Extras.Amusements.Screensavers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Reader;
using Terminaux.Colors.Transformation.Contrast;
using Terminaux.Base.Buffered;
using System.Text;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Inputs;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    static class Snaker
    {

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeSnaker(bool Simulation)
        {
            // Clear the screen
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBack();

            // Variables
            int SnakeLength = 1;
            var SnakeMassPositions = new List<string>();
            var Direction = Enum.Parse<SnakeDirection>(RandomDriver.Random(3).ToString());

            // Get the floor color ready
            var FloorColor = ChangeSnakeColor();

            // Add a buffer
            Screen screen = new();
            ScreenPart part = new();

            // Draw the floor
            part.BackgroundColor(FloorColor);
            part.AddDynamicText(() =>
            {
                StringBuilder floor = new();

                int FloorTopLeftEdge = 2;
                int FloorBottomLeftEdge = 2;
                DebugWriter.WriteDebug(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", FloorTopLeftEdge, FloorBottomLeftEdge);

                int FloorTopRightEdge = ConsoleWrapper.WindowWidth - 3;
                int FloorBottomRightEdge = ConsoleWrapper.WindowWidth - 3;
                DebugWriter.WriteDebug(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", FloorTopRightEdge, FloorBottomRightEdge);

                int FloorTopEdge = 2;
                int FloorBottomEdge = ConsoleWrapper.WindowHeight - 2;
                DebugWriter.WriteDebug(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", FloorTopEdge, FloorBottomEdge);

                int FloorLeftEdge = 2;
                int FloorRightEdge = ConsoleWrapper.WindowWidth - 4;
                DebugWriter.WriteDebug(DebugLevel.I, "Left edge: {0}, Right edge: {1}", FloorLeftEdge, FloorRightEdge);

                // First, draw the floor top edge
                for (int x = FloorTopLeftEdge; x <= FloorTopRightEdge; x++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor top edge ({0}, {1})", x, 1);
                    floor.Append(
                        CsiSequences.GenerateCsiCursorPosition(x + 1, 2) +
                        " "
                    );
                }

                // Second, draw the floor bottom edge
                for (int x = FloorBottomLeftEdge; x <= FloorBottomRightEdge; x++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", x, FloorBottomEdge);
                    floor.Append(
                        CsiSequences.GenerateCsiCursorPosition(x + 1, FloorBottomEdge + 1) +
                        " "
                    );
                }

                // Third, draw the floor left edge
                for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor left edge ({0}, {1})", FloorLeftEdge, y);
                    floor.Append(
                        CsiSequences.GenerateCsiCursorPosition(FloorLeftEdge + 1, y + 1) +
                        " "
                    );
                }

                // Finally, draw the floor right edge
                for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor right edge ({0}, {1})", FloorRightEdge, y);
                    floor.Append(
                        CsiSequences.GenerateCsiCursorPosition(FloorRightEdge + 2, y + 1) +
                        " "
                    );
                }

                // Render the result
                return floor.ToString();
            });

            // Get the snake color ready
            var SnakeColor = ChangeSnakeColor();
            var snakeForeColor = ColorTools.GetGray(SnakeColor, ColorContrastType.Ntsc);

            // A typical snake usually starts in the middle.
            int FloorTopEdge = 1;
            int FloorBottomEdge = ConsoleWrapper.WindowHeight - 2;
            int FloorLeftEdge = 3;
            int FloorRightEdge = ConsoleWrapper.WindowWidth - 4;
            DebugWriter.WriteDebug(DebugLevel.I, "Floor top edge {0}", FloorTopEdge);
            DebugWriter.WriteDebug(DebugLevel.I, "Floor bottom edge {0}", FloorBottomEdge);
            DebugWriter.WriteDebug(DebugLevel.I, "Floor left edge {0}", FloorLeftEdge);
            DebugWriter.WriteDebug(DebugLevel.I, "Floor right edge {0}", FloorRightEdge);

            int SnakeCurrentX = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int SnakeCurrentY = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebug(DebugLevel.I, "Initial snake position ({0}, {1})", SnakeCurrentX, SnakeCurrentY);

            int SnakeAppleX = RandomDriver.Random(FloorLeftEdge + 1, FloorRightEdge - 1);
            int SnakeAppleY = RandomDriver.Random(FloorTopEdge + 1, FloorBottomEdge - 1);
            DebugWriter.WriteDebug(DebugLevel.I, "Initial snake apple position ({0}, {1})", SnakeAppleX, SnakeAppleY);

            bool Dead = false;
            bool DidHorizontal = false;
            bool DidVertical = false;
            int SnakePreviousX = SnakeCurrentX;
            int SnakePreviousY = SnakeCurrentY;
            var SnakeLastTailToWipeX = 0;
            var SnakeLastTailToWipeY = 0;
            part.AddDynamicText(() =>
            {
                StringBuilder buffer = new();
                FloorBottomEdge = ConsoleWrapper.WindowHeight - 2;
                FloorRightEdge = ConsoleWrapper.WindowWidth - 4;

                // Remove excess mass and set the snake color
                buffer.Append(
                    CsiSequences.GenerateCsiCursorPosition(SnakeLastTailToWipeX + 1, SnakeLastTailToWipeY + 1) +
                    $"{ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)} " +
                    ColorTools.RenderSetConsoleColor(SnakeColor, true) +
                    ColorTools.RenderSetConsoleColor(snakeForeColor)
                );

                // Draw an apple
                buffer.Append(
                    CsiSequences.GenerateCsiCursorPosition(SnakeAppleX + 1, SnakeAppleY + 1) +
                    "+"
                );
                DebugWriter.WriteDebug(DebugLevel.I, "Drawn apple at ({0}, {1})", SnakeAppleX, SnakeAppleY);

                // Make a snake
                for (int PositionIndex = SnakeMassPositions.Count - 1; PositionIndex >= 0; PositionIndex -= 1)
                {
                    var PositionStrings = SnakeMassPositions[PositionIndex].Split('/');
                    int PositionX = Convert.ToInt32(PositionStrings[0]);
                    int PositionY = Convert.ToInt32(PositionStrings[1]);
                    string snakeHead = PositionIndex < SnakeMassPositions.Count - 1 ? " " :
                        Direction == SnakeDirection.Top ? "^" :
                        Direction == SnakeDirection.Bottom ? "v" :
                        Direction == SnakeDirection.Left ? "<" :
                        Direction == SnakeDirection.Right ? ">" :
                        " ";
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(PositionX + 1, PositionY + 1) +
                        snakeHead
                    );
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawn snake at ({0}, {1}) for mass {2}/{3}", PositionX, PositionY, PositionIndex + 1, SnakeMassPositions.Count);
                }

                // If dead, show dead face to indicate that the dead snake died. Dead snake is game over.
                if (Dead)
                {
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(SnakePreviousX + 1, SnakePreviousY + 1) +
                        "X"
                    );
                    DebugWriter.WriteDebug(DebugLevel.I, "Snake died at {0}/{1}.", SnakePreviousX, SnakePreviousY);
                }

                // Return the result
                return buffer.ToString();
            });
            screen.AddBufferedPart("Snaker - UI", part);
            ScreenTools.SetCurrent(screen);

            // Main loop
            while (!Dead)
            {
                // Delay
                if (Simulation)
                    ThreadManager.SleepNoBlock(SnakerSettings.SnakerDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                else
                    Thread.Sleep(SnakerSettings.SnakerDelay);
                ScreenTools.Render();

                // Move the snake according to the mode
                if (Simulation)
                {
                    // Change the snake direction
                    float PossibilityToChange = (float)RandomDriver.RandomDouble();
                    if ((int)Math.Round(PossibilityToChange) == 1)
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Change guaranteed. {0}", PossibilityToChange);
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Horizontal? {0}, Vertical? {1}", DidHorizontal, DidVertical);
                        if (DidHorizontal)
                            Direction = (SnakeDirection)Convert.ToInt32(Enum.Parse(typeof(SnakeDirection), RandomDriver.RandomIdx(2).ToString()));
                        else if (DidVertical)
                            Direction = (SnakeDirection)Convert.ToInt32(Enum.Parse(typeof(SnakeDirection), RandomDriver.RandomIdx(2, 4).ToString()));
                    }
                    switch (Direction)
                    {
                        case SnakeDirection.Bottom:
                            SnakeCurrentY += 1;
                            DidHorizontal = false;
                            DidVertical = true;
                            DebugWriter.WriteDebug(DebugLevel.I, "Increased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY);
                            break;
                        case SnakeDirection.Top:
                            SnakeCurrentY -= 1;
                            DidHorizontal = false;
                            DidVertical = true;
                            DebugWriter.WriteDebug(DebugLevel.I, "Decreased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY);
                            break;
                        case SnakeDirection.Left:
                            SnakeCurrentX -= 1;
                            DidHorizontal = true;
                            DidVertical = false;
                            DebugWriter.WriteDebug(DebugLevel.I, "Decreased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX);
                            break;
                        case SnakeDirection.Right:
                            SnakeCurrentX += 1;
                            DidHorizontal = true;
                            DidVertical = false;
                            DebugWriter.WriteDebug(DebugLevel.I, "Increased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX);
                            break;
                    }
                }
                else
                {
                    // User pressed the arrow button to move the snake
                    if (ConsoleWrapper.KeyAvailable)
                    {
                        var Pressed = Input.ReadKey().Key;
                        switch (Pressed)
                        {
                            case ConsoleKey.DownArrow:
                                if (DidHorizontal)
                                    Direction = SnakeDirection.Bottom;
                                break;
                            case ConsoleKey.UpArrow:
                                if (DidHorizontal)
                                    Direction = SnakeDirection.Top;
                                break;
                            case ConsoleKey.LeftArrow:
                                if (DidVertical)
                                    Direction = SnakeDirection.Left;
                                break;
                            case ConsoleKey.RightArrow:
                                if (DidVertical)
                                    Direction = SnakeDirection.Right;
                                break;
                        }
                    }
                    switch (Direction)
                    {
                        case SnakeDirection.Bottom:
                            SnakeCurrentY += 1;
                            DidHorizontal = false;
                            DidVertical = true;
                            DebugWriter.WriteDebug(DebugLevel.I, "Increased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY);
                            break;
                        case SnakeDirection.Top:
                            SnakeCurrentY -= 1;
                            DidHorizontal = false;
                            DidVertical = true;
                            DebugWriter.WriteDebug(DebugLevel.I, "Decreased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY);
                            break;
                        case SnakeDirection.Left:
                            SnakeCurrentX -= 1;
                            DidHorizontal = true;
                            DidVertical = false;
                            DebugWriter.WriteDebug(DebugLevel.I, "Decreased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX);
                            break;
                        case SnakeDirection.Right:
                            SnakeCurrentX += 1;
                            DidHorizontal = true;
                            DidVertical = false;
                            DebugWriter.WriteDebug(DebugLevel.I, "Increased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX);
                            break;
                    }
                }
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Snake is facing {0}.", Direction.ToString());

                // Check death using mass position check
                Dead = SnakeMassPositions.Contains($"{SnakeCurrentX}/{SnakeCurrentY}");
                DebugWriter.WriteDebug(DebugLevel.I, "Mass position contains the current position ({0}, {1})? {2}", SnakeCurrentX, SnakeCurrentY, Dead);

                // Add the mass position
                if (!Dead)
                    SnakeMassPositions.Add($"{SnakeCurrentX}/{SnakeCurrentY}");
                if (SnakeMassPositions.Count > SnakeLength)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Mass position count {0} exceeds snake length of {1}. Removing index 0...", SnakeMassPositions.Count, SnakeLength);
                    var LastTailPositionStrings = SnakeMassPositions[0].Split('/');
                    SnakeLastTailToWipeX = Convert.ToInt32(LastTailPositionStrings[0]);
                    SnakeLastTailToWipeY = Convert.ToInt32(LastTailPositionStrings[1]);
                    SnakeMassPositions.RemoveAt(0);
                }

                // Check death state
                if (!Dead)
                    Dead = SnakeCurrentY == FloorTopEdge;
                DebugWriter.WriteDebug(DebugLevel.I, "Dead? {0} because current Y is {1} and top edge is {2}", Dead, SnakeCurrentY, FloorTopEdge);
                if (!Dead)
                    Dead = SnakeCurrentY == FloorBottomEdge;
                DebugWriter.WriteDebug(DebugLevel.I, "Dead? {0} because current Y is {1} and bottom edge is {2}", Dead, SnakeCurrentY, FloorBottomEdge);
                if (!Dead)
                    Dead = SnakeCurrentX == FloorLeftEdge;
                DebugWriter.WriteDebug(DebugLevel.I, "Dead? {0} because current X is {1} and left edge is {2}", Dead, SnakeCurrentX, FloorLeftEdge);
                if (!Dead)
                    Dead = SnakeCurrentX == FloorRightEdge;
                DebugWriter.WriteDebug(DebugLevel.I, "Dead? {0} because current X is {1} and right edge is {2}", Dead, SnakeCurrentX, FloorRightEdge);

                // If the snake ate the apple, grow it up
                if (SnakeCurrentX == SnakeAppleX & SnakeCurrentY == SnakeAppleY)
                {
                    SnakeLength += 1;
                    DebugWriter.WriteDebug(DebugLevel.I, "Snake grew up to {0}.", SnakeLength);

                    // Relocate the apple
                    SnakeAppleX = RandomDriver.Random(FloorLeftEdge + 1, FloorRightEdge - 1);
                    SnakeAppleY = RandomDriver.Random(FloorTopEdge + 1, FloorBottomEdge - 1);
                    DebugWriter.WriteDebug(DebugLevel.I, "New snake apple position ({0}, {1})", SnakeAppleX, SnakeAppleY);
                }

                // Set the previous positions
                SnakePreviousX = SnakeCurrentX;
                SnakePreviousY = SnakeCurrentY;
            }
            ScreenTools.Render();

            // Show the stage for few seconds before wiping
            if (Simulation)
                ThreadManager.SleepNoBlock(SnakerSettings.SnakerStageDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            else
                Thread.Sleep(SnakerSettings.SnakerStageDelay);

            // Reset mass and console display
            SnakeMassPositions.Clear();
            ColorTools.LoadBack();
            ConsoleResizeHandler.WasResized();
        }

        /// <summary>
        /// Changes the snake color
        /// </summary>
        public static Color ChangeSnakeColor()
        {
            var RandomDriver = new Random();
            if (SnakerSettings.SnakerTrueColor)
            {
                int RedColorNum = RandomDriver.Next(SnakerSettings.SnakerMinimumRedColorLevel, SnakerSettings.SnakerMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Next(SnakerSettings.SnakerMinimumGreenColorLevel, SnakerSettings.SnakerMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Next(SnakerSettings.SnakerMinimumBlueColorLevel, SnakerSettings.SnakerMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                return new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
            }
            else
            {
                int ColorNum = RandomDriver.Next(SnakerSettings.SnakerMinimumColorLevel, SnakerSettings.SnakerMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                return new Color(ColorNum);
            }
        }

        /// <summary>
        /// Where would the snake go?
        /// </summary>
        public enum SnakeDirection
        {
            Top,
            Bottom,
            Left,
            Right
        }

    }
}
