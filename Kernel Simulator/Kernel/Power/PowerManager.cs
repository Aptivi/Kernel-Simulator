﻿using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

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

using KS.Network.RPC;

namespace KS.Kernel.Power
{
    public static class PowerManager
    {

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        public static void PowerManage(PowerMode PowerMode)
        {
            PowerManage(PowerMode, "0.0.0.0", RemoteProcedure.RPCPort);
        }

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        public static void PowerManage(PowerMode PowerMode, string IP)
        {
            PowerManage(PowerMode, IP, RemoteProcedure.RPCPort);
        }

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        public static void PowerManage(PowerMode PowerMode, string IP, int Port)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Power management has the argument of {0}", PowerMode);
            switch (PowerMode)
            {
                case PowerMode.Shutdown:
                    {
                        Kernel.KernelEventManager.RaisePreShutdown();
                        TextWriterColor.Write(Translate.DoTranslation("Shutting down..."), true, ColorTools.ColTypes.Neutral);
                        KernelTools.ResetEverything();
                        Kernel.KernelEventManager.RaisePostShutdown();
                        Flags.RebootRequested = true;
                        Flags.LogoutRequested = true;
                        Flags.KernelShutdown = true;
                        break;
                    }
                case PowerMode.Reboot:
                case PowerMode.RebootSafe:
                    {
                        Kernel.KernelEventManager.RaisePreReboot();
                        TextWriterColor.Write(Translate.DoTranslation("Rebooting..."), true, ColorTools.ColTypes.Neutral);
                        KernelTools.ResetEverything();
                        Kernel.KernelEventManager.RaisePostReboot();
                        Console.Clear();
                        Flags.RebootRequested = true;
                        Flags.LogoutRequested = true;
                        break;
                    }
                case PowerMode.RemoteShutdown:
                    {
                        RPCCommands.SendCommand("<Request:Shutdown>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestart:
                    {
                        RPCCommands.SendCommand("<Request:Reboot>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestartSafe:
                    {
                        RPCCommands.SendCommand("<Request:RebootSafe>(" + IP + ")", IP, Port);
                        break;
                    }
            }
            Flags.SafeMode = PowerMode == PowerMode.RebootSafe;
        }

    }
}