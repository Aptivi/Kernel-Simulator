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

namespace KS.Misc.Splash
{
    /// <summary>
    /// Splash screen interface
    /// </summary>
    public interface ISplash
    {

        /// <summary>
        /// Whether the splash is closing. If true, the thread of which handles the display should close itself. <see cref="Closing()"/> should set this property to True.
        /// </summary>
        bool SplashClosing { get; set; }
        /// <summary>
        /// Splash name
        /// </summary>
        string SplashName { get; }
        /// <summary>
        /// Splash displays progress
        /// </summary>
        bool SplashDisplaysProgress { get; }
        /// <summary>
        /// The opening screen. Should be synchronous.
        /// </summary>
        void Opening();
        /// <summary>
        /// The screen which is meant to be looped. You can set it to do nothing. Should be async. It should also handle <see cref="System.Threading.ThreadInterruptedException"/> to avoid kernel exiting on startup.
        /// </summary>
        void Display();
        /// <summary>
        /// The closing screen. Should be synchronous.
        /// </summary>
        void Closing();
        /// <summary>
        /// Report the progress
        /// </summary>
        /// <param name="ProgressReport">The progress text to indicate how did the kernel progress</param>
        /// <param name="Progress">The progress indicator of the kernel</param>
        void Report(int Progress, string ProgressReport, params object[] Vars);

    }
}