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

using System.Net.Sockets;
using System.Threading;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Splash;
using KS.Misc.Threading;

namespace KS.Network.RPC
{
    /// <summary>
    /// Remote procedure module
    /// </summary>
    public static class RemoteProcedure
    {

        /// <summary>
        /// RPC listener client
        /// </summary>
        public static UdpClient RPCListen;
        /// <summary>
        /// RPC port
        /// </summary>
        public static int RPCPort = 12345;
        /// <summary>
        /// Whether the RPC is enabled or not
        /// </summary>
        public static bool RPCEnabled = true;
        internal static KernelThread RPCThread = new("RPC Thread", true, RPCCommands.ReceiveCommand);

        /// <summary>
        /// Whether the RPC started
        /// </summary>
        public static bool RPCStarted => RPCThread.IsAlive;

        /// <summary>
        /// Starts the RPC listener
        /// </summary>
        public static void StartRPC()
        {
            if (RPCEnabled)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "RPC: Starting...");
                if (!RPCStarted)
                {
                    RPCListen = new UdpClient(RPCPort) { EnableBroadcast = true };
                    DebugWriter.WriteDebug(DebugLevel.I, "RPC: Listener started");
                    RPCThread.Start();
                    DebugWriter.WriteDebug(DebugLevel.I, "RPC: Thread started");
                }
                else
                {
                    throw new ThreadStateException(Translate.DoTranslation("Trying to start RPC while it's already started."));
                }
            }
            else
            {
                throw new ThreadStateException(Translate.DoTranslation("Not starting RPC because it's disabled."));
            }
        }

        /// <summary>
        /// The wrapper for <see cref="StartRPC"/>
        /// </summary>
        public static void WrapperStartRPC()
        {
            if (RPCEnabled)
            {
                try
                {
                    StartRPC();
                    SplashReport.ReportProgress(Translate.DoTranslation("RPC listening on all addresses using port {0}.").FormatString(RPCPort), 5, ColorTools.ColTypes.NeutralText);
                }
                catch (ThreadStateException ex)
                {
                    SplashReport.ReportProgressError(Translate.DoTranslation("RPC is already running."));
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Not starting RPC because it's disabled."), 3, ColorTools.ColTypes.NeutralText);
            }
        }

        /// <summary>
        /// Stops the RPC listener
        /// </summary>
        public static void StopRPC()
        {
            if (RPCStarted)
            {
                RPCThread.Stop();
                RPCListen?.Close();
                RPCListen = null;
                DebugWriter.WriteDebug(DebugLevel.I, "RPC stopped.");
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "RPC hasn't started yet!");
            }
        }

    }
}
