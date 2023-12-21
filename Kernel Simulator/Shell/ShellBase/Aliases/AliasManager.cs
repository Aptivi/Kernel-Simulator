﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using KS.ConsoleBase.Colors;
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

using KS.Files.Operations;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Shell.ShellBase.Aliases
{
    public static class AliasManager
    {

        internal static Dictionary<string, string> Aliases = [];
        internal static Dictionary<string, string> RemoteDebugAliases = [];
        internal static Dictionary<string, string> FTPShellAliases = [];
        internal static Dictionary<string, string> MailShellAliases = [];
        internal static Dictionary<string, string> SFTPShellAliases = [];
        internal static Dictionary<string, string> TextShellAliases = [];
        internal static Dictionary<string, string> TestShellAliases = [];
        internal static Dictionary<string, string> ZIPShellAliases = [];
        internal static Dictionary<string, string> RSSShellAliases = [];
        internal static Dictionary<string, string> JsonShellAliases = [];
        internal static Dictionary<string, string> HTTPShellAliases = [];
        internal static Dictionary<string, string> HexShellAliases = [];
        internal static Dictionary<string, string> RARShellAliases = [];
        internal static Dictionary<string, ShellType> AliasesToBeRemoved = [];

        /// <summary>
        /// Initializes aliases
        /// </summary>
        public static void InitAliases()
        {
            // Get all aliases from file
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.Aliases), false);
            string AliasJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Aliases));
            var AliasNameToken = JToken.Parse(!string.IsNullOrEmpty(AliasJsonContent) ? AliasJsonContent : "{}");
            string AliasCmd, ActualCmd;
            ShellType AliasType;

            foreach (JObject AliasObject in AliasNameToken.Cast<JObject>())
            {
                AliasCmd = (string)AliasObject["Alias"];
                ActualCmd = (string)AliasObject["Command"];
                AliasType = (ShellType)Convert.ToInt32(AliasObject["Type"].ToObject(typeof(ShellType)));
                DebugWriter.Wdbg(DebugLevel.I, "Adding \"{0}\" and \"{1}\" from Aliases.json to {2} list...", AliasCmd, ActualCmd, AliasType.ToString());
                var TargetAliasList = GetAliasesListFromType(AliasType);
                if (!TargetAliasList.ContainsKey(AliasCmd))
                {
                    TargetAliasList.Add(AliasCmd, ActualCmd);
                }
                else
                {
                    TargetAliasList[AliasCmd] = ActualCmd;
                }
            }
        }

        /// <summary>
        /// Saves aliases
        /// </summary>
        public static void SaveAliases()
        {
            // Save all aliases
            foreach (ShellType Shell in Enum.GetValues(typeof(ShellType)))
                SaveAliasesInternal(Shell);
        }

        internal static void SaveAliasesInternal(ShellType ShellType)
        {
            // Get all aliases from file
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.Aliases), false);
            string AliasJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Aliases));
            var AliasNameToken = JArray.Parse(!string.IsNullOrEmpty(AliasJsonContent) ? AliasJsonContent : "[]");

            // Save the alias
            var ShellAliases = GetAliasesListFromType(ShellType);
            for (int i = 0, loopTo = ShellAliases.Count - 1; i <= loopTo; i++)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Adding \"{0}\" and \"{1}\" from list to Aliases.json with type {2}...", ShellAliases.Keys.ElementAtOrDefault(i), ShellAliases.Values.ElementAtOrDefault(i), ShellType.ToString());
                var AliasObject = new JObject() { { "Alias", ShellAliases.Keys.ElementAtOrDefault(i) }, { "Command", ShellAliases.Values.ElementAtOrDefault(i) }, { "Type", ShellType.ToString() } };
                if (!DoesAliasExist(ShellAliases.Keys.ElementAtOrDefault(i), ShellType))
                    AliasNameToken.Add(AliasObject);
            }

            // Save changes
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Aliases), JsonConvert.SerializeObject(AliasNameToken, Formatting.Indented));
        }

        /// <summary>
        /// Manages the alias
        /// </summary>
        /// <param name="mode">Either add or rem</param>
        /// <param name="Type">Alias type (Shell or Remote Debug)</param>
        /// <param name="AliasCmd">A specified alias</param>
        /// <param name="DestCmd">A destination command (target)</param>
        public static void ManageAlias(string mode, ShellType Type, string AliasCmd, string DestCmd = "")
        {
            if (Enum.IsDefined(typeof(ShellType), Type))
            {
                if (mode == "add")
                {
                    // User tries to add an alias.
                    try
                    {
                        AddAlias(AliasCmd, DestCmd, Type);
                        TextWriterColor.Write(Translate.DoTranslation("You can now run \"{0}\" as a command: \"{1}\"."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), AliasCmd, DestCmd);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Failed to add alias. Stack trace written using WStkTrc(). {0}", ex.Message);
                        DebugWriter.WStkTrc(ex);
                        TextWriterColor.Write(ex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                    }
                }
                else if (mode == "rem")
                {
                    // User tries to remove an alias
                    try
                    {
                        RemoveAlias(AliasCmd, Type);
                        PurgeAliases();
                        TextWriterColor.Write(Translate.DoTranslation("Removed alias {0} successfully."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), AliasCmd);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Failed to remove alias. Stack trace written using WStkTrc(). {0}", ex.Message);
                        DebugWriter.WStkTrc(ex);
                        TextWriterColor.Write(ex.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                    }
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Mode {0} was neither add nor rem.", mode);
                    TextWriterColor.Write(Translate.DoTranslation("Invalid mode {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), mode);
                }

                // Save all aliases
                SaveAliases();
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.E, "Type {0} not found.", Type);
                TextWriterColor.Write(Translate.DoTranslation("Invalid type {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), Type);
            }
        }

        /// <summary>
        /// Adds alias to kernel
        /// </summary>
        /// <param name="SourceAlias">A command to be aliased. It should exist in both shell and remote debug.</param>
        /// <param name="Destination">A one-word command to alias to.</param>
        /// <param name="Type">Alias type, whether it be shell or remote debug.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool AddAlias(string SourceAlias, string Destination, ShellType Type)
        {
            if (Enum.IsDefined(typeof(ShellType), Type))
            {
                if ((SourceAlias ?? "") == (Destination ?? ""))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Assertion succeeded: {0} = {1}", SourceAlias, Destination);
                    throw new Kernel.Exceptions.AliasInvalidOperationException(Translate.DoTranslation("Alias can't be the same name as a command."));
                }
                else if (!CommandManager.IsCommandFound(Destination))
                {
                    DebugWriter.Wdbg(DebugLevel.W, "{0} not found in all the command lists", Destination);
                    throw new Kernel.Exceptions.AliasNoSuchCommandException(Translate.DoTranslation("Command not found to alias to {0}."), Destination);
                }
                else if (DoesAliasExist(SourceAlias, Type))
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Alias {0} already found", SourceAlias);
                    throw new Kernel.Exceptions.AliasAlreadyExistsException(Translate.DoTranslation("Alias already found: {0}"), SourceAlias);
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Aliasing {0} to {1}", SourceAlias, Destination);
                    var TargetAliasList = GetAliasesListFromType(Type);
                    TargetAliasList.Add(SourceAlias, Destination);
                    return true;
                }
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.E, "Type {0} not found.", Type);
                throw new Kernel.Exceptions.AliasNoSuchTypeException(Translate.DoTranslation("Invalid type {0}."), Type);
            }
        }

        /// <summary>
        /// Removes alias from kernel
        /// </summary>
        /// <param name="TargetAlias">An alias that needs to be removed.</param>
        /// <param name="Type">Alias type, whether it be shell or remote debug.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool RemoveAlias(string TargetAlias, ShellType Type)
        {
            // Variables
            var TargetAliasList = GetAliasesListFromType(Type);

            // Do the action!
            if (TargetAliasList.ContainsKey(TargetAlias))
            {
                string Aliased = TargetAliasList[TargetAlias];
                DebugWriter.Wdbg(DebugLevel.I, "aliases({0}) is found. That makes it {1}", TargetAlias, Aliased);
                TargetAliasList.Remove(TargetAlias);
                AliasesToBeRemoved.Add($"{AliasesToBeRemoved.Count + 1}-{TargetAlias}", Type);
                return true;
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.W, "{0} is not found in the {1} aliases", TargetAlias, Type.ToString());
                throw new Kernel.Exceptions.AliasNoSuchAliasException(Translate.DoTranslation("Alias {0} is not found to be removed."), TargetAlias);
            }
        }

        /// <summary>
        /// Purge aliases that are removed from config
        /// </summary>
        public static void PurgeAliases()
        {
            // Get all aliases from file
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.Aliases), false);
            string AliasJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Aliases));
            var AliasNameToken = JArray.Parse(!string.IsNullOrEmpty(AliasJsonContent) ? AliasJsonContent : "[]");

            // Purge aliases that are to be removed from config
            foreach (string TargetAliasItem in AliasesToBeRemoved.Keys)
            {
                for (int RemovedAliasIndex = AliasNameToken.Count - 1; RemovedAliasIndex >= 0; RemovedAliasIndex -= 1)
                {
                    var TargetAliasType = AliasesToBeRemoved[TargetAliasItem];
                    string TargetAlias = TargetAliasItem.Substring(TargetAliasItem.IndexOf("-") + 1);
                    if ((string)AliasNameToken[RemovedAliasIndex]["Alias"] == TargetAlias & (string)AliasNameToken[RemovedAliasIndex]["Type"] == TargetAliasType.ToString())
                        AliasNameToken.RemoveAt(RemovedAliasIndex);
                }
            }

            // Clear the "to be removed" list of aliases
            AliasesToBeRemoved.Clear();

            // Save the changes
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Aliases), JsonConvert.SerializeObject(AliasNameToken, Formatting.Indented));
        }

        /// <summary>
        /// Checks to see if the specified alias exists.
        /// </summary>
        /// <param name="TargetAlias">The existing alias</param>
        /// <param name="Type">The alias type</param>
        /// <returns>True if it exists; false if it doesn't exist</returns>
        public static bool DoesAliasExist(string TargetAlias, ShellType Type)
        {
            // Get all aliases from file
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.Aliases), false);
            string AliasJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Aliases));
            var AliasNameToken = JArray.Parse(!string.IsNullOrEmpty(AliasJsonContent) ? AliasJsonContent : "[]");

            // Check to see if the specified alias exists
            foreach (JObject AliasName in AliasNameToken.Cast<JObject>())
            {
                if ((string)AliasName["Alias"] == TargetAlias & (string)AliasName["Type"] == Type.ToString())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the aliases list from the shell type
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static Dictionary<string, string> GetAliasesListFromType(ShellType ShellType)
        {
            switch (ShellType)
            {
                case ShellType.Shell:
                    {
                        return Aliases;
                    }
                case ShellType.RemoteDebugShell:
                    {
                        return RemoteDebugAliases;
                    }
                case ShellType.FTPShell:
                    {
                        return FTPShellAliases;
                    }
                case ShellType.SFTPShell:
                    {
                        return SFTPShellAliases;
                    }
                case ShellType.MailShell:
                    {
                        return MailShellAliases;
                    }
                case ShellType.TextShell:
                    {
                        return TextShellAliases;
                    }
                case ShellType.TestShell:
                    {
                        return TestShellAliases;
                    }
                case ShellType.ZIPShell:
                    {
                        return ZIPShellAliases;
                    }
                case ShellType.RSSShell:
                    {
                        return RSSShellAliases;
                    }
                case ShellType.JsonShell:
                    {
                        return JsonShellAliases;
                    }
                case ShellType.HTTPShell:
                    {
                        return HTTPShellAliases;
                    }
                case ShellType.HexShell:
                    {
                        return HexShellAliases;
                    }
                case ShellType.RARShell:
                    {
                        return RARShellAliases;
                    }

                default:
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Type {0} not found.", ShellType);
                        throw new Kernel.Exceptions.AliasNoSuchTypeException(Translate.DoTranslation("Invalid type {0}."), ShellType);
                    }
            }
        }

    }
}
