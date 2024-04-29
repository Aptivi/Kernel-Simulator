﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
namespace KS.Misc.Screensaver
{
    /// <summary>
    /// Screensaver (KS built-in or custom) interface
    /// </summary>
    public interface IScreensaver
    {
        /// <summary>
        /// Prepare the screensaver before displaying.
        /// </summary>
        void ScreensaverPreparation();
        /// <summary>
        /// Display a screensaver. This is executed inside the loop.
        /// </summary>
        void ScreensaverLogic();
        /// <summary>
        /// The name of screensaver, usually the assembly name of the custom screensaver
        /// </summary>
        string ScreensaverName { get; set; }
        /// <summary>
        /// Settings for custom screensaver
        /// </summary>
        /// <returns>A set of keys and values holding settings for the screensaver</returns>
        Dictionary<string, object> ScreensaverSettings { get; set; }
    }
}