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
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Network.RPC;
using System.Threading;
using KS.Kernel.Events;
using KS.Users.Permissions;
using System.Diagnostics;
using KS.Kernel.Journaling;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Threading;
using Terminaux.Reader;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Configuration;
using KS.Users.Login;
using KS.Misc.Splash;
using System.Reflection;
using System.IO;
using KS.Kernel.Starting.Environment;

namespace KS.Kernel.Power
{
    /// <summary>
    /// Power management module
    /// </summary>
    public static class PowerManager
    {

        internal static bool KernelShutdown;
        internal static bool RebootRequested;
        internal static bool hardShutdown;
        internal static bool elevating;
        internal static Stopwatch Uptime = new();
        internal static KernelThread RPCPowerListener = new("RPC Power Listener Thread", true, (object arg) => PowerManage((PowerMode)arg));

        /// <summary>
        /// Beeps on shutdown (to restore the way of 0.0.1's shutdown)
        /// </summary>
        public static bool BeepOnShutdown =>
            Config.MainConfig.BeepOnShutdown;

        /// <summary>
        /// Delay on shutdown (to restore the way of 0.0.1's shutdown)
        /// </summary>
        public static bool DelayOnShutdown =>
            Config.MainConfig.DelayOnShutdown;

        /// <summary>
        /// Whether to simulate a situation where there is no APM available. If enabled, it shows the "It's now safe to
        /// turn off your computer" text.
        /// </summary>
        public static bool SimulateNoAPM =>
            Config.MainConfig.SimulateNoAPM;

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        public static void PowerManage(PowerMode PowerMode) =>
            PowerManage(PowerMode, "0.0.0.0", RemoteProcedure.RPCPort);

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        /// <param name="IP">IP address to remotely manage power</param>
        public static void PowerManage(PowerMode PowerMode, string IP) =>
            PowerManage(PowerMode, IP, RemoteProcedure.RPCPort);

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        /// <param name="IP">IP address to remotely manage power</param>
        /// <param name="Port">Port of the remote system running KS RPC</param>
        public static void PowerManage(PowerMode PowerMode, string IP, int Port)
        {
            // Check to see if the current user is granted power management or not
            PermissionsTools.Demand(PermissionTypes.ManagePower);

            DebugWriter.WriteDebug(DebugLevel.I, "Power management has the argument of {0}", PowerMode);
            switch (PowerMode)
            {
                case PowerMode.Shutdown:
                    {
                        EventsManager.FireEvent(EventType.PreShutdown);
                        DebugWriter.WriteDebug(DebugLevel.W, "Kernel is shutting down!");

                        // Simulate 0.0.1's behavior on shutting down
                        if (!SplashManager.EnableSplash)
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Shutting down..."));
                            if (BeepOnShutdown)
                                ConsoleWrapper.Beep();
                            if (DelayOnShutdown)
                                Thread.Sleep(3000);
                        }

                        // Set appropriate flags
                        RebootRequested = true;
                        Login.LogoutRequested = true;
                        KernelShutdown = true;

                        // Kill all shells and interrupt any input
                        for (int i = ShellStart.ShellStack.Count - 1; i >= 0; i--)
                            ShellStart.KillShellForced();
                        TermReaderTools.Interrupt();
                        break;
                    }
                case PowerMode.Reboot:
                case PowerMode.RebootSafe:
                    {
                        EventsManager.FireEvent(EventType.PreReboot);
                        DebugWriter.WriteDebug(DebugLevel.W, "Kernel is restarting!");
                        if (!SplashManager.EnableSplash)
                            TextWriterColor.Write(Translate.DoTranslation("Rebooting..."));

                        // Set appropriate flags
                        RebootRequested = true;
                        Login.LogoutRequested = true;
                        KernelEntry.SafeMode = PowerMode == PowerMode.RebootSafe;

                        // Kill all shells and interrupt any input
                        for (int i = ShellStart.ShellStack.Count - 1; i >= 0; i--)
                            ShellStart.KillShellForced();
                        TermReaderTools.Interrupt();
                        DebugWriter.WriteDebug(DebugLevel.I, "Safe mode changed to {0}", KernelEntry.SafeMode);
                        break;
                    }
                case PowerMode.RemoteShutdown:
                    {
                        JournalManager.WriteJournal(Translate.DoTranslation("Remote power management invoked:") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:Shutdown>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestart:
                    {
                        JournalManager.WriteJournal(Translate.DoTranslation("Remote power management invoked:") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:Reboot>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestartSafe:
                    {
                        JournalManager.WriteJournal(Translate.DoTranslation("Remote power management invoked:") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:RebootSafe>(" + IP + ")", IP, Port);
                        break;
                    }
            }
        }

        /// <summary>
        /// The kernel uptime (how long since the kernel booted up)
        /// </summary>
        public static string KernelUptime =>
            Uptime.Elapsed.ToString();

        internal static void ElevateSelf()
        {
            DebugCheck.Assert(KernelPlatform.IsOnWindows(), "tried to call this on non-Windows platforms");
            var selfProcess = new Process
            {
                StartInfo = new(Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".exe"))
                {
                    UseShellExecute = true,
                    Verb = "runas",
                    Arguments = string.Join(" ", EnvironmentTools.arguments)
                },
            };

            // --- UseShellExecute and the Environment property population Hack ---
            //
            // We need UseShellExecute to be able to use the runas verb, but it looks like that we can't start the process with the VS debugger,
            // because the StartInfo always populates the _environmentVariables field once the Environment property is populated.
            // _environmentVariables is not a public field.
            //
            // .NET expects _environmentVariables to be null when trying to start the process with the UseShellExecute being set to true,
            // but when calling Start(), .NET calls StartWithShellExecuteEx() and checks to see if that variable is null, so executing the
            // process in this way is basically impossible after evaluating the Environment property without having to somehow nullify this
            // _environmentVariables field using private reflection after evaluating the Environment property.
            //
            // if (startInfo._environmentVariables != null)
            //     throw new InvalidOperationException(SR.CantUseEnvVars);
            //
            // Please DO NOT even try to evaluate selfProcess.StartInfo.Environment in your debugger even if hovering over selfProcess.StartInfo,
            // because that would undo all the changes that we've made to the _environmentVariables and causes us to lose all the changes made
            // to this instance of StartInfo.
            //
            // if (_environmentVariables == null)
            // {
            //     IDictionary envVars = System.Environment.GetEnvironmentVariables();
            //     _environmentVariables = new DictionaryWrapper(new Dictionary<string, string?>(
            //     (...)
            // }
            //
            // This hack is only applicable to developers debugging the StartInfo instance of this specific process using VS. Nitrocid should
            // be able to restart itself as elevated normally if no debugger is attached.
            //
            // References:
            //   - https://github.com/dotnet/runtime/blob/release/8.0/src/libraries/System.Diagnostics.Process/src/System/Diagnostics/Process.Win32.cs#L47
            //   - https://github.com/dotnet/runtime/blob/release/8.0/src/libraries/System.Diagnostics.Process/src/System/Diagnostics/ProcessStartInfo.cs#L91
            if (Debugger.IsAttached)
            {
                var privateReflection = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField;
                var startInfoType = selfProcess.StartInfo.GetType();
                var envVarsField = startInfoType.GetField("_environmentVariables", privateReflection);
                envVarsField.SetValue(selfProcess.StartInfo, null);
            }
            // 
            // --- UseShellExecute and the Environment property population Hack End ---

            // Now, go ahead and start.
            selfProcess.Start();
            elevating = false;
        }

    }
}
