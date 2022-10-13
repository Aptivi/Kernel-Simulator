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

using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;
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
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Progress">The progress indicator of the kernel</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgress(string Text, int Progress, ColorTools.ColTypes ColTypes = ColorTools.ColTypes.NeutralText, params string[] Vars) => 
            ReportProgress(Text, Progress, false, SplashManager.CurrentSplash, ColTypes, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Progress">The progress indicator of the kernel</param>
        /// <param name="force">Force report progress to splash</param>
        /// <param name="splash">Splash interface</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Varibales to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        internal static void ReportProgress(string Text, int Progress, bool force = false, ISplash splash = null, ColorTools.ColTypes ColTypes = ColorTools.ColTypes.NeutralText, params string[] Vars)
        {
            if (!KernelBooted || force)
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
                        TextWriterColor.Write($"[{_Progress}%] {Text}", true, ColTypes, Vars);
                    }
                }
            }
            else
            {
                TextWriterColor.Write(Text, true, ColTypes, Vars);
            }
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
        internal static void ReportProgressError(string Text, params string[] Vars) =>
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
        internal static void ReportProgressError(string Text, Exception exception, params string[] Vars) =>
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
        internal static void ReportProgressError(string Text, bool force = false, ISplash splash = null, Exception exception = null, params string[] Vars)
        {
            if (!KernelBooted || force)
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
                        TextWriterColor.Write($"[{_Progress}%] Error: {Text}", true, ColorTools.ColTypes.Error, Vars);
                    }
                }
            }
            else
            {
                TextWriterColor.Write(Text, true, ColorTools.ColTypes.Error, Vars);
            }
        }

    }
}
