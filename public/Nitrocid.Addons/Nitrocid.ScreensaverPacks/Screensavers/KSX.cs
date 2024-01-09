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
using Figletize;
using Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations;
using Nitrocid.ScreensaverPacks.Animations.Glitch;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Drivers;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Kernel.Time;
using Nitrocid.Drivers.RNG;
using Nitrocid.Languages;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for KSX
    /// </summary>
    public class KSXDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "KSX";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBack(new Color(0, 0, 0));
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int step;
            int maxSteps = 13;
            Color darkGreen = new(0, 128, 0);
            Color green = new(0, 255, 0);
            Color black = new(0, 0, 0);
            Color red = new(255, 0, 0);
            Color white = new(255, 255, 255);

            // Start stepping
            for (step = 1; step <= maxSteps; step++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                switch (step)
                {
                    // Step 1: fade the X character in, and use figlet
                    case 1:
                        int colorSteps = 30;

                        // Get the color thresholds
                        double thresholdR = darkGreen.R / (double)colorSteps;
                        double thresholdG = darkGreen.G / (double)colorSteps;
                        double thresholdB = darkGreen.B / (double)colorSteps;

                        // Now, transition from black to the target color
                        int currentR = 0;
                        int currentG = 0;
                        int currentB = 0;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Add the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdR);
                            currentG = (int)Math.Round(currentG + thresholdG);
                            currentB = (int)Math.Round(currentB + thresholdB);

                            // Now, make a color and write the X character using figlet
                            Color col = new(currentR, currentG, currentB);
                            var figFont = FigletTools.GetFigletFont("banner");
                            CenteredFigletTextColor.WriteCenteredFigletColor(figFont, "X", col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 2:
                        int pulses = 11;
                        for (int currentPulse = 1; currentPulse <= pulses; currentPulse++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Get the final color by using the odd/even comparison
                            var finalCol = currentPulse % 2 == 0 ? green : darkGreen;

                            // Pulse the X character, alternating between darkGreen and Green colors
                            var figFont = FigletTools.GetFigletFont("banner");
                            CenteredFigletTextColor.WriteCenteredFigletColor(figFont, "X", finalCol);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 3:
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = darkGreen.R / (double)colorSteps;
                        thresholdG = darkGreen.G / (double)colorSteps;
                        thresholdB = darkGreen.B / (double)colorSteps;

                        // Now, transition from black to the target color
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(darkGreen.R - thresholdR * currentStep);
                            currentG = (int)Math.Round(darkGreen.G - thresholdG * currentStep);
                            currentB = (int)Math.Round(darkGreen.B - thresholdB * currentStep);

                            // Now, make a color and write the X character using figlet
                            Color col = new(currentR, currentG, currentB);
                            var figFont = FigletTools.GetFigletFont("banner");
                            CenteredFigletTextColor.WriteCenteredFigletColor(figFont, "X", col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        // Print the 2018s
                        string sample = "2018|";
                        bool printDone = false;
                        ConsoleWrapper.CursorLeft = 0;
                        ConsoleWrapper.CursorTop = 0;
                        while (!printDone)
                        {
                            // Keep writing 2018 until it reaches the end
                            for (int currentIdx = 0; currentIdx <= sample.Length - 1 && !printDone; currentIdx++)
                            {
                                // Write the current character
                                TextWriterColor.WriteColorBack(sample[currentIdx].ToString(), false, darkGreen, black);

                                // Sleep
                                ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);

                                // Check to see if we're at the end
                                if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth - 1)
                                {
                                    if (ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 1)
                                    {
                                        // We're at the end. Increment the index or reset to zero
                                        currentIdx++;
                                        if (currentIdx > sample.Length - 1)
                                            currentIdx = 0;

                                        // Write the current character
                                        TextWriterColor.WriteColorBack(sample[currentIdx].ToString(), false, darkGreen, black);

                                        // Reset position
                                        ConsoleWrapper.CursorLeft = 0;
                                        ConsoleWrapper.CursorTop = 0;

                                        // Declare as done
                                        printDone = true;
                                    }
                                    else
                                    {
                                        // We're at the end. Increment the index or reset to zero
                                        currentIdx++;
                                        if (currentIdx > sample.Length - 1)
                                            currentIdx = 0;

                                        // Write the current character
                                        TextWriterColor.WriteColorBack(sample[currentIdx].ToString(), false, darkGreen, black);
                                        TextWriterColor.Write();
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        // Print the big 2018
                        var s4figFont = FigletTools.GetFigletFont("banner");
                        int s4figWidth = FigletTools.GetFigletWidth("2018", s4figFont) / 2;
                        int s4figHeight = FigletTools.GetFigletHeight("2018", s4figFont) / 2;
                        int s4consoleX = ConsoleWrapper.WindowWidth / 2 - s4figWidth;
                        int s4consoleY = ConsoleWrapper.WindowHeight / 2 - s4figHeight;
                        FigletWhereColor.WriteFigletWhereColorBack("2018", s4consoleX, s4consoleY, true, s4figFont, green, black);
                        ThreadManager.SleepNoBlock(5000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                    case 5:
                        // Fade the console out
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = darkGreen.R / (double)colorSteps;
                        thresholdG = darkGreen.G / (double)colorSteps;
                        thresholdB = darkGreen.B / (double)colorSteps;

                        // Now, transition from target color to black
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(darkGreen.R - thresholdR * currentStep);
                            currentG = (int)Math.Round(darkGreen.G - thresholdG * currentStep);
                            currentB = (int)Math.Round(darkGreen.B - thresholdB * currentStep);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ColorTools.LoadBack(col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 6:
                        // Show all released kernel versions (only the LTS versions)
                        Dictionary<string, string> versions = new()
                        {
                            { "Kernel 0.0.1",  TimeDateRenderers.RenderDate(new DateTime(2018, 2,  22)) },
                            { "Kernel 0.0.4",  TimeDateRenderers.RenderDate(new DateTime(2018, 5,  20)) },
                            { "Kernel 0.0.8",  TimeDateRenderers.RenderDate(new DateTime(2020, 2,  22)) },
                            { "Kernel 0.0.12", TimeDateRenderers.RenderDate(new DateTime(2020, 11, 6 )) },
                            { "Kernel 0.0.16", TimeDateRenderers.RenderDate(new DateTime(2021, 6,  12)) },
                            { "Kernel 0.0.20", TimeDateRenderers.RenderDate(new DateTime(2022, 2,  22)) },
                            { "Kernel 0.0.24", TimeDateRenderers.RenderDate(new DateTime(2022, 8,  2 )) },
                        };
                        int maxKernels = versions.Count;
                        int selectedKernel = 5;
                        int minimumMoves = 20;
                        bool canSelectFirst = false;
                        bool selectedFirst = false;

                        // Make a random move
                        int currentMove = 0;
                        while (!selectedFirst)
                        {
                            bool movingTop = DriverHandler.CurrentRandomDriverLocal.RandomChance(30);

                            // Make a move
                            currentMove++;
                            if (movingTop)
                            {
                                selectedKernel--;

                                // If we went after the beginning, go to the end
                                if (selectedKernel == 0)
                                    selectedKernel = maxKernels;
                            }
                            else
                            {
                                selectedKernel++;

                                // If we went after the end, go to the beginning
                                if (selectedKernel > maxKernels)
                                    selectedKernel = 1;
                            }

                            // Check to see if we can finally select 0.0.1
                            if (currentMove > minimumMoves)
                                canSelectFirst = true;

                            // If we can select first, wait until the first is selected
                            if (canSelectFirst)
                                if (selectedKernel == 1)
                                    selectedFirst = true;

                            // Render the selection
                            for (int i = 0; i < maxKernels; i++)
                            {
                                int idx = selectedKernel - 1;
                                var ver = versions.ElementAt(i);
                                TextWriterColor.WriteColorBack("- {0}: {1}", true, i == idx ? green : darkGreen, black, ver.Key, ver.Value);
                            }

                            // Sleep
                            ThreadManager.SleepNoBlock(selectedFirst ? 3000 : 250, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            ColorTools.LoadBack(black);
                        }
                        break;
                    case 7:
                        // Display time warp text
                        ColorTools.LoadBack(darkGreen);
                        string timeWarpText = $"Time machine... Warping to {TimeDateRenderers.RenderDate(new DateTime(2018, 2, 22))}...";
                        int textPosX = ConsoleWrapper.WindowWidth / 2 - timeWarpText.Length / 2;
                        int textPosY = ConsoleWrapper.WindowHeight - 8;
                        int textTravelledPosY = ConsoleWrapper.WindowHeight - 6;
                        TextWriterWhereColor.WriteWhereColorBack(timeWarpText, textPosX, textPosY, black, darkGreen);

                        // Display the progress
                        int progPosX = 3;
                        int progPosY = ConsoleWrapper.WindowHeight - 4;
                        int maxProg = 3000;
                        long ksEpochTick = new DateTime(2018, 2, 22).Ticks;
                        long currentTick = TimeDateTools.KernelDateTime.Date.Ticks;
                        long tickDiff = currentTick - ksEpochTick;
                        for (int iteration = 0; iteration < maxProg; iteration++)
                        {
                            // Some power function to make the glitches intense
                            double currentProg = Math.Pow((double)iteration / maxProg * 10, 2);
                            ProgressBarColor.WriteProgress(currentProg, progPosX, progPosY, black, black, darkGreen);

                            // Show current date
                            long travelledTicks = (long)Math.Round(tickDiff * ((double)currentProg / 100));
                            long travelledTickFromCurrent = currentTick - travelledTicks;
                            DateTime travelled = new(travelledTickFromCurrent);
                            string timeWarpCurrentDate = $"Travelled: {TimeDateRenderers.RenderDate(travelled)}";
                            TextWriterWhereColor.WriteWhereColorBack(timeWarpCurrentDate + $"{$"{ConsoleExtensions.GetClearLineToRightSequence()}"}", progPosX, textTravelledPosY, black, darkGreen);

                            // Now, do the glitch
                            bool isGlitch = RandomDriver.RandomChance(currentProg);
                            if (isGlitch)
                                Glitch.GlitchAt();

                            // Sleep
                            if (iteration >= maxProg - 50)
                            {
                                for (int delayed = 0; delayed < 5000; delayed += 10)
                                {
                                    ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                                    if (RandomDriver.RandomChance(currentProg))
                                        Glitch.GlitchAt();
                                }
                                break;
                            }
                            else
                                ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 8:
                        // Print the big SYSTEM ERROR
                        var s8figFont = FigletTools.GetFigletFont("banner");
                        int s8figWidth = FigletTools.GetFigletWidth("SYSTEM ERROR", s8figFont) / 2;
                        int s8figHeight = FigletTools.GetFigletHeight("SYSTEM ERROR", s8figFont) / 2;
                        int s8consoleX = ConsoleWrapper.WindowWidth / 2 - s8figWidth;
                        int s8consoleY = ConsoleWrapper.WindowHeight / 2 - s8figHeight;
                        FigletWhereColor.WriteFigletWhereColor("SYSTEM ERROR", s8consoleX, s8consoleY, true, s8figFont, red);
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            if (RandomDriver.RandomChance(90))
                                Glitch.GlitchAt();
                        }
                        break;
                    case 9:
                        string SysWipeText = $"Deleting SYSTEM32...";
                        int sysWipeTextPosX = ConsoleWrapper.WindowWidth / 2 - SysWipeText.Length / 2;
                        int sysWipeTextPosY = ConsoleWrapper.WindowHeight - 8;
                        TextWriterWhereColor.WriteWhereColorBack(SysWipeText, sysWipeTextPosX, sysWipeTextPosY, black, darkGreen);

                        // Display the progress
                        int sysWipeProgPosX = 3;
                        int sysWipeProgPosY = ConsoleWrapper.WindowHeight - 4;
                        int sysWipeMaxProg = 800;
                        for (int iteration = 0; iteration < sysWipeMaxProg; iteration++)
                        {
                            // Some power function to make the glitches intense
                            double currentProg = (double)iteration / sysWipeMaxProg * 100;
                            ProgressBarColor.WriteProgress(currentProg, sysWipeProgPosX, sysWipeProgPosY, black, black, darkGreen);

                            // Now, do the glitch
                            Glitch.GlitchAt();

                            // Sleep
                            if (iteration >= sysWipeMaxProg - 50)
                            {
                                for (int delayed = 0; delayed < 1000; delayed += 10)
                                {
                                    ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                                    if (RandomDriver.RandomChance(currentProg))
                                        Glitch.GlitchAt();
                                }
                                break;
                            }
                            else
                                ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 10:
                        new WindowsXP().DisplayBugCheck(WindowsXP.BugCheckCodes.IRQL_NOT_LESS_OR_EQUAL);
                        int width = ConsoleWrapper.CursorLeft;
                        int height = ConsoleWrapper.CursorTop;
                        for (int dumpIter = 0; dumpIter < 22; dumpIter++)
                        {
                            if (dumpIter % 10 == 0)
                                TextWriterWhereColor.WriteWhere("{0}", width, height, dumpIter);
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        TextWriterWhereColor.WriteWhere("Physical memory dump FAILED with status 0xC0000010", 0, height);
                        break;
                    case 11:
                        for (int xIter = 0; xIter < 1000; xIter++)
                        {
                            int xwidth = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                            int xheight = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                            TextWriterWhereColor.WriteWhereColorBack("X", xwidth, xheight, white, black);
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 12:
                        // Fade the console out
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = white.R / (double)colorSteps;
                        thresholdG = white.G / (double)colorSteps;
                        thresholdB = white.B / (double)colorSteps;

                        // Now, transition from target color to black
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(white.R - thresholdR * currentStep);
                            currentG = (int)Math.Round(white.G - thresholdG * currentStep);
                            currentB = (int)Math.Round(white.B - thresholdB * currentStep);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ColorTools.LoadBack(col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 13:
                        string tbc = Translate.DoTranslation("To be continued...").ToUpper();
                        ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ThreadManager.SleepNoBlock(40, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, black, black);
                        ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, black, black);
                        ThreadManager.SleepNoBlock(1000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ThreadManager.SleepNoBlock(5000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                }
            }

            // Reset
            ConsoleResizeHandler.WasResized();
            ColorTools.LoadBack(black);
        }

    }
}
