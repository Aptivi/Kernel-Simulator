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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Extensification.DictionaryExts;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Misc.Reflection;
using KS.Misc.Splash.Splashes;
using KS.Misc.Threading;

namespace KS.Misc.Splash
{
    public static class SplashManager
    {

        public static string SplashName = "Simple";
        internal static KernelThread SplashThread = new("Kernel Splash Thread", false, () => CurrentSplash.Display());
        private readonly static Dictionary<string, SplashInfo> InstalledSplashes = new()
        {
            { "Simple", new SplashInfo("Simple", new SplashSimple()) },
            { "Progress", new SplashInfo("Progress", new SplashProgress()) },
            { "Blank", new SplashInfo("Blank", new SplashBlank(), false) },
            { "Fader", new SplashInfo("Fader", new SplashFader()) },
            { "FaderBack", new SplashInfo("FaderBack", new SplashFaderBack()) },
            { "BeatFader", new SplashInfo("BeatFader", new SplashBeatFader()) },
            { "systemd", new SplashInfo("systemd", new SplashSystemd()) },
            { "sysvinit", new SplashInfo("sysvinit", new SplashSysvinit()) },
            { "openrc", new SplashInfo("openrc", new SplashOpenRC()) },
            { "Pulse", new SplashInfo("Pulse", new SplashPulse()) },
            { "BeatPulse", new SplashInfo("BeatPulse", new SplashBeatPulse()) },
            { "EdgePulse", new SplashInfo("EdgePulse", new SplashEdgePulse()) },
            { "BeatEdgePulse", new SplashInfo("BeatEdgePulse", new SplashBeatEdgePulse()) },
            { "PowerLine", new SplashInfo("PowerLine", new SplashPowerLine()) },
            { "PowerLineProgress", new SplashInfo("PowerLine", new SplashPowerLineProgress()) }
        };

        /// <summary>
        /// Current splash screen
        /// </summary>
        public static ISplash CurrentSplash
        {
            get
            {
                if (Splashes.ContainsKey(SplashName))
                {
                    return Splashes[SplashName].EntryPoint;
                }
                else
                {
                    return Splashes["Simple"].EntryPoint;
                }
            }
        }

        /// <summary>
        /// Current splash screen info instance
        /// </summary>
        public static SplashInfo CurrentSplashInfo
        {
            get
            {
                if (Splashes.ContainsKey(SplashName))
                {
                    return Splashes[SplashName];
                }
                else
                {
                    return Splashes["Simple"];
                }
            }
        }

        /// <summary>
        /// All the installed splashes either normal or custom
        /// </summary>
        public static Dictionary<string, SplashInfo> Splashes => InstalledSplashes;

        /// <summary>
        /// Gets names of the installed splashes
        /// </summary>
        public static List<string> GetNamesOfSplashes() => Splashes.Keys.ToList();

