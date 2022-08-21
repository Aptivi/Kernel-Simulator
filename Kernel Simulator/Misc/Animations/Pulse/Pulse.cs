﻿using System;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Animations.Pulse
{
    public static class Pulse
    {

        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static bool ResizeSyncing;

        /// <summary>
        /// Simulates the pulsing animation
        /// </summary>
        public static void Simulate(PulseSettings Settings)
        {
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            var RandomDriver = Settings.RandomDriver;
            int RedColorNum = RandomDriver.Next(Settings.PulseMinimumRedColorLevel, Settings.PulseMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Next(Settings.PulseMinimumGreenColorLevel, Settings.PulseMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Next(Settings.PulseMinimumBlueColorLevel, Settings.PulseMaximumBlueColorLevel);
            Console.CursorVisible = false;

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.PulseMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.PulseMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.PulseMaxSteps;
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.PulseMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.PulseMaxSteps);
                ThreadManager.SleepNoBlock(Settings.PulseDelay, System.Threading.Thread.CurrentThread);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn), true, true);
                    Console.Clear();
                }
            }

            // Fade out
            for (int CurrentStep = 1, loopTo = Settings.PulseMaxSteps; CurrentStep <= loopTo; CurrentStep++)
            {
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.PulseMaxSteps);
                ThreadManager.SleepNoBlock(Settings.PulseDelay, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut), true, true);
                    Console.Clear();
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            ThreadManager.SleepNoBlock(Settings.PulseDelay, System.Threading.Thread.CurrentThread);
        }

    }
}