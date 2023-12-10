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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Network.Base;
using KS.Shell.ShellBase.Commands;
using System;
using System.Net;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists network devices
    /// </summary>
    /// <remarks>
    /// This command lists all the network devices by their IP addresses and, if possible, their names.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class LsNetCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            foreach (IPAddress Device in NetworkTools.GetOnlineDevicesInNetwork())
            {
                string address = string.Join(".", Device.GetAddressBytes());
                string host = Translate.DoTranslation("Unknown host");

                // Try to get host name from device
                try
                {
                    host = Dns.GetHostEntry(address).HostName;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get host name from target device {0}: {1}", address, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }

                // Print info
                TextWriterColor.WriteKernelColor($"- {address}: ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor(host, true, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}
