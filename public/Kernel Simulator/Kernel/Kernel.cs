﻿
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.IO;
using ColorSeq;
using Extensification.StringExts;
using KS.Arguments.ArgumentBase;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Hardware;
using KS.Kernel.Exceptions;
using KS.Kernel.Updates;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Probers.Motd;
using KS.Misc.Reflection;
using KS.Misc.Splash;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.MiscWriters;
using KS.Modifications;
using KS.Network.RPC;
using ReadLineReboot;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Users.Groups;
using KS.Users.Login;
using KS.Users;
using KS.Kernel.Events;
using KS.Misc.Text;
using KS.Kernel.Administration.Journalling;
using KS.Files.Operations;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel main class
    /// </summary>
    internal static class Kernel
    {

        internal static Stopwatch StageTimer = new();

        // #ifdef'd variables ... Release specifiers (SPECIFIER: REL, RC, or DEV | MILESTONESPECIFIER: ALPHA, BETA, NONE | None satisfied: Unsupported Release)
#if SPECIFIERREL
        internal readonly static string ReleaseSpecifier = $"RTM";
#elif SPECIFIERRC
        internal readonly static string ReleaseSpecifier = $"RC";
#elif SPECIFIERDEV
#if MILESTONESPECIFIERALPHA
        internal readonly static string ReleaseSpecifier = $"DEV - M7";
#elif MILESTONESPECIFIERBETA
        internal readonly static string ReleaseSpecifier = $"DEV - B1";
#else
        internal readonly static string ReleaseSpecifier = $"DEV - PRE";
#endif
#else
        internal readonly static string ReleaseSpecifier = $"UNSUPPORTED";
#endif

        // Final console window title
        internal readonly static string ConsoleTitle = $"[{(KernelPlatform.IsDotnetCoreClr() ? ".NET CoreCLR" : ".NET Framework")}] [{ReleaseSpecifier}] - Kernel Simulator v{KernelTools.KernelVersion} (API v{KernelTools.KernelApiVersion})";

        /// <summary>
        /// Entry point
        /// </summary>
        internal static void Main(string[] Args)
        {
            // Set main thread name
            Thread.CurrentThread.Name = "Main Kernel Thread";

            // This is a kernel entry point
            while (!Flags.KernelShutdown)
            {
                try
                {
                    // A title
                    ConsoleExtensions.SetTitle(ConsoleTitle);

                    // Initial ReadLine settings
                    ReadLine.CtrlCEnabled = true;
                    Flags.InputHistoryEnabled = true;
                    ReadLine.PrewriteDefaultValue = true;
                    ReadLine.AutoCompletionEnabled = true;

                    // Check for terminal
                    ConsoleSanityChecker.CheckConsole();

                    // Initialize crucial things
                    if (!KernelPlatform.IsOnUnix())
                        Color255.Initialize255();
                    AppDomain.CurrentDomain.AssemblyResolve += AssemblyLookup.LoadFromAssemblySearchPaths;

                    // Check to see if we have an appdata folder for KS
                    if (!Checking.FolderExists(Paths.AppDataPath))
                        Making.MakeDirectory(Paths.AppDataPath, false);

                    // Initialize debug and journal path
                    DebugManager.DebugPath = Getting.GetNumberedFileName(Path.GetDirectoryName(Paths.GetKernelPath(KernelPathType.Debugging)), Paths.GetKernelPath(KernelPathType.Debugging));
                    JournalManager.JournalPath = Getting.GetNumberedFileName(Path.GetDirectoryName(Paths.GetKernelPath(KernelPathType.Journalling)), Paths.GetKernelPath(KernelPathType.Journalling));

                    // Check for kernel command-line arguments
                    ArgumentParse.ParseArguments(Args.ToList(), ArgumentType.CommandLineArgs);

                    // Download debug symbols if not found (loads automatically, useful for debugging problems and stack traces)
                    KernelTools.CheckDebugSymbols();

                    // Check for console size
                    if (Flags.CheckingForConsoleSize)
                    {
                        // Check for the minimum console window requirements (80x24)
                        while (ConsoleWrapper.WindowWidth < 80 | ConsoleWrapper.WindowHeight < 24)
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Your console is too small to run properly:") + " {0}x{1}", true, ColorTools.ColTypes.Warning, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
                            TextWriterColor.Write(Translate.DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), true, ColorTools.ColTypes.Warning);
                            ConsoleWrapper.ReadKey(true);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Looks like you're bypassing the console size detection. Things may not work properly on small screens.") + CharManager.NewLine + Translate.DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), true, ColorTools.ColTypes.Warning);
                        ConsoleWrapper.ReadKey(true);
                        Flags.CheckingForConsoleSize = true;
                    }

                    // Initialize everything
                    StageTimer.Start();
                    KernelTools.InitEverything();
                    CheckErrored();

                    // Stage 1: Initialize the system
                    KernelTools.ReportNewStage(1, Translate.DoTranslation("- Stage 1: System initialization"));
                    if (RemoteDebugger.RDebugAutoStart & Flags.DebugMode)
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("Starting the remote debugger..."), 3, ColorTools.ColTypes.NeutralText);
                        RemoteDebugger.StartRDebugThread();
                        if (!RemoteDebugger.RDebugFailed)
                        {
                            SplashReport.ReportProgress(Translate.DoTranslation("Debug listening on all addresses using port {0}.").FormatString(RemoteDebugger.DebugPort), 5, ColorTools.ColTypes.NeutralText);
                        }
                        else
                        {
                            SplashReport.ReportProgressError(Translate.DoTranslation("Remote debug failed to start: {0}").FormatString(RemoteDebugger.RDebugFailedReason.Message));
                        }
                    }
                    SplashReport.ReportProgress(Translate.DoTranslation("Starting RPC..."), 3, ColorTools.ColTypes.NeutralText);
                    RemoteProcedure.WrapperStartRPC();

                    // If the two files are not found, create two MOTD files with current config.
                    if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MOTD)))
                    {
                        MotdParse.SetMotd(Translate.DoTranslation("Welcome to Kernel!"));
                        SplashReport.ReportProgress(Translate.DoTranslation("Generated default MOTD."), 3, ColorTools.ColTypes.NeutralText);
                    }
                    if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MAL)))
                    {
                        MalParse.SetMal(Translate.DoTranslation("Logged in successfully as <user>"));
                        SplashReport.ReportProgress(Translate.DoTranslation("Generated default MAL."), 3, ColorTools.ColTypes.NeutralText);
                    }

                    // Check for kernel updates
                    if (Flags.CheckUpdateStart)
                        UpdateManager.CheckKernelUpdates();

                    // Phase 2: Probe hardware
                    KernelTools.ReportNewStage(2, Translate.DoTranslation("- Stage 2: Hardware detection"));
                    if (!Flags.QuietHardwareProbe)
                        SplashReport.ReportProgress(Translate.DoTranslation("hwprobe: Your hardware will be probed. Please wait..."), 15, ColorTools.ColTypes.Progress);
                    HardwareProbe.StartProbing();
                    if (!Flags.EnableSplash & !Flags.QuietKernel)
                        HardwareList.ListHardware();
                    CheckErrored();

                    // Phase 3: Parse Mods and Screensavers
                    KernelTools.ReportNewStage(3, Translate.DoTranslation("- Stage 3: Mods and screensavers detection"));
                    DebugWriter.WriteDebug(DebugLevel.I, "Safe mode flag is set to {0}", Flags.SafeMode);
                    if (!Flags.SafeMode)
                    {
                        if (Flags.StartKernelMods)
                            ModManager.StartMods();
                    }
                    else
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("Running in safe mode. Skipping stage..."), 0, ColorTools.ColTypes.NeutralText);
                    }
                    EventsManager.FireEvent("StartKernel");

                    // Phase 4: Log-in
                    KernelTools.ReportNewStage(4, Translate.DoTranslation("- Stage 4: Log in"));
                    UserManagement.InitializeSystemAccount();
                    SplashReport.ReportProgress(Translate.DoTranslation("System account initialized"), 5, ColorTools.ColTypes.NeutralText);
                    UserManagement.InitializeUsers();
                    SplashReport.ReportProgress(Translate.DoTranslation("Users initialized"), 5, ColorTools.ColTypes.NeutralText);
                    GroupManagement.LoadGroups();
                    SplashReport.ReportProgress(Translate.DoTranslation("Groups loaded"), 5, ColorTools.ColTypes.NeutralText);

                    // Reset console state and stop stage timer
                    KernelTools.ReportNewStage(5, "");

                    // Show the closing screen
                    SplashReport.ReportProgress(Translate.DoTranslation("Welcome!"), 100, ColorTools.ColTypes.Success);
                    SplashManager.CloseSplash();

                    // Show current time
                    if (Flags.ShowCurrentTimeBeforeLogin)
                        TimeDate.TimeDate.ShowCurrentTimes();

                    // Notify user of errors if appropriate
                    if (Flags.NotifyKernelError)
                    {
                        Flags.NotifyKernelError = false;
                        Notifications.NotifySend(new Notification(Translate.DoTranslation("Previous boot failed"), KernelTools.LastKernelErrorException.Message, Notifications.NotifPriority.High, Notifications.NotifType.Normal));
                    }

                    // Show license if new style used
                    if (Flags.NewWelcomeStyle | Flags.EnableSplash)
                    {
                        TextWriterColor.Write();
                        SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("License information"), true, ColorTools.ColTypes.Stage);
                        WelcomeMessage.WriteLicense(false);
                    }

