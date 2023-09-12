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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.Kernel.Debugging.RemoteDebug
{
    /// <summary>
    /// Remote debug device info class
    /// </summary>
    public class RemoteDebugDeviceInfo
    {
        [JsonProperty(nameof(Address))]
        internal string address = "";
        [JsonProperty(nameof(Name))]
        internal string name = "";
        [JsonProperty(nameof(Blocked))]
        internal bool blocked;
        [JsonProperty(nameof(ChatHistory))]
        internal List<string> chatHistory = new();

        /// <summary>
        /// Remote debug device address
        /// </summary>
        [JsonIgnore]
        public string Address =>
            address;
        /// <summary>
        /// Remote debug device name
        /// </summary>
        [JsonIgnore]
        public string Name =>
            name;
        /// <summary>
        /// Is the device blocked?
        /// </summary>
        [JsonIgnore]
        public bool Blocked =>
            blocked;
        /// <summary>
        /// Chat history of the device
        /// </summary>
        [JsonIgnore]
        public string[] ChatHistory =>
            chatHistory.ToArray();

        [JsonConstructor]
        internal RemoteDebugDeviceInfo()
        { }
    }
}
