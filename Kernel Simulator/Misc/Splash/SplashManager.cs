﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using KS.Files;

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

using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel;
using KS.Misc.Reflection;
using KS.Misc.Splash.Splashes;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;

namespace KS.Misc.Splash
{
	public static class SplashManager
	{

		public static string SplashName = "Simple";
		internal static KernelThread SplashThread = new("Kernel Splash Thread", false, () => CurrentSplash.Display());
		private static readonly Dictionary<string, SplashInfo> InstalledSplashes = new() { { "Simple", new SplashInfo("Simple", true, new SplashSimple()) }, { "Progress", new SplashInfo("Progress", true, new SplashProgress()) }, { "Blank", new SplashInfo("Blank", false, new SplashBlank()) }, { "Fader", new SplashInfo("Fader", true, new SplashFader()) }, { "FaderBack", new SplashInfo("FaderBack", true, new SplashFaderBack()) }, { "BeatFader", new SplashInfo("BeatFader", true, new SplashBeatFader()) }, { "systemd", new SplashInfo("systemd", true, new SplashSystemd()) }, { "sysvinit", new SplashInfo("sysvinit", true, new SplashSysvinit()) }, { "openrc", new SplashInfo("openrc", true, new SplashOpenRC()) }, { "Pulse", new SplashInfo("Pulse", true, new SplashPulse()) }, { "BeatPulse", new SplashInfo("BeatPulse", true, new SplashBeatPulse()) }, { "EdgePulse", new SplashInfo("EdgePulse", true, new SplashEdgePulse()) }, { "BeatEdgePulse", new SplashInfo("BeatEdgePulse", true, new SplashBeatEdgePulse()) } };

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
		public static Dictionary<string, SplashInfo> Splashes
		{
			get
			{
				return InstalledSplashes;
			}
		}

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
						DebugWriter.Wdbg(DebugLevel.I, "Parsing splash file {0}...", FilePath);
						var SplashAssembly = Assembly.LoadFrom(FilePath);
						var SplashInstance = GetSplashInstance(SplashAssembly);
						if (SplashInstance is not null)
						{
							DebugWriter.Wdbg(DebugLevel.I, "Found valid splash! Getting information...");
							string Name = SplashInstance.SplashName;
							bool DisplaysProgress = SplashInstance.SplashDisplaysProgress;

							// Install the values to the new instance
							DebugWriter.Wdbg(DebugLevel.I, "- Name: {0}", Name);
							DebugWriter.Wdbg(DebugLevel.I, "- Displays Progress: {0}", DisplaysProgress);
							DebugWriter.Wdbg(DebugLevel.I, "Installing splash...");
							var InstalledSplash = new SplashInfo(Name, DisplaysProgress, SplashInstance);
							if (InstalledSplashes.ContainsKey(Name))
							{
								InstalledSplashes[Name] = InstalledSplash;
							}
							else
							{
								InstalledSplashes.Add(Name, InstalledSplash);
							}
						}
						else
						{
							DebugWriter.Wdbg(DebugLevel.W, "Skipping incompatible splash file {0}...", FilePath);
						}
					}
					catch (ReflectionTypeLoadException ex)
					{
						DebugWriter.Wdbg(DebugLevel.W, "Could not handle splash file {0}! {1}", FilePath, ex.Message);
						DebugWriter.WStkTrc(ex);
					}
				}
				else
				{
					DebugWriter.Wdbg(DebugLevel.W, "Skipping incompatible splash file {0} because file extension is not .dll ({1})...", FilePath, SplashFileInfo.Extension);
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
					DebugWriter.Wdbg(DebugLevel.I, "Parsing splash file {0}...", FilePath);
					var SplashAssembly = Assembly.LoadFrom(FilePath);
					var SplashInstance = GetSplashInstance(SplashAssembly);
					if (SplashInstance is not null)
					{
						DebugWriter.Wdbg(DebugLevel.I, "Found valid splash! Getting information...");
						string Name = SplashInstance.SplashName;

						// Uninstall the splash
						DebugWriter.Wdbg(DebugLevel.I, "- Name: {0}", Name);
						DebugWriter.Wdbg(DebugLevel.I, "Uninstalling splash...");
						InstalledSplashes.Remove(Name);
					}
					else
					{
						DebugWriter.Wdbg(DebugLevel.W, "Skipping incompatible splash file {0}...", FilePath);
					}
				}
				catch (ReflectionTypeLoadException ex)
				{
					DebugWriter.Wdbg(DebugLevel.W, "Could not handle splash file {0}! {1}", FilePath, ex.Message);
					DebugWriter.WStkTrc(ex);
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
				ConsoleWrapper.CursorVisible = false;
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
				SplashThread.Stop();
				ConsoleWrapper.CursorVisible = true;
				CurrentSplash.SplashClosing = false;
			}
			SplashReport._KernelBooted = true;
		}

	}
}