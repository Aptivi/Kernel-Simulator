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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Misc.Screensaver;
using KS.Misc.Screensaver.Displays;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.Misc.Games
{
    static class Snaker
    {

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeSnaker(bool Simulation)
        {
            // Variables
            var RandomDriver = new Random();
            int CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            int CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            var ResizeSyncing = default(bool);
            int SnakeLength = 1;
            var SnakeMassPositions = new List<string>();
            var Direction = SnakeDirection.Bottom;
            ConsoleWrapper.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleWrapper.Clear();

            // Get the floor color ready
            var FloorColor = ChangeSnakeColor();

            // Draw the floor
            if (!ResizeSyncing)
            {
                int FloorTopLeftEdge = 2;
                int FloorBottomLeftEdge = 2;
                DebugWriter.Wdbg(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", FloorTopLeftEdge, FloorBottomLeftEdge);

                int FloorTopRightEdge = ConsoleWrapper.WindowWidth - 3;
                int FloorBottomRightEdge = ConsoleWrapper.WindowWidth - 3;
                DebugWriter.Wdbg(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", FloorTopRightEdge, FloorBottomRightEdge);

                int FloorTopEdge = 2;
                int FloorBottomEdge = ConsoleWrapper.WindowHeight - 2;
                DebugWriter.Wdbg(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", FloorTopEdge, FloorBottomEdge);

                int FloorLeftEdge = 2;
                int FloorRightEdge = ConsoleWrapper.WindowWidth - 4;
                DebugWriter.Wdbg(DebugLevel.I, "Left edge: {0}, Right edge: {1}", FloorLeftEdge, FloorRightEdge);
                KernelColorTools.SetConsoleColor(FloorColor, true);

                // First, draw the floor top edge
                for (int x = FloorTopLeftEdge, loopTo = FloorTopRightEdge; x <= loopTo; x++)
                {
                    ConsoleWrapper.SetCursorPosition(x, 1);
                    DebugWriter.Wdbg(DebugLevel.I, "Drawing floor top edge ({0}, {1})", x, 1);
                    TextWriterColor.WritePlain(" ", false);
                }

                // Second, draw the floor bottom edge
                for (int x = FloorBottomLeftEdge, loopTo1 = FloorBottomRightEdge; x <= loopTo1; x++)
                {
                    ConsoleWrapper.SetCursorPosition(x, FloorBottomEdge);
                    DebugWriter.Wdbg(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", x, FloorBottomEdge);
                    TextWriterColor.WritePlain(" ", false);
                }

                // Third, draw the floor left edge
                for (int y = FloorTopEdge, loopTo2 = FloorBottomEdge; y <= loopTo2; y++)
                {
                    ConsoleWrapper.SetCursorPosition(FloorLeftEdge, y);
                    DebugWriter.Wdbg(DebugLevel.I, "Drawing floor left edge ({0}, {1})", FloorLeftEdge, y);
                    TextWriterColor.WritePlain("  ", false);
                }

                // Finally, draw the floor right edge
                for (int y = FloorTopEdge, loopTo3 = FloorBottomEdge; y <= loopTo3; y++)
                {
                    ConsoleWrapper.SetCursorPosition(FloorRightEdge, y);
                    DebugWriter.Wdbg(DebugLevel.I, "Drawing floor right edge ({0}, {1})", FloorRightEdge, y);
                    TextWriterColor.WritePlain("  ", false);
                }
            }

            // Get the snake color ready
            var SnakeColor = ChangeSnakeColor();

            // A typical snake usually starts in the middle.
            if (!ResizeSyncing)
            {
                bool Dead = false;
                int FloorTopEdge = 1;
                int FloorBottomEdge = ConsoleWrapper.WindowHeight - 2;
                int FloorLeftEdge = 3;
                int FloorRightEdge = ConsoleWrapper.WindowWidth - 4;
                DebugWriter.Wdbg(DebugLevel.I, "Floor top edge {0}", FloorTopEdge);
                DebugWriter.Wdbg(DebugLevel.I, "Floor bottom edge {0}", FloorBottomEdge);
                DebugWriter.Wdbg(DebugLevel.I, "Floor left edge {0}", FloorLeftEdge);
                DebugWriter.Wdbg(DebugLevel.I, "Floor right edge {0}", FloorRightEdge);

                int SnakeCurrentX = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
                int SnakeCurrentY = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                DebugWriter.Wdbg(DebugLevel.I, "Initial snake position ({0}, {1})", SnakeCurrentX, SnakeCurrentY);

                int SnakeAppleX = RandomDriver.Next(FloorLeftEdge + 1, FloorRightEdge - 1);
                int SnakeAppleY = RandomDriver.Next(FloorTopEdge + 1, FloorBottomEdge - 1);
                DebugWriter.Wdbg(DebugLevel.I, "Initial snake apple position ({0}, {1})", SnakeAppleX, SnakeAppleY);

                bool DidHorizontal = false;
                bool DidVertical = false;
                bool AppleDrawn = false;
                int SnakePreviousX;
                int SnakePreviousY;
                var SnakeLastTailToWipeX = default(int);
                var SnakeLastTailToWipeY = default(int);

                while (!Dead)
                {
                    // Delay
                    if (Simulation)
                        ThreadManager.SleepNoBlock(SnakerSettings.SnakerDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    else
                        Thread.Sleep(SnakerSettings.SnakerDelay);
                    if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                        ResizeSyncing = true;
                    if (ResizeSyncing)
                        break;

                    // Remove excess mass
                    Console.BackgroundColor = ConsoleColor.Black;
                    ConsoleWrapper.SetCursorPosition(SnakeLastTailToWipeX, SnakeLastTailToWipeY);
                    TextWriterColor.WritePlain(" ", false);

                    // Set the snake color
                    KernelColorTools.SetConsoleColor(SnakeColor, true);

                    // Draw an apple
                    if (!AppleDrawn)
                    {
                        AppleDrawn = true;
                        ConsoleWrapper.SetCursorPosition(SnakeAppleX, SnakeAppleY);
                        TextWriterColor.WritePlain("+", false);
                        DebugWriter.Wdbg(DebugLevel.I, "Drawn apple at ({0}, {1})", SnakeAppleX, SnakeAppleY);
                    }

                    // Make a snake
                    for (int PositionIndex = SnakeMassPositions.Count - 1; PositionIndex >= 0; PositionIndex -= 1)
                    {
                        string[] PositionStrings = SnakeMassPositions[PositionIndex].Split('/');
                        int PositionX = Convert.ToInt32(PositionStrings[0]);
                        int PositionY = Convert.ToInt32(PositionStrings[1]);
                        ConsoleWrapper.SetCursorPosition(PositionX, PositionY);
                        TextWriterColor.WritePlain(" ", false);
                        ConsoleWrapper.SetCursorPosition(PositionX, PositionY);
                        DebugWriter.Wdbg(DebugLevel.I, "Drawn snake at ({0}, {1}) for mass {2}/{3}", PositionX, PositionY, PositionIndex + 1, SnakeMassPositions.Count);
                    }

                    // Set the previous positions
                    SnakePreviousX = SnakeCurrentX;
                    SnakePreviousY = SnakeCurrentY;

                    if (Simulation)
                    {
                        // Change the snake direction
                        float PossibilityToChange = (float)RandomDriver.NextDouble();
                        if ((int)Math.Round(PossibilityToChange) == 1)
                        {
                            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Change guaranteed. {0}", PossibilityToChange);
                            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Horizontal? {0}, Vertical? {1}", DidHorizontal, DidVertical);
                            if (DidHorizontal)
                            {
                                Direction = (SnakeDirection)Convert.ToInt32(Enum.Parse(typeof(SnakeDirection), RandomDriver.Next(2).ToString()));
                            }
                            else if (DidVertical)
                            {
                                Direction = (SnakeDirection)Convert.ToInt32(Enum.Parse(typeof(SnakeDirection), RandomDriver.Next(2, 4).ToString()));
                            }
                        }
                        switch (Direction)
                        {
                            case SnakeDirection.Bottom:
                                {
                                    SnakeCurrentY += 1;
                                    DidHorizontal = false;
                                    DidVertical = true;
                                    DebugWriter.Wdbg(DebugLevel.I, "Increased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY);
                                    break;
                                }
                            case SnakeDirection.Top:
                                {
                                    SnakeCurrentY -= 1;
                                    DidHorizontal = false;
                                    DidVertical = true;
                                    DebugWriter.Wdbg(DebugLevel.I, "Decreased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY);
                                    break;
                                }
                            case SnakeDirection.Left:
                                {
                                    SnakeCurrentX -= 1;
                                    DidHorizontal = true;
                                    DidVertical = false;
                                    DebugWriter.Wdbg(DebugLevel.I, "Decreased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX);
                                    break;
                                }
                            case SnakeDirection.Right:
                                {
                                    SnakeCurrentX += 1;
                                    DidHorizontal = true;
                                    DidVertical = false;
                                    DebugWriter.Wdbg(DebugLevel.I, "Increased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        // User pressed the arrow button to move the snake
                        if (Console.KeyAvailable)
                        {
                            var Pressed = Input.DetectKeypress().Key;
                            switch (Pressed)
                            {
                                case ConsoleKey.DownArrow:
                                    {
                                        if (DidHorizontal)
                                        {
                                            Direction = SnakeDirection.Bottom;
                                        }

                                        break;
                                    }
                                case ConsoleKey.UpArrow:
                                    {
                                        if (DidHorizontal)
                                        {
                                            Direction = SnakeDirection.Top;
                                        }

                                        break;
                                    }
                                case ConsoleKey.LeftArrow:
                                    {
                                        if (DidVertical)
                                        {
                                            Direction = SnakeDirection.Left;
                                        }

                                        break;
                                    }
                                case ConsoleKey.RightArrow:
                                    {
                                        if (DidVertical)
                                        {
                                            Direction = SnakeDirection.Right;
                                        }

                                        break;
                                    }
                            }
                        }
                        switch (Direction)
                        {
                            case SnakeDirection.Bottom:
                                {
                                    SnakeCurrentY += 1;
                                    DidHorizontal = false;
                                    DidVertical = true;
                                    DebugWriter.Wdbg(DebugLevel.I, "Increased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY);
                                    break;
                                }
                            case SnakeDirection.Top:
                                {
                                    SnakeCurrentY -= 1;
                                    DidHorizontal = false;
                                    DidVertical = true;
                                    DebugWriter.Wdbg(DebugLevel.I, "Decreased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY);
                                    break;
                                }
                            case SnakeDirection.Left:
                                {
                                    SnakeCurrentX -= 1;
                                    DidHorizontal = true;
                                    DidVertical = false;
                                    DebugWriter.Wdbg(DebugLevel.I, "Decreased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX);
                                    break;
                                }
                            case SnakeDirection.Right:
                                {
                                    SnakeCurrentX += 1;
                                    DidHorizontal = true;
                                    DidVertical = false;
                                    DebugWriter.Wdbg(DebugLevel.I, "Increased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX);
                                    break;
                                }
                        }
                    }
                    DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Snake is facing {0}.", Direction.ToString());

                    // Check death using mass position check
                    Dead = SnakeMassPositions.Contains($"{SnakeCurrentX}/{SnakeCurrentY}");
                    DebugWriter.Wdbg(DebugLevel.I, "Mass position contains the current position ({0}, {1})? {2}", SnakeCurrentX, SnakeCurrentY, Dead);

                    // Add the mass position
                    if (!Dead)
                        SnakeMassPositions.Add($"{SnakeCurrentX}/{SnakeCurrentY}");
                    if (SnakeMassPositions.Count > SnakeLength)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Mass position count {0} exceeds snake length of {1}. Removing index 0...", SnakeMassPositions.Count, SnakeLength);
                        string[] LastTailPositionStrings = SnakeMassPositions[0].Split('/');
                        SnakeLastTailToWipeX = Convert.ToInt32(LastTailPositionStrings[0]);
                        SnakeLastTailToWipeY = Convert.ToInt32(LastTailPositionStrings[1]);
                        SnakeMassPositions.RemoveAt(0);
                    }

                    // Check death state
                    if (!Dead)
                        Dead = SnakeCurrentY == FloorTopEdge;
                    DebugWriter.Wdbg(DebugLevel.I, "Dead? {0} because current Y is {1} and top edge is {2}", Dead, SnakeCurrentY, FloorTopEdge);
                    if (!Dead)
                        Dead = SnakeCurrentY == FloorBottomEdge;
                    DebugWriter.Wdbg(DebugLevel.I, "Dead? {0} because current Y is {1} and bottom edge is {2}", Dead, SnakeCurrentY, FloorBottomEdge);
                    if (!Dead)
                        Dead = SnakeCurrentX == FloorLeftEdge;
                    DebugWriter.Wdbg(DebugLevel.I, "Dead? {0} because current X is {1} and left edge is {2}", Dead, SnakeCurrentX, FloorLeftEdge);
                    if (!Dead)
                        Dead = SnakeCurrentX == FloorRightEdge;
                    DebugWriter.Wdbg(DebugLevel.I, "Dead? {0} because current X is {1} and right edge is {2}", Dead, SnakeCurrentX, FloorRightEdge);

                    // If dead, show dead face
                    if (Dead)
                    {
                        ConsoleWrapper.SetCursorPosition(SnakePreviousX, SnakePreviousY);
                        TextWriterColor.WritePlain("X", false);
                        DebugWriter.Wdbg(DebugLevel.I, "Snake dead at {0}/{1}.", SnakePreviousX, SnakePreviousY);
                    }

                    // If the snake ate the apple, grow it up
                    if (SnakeCurrentX == SnakeAppleX & SnakeCurrentY == SnakeAppleY)
                    {
                        SnakeLength += 1;
                        DebugWriter.Wdbg(DebugLevel.I, "Snake grew up to {0}.", SnakeLength);

                        // Relocate the apple
                        SnakeAppleX = RandomDriver.Next(FloorLeftEdge + 1, FloorRightEdge - 1);
                        SnakeAppleY = RandomDriver.Next(FloorTopEdge + 1, FloorBottomEdge - 1);
                        AppleDrawn = false;
                        DebugWriter.Wdbg(DebugLevel.I, "New snake apple position ({0}, {1})", SnakeAppleX, SnakeAppleY);
                    }
                }
            }

            // Show the stage for few seconds before wiping
            if (Simulation)
                ThreadManager.SleepNoBlock(SnakerSettings.SnakerStageDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            else
                Thread.Sleep(SnakerSettings.SnakerStageDelay);

            // Reset mass and console display
            SnakeMassPositions.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleWrapper.Clear();
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
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                return new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
            }
            else
            {
                int ColorNum = RandomDriver.Next(SnakerSettings.SnakerMinimumColorLevel, SnakerSettings.SnakerMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
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