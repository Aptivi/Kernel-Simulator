﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using System;
using Terminaux.Writer.FancyWriters;
using Textify.Data.Figlet;
using Nitrocid.ScreensaverPacks.Animations.Glitch;
using Nitrocid.Languages;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for KSXTheEnd
    /// </summary>
    public class KSXTheEndDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "KSXTheEnd";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(0, 0, 0));
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int step;
            int maxSteps = 6;
            Color green = new(0, 255, 0);
            Color red = new(255, 0, 0);
            Color pink = new(255, 0, 255);
            Color blue = new(0, 0, 255);
            Color black = new(0, 0, 0);
            Color white = new(255, 255, 255);
            var font = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int colorSteps = 30;
            int currentR = 0;
            int currentG = 0;
            int currentB = 0;
            int height = ConsoleWrapper.WindowHeight - 2;

            // Start stepping
            for (step = 1; step <= maxSteps; step++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                switch (step)
                {
                    // Step 1: Shows all the versions, but with 0.1.0.
                    case 1:
                        ColorTools.LoadBackDry(black);

                        // Get the color thresholds
                        double thresholdGR = green.RGB.R / (double)colorSteps;
                        double thresholdGG = green.RGB.G / (double)colorSteps;
                        double thresholdGB = green.RGB.B / (double)colorSteps;
                        double thresholdRR = red.RGB.R / (double)colorSteps;
                        double thresholdRG = red.RGB.G / (double)colorSteps;
                        double thresholdRB = red.RGB.B / (double)colorSteps;
                        double thresholdPR = pink.RGB.R / (double)colorSteps;
                        double thresholdPG = pink.RGB.G / (double)colorSteps;
                        double thresholdPB = pink.RGB.B / (double)colorSteps;
                        double thresholdBR = blue.RGB.R / (double)colorSteps;
                        double thresholdBG = blue.RGB.G / (double)colorSteps;
                        double thresholdBB = blue.RGB.B / (double)colorSteps;

                        // Now, transition
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdGR);
                            currentG = (int)Math.Round(currentG + thresholdGG);
                            currentB = (int)Math.Round(currentB + thresholdGB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.1", col, black);
                            CenteredTextColor.WriteCenteredColorBack(height, TimeDateRenderers.RenderDate(new DateTime(2018, 2, 22)), col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdGR);
                            currentG = (int)Math.Round(currentG - thresholdGG);
                            currentB = (int)Math.Round(currentB - thresholdGB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.1", col, black);
                            CenteredTextColor.WriteCenteredColorBack(height, TimeDateRenderers.RenderDate(new DateTime(2018, 2, 22)), col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdRR);
                            currentG = (int)Math.Round(currentG + thresholdRG);
                            currentB = (int)Math.Round(currentB + thresholdRB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.16", col, black);
                            CenteredTextColor.WriteCenteredColorBack(height, TimeDateRenderers.RenderDate(new DateTime(2021, 6, 12)), col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdRR);
                            currentG = (int)Math.Round(currentG - thresholdRG);
                            currentB = (int)Math.Round(currentB - thresholdRB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.16", col, black);
                            CenteredTextColor.WriteCenteredColorBack(height, TimeDateRenderers.RenderDate(new DateTime(2021, 6, 12)), col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdPR);
                            currentG = (int)Math.Round(currentG + thresholdPG);
                            currentB = (int)Math.Round(currentB + thresholdPB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.24", col, black);
                            CenteredTextColor.WriteCenteredColorBack(height, TimeDateRenderers.RenderDate(new DateTime(2022, 8, 2)), col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdPR);
                            currentG = (int)Math.Round(currentG - thresholdPG);
                            currentB = (int)Math.Round(currentB - thresholdPB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.0.24", col, black);
                            CenteredTextColor.WriteCenteredColorBack(height, TimeDateRenderers.RenderDate(new DateTime(2022, 8, 2)), col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdBR);
                            currentG = (int)Math.Round(currentG + thresholdBG);
                            currentB = (int)Math.Round(currentB + thresholdBB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, "v0.1.0", col, black);
                            CenteredTextColor.WriteCenteredColorBack(height, TimeDateRenderers.RenderDate(new DateTime(2024, 3, 11)), col, black);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 2: 0.1.0 doesn't fade out, but a glitch shows
                    case 2:
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            Glitch.GlitchAt();
                        }
                        break;
                    // Step 3: Flickering colors between white and each version's color
                    case 3:
                        int maxFlashes = 200;
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            bool showGreen = flashes % 2 == 0;
                            if (showGreen)
                                ColorTools.LoadBackDry(green);
                            else
                                ColorTools.LoadBackDry(white);
                            ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            bool showRed = flashes % 2 == 0;
                            if (showRed)
                                ColorTools.LoadBackDry(red);
                            else
                                ColorTools.LoadBackDry(white);
                            ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            bool showPink = flashes % 2 == 0;
                            if (showPink)
                                ColorTools.LoadBackDry(pink);
                            else
                                ColorTools.LoadBackDry(white);
                            ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            bool showBlue = flashes % 2 == 0;
                            if (showBlue)
                                ColorTools.LoadBackDry(blue);
                            else
                                ColorTools.LoadBackDry(white);
                            ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 4: White background for a few seconds
                    case 4:
                        ColorTools.LoadBackDry(white);
                        ThreadManager.SleepNoBlock(5000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                    // Step 5: "THE END" shows for a few seconds
                    case 5:
                        CenteredFigletTextColor.WriteCenteredFigletColorBack(font, Translate.DoTranslation("The End").ToUpper(), green, white);
                        ThreadManager.SleepNoBlock(5000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                    // Step 6: With the figlet text, the background fades out
                    case 6:
                        // Get the color thresholds
                        double thresholdR = green.RGB.R / (double)colorSteps;
                        double thresholdG = green.RGB.G / (double)colorSteps;
                        double thresholdB = green.RGB.B / (double)colorSteps;
                        double thresholdBGR = white.RGB.R / (double)colorSteps;
                        double thresholdBGG = white.RGB.G / (double)colorSteps;
                        double thresholdBGB = white.RGB.B / (double)colorSteps;
                        currentR = green.RGB.R;
                        currentG = green.RGB.G;
                        currentB = green.RGB.B;
                        int currentBGR = white.RGB.R;
                        int currentBGG = white.RGB.G;
                        int currentBGB = white.RGB.B;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdR);
                            currentG = (int)Math.Round(currentG - thresholdG);
                            currentB = (int)Math.Round(currentB - thresholdB);
                            currentBGR = (int)Math.Round(currentBGR - thresholdBGR);
                            currentBGG = (int)Math.Round(currentBGG - thresholdBGG);
                            currentBGB = (int)Math.Round(currentBGB - thresholdBGB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            Color colBG = new(currentBGR, currentBGG, currentBGB);
                            ColorTools.LoadBackDry(colBG);
                            CenteredFigletTextColor.WriteCenteredFigletColorBack(font, Translate.DoTranslation("The End").ToUpper(), col, colBG);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                }
            }

            // Reset
            ConsoleResizeHandler.WasResized();
            ColorTools.LoadBackDry(black);
        }

    }
}
