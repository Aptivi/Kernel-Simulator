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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Journaling;
using KS.Languages;
using System;

namespace KS.Misc.Splash
{
    /// <summary>
    /// Splash reporting module
    /// </summary>
    public static class SplashReport
    {

        internal static int _Progress = 0;
        internal static string _ProgressText = "";
        internal static bool _KernelBooted = false;
        internal static bool _InSplash = false;

        /// <summary>
        /// The progress indicator of the kernel 
        /// </summary>
        public static int Progress => _Progress;

        /// <summary>
        /// The progress text to indicate how did the kernel progress
        /// </summary>
        public static string ProgressText => _ProgressText;

        /// <summary>
        /// Did the kernel boot successfully?
        /// </summary>
        public static bool KernelBooted => _KernelBooted;

        /// <summary>
        /// Did the kernel enter splash screen?
        /// </summary>
        public static bool InSplash => _InSplash;

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Progress">The progress indicator of the kernel</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgress(string Text, int Progress, params object[] Vars) => 
            ReportProgress(Text, Progress, false, SplashManager.CurrentSplash, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Progress">The progress indicator of the kernel</param>
        /// <param name="force">Force report progress to splash</param>
        /// <param name="splash">Splash interface</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgress(string Text, int Progress, bool force = false, ISplash splash = null, params object[] Vars)
        {
            if (!KernelBooted && InSplash || force)
            {
                _Progress += Progress;
                _ProgressText = Text;
                if (_Progress >= 100)
                    _Progress = 100;
                if (SplashManager.CurrentSplashInfo.DisplaysProgress)
                {
                    if (Flags.EnableSplash && splash != null)
                    {
                        splash.Report(_Progress, Text, Vars);
                    }
                    else if (!Flags.QuietKernel)
                    {
                        TextWriterColor.Write($"  [{_Progress}%] {Text}", true, KernelColorType.Tip, Vars);
                    }
                }
            }
            else
            {
                TextWriterColor.Write(Text, true, KernelColorType.Tip, Vars);
            }
            JournalManager.WriteJournal(Text, Vars);
        }

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgressWarning(string Text, params object[] Vars) =>
            ReportProgressWarning(Text, false, SplashManager.CurrentSplash, null, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="exception">Exception information</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgressWarning(string Text, Exception exception, params object[] Vars) =>
            ReportProgressWarning(Text, false, SplashManager.CurrentSplash, exception, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="force">Force report progress to splash</param>
        /// <param name="splash">Splash interface</param>
        /// <param name="exception">Exception information</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgressWarning(string Text, bool force = false, ISplash splash = null, Exception exception = null, params object[] Vars)
        {
            if (!KernelBooted && InSplash || force)
            {
                _ProgressText = Text;
                if (SplashManager.CurrentSplashInfo.DisplaysProgress)
                {
                    if (Flags.EnableSplash && splash != null)
                    {
                        splash.ReportWarning(_Progress, Text, exception, Vars);
                    }
                    else if (!Flags.QuietKernel)
                    {
                        TextWriterColor.Write($"  [{_Progress}%] Warning: {Text}", true, KernelColorType.Warning, Vars);
                    }
                }
            }
            else
            {
                TextWriterColor.Write(Text, true, KernelColorType.Warning, Vars);
            }
            JournalManager.WriteJournal(Text, JournalStatus.Warning, Vars);
        }

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgressError(string Text, params object[] Vars) =>
            ReportProgressError(Text, false, SplashManager.CurrentSplash, null, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="exception">Exception information</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgressError(string Text, Exception exception, params object[] Vars) =>
            ReportProgressError(Text, false, SplashManager.CurrentSplash, exception, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="force">Force report progress to splash</param>
        /// <param name="splash">Splash interface</param>
        /// <param name="exception">Exception information</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgressError(string Text, bool force = false, ISplash splash = null, Exception exception = null, params object[] Vars)
        {
            if (!KernelBooted && InSplash || force)
            {
                _ProgressText = Text;
                if (SplashManager.CurrentSplashInfo.DisplaysProgress)
                {
                    if (Flags.EnableSplash && splash != null)
                    {
                        splash.ReportError(_Progress, Text, exception, Vars);
                    }
                    else if (!Flags.QuietKernel)
                    {
                        TextWriterColor.Write($"  [{_Progress}%] Error: {Text}", true, KernelColorType.Error, Vars);
                    }
                }
            }
            else
            {
                TextWriterColor.Write(Text, true, KernelColorType.Error, Vars);
            }
            JournalManager.WriteJournal(Text, JournalStatus.Error, Vars);
        }

        /// <summary>
        /// Reports the new kernel stage
        /// </summary>
        /// <param name="StageNumber">The stage number</param>
        /// <param name="StageText">The stage text</param>
        internal static void ReportNewStage(int StageNumber, string StageText)
        {
            // Show the stage finish times
            if (StageNumber <= 1)
            {
                if (Flags.ShowStageFinishTimes)
                {
                    ReportProgress(Translate.DoTranslation("Internal initialization finished in") + $" {KernelTools.StageTimer.Elapsed}", 0);
                    KernelTools.StageTimer.Restart();
                }
            }
            else if (StageNumber >= 5)
            {
                if (Flags.ShowStageFinishTimes)
                {
                    ReportProgress(Translate.DoTranslation("Stage finished in") + $" {KernelTools.StageTimer.Elapsed}", 10);
                    KernelTools.StageTimer.Reset();
                    TextWriterColor.Write();
                }
            }
            else if (Flags.ShowStageFinishTimes)
            {
                ReportProgress(Translate.DoTranslation("Stage finished in") + $" {KernelTools.StageTimer.Elapsed}", 10);
                KernelTools.StageTimer.Restart();
            }

            // Actually report the stage
            if (StageNumber >= 1 & StageNumber <= 4)
            {
                if (!Flags.EnableSplash & !Flags.QuietKernel)
                {
                    TextWriterColor.Write();
                    SeparatorWriterColor.WriteSeparator(StageText, false, KernelColorType.Stage);
                }
                DebugWriter.WriteDebug(DebugLevel.I, $"- Kernel stage {StageNumber} | Text: {StageText}");
            }
        }

    }
}
