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

namespace KS.Drivers.Network
{
    /// <summary>
    /// Network driver tools (strongly typed)
    /// </summary>
    public static class NetworkDriverTools
    {
        /// <summary>
        /// Sets the network driver
        /// </summary>
        /// <param name="name">Name of the available network driver to set to</param>
        /// <exception cref="KernelException"></exception>
        public static void SetNetworkDriver(string name) =>
            DriverHandler.SetDriverSafe<INetworkDriver>(name);

        /// <summary>
        /// Gets the network drivers
        /// </summary>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, IDriver> GetNetworkDrivers()
        {
            // First, exclude internal drivers from the list
            var filteredDrivers = DriverHandler.drivers[DriverTypes.Network].Where((kvp) => !kvp.Value.DriverInternal);
            var filteredCustomDrivers = DriverHandler.customDrivers[DriverTypes.Network].Where((kvp) => !kvp.Value.DriverInternal);

            // Then, get the list of drivers
            return filteredDrivers.Union(filteredCustomDrivers).ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);
        }

        /// <summary>
        /// Gets the network driver names
        /// </summary>
        /// <returns>List of driver names</returns>
        public static string[] GetNetworkDriverNames()
        {
            // Get the drivers and fetch their names
            var drivers = GetNetworkDrivers();
            return drivers.Select((kvp) => kvp.Key).ToArray();
        }
    }
}
