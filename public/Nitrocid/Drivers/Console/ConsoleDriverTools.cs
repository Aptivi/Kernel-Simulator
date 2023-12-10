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

using KS.Kernel.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace KS.Drivers.Console
{
    /// <summary>
    /// Console driver tools (strongly typed)
    /// </summary>
    public static class ConsoleDriverTools
    {
        /// <summary>
        /// Sets the console driver
        /// </summary>
        /// <param name="name">Name of the available console driver to set to</param>
        /// <exception cref="KernelException"></exception>
        public static void SetConsoleDriver(string name) =>
            DriverHandler.SetDriverSafe<IConsoleDriver>(name);

        /// <summary>
        /// Gets the console drivers
        /// </summary>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, IDriver> GetConsoleDrivers()
        {
            // First, exclude internal drivers from the list
            var filteredDrivers = DriverHandler.drivers[DriverTypes.Console].Where((kvp) => !kvp.Value.DriverInternal);
            var filteredCustomDrivers = DriverHandler.customDrivers[DriverTypes.Console].Where((kvp) => !kvp.Value.DriverInternal);

            // Then, get the list of drivers
            return filteredDrivers.Union(filteredCustomDrivers).ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);
        }

        /// <summary>
        /// Gets the console driver names
        /// </summary>
        /// <returns>List of driver names</returns>
        public static string[] GetConsoleDriverNames()
        {
            // Get the drivers and fetch their names
            var drivers = GetConsoleDrivers();
            return drivers.Select((kvp) => kvp.Key).ToArray();
        }
    }
}
