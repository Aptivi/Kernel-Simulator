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

namespace KS.Drivers
{
    /// <summary>
    /// Driver interface
    /// </summary>
    public interface IDriver
    {
        /// <summary>
        /// Driver name
        /// </summary>
        string DriverName { get; }
        /// <summary>
        /// Driver type
        /// </summary>
        DriverTypes DriverType { get; }
        /// <summary>
        /// Whether the driver is required to promise that the inputs are valid before performing an operation
        /// </summary>
        bool DriverPromiseRequired { get; }
        /// <summary>
        /// The driver promise action that will be executed if promise is required
        /// </summary>
        Func<object[], bool> DriverPromiseAction { get; }
    }
}
