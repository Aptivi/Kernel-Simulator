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

using System.Linq;
using KS.ConsoleBase;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Splash;
using KS.Users.Login;
using KS.Misc.Text;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Colors;
using KS.Kernel.Power;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Starting;
using KS.Kernel.Configuration;
using KS.Arguments;
using KS.ConsoleBase.Writers.MiscWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Time.Renderers;
using KS.Kernel.Debugging;
using KS.Misc.Text.Probers.Placeholder;
using KS.Network.RSS;
using KS.Shell.ShellBase.Shells;
using KS.Misc.Text.Probers.Motd;

namespace KS.Kernel
{
    internal static class KernelEntry
    {
        internal static void EntryPoint(string[] args)
        {
            // Initialize very important components
            KernelInitializers.InitializeCritical();

            // Check for kernel command-line arguments
            ArgumentParse.ParseArguments(args.ToList());

            // Some command-line arguments may request kernel shutdown
            if (KernelFlags.KernelShutdown)
                return;

            // Check for console size
            if (KernelFlags.CheckingForConsoleSize)
            {
                ConsoleChecker.CheckConsoleSize();
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Looks like you're bypassing the console size detection. Things may not work properly on small screens.") + CharManager.NewLine +
                                      Translate.DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), true, KernelColorType.Warning);
                Input.DetectKeypress();
                KernelFlags.CheckingForConsoleSize = true;
            }

            // Initialize important components
            KernelTools.StageTimer.Start();
            PowerManager.Uptime.Start();
            KernelInitializers.InitializeEssential();
            KernelInitializers.InitializeWelcomeMessages();
            KernelTools.CheckErrored();

            // Notify user of errors if appropriate
            KernelPanic.NotifyBootFailure();

            // Iterate through available stages
            for (int i = 1; i <= KernelStageTools.Stages.Count + 1; i++)
                KernelStageTools.RunKernelStage(i);

            // Show the closing screen
            SplashReport.ReportProgress(Translate.DoTranslation("Welcome!"), 100);
            SplashManager.CloseSplash();
            SplashReport._KernelBooted = true;
            if (!KernelFlags.EnableSplash)
                TextWriterColor.Write();

            // If this is the first time, run the first run presentation
            if (KernelFlags.FirstTime)
            {
                KernelFlags.FirstTime = false;
                KernelFirstRun.PresentFirstRun();
            }

            // Start the main loop
            DebugWriter.WriteDebug(DebugLevel.I, "Main Loop start.");
            MainLoop();
            ShellStart.PurgeShells();
            DebugWriter.WriteDebug(DebugLevel.I, "Main Loop end.");
        }

        private static void MainLoop()
        {
            while (!(KernelFlags.RebootRequested | KernelFlags.KernelShutdown))
            {
                // Initialize login prompt
                if (!KernelFlags.Maintenance)
                    Login.LoginPrompt();
                else
                    Login.PromptMaintenanceLogin();
                KernelTools.CheckErrored();

                // Check to see if login handler requested power action
                if (KernelFlags.RebootRequested | KernelFlags.KernelShutdown)
                    return;

                // Show license information
                WelcomeMessage.WriteLicense();

                // Show current time
                SeparatorWriterColor.WriteSeparatorKernelColor(Translate.DoTranslation("Welcome!"), true, KernelColorType.Stage);
                if (KernelFlags.ShowCurrentTimeBeforeLogin)
                    TimeDateMiscRenderers.ShowCurrentTimes();
                TextWriterColor.Write();

                // Show the tip
                if (KernelFlags.ShowTip)
                    WelcomeMessage.ShowTip();

                // Show MOTD
                KernelFlags.ShowMOTDOnceFlag = true;
                if (KernelFlags.ShowMAL)
                    TextWriterColor.WriteKernelColor(PlaceParse.ProbePlaces(MalParse.MAL), true, KernelColorType.Banner);
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded MAL.");

                // Show headline
                RSSTools.ShowHeadlineLogin();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded headline.");

                // Initialize shell
                DebugWriter.WriteDebug(DebugLevel.I, "Shell is being initialized.");
                ShellStart.StartShellForced(ShellType.Shell);
            }
        }
    }
}
