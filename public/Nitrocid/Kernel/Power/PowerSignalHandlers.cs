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

using KS.ConsoleBase;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terminaux.Reader;

namespace KS.Kernel.Power
{
    internal static class PowerSignalHandlers
    {
        internal static List<PosixSignalRegistration> signalHandlers = new();
        internal static bool initialized = false;

        internal static void RegisterHandlers()
        {
            // Works on Windows and Linux
            signalHandlers.Add(PosixSignalRegistration.Create((PosixSignal)PowerSignals.SIGINT, SigQuit));
            signalHandlers.Add(PosixSignalRegistration.Create((PosixSignal)PowerSignals.SIGTERM, SigQuit));
            if (KernelPlatform.IsOnWindows())
                return;

            // Works on Linux only
            signalHandlers.Add(PosixSignalRegistration.Create((PosixSignal)PowerSignals.SIGUSR1, SigReboot));
            signalHandlers.Add(PosixSignalRegistration.Create((PosixSignal)PowerSignals.SIGUSR2, SigReboot));

            // Handle window change
            if (KernelPlatform.IsOnUnix())
                signalHandlers.Add(PosixSignalRegistration.Create((PosixSignal)PowerSignals.SIGWINCH, SigWindowChange));
            else
            {
                // Initialize console resize listener
                if (Flags.TalkativePreboot)
                    TextWriterColor.Write(Translate.DoTranslation("Loading resize listener..."));
                ConsoleResizeListener.StartResizeListener();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded resize listener.");
            }
        }

        internal static void DisposeHandlers()
        {
            foreach (var signalHandler in signalHandlers)
                signalHandler.Dispose();
        }

        private static void SigQuit(PosixSignalContext psc)
        {
            PowerManager.PowerManage(PowerMode.Shutdown);
            psc.Cancel = true;
        }

        private static void SigReboot(PosixSignalContext psc)
        {
            PowerManager.PowerManage(PowerMode.Reboot);
            psc.Cancel = true;
        }

        private static void SigWindowChange(PosixSignalContext psc)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "SIGWINCH recieved!");
            ConsoleResizeListener.ResizeDetected = true;
            psc.Cancel = true;
        }
    }
}
