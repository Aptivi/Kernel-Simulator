﻿
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Screensaver.Displays;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Users.Login;
using KS.Kernel.Events;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.ConsoleBase.Colors;
using KS.Drivers.Console;
using KS.Drivers;
using System.Linq;
using KS.Users;
using System.Diagnostics;

namespace KS.Misc.Screensaver
{
    /// <summary>
    /// Screensaver management module
    /// </summary>
    public static class Screensaver
    {

        // Private variables
        internal static Dictionary<string, BaseScreensaver> Screensavers = new()
        {
            { "aurora", new AuroraDisplay() },
            { "barrot", new BarRotDisplay() },
            { "barwave", new BarWaveDisplay() },
            { "beatfader", new BeatFaderDisplay() },
            { "beatpulse", new BeatPulseDisplay() },
            { "beatedgepulse", new BeatEdgePulseDisplay() },
            { "bouncingblock", new BouncingBlockDisplay() },
            { "bouncingtext", new BouncingTextDisplay() },
            { "bsod", new BSODDisplay() },
            { "colormix", new ColorMixDisplay() },
            { "dateandtime", new DateAndTimeDisplay() },
            { "disco", new DiscoDisplay() },
            { "dissolve", new DissolveDisplay() },
            { "edgepulse", new EdgePulseDisplay() },
            { "equalizer", new EqualizerDisplay() },
            { "excalibeats", new ExcaliBeatsDisplay() },
            { "fader", new FaderDisplay() },
            { "faderback", new FaderBackDisplay() },
            { "fallingline", new FallingLineDisplay() },
            { "figlet", new FigletDisplay() },
            { "fireworks", new FireworksDisplay() },
            { "flashcolor", new FlashColorDisplay() },
            { "flashtext", new FlashTextDisplay() },
            { "glitch", new GlitchDisplay() },
            { "glittercolor", new GlitterColorDisplay() },
            { "glittermatrix", new GlitterMatrixDisplay() },
            { "gradient", new GradientDisplay() },
            { "gradientrot", new GradientRotDisplay() },
            { "indeterminate", new IndeterminateDisplay() },
            { "ksx", new KSXDisplay() },
            { "lighter", new LighterDisplay() },
            { "lightspeed", new LightspeedDisplay() },
            { "lines", new LinesDisplay() },
            { "linotypo", new LinotypoDisplay() },
            { "lyrics", new LyricsDisplay() },
            { "marquee", new MarqueeDisplay() },
            { "matrix", new MatrixDisplay() },
            { "memdump", new MemdumpDisplay() },
            { "mesmerize", new MesmerizeDisplay() },
            { "meteor", new MeteorDisplay() },
            { "newyear", new NewYearDisplay() },
            { "noise", new NoiseDisplay() },
            { "personlookup", new PersonLookupDisplay() },
            { "plain", new PlainDisplay() },
            { "progressclock", new ProgressClockDisplay() },
            { "pulse", new PulseDisplay() },
            { "ramp", new RampDisplay() },
            { "random", new RandomSaverDisplay() },
            { "siren", new SirenDisplay() },
            { "snakefill", new SnakeFillDisplay() },
            { "snaker", new SnakerDisplay() },
            { "spin", new SpinDisplay() },
            { "spotwrite", new SpotWriteDisplay() },
            { "stackbox", new StackBoxDisplay() },
            { "starfield", new StarfieldDisplay() },
            { "typewriter", new TypewriterDisplay() },
            { "typo", new TypoDisplay() },
            { "wave", new WaveDisplay() },
            { "windowslogo", new WindowsLogoDisplay() },
            { "wipe", new WipeDisplay() }
        };
        internal static int scrnTimeout = 300000;
        internal static string defSaverName = "plain";
        internal static bool LockMode;
        internal static bool inSaver;
        internal static AutoResetEvent SaverAutoReset = new(false);
        internal static KernelThread Timeout = new("Screensaver timeout thread", false, HandleTimeout) { isCritical = true };

        // Public Variables
        /// <summary>
        /// Screensaver debugging
        /// </summary>
        public static bool ScreensaverDebug =>
            Config.MainConfig.ScreensaverDebug;

        /// <summary>
        /// Password lock enabled
        /// </summary>
        public static bool PasswordLock =>
            Config.MainConfig.PasswordLock;

        /// <summary>
        /// Whether the kernel is on the screensaver mode
        /// </summary>
        public static bool InSaver =>
            inSaver;

        /// <summary>
        /// Screen timeout in milliseconds
        /// </summary>
        public static int ScreenTimeout =>
            Config.MainConfig.ScreenTimeout;

        /// <summary>
        /// Default screensaver name
        /// </summary>
        public static string DefaultSaverName =>
            Config.MainConfig.DefaultSaverName;

        /// <summary>
        /// Gets the name of the screensavers
        /// </summary>
        public static string[] GetScreensaverNames() =>
            Screensavers.Keys.ToArray();

        /// <summary>
        /// Handles the screensaver time so that when it reaches the time threshold, the screensaver launches
        /// </summary>
        public static void HandleTimeout()
        {
            try
            {
                var termDriver = DriverHandler.GetDriver<IConsoleDriver>("Default");
                while (!Flags.KernelShutdown)
                {
                    int OldCursorLeft = termDriver.CursorLeft;
                    SpinWait.SpinUntil(() => !Flags.ScrnTimeReached || Flags.KernelShutdown);
                    if (!Flags.ScrnTimeReached)
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();
                        SpinWait.SpinUntil(() => (termDriver.KeyAvailable | OldCursorLeft != termDriver.CursorLeft | Flags.KernelShutdown) && stopwatch.ElapsedMilliseconds >= ScreenTimeout, ScreenTimeout);
                        bool locking = !(termDriver.KeyAvailable | OldCursorLeft != termDriver.CursorLeft | Flags.KernelShutdown) && stopwatch.ElapsedMilliseconds >= ScreenTimeout;
                        stopwatch.Reset();
                        if (Flags.KernelShutdown)
                            break;
                        else if (!Flags.RebootRequested && locking)
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Screen time has reached.");
                            LockScreen();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Shutting down screensaver timeout thread: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Shows the screensaver
        /// </summary>
        public static void ShowSavers() =>
            ShowSavers(DefaultSaverName);

        /// <summary>
        /// Shows the screensaver
        /// </summary>
        /// <param name="saver">A specified screensaver</param>
        public static void ShowSavers(string saver)
        {
            try
            {
                EventsManager.FireEvent(EventType.PreShowScreensaver);
                DebugWriter.WriteDebug(DebugLevel.I, "Requested screensaver: {0}", saver);
                if (Screensavers.ContainsKey(saver.ToLower()))
                {
                    saver = saver.ToLower();
                    var BaseSaver = Screensavers[saver];
                    inSaver = true;
                    Flags.ScrnTimeReached = true;
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(BaseSaver);
                    DebugWriter.WriteDebug(DebugLevel.I, "{0} started", saver);
                }
                else if (CustomSaverTools.CustomSavers.ContainsKey(saver))
                {
                    // Only one custom screensaver can be used.
                    inSaver = true;
                    Flags.ScrnTimeReached = true;
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(new CustomDisplay(CustomSaverTools.CustomSavers[saver].ScreensaverBase));
                    DebugWriter.WriteDebug(DebugLevel.I, "Custom screensaver {0} started", saver);
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("The requested screensaver {0} is not found."), true, KernelColorType.Error, saver);
                    DebugWriter.WriteDebug(DebugLevel.I, "Screensaver {0} not found in the dictionary.", saver);
                }
            }
            catch (InvalidOperationException ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Error when trying to start screensaver, because of an invalid operation."), true, KernelColorType.Error);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Error when trying to start screensaver:") + " {0}", true, KernelColorType.Error, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Locks the screen. The password will be required when unlocking, depending on the kernel settings.
        /// </summary>
        public static void LockScreen()
        {
            LockMode = true;
            ShowSavers();
            EventsManager.FireEvent(EventType.PreUnlock, DefaultSaverName);
            while (inSaver)
                Thread.Sleep(1);
            if (PasswordLock)
                Login.ShowPasswordPrompt(UserManagement.CurrentUser.Username);
            else
                LockMode = false;
        }

        /// <summary>
        /// Sets the default screensaver
        /// </summary>
        /// <param name="saver">Specified screensaver</param>
        public static void SetDefaultScreensaver(string saver)
        {
            saver = saver.ToLower();
            if (Screensavers.ContainsKey(saver) | CustomSaverTools.CustomSavers.ContainsKey(saver))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is found. Setting it to default...", saver);
                Config.MainConfig.DefaultSaverName = saver;
                Config.CreateConfig();
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "{0} is not found.", saver);
                throw new KernelException(KernelExceptionType.NoSuchScreensaver, Translate.DoTranslation("Screensaver {0} not found in database. Check the name and try again."), saver);
            }
        }

        /// <summary>
        /// Gets a screensaver instance from loaded assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        public static BaseScreensaver GetScreensaverInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(IScreensaver).Name) is not null)
                    return (BaseScreensaver)Assembly.CreateInstance(t.FullName);
            }
            return null;
        }

        /// <summary>
        /// Screensaver error handler
        /// </summary>
        internal static void HandleSaverError(Exception Exception)
        {
            if (Exception is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Screensaver experienced an error: {0}.", Exception.Message);
                DebugWriter.WriteDebugStackTrace(Exception);
                HandleSaverCancel();
                TextWriterColor.Write(Translate.DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), true, KernelColorType.Error, Exception.Message);
            }
        }

        /// <summary>
        /// Screensaver cancellation handler
        /// </summary>
        internal static void HandleSaverCancel()
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Cancellation is pending. Cleaning everything up...");
            ColorTools.LoadBack();
            ConsoleBase.ConsoleWrapper.CursorVisible = true;
            DebugWriter.WriteDebug(DebugLevel.I, "All clean. Screensaver stopped.");
            SaverAutoReset.Set();
        }

    }
}
