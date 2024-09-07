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
using System.Runtime.CompilerServices;
using System.Timers;
using Timer = System.Timers.Timer;
using Textify.Figlet;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Terminaux.Base.Buffered;
using System.Text;
using Terminaux.Writer.FancyWriters;
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.Drivers.RNG;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Inputs;
using Terminaux.Writer.MiscWriters.Tools;
using Terminaux.Writer.MiscWriters;

namespace Nitrocid.Extras.Timers.Timers
{
    /// <summary>
    /// Timer CLI module
    /// </summary>
    public static class TimerScreen
    {
        internal static DateTime TimerStarted;
        internal static int FigletTimeOldWidth;
        internal static int FigletTimeOldWidthEnd;
        internal static string timerFigletFont = "Small";
        internal static bool running;
        internal static Color? timerColor;
        private static Timer? _Timer;
        private readonly static Keybinding[] keyBindings =
        [
            new( /* Localizable */ "Start counting down", ConsoleKey.Enter),
            new( /* Localizable */ "Set interval", ConsoleKey.T),
            new( /* Localizable */ "Exit", ConsoleKey.Escape),
        ];

        internal static Timer? Timer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Timer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Timer != null)
                    _Timer.Elapsed -= TimerElapsed;
                _Timer = value;
                if (_Timer != null)
                    _Timer.Elapsed += TimerElapsed;
            }
        }

        static TimerScreen() =>
            Timer = new Timer();

        /// <summary>
        /// Timer figlet font
        /// </summary>
        public static string TimerFigletFont
        {
            get => TimersInit.TimersConfig.TimerFigletFont;
            set => TimersInit.TimersConfig.TimerFigletFont = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "small";
        }

        /// <summary>
        /// Opens the timer screen
        /// </summary>
        public static void OpenTimer()
        {
            Screen timerScreen = new();
            ScreenPart timerScreenPart = new();
            ScreenTools.SetCurrent(timerScreen);
            ColorTools.LoadBack();

            // Populate the figlet font (if any)
            int RedValue = RandomDriver.Random(255);
            int GreenValue = RandomDriver.Random(255);
            int BlueValue = RandomDriver.Random(255);
            timerColor = new(RedValue, GreenValue, BlueValue);
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);

            // Populate the time
            double TimerInterval = 60000d;
            Timer = new Timer
            {
                Interval = TimerInterval
            };

            // Add a dynamic text that shows you the remaining time dynamically
            timerScreenPart.AddDynamicText(() =>
            {
                // If prompted, clear the console
                var builder = new StringBuilder();
                ConsoleWrapper.CursorVisible = false;

                // Populate the positions for time
                var Until =
                    running ?
                    TimerStarted.AddMilliseconds(Timer.Interval) - DateTime.Now :
                    TimeDateMiscRenderers.GetRemainingTimeFromNow((int)Math.Round(TimerInterval));
                int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
                int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                string UntilText = Until.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult);
                int TimeLeftPosition = 0;
                int TimeTopPosition = 0;

                // Prepare the display
                UpdateRemainingPositions(UntilText, ref TimeLeftPosition, ref TimeTopPosition);
                if (FigletTimeOldWidth > 0 && FigletTimeOldWidthEnd > 0)
                {
                    builder.Append(
                        ClearRemainingTimeDisplay(UntilText, FigletTimeOldWidth, FigletTimeOldWidthEnd)
                    );
                }

                // Update the old positions
                FigletTimeOldWidth = (int)Math.Round(HalfWidth - FigletTools.GetFigletWidth(UntilText, FigletFont) / 2d);
                FigletTimeOldWidthEnd = (int)Math.Round(HalfWidth + FigletTools.GetFigletWidth(UntilText, FigletFont) / 2d);

                // Print the keybindings
                int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 1;
                builder.Append(
                    KeybindingsWriter.RenderKeybindings(keyBindings, 0, KeysTextTopPosition)
                );

                // Print the time interval
                if (TimersInit.TimersConfig.EnableFigletTimer)
                {
                    builder.Append(
                        FigletWhereColor.RenderFigletWhere(UntilText, TimeLeftPosition, TimeTopPosition, true, FigletFont, timerColor, KernelColorTools.GetColor(KernelColorType.Background))
                    );
                }
                else
                {
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColorBack(UntilText, TimeLeftPosition, TimeTopPosition, true, timerColor, KernelColorTools.GetColor(KernelColorType.Background))
                    );
                }

                // Return the final result
                return builder.ToString();
            });

            // Main loop
            timerScreen.AddBufferedPart("Timer Update", timerScreenPart);
            bool exiting = false;
            while (!exiting)
            {
                ScreenTools.Render(timerScreen);

                // Check to see if the timer is running to continually update the timer render
                ConsoleKey KeysKeypress = default;
                if (running)
                {
                    // Wait for a keypress
                    if (ConsoleWrapper.KeyAvailable)
                        KeysKeypress = Input.ReadKey().Key;
                }
                else
                {
                    // Wait for a keypress
                    KeysKeypress = Input.ReadKey().Key;
                }

                // Check for a keypress
                switch (KeysKeypress)
                {
                    case ConsoleKey.Enter:
                        // User requested to start up the timer
                        if (Timer.Enabled)
                            break;
                        Timer.Interval = TimerInterval;
                        Timer.Start();
                        TimerStarted = DateTime.Now;
                        running = true;
                        break;
                    case ConsoleKey.T:
                        // User requested to specify the timeout in milliseconds
                        if (Timer.Enabled)
                            break;

                        // Try to parse the interval
                        string UnparsedInterval = InfoBoxInputColor.WriteInfoBoxInputColor(Translate.DoTranslation("Specify the timeout in milliseconds") + " [{0}] ", KernelColorTools.GetColor(KernelColorType.Question), TimerInterval);
                        if (!double.TryParse(UnparsedInterval, out TimerInterval))
                        {
                            // Not numeric.
                            timerScreen.RequireRefresh();
                            InfoBoxColor.WriteInfoBoxColor(Translate.DoTranslation("Indicated timeout is not numeric."), KernelColorTools.GetColor(KernelColorType.Error));
                            TimerInterval = 60000d;
                        }
                        timerScreen.RequireRefresh();
                        break;
                    case ConsoleKey.Escape:
                        // Stop the timer
                        Timer.Stop();
                        Timer.Dispose();
                        exiting = true;
                        break;
                }
            }

            // Clear for cleanliness
            ScreenTools.UnsetCurrent(timerScreen);
            running = false;
            Timer = new Timer();
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = true;
        }

        /// <summary>
        /// Indicates that the timer has elapsed
        /// </summary>
        private static void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            running = false;
            Timer?.Stop();
            int RedValue = RandomDriver.Random(255);
            int GreenValue = RandomDriver.Random(255);
            int BlueValue = RandomDriver.Random(255);
            timerColor = new(RedValue, GreenValue, BlueValue);
        }

        /// <summary>
        /// Updates the remaining positions for time, adapting to Figlet if possible
        /// </summary>
        private static void UpdateRemainingPositions(string RemainingTimeText, ref int TimeLeftPosition, ref int TimeTopPosition)
        {
            // Some initial variables
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);

            // Get the Figlet time left and top position
            int FigletTimeLeftPosition = (int)Math.Round(HalfWidth - FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
            int FigletTimeTopPosition = (int)Math.Round(HalfHeight - FigletTools.GetFigletHeight(RemainingTimeText, FigletFont) / 2d);

            // Now, get the normal time left and top position and update the values according to timer type
            TimeLeftPosition = (int)Math.Round(HalfWidth - RemainingTimeText.Length / 2d);
            TimeTopPosition = HalfHeight - 3;
            if (TimersInit.TimersConfig.EnableFigletTimer)
            {
                TimeLeftPosition = FigletTimeLeftPosition;
                TimeTopPosition = FigletTimeTopPosition;
            }
        }

        private static string ClearRemainingTimeDisplay(string RemainingTimeText, int FigletOldWidth, int FigletOldWidthEnd)
        {
            // Some initial variables
            var builder = new StringBuilder();
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);

            // Get the Figlet time left and top position
            int FigletTimeLeftPosition = (int)Math.Round(HalfWidth - FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
            int FigletTimeLeftEndPosition = (int)Math.Round(HalfWidth + FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
            int FigletTimeTopPosition = (int)Math.Round(HalfHeight - FigletTools.GetFigletHeight(RemainingTimeText, FigletFont) / 2d);
            int FigletTimeBottomPosition = (int)Math.Round(HalfHeight + FigletTools.GetFigletHeight(RemainingTimeText, FigletFont) / 2d);

            // If figlet is enabled, clear the display
            if (TimersInit.TimersConfig.EnableFigletTimer)
            {
                for (int FigletTimePosition = FigletTimeTopPosition; FigletTimePosition <= FigletTimeBottomPosition; FigletTimePosition++)
                {
                    ConsoleWrapper.CursorTop = FigletTimePosition;
                    for (int Position = FigletOldWidth - 1; Position <= FigletTimeLeftPosition - 1; Position++)
                    {
                        builder.Append(
                            CsiSequences.GenerateCsiCursorPosition(Position + 1, FigletTimePosition + 1) +
                            " "
                        );
                    }
                    for (int Position = FigletOldWidthEnd; Position <= FigletTimeLeftEndPosition + 1; Position++)
                    {
                        builder.Append(
                            CsiSequences.GenerateCsiCursorPosition(Position + 1, FigletTimePosition + 1) +
                            " "
                        );
                    }
                }
            }

            // Return the result
            return builder.ToString();
        }

    }
}
