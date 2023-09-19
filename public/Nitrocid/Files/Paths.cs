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

using System;
using System.Collections.Generic;
using System.IO;
using KS.Kernel;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Files
{
    /// <summary>
    /// Paths module
    /// </summary>
    public static class Paths
    {

        private static readonly Dictionary<KernelPathType, (string, bool)> knownPaths = new()
        {
            { KernelPathType.Aliases,             (AliasesPath, true) },
            { KernelPathType.Configuration,       (ConfigurationPath, true) },
            { KernelPathType.CustomLanguages,     (CustomLanguagesPath, true) },
            { KernelPathType.CustomSplashes,      (CustomSplashesPath, true) },
            { KernelPathType.DebugDevices,        (DebugDevicesPath, true) },
            { KernelPathType.Debugging,           (DebuggingPath, true) },
            { KernelPathType.Events,              (EventsPath, true) },
            { KernelPathType.SpeedDial,           (SpeedDialPath, true) },
            { KernelPathType.MAL,                 (MALPath, true) },
            { KernelPathType.Mods,                (ModsPath, true) },
            { KernelPathType.MOTD,                (MOTDPath, true) },
            { KernelPathType.Reminders,           (RemindersPath, true) },
            { KernelPathType.Users,               (UsersPath, true) },
            { KernelPathType.Journaling,          (JournalingPath, true) },
            { KernelPathType.Contacts,            (ContactsPath, true) },
            { KernelPathType.ContactsImport,      (ContactsImportPath, true) },
            { KernelPathType.SaverConfiguration,  (SaverConfigurationPath, true) },
            { KernelPathType.SplashConfiguration, (SplashConfigurationPath, true) },
            { KernelPathType.ToDoList,            (ToDoListPath, true) },
            { KernelPathType.UserGroups,          (UserGroupsPath, true) },
            { KernelPathType.Addons,              (AddonsPath, false) },
            { KernelPathType.ShellHistories,      (ShellHistoriesPath, true) },
        };

        /// <summary>
        /// Path to KS executable folder
        /// </summary>
        public static string ExecPath =>
            Path.GetDirectoryName(typeof(Paths).Assembly.Location);

        /// <summary>
        /// Platform-dependent home path
        /// </summary>
        public static string HomePath
        {
            get
            {
                if (KernelPlatform.IsOnUnix())
                    return Environment.GetEnvironmentVariable("HOME");
                else
                    return Environment.GetEnvironmentVariable("USERPROFILE").Replace(@"\", "/");
            }
        }

        /// <summary>
        /// Platform-dependent application data path
        /// </summary>
        public static string AppDataPath
        {
            get
            {
                if (KernelPlatform.IsOnUnix())
                    return Environment.GetEnvironmentVariable("HOME") + "/.config/ks";
                else
                    return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/KS").Replace("\\", "/");
            }
        }

        /// <summary>
        /// Platform-dependent temp path
        /// </summary>
        public static string TempPath
        {
            get
            {
                if (KernelPlatform.IsOnUnix())
                    return "/tmp";
                else
                    return Environment.GetEnvironmentVariable("TEMP").Replace(@"\", "/");
            }
        }

        /// <summary>
        /// Retro Nitrocid KS download path
        /// </summary>
        public static string RetroKSDownloadPath
        {
            get
            {
                if (KernelPlatform.IsOnUnix())
                    return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/coreclr";
                else
                    return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/coreclr").Replace("\\", "/");
            }
        }

        /// <summary>
        /// Path to KS addons folder
        /// </summary>
        public static string AddonsPath =>
            Filesystem.NeutralizePath(ExecPath + "/Addons");

        /// <summary>
        /// Mods path
        /// </summary>
        public static string ModsPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSMods/");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string ConfigurationPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KernelMainConfig.json");

        /// <summary>
        /// Debugging path
        /// </summary>
        public static string DebuggingPath =>
            Filesystem.NeutralizePath(AppDataPath + "/kernelDbg.log");

        /// <summary>
        /// Aliases path
        /// </summary>
        public static string AliasesPath =>
            Filesystem.NeutralizePath(AppDataPath + "/Aliases.json");

        /// <summary>
        /// Users path
        /// </summary>
        public static string UsersPath =>
            Filesystem.NeutralizePath(AppDataPath + "/Users.json");

        /// <summary>
        /// Speed dial path
        /// </summary>
        public static string SpeedDialPath =>
            Filesystem.NeutralizePath(AppDataPath + "/SpeedDial.json");

        /// <summary>
        /// Debug devices path
        /// </summary>
        public static string DebugDevicesPath =>
            Filesystem.NeutralizePath(AppDataPath + "/DebugDevices.json");

        /// <summary>
        /// MOTD path
        /// </summary>
        public static string MOTDPath =>
            Filesystem.NeutralizePath(AppDataPath + "/MOTD.txt");

        /// <summary>
        /// MAL path
        /// </summary>
        public static string MALPath =>
            Filesystem.NeutralizePath(AppDataPath + "/MAL.txt");

        /// <summary>
        /// Events path
        /// </summary>
        public static string EventsPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSEvents/");

        /// <summary>
        /// Reminders path
        /// </summary>
        public static string RemindersPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSReminders/");

        /// <summary>
        /// CustomLanguages path
        /// </summary>
        public static string CustomLanguagesPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSLanguages/");

        /// <summary>
        /// CustomSplashes path
        /// </summary>
        public static string CustomSplashesPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSSplashes/");

        /// <summary>
        /// Journaling path
        /// </summary>
        public static string JournalingPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSJournal.json");

        /// <summary>
        /// Contacts path
        /// </summary>
        public static string ContactsPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSContacts/");

        /// <summary>
        /// Contacts path
        /// </summary>
        public static string ContactsImportPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSContactsImport/");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string SaverConfigurationPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KernelSaverConfig.json");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string SplashConfigurationPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KernelSplashConfig.json");

        /// <summary>
        /// User groups path
        /// </summary>
        public static string UserGroupsPath =>
            Filesystem.NeutralizePath(AppDataPath + "/UserGroups.json");

        /// <summary>
        /// To-do list path
        /// </summary>
        public static string ToDoListPath =>
            Filesystem.NeutralizePath(AppDataPath + "/ToDoList.json");

        /// <summary>
        /// Shell histories path
        /// </summary>
        public static string ShellHistoriesPath =>
            Filesystem.NeutralizePath(AppDataPath + "/ShellHistories.json");

        /// <summary>
        /// Gets the neutralized kernel path
        /// </summary>
        /// <param name="PathType">Kernel path type</param>
        /// <returns>A kernel path</returns>
        public static string GetKernelPath(KernelPathType PathType)
        {
            if (knownPaths.ContainsKey(PathType))
                return knownPaths[PathType].Item1;
            throw new KernelException(KernelExceptionType.InvalidKernelPath, Translate.DoTranslation("Invalid kernel path type."));
        }

        /// <summary>
        /// Checks to see if the provided path is resettable
        /// </summary>
        /// <param name="PathType">Kernel path type</param>
        /// <returns>True if we're able to wipe. Otherwise, false.</returns>
        public static bool IsResettable(KernelPathType PathType)
        {
            if (knownPaths.ContainsKey(PathType))
                return knownPaths[PathType].Item2;
            throw new KernelException(KernelExceptionType.InvalidKernelPath, Translate.DoTranslation("Invalid kernel path type."));
        }

    }
}