        /// <summary>
        /// Loads all the splashes from the KSSplashes folder
        /// </summary>
        public static void LoadSplashes()
        {
            string SplashPath = Paths.GetKernelPath(KernelPathType.CustomSplashes);
            if (!Checking.FolderExists(SplashPath))
                Making.MakeDirectory(SplashPath);
            var SplashFiles = Listing.CreateList(SplashPath);
            foreach (FileSystemInfo SplashFileInfo in SplashFiles)
            {
                string FilePath = SplashFileInfo.FullName;
                string FileName = SplashFileInfo.Name;

                // Try to parse the splash file
                if (SplashFileInfo.Extension == ".dll")
                {
                    // We got a .dll file that may or may not contain splash file. Parse that to verify.
                    try
                    {
                        // Add splash dependencies folder (if any) to the private appdomain lookup folder
                        string SplashDepPath = SplashPath + "Deps/" + Path.GetFileNameWithoutExtension(FileName) + "-" + FileVersionInfo.GetVersionInfo(FilePath).FileVersion + "/";
                        AssemblyLookup.AddPathToAssemblySearchPath(SplashDepPath);

                        // Now, actually parse that.
                        DebugWriter.WriteDebug(DebugLevel.I, "Parsing splash file {0}...", FilePath);
                        var SplashAssembly = Assembly.LoadFrom(FilePath);
                        var SplashInstance = GetSplashInstance(SplashAssembly);
                        if (SplashInstance is not null)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Found valid splash! Getting information...");
                            string Name = SplashInstance.SplashName;
                            bool DisplaysProgress = SplashInstance.SplashDisplaysProgress;

                            // Install the values to the new instance
                            DebugWriter.WriteDebug(DebugLevel.I, "- Name: {0}", Name);
                            DebugWriter.WriteDebug(DebugLevel.I, "- Displays Progress: {0}", DisplaysProgress);
                            DebugWriter.WriteDebug(DebugLevel.I, "Installing splash...");
                            var InstalledSplash = new SplashInfo(Name, SplashInstance, DisplaysProgress);
                            InstalledSplashes.AddOrModify(Name, InstalledSplash);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Skipping incompatible splash file {0}...", FilePath);
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Could not handle splash file {0}! {1}", FilePath, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping incompatible splash file {0} because file extension is not .dll ({1})...", FilePath, SplashFileInfo.Extension);
                }
            }
        }

        /// <summary>
        /// Unloads all the splashes from the KSSplashes folder
        /// </summary>
        public static void UnloadSplashes()
        {
            var SplashFiles = Listing.CreateList(Paths.GetKernelPath(KernelPathType.CustomSplashes));
            foreach (FileSystemInfo SplashFileInfo in SplashFiles)
            {
                string FilePath = SplashFileInfo.FullName;

                // Try to parse the splash file
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Parsing splash file {0}...", FilePath);
                    var SplashAssembly = Assembly.LoadFrom(FilePath);
                    var SplashInstance = GetSplashInstance(SplashAssembly);
                    if (SplashInstance is not null)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Found valid splash! Getting information...");
                        string Name = SplashInstance.SplashName;

                        // Uninstall the splash
                        DebugWriter.WriteDebug(DebugLevel.I, "- Name: {0}", Name);
                        DebugWriter.WriteDebug(DebugLevel.I, "Uninstalling splash...");
                        InstalledSplashes.Remove(Name);
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Skipping incompatible splash file {0}...", FilePath);
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Could not handle splash file {0}! {1}", FilePath, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

        /// <summary>
        /// Gets the splash instance from compiled assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        public static ISplash GetSplashInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(ISplash).Name) is not null)
                    return (ISplash)Assembly.CreateInstance(t.FullName);
            }
            return null;
        }

        /// <summary>
        /// Opens the splash screen
        /// </summary>
        public static void OpenSplash()
        {
            if (Flags.EnableSplash)
            {
                ConsoleBase.ConsoleWrapper.CursorVisible = false;
                CurrentSplash.Opening();
                SplashThread.Stop();
                SplashThread.Start();
            }
        }

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        public static void CloseSplash()
        {
            if (Flags.EnableSplash)
            {
                CurrentSplash.Closing();

                // We need to wait for the splash display thread to finish its work once Closing() is called, because some splashes, like PowerLine,
                // actually do some operations that take a few milliseconds to finish what it's doing, and if we didn't wait here until the operations
                // are done in the Display() function, we'd abruptly stop without waiting, causing race condition. If this happened, visual glitches
                // manifest, which is not good.
                SplashThread.Wait();
                SplashThread.Stop();
                ConsoleBase.ConsoleWrapper.CursorVisible = true;

                // Reset the SplashClosing variable in case it needs to be open again. Some splashes don't do anything if they detect that the splash
                // screen is closing.
                CurrentSplash.SplashClosing = false;
            }
            SplashReport._KernelBooted = true;
        }

    }
}