#if SPECIFIERDEV
                        TextWriterColor.Write();
                        TextWriterColor.Write("* " + Translate.DoTranslation("You're running the development version of the kernel. While you can experience upcoming features which may exist in the final release, you may run into bugs, instabilities, or even data loss. We recommend using the stable version, if possible."), true, ColorTools.ColTypes.DevelopmentWarning);
#elif SPECIFIERRC
                        TextWriterColor.Write();
                        TextWriterColor.Write("* " + Translate.DoTranslation("You're running the release candidate version of the kernel. While you can experience the final touches, you may run into bugs, instabilities, or even data loss. We recommend using the stable version, if possible."), true, ColorTools.ColTypes.DevelopmentWarning);
#elif SPECIFIERREL == false
                        TextWriterColor.Write();
                        TextWriterColor.Write("* " + Translate.DoTranslation("We recommend against running this version of the kernel, because it is unsupported. If you have downloaded this kernel from unknown sources, this message may appear. Please download from our official downloads page."), true, ColorTools.ColTypes.DevelopmentWarning);
#endif

                    // Initialize login prompt
                    if (!Flags.Maintenance)
                    {
                        Login.LoginPrompt();
                    }
                    else
                    {
                        MotdParse.ReadMotd();
                        MalParse.ReadMal();
                        TextWriterColor.Write(Translate.DoTranslation("Enter the admin password for maintenance."));
                        if (Login.Users.ContainsKey("root"))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Root account found. Prompting for password...");
                            Login.ShowPasswordPrompt("root");
                        }
                        else
                        {
                            // Some malicious mod removed the root account, or rare situation happened and it was gone.
                            DebugWriter.WriteDebug(DebugLevel.W, "Root account not found for maintenance. Initializing it...");
                            UserManagement.InitializeSystemAccount();
                            Login.ShowPasswordPrompt("root");
                        }
                    }

                    // Clear all active threads as we're rebooting
                    ThreadManager.StopAllThreads();
                }
                catch (InsaneConsoleDetectedException icde)
                {
                    ConsoleWrapper.WriteLine(icde.Message);
                    ConsoleWrapper.WriteLine(icde.InsanityReason);
                    Flags.KernelShutdown = true;
                }
                catch (KernelErrorException kee)
                {
                    DebugWriter.WriteDebugStackTrace(kee);
                    Flags.RebootRequested = false;
                    Flags.LogoutRequested = false;
                    Flags.SafeMode = false;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    KernelTools.KernelError(KernelErrorLevel.U, true, 5L, Translate.DoTranslation("Kernel Error while booting: {0}"), ex, ex.Message);
                }
            }

            // Clear the console and reset the colors
            ConsoleWrapper.ResetColor();
            ConsoleWrapper.Clear();

            // If "No APM" is enabled, simply print the text
            if (Flags.SimulateNoAPM)
            {
                ConsoleWrapper.WriteLine(Translate.DoTranslation("It's now safe to turn off your computer."));
                ConsoleWrapper.ReadKey(true);
            }
        }

        /// <summary>
        /// Check to see if KernelError has been called
        /// </summary>
        private static void CheckErrored()
        {
            if (Flags.KernelErrored)
            {
                Flags.KernelErrored = false;
                throw new KernelErrorException(Translate.DoTranslation("Kernel Error while booting: {0}"), KernelTools.LastKernelErrorException, KernelTools.LastKernelErrorException.Message);
            }
        }

    }
}
