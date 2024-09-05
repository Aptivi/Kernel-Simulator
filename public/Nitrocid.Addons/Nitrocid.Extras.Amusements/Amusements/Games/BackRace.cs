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
using System.Linq;
using System.Threading;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Languages;
using Nitrocid.Drivers.RNG;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Reader;
using Terminaux.Base.Buffered;
using System.Text;
using Terminaux.Inputs;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    static class BackRace
    {
        internal static BackRaceHorse[] horses = [];

        internal static void OpenBackRace()
        {
            // Clear the screen
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBack();

            // Some essential variables
            int chance = 30;

            // Reset all the horses
            static void ResetAll()
            {
                horses =
                [
                    new BackRaceHorse(1),
                    new BackRaceHorse(2),
                    new BackRaceHorse(3),
                    new BackRaceHorse(4),
                    new BackRaceHorse(5),
                ];
            }
            ResetAll();

            // Add a buffer
            Screen screen = new();
            ScreenPart part = new();

            // Add a UI rendering logic
            int selected = 1;
            bool racing = false;
            int winner = 0;
            Color color = KernelColorTools.GetColor(KernelColorType.NeutralText);
            part.AddDynamicText(() =>
            {
                StringBuilder builder = new();

                // We need to show five boxes and five progress bars representing the horses
                int consoleSixthsHeight = ConsoleWrapper.WindowHeight / 6;
                int boxLeft = 1;
                int boxWidth = 4;
                int progressLeft = 7;
                for (int i = 0; i < 5; i++)
                {
                    // Indicate the selected horse by coloring it white
                    int height = consoleSixthsHeight * i + 3;
                    var horse = horses[i];
                    var finalColor = i + 1 == selected ? ConsoleColors.White : horse.HorseColor;
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColor(Translate.DoTranslation("Horse") + $" {horse.HorseNumber}", 1, height - 1, finalColor) +
                        BorderColor.RenderBorder(boxLeft, height, boxWidth, 1, finalColor) +
                        TextWriterWhereColor.RenderWhereColor($"{horse.HorseProgress:000}%", 2, height + 1, finalColor) +
                        ProgressBarColor.RenderProgress(horse.HorseProgress, progressLeft, height, 8, 2, finalColor, finalColor)
                    );
                }

                // Check to see if we're on the rest mode or on the race mode
                string bindings = Translate.DoTranslation("[ENTER] Start the race | [ESC] Exit | [UP/DOWN] Move selection");
                int bindingsPositionX = ConsoleWrapper.WindowWidth / 2 - bindings.Length / 2;
                int bindingsPositionY = ConsoleWrapper.WindowHeight - 2;
                if (racing)
                {
                    // Write the positions
                    var horsesSorted = horses
                        .OrderByDescending((progress) => progress.HorseProgress)
                        .ToArray();
                    List<string> positions = [];
                    for (int i = 0; i < horsesSorted.Length; i++)
                        positions.Add($"{ColorTools.RenderSetConsoleColor(color)}#{i + 1}: {ColorTools.RenderSetConsoleColor(horsesSorted[i].HorseColor)}{Translate.DoTranslation("Horse")} {horsesSorted[i].HorseNumber}{ColorTools.RenderSetConsoleColor(color)}");
                    string renderedPositions = string.Join(" | ", positions);
                    builder.Append(
                        CenteredTextColor.RenderCenteredOneLine(bindingsPositionY, renderedPositions, color)
                    );
                }
                else
                {
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColor(bindings, bindingsPositionX, bindingsPositionY, color)
                    );
                }

                // Return the result
                return builder.ToString();
            });
            screen.AddBufferedPart("Back Race - UI", part);
            ScreenTools.SetCurrent(screen);

            // Main game loop
            bool exiting = false;
            while (!exiting)
            {
                ScreenTools.Render();
                if (racing)
                {
                    // Update each horse with their own movement
                    for (int i = 0; i < 5; i++)
                    {
                        // Check to see if the horse is moving
                        bool isMoving = RandomDriver.RandomChance(chance);
                        if (isMoving)
                            horses[i].HorseProgress += 1;

                        // Check for the winner
                        if (horses[i].HorseProgress >= 100)
                        {
                            racing = false;
                            winner = i + 1;
                            screen.RequireRefresh();
                        }
                    }

                    // Wait for a few milliseconds
                    Thread.Sleep(100);

                    // If the user chose the same horse that won, congratulate the user.
                    if (selected == winner)
                    {
                        InfoBoxColor.WriteInfoBoxColor(Translate.DoTranslation("Your horse won the race!"), KernelColorTools.GetColor(KernelColorType.Success));
                        ConsoleWrapper.Clear();
                        ResetAll();
                    }
                    else if (winner > 0)
                    {
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Your horse lost the race!"));
                        ConsoleWrapper.Clear();
                        ResetAll();
                    }
                }
                else
                {
                    // Wait for the input
                    winner = 0;
                    var input = Input.ReadKey().Key;
                    switch (input)
                    {
                        case ConsoleKey.UpArrow:
                            // Selected the previous horse
                            selected--;
                            if (selected < 1)
                                selected = 5;
                            break;
                        case ConsoleKey.DownArrow:
                            // Selected the next horse
                            selected++;
                            if (selected > 5)
                                selected = 1;
                            break;
                        case ConsoleKey.Enter:
                            // Started the race
                            racing = true;
                            screen.RequireRefresh();
                            break;
                        case ConsoleKey.Escape:
                            // Exited the game
                            exiting = true;
                            break;
                    }
                }
            }

            // Reset everything
            ScreenTools.UnsetCurrent(screen);
            ColorTools.LoadBack();
        }
    }

    internal class BackRaceHorse
    {
        internal int HorseNumber { get; private set; }
        internal int HorseProgress { get; set; } = 0;
        internal Color HorseColor { get; private set; }

        internal BackRaceHorse(int horseNumber)
        {
            HorseNumber = horseNumber;
            HorseColor = ColorTools.GetRandomColor(ColorType.TrueColor);
        }
    }
}
