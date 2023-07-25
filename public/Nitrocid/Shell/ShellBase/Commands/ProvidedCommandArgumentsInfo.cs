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

using System.Collections.Generic;
using System.Linq;
using KS.Drivers;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.Modifications;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Provided command arguments information class
    /// </summary>
    public class ProvidedCommandArgumentsInfo
    {

        internal string[] unknownSwitchesList;
        internal string[] conflictingSwitchesList;

        /// <summary>
        /// Target command that the user executed in shell
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// Text version of the provided arguments and switches
        /// </summary>
        public string ArgumentsText { get; private set; }
        /// <summary>
        /// List version of the provided arguments
        /// </summary>
        public string[] ArgumentsList { get; private set; }
        /// <summary>
        /// List version of the provided switches
        /// </summary>
        public string[] SwitchesList { get; private set; }
        /// <summary>
        /// Checks to see if the required arguments are provided
        /// </summary>
        public bool RequiredArgumentsProvided { get; private set; }
        /// <summary>
        /// Checks to see if the required switches are provided
        /// </summary>
        public bool RequiredSwitchesProvided { get; private set; }
        /// <summary>
        /// Checks to see if the required switch arguments for switches that require values are provided
        /// </summary>
        public bool RequiredSwitchArgumentsProvided { get; private set; }

        /// <summary>
        /// Makes a new instance of the command argument info with the user-provided command text
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="CommandType">Shell command type. Consult the <see cref="ShellType"/> enum for information about supported shells.</param>
        internal ProvidedCommandArgumentsInfo(string CommandText, ShellType CommandType) :
            this(CommandText, ShellManager.GetShellTypeName(CommandType)) 
        { }

        /// <summary>
        /// Makes a new instance of the command argument info with the user-provided command text
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="CommandType">Shell command type.</param>
        internal ProvidedCommandArgumentsInfo(string CommandText, string CommandType)
        {
            string Command;
            bool RequiredArgumentsProvided = true;
            bool RequiredSwitchesProvided = true;
            bool RequiredSwitchArgumentsProvided = true;
            Dictionary<string, CommandInfo> ShellCommands;
            Dictionary<string, CommandInfo> ModCommands;

            // Change the available commands list according to command type
            ShellCommands = CommandManager.GetCommands(CommandType);
            ModCommands = ModManager.ListModCommands(CommandType);

            // Split the switches properly now
            string switchRegex =
                /* lang=regex */ @"(-\S+=((""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s]|\\.)+|\S+))|(?<= )-\S+";
            var EnclosedSwitches = DriverHandler.CurrentRegexpDriverLocal
                .Matches(CommandText, switchRegex)
                .Select((match) => match.Value)
                .ToArray();
            CommandText = DriverHandler.CurrentRegexpDriverLocal.Filter(CommandText, switchRegex);

            // Split the switches to their key-value counterparts
            var EnclosedSwitchKeyValuePairs = SwitchManager.GetSwitchValues(EnclosedSwitches, true);

            // Split the requested command string into words
            var words = CommandText.SplitEncloseDoubleQuotes();
            for (int i = 0; i <= words.Length - 1; i++)
                DebugWriter.WriteDebug(DebugLevel.I, "Word {0}: {1}", i + 1, words[i]);
            Command = words[0];

            // Split the arguments with enclosed quotes
            var EnclosedArgMatches = words.Skip(1);
            var EnclosedArgs = EnclosedArgMatches.ToArray();
            DebugWriter.WriteDebug(DebugLevel.I, "{0} arguments parsed: {1}", EnclosedArgs.Length, string.Join(", ", EnclosedArgs));

            // Get the string of arguments
            string strArgs = words.Length > 0 ? string.Join(" ", EnclosedArgMatches) : "";
            DebugWriter.WriteDebug(DebugLevel.I, "Finished strArgs: {0}", strArgs);

            // Check to see if the caller has provided required number of arguments
            var CommandInfo = ModCommands.ContainsKey(Command) ? ModCommands[Command] :
                              ShellCommands.ContainsKey(Command) ? ShellCommands[Command] :
                              null;
            if (CommandInfo?.CommandArgumentInfo is not null)
                RequiredArgumentsProvided = 
                    (EnclosedArgs.Length >= CommandInfo.CommandArgumentInfo.MinimumArguments) ||
                    !CommandInfo.CommandArgumentInfo.ArgumentsRequired;
            else
                RequiredArgumentsProvided = true;

            // Check to see if the caller has provided required number of switches
            if (CommandInfo?.CommandArgumentInfo is not null)
                RequiredSwitchesProvided =
                    CommandInfo.CommandArgumentInfo.Switches.Length == 0 ||
                    EnclosedSwitches.Length >= CommandInfo.CommandArgumentInfo.Switches.Where((@switch) => @switch.IsRequired).Count() ||
                    !CommandInfo.CommandArgumentInfo.Switches.Any((@switch) => @switch.IsRequired);
            else
                RequiredSwitchesProvided = true;

            // Check to see if the caller has provided required number of switches that require arguments
            if (CommandInfo?.CommandArgumentInfo is not null)
                RequiredSwitchArgumentsProvided =
                    CommandInfo.CommandArgumentInfo.Switches.Length == 0 ||
                    EnclosedSwitches.Length == 0 ||
                    EnclosedSwitchKeyValuePairs.Where((kvp) => !string.IsNullOrEmpty(kvp.Item2)).Count() >= CommandInfo.CommandArgumentInfo.Switches.Where((@switch) => @switch.ArgumentsRequired).Count() ||
                    !CommandInfo.CommandArgumentInfo.Switches.Any((@switch) => @switch.ArgumentsRequired);
            else
                RequiredSwitchArgumentsProvided = true;

            // Check to see if the caller has provided non-existent switches
            if (CommandInfo?.CommandArgumentInfo is not null)
                unknownSwitchesList = EnclosedSwitchKeyValuePairs
                    .Select((kvp) => kvp.Item1)
                    .Where((key) => !CommandInfo.CommandArgumentInfo.Switches.Any((switchInfo) => switchInfo.SwitchName == key[1..]))
                    .ToArray();

            // Check to see if the caller has provided conflicting switches
            if (CommandInfo?.CommandArgumentInfo is not null)
            {
                List<string> processed = new();
                List<string> conflicts = new();
                foreach (var kvp in EnclosedSwitchKeyValuePairs)
                {
                    // Get the switch and its conflicts list
                    string @switch = kvp.Item1;
                    string[] switchConflicts = CommandInfo.CommandArgumentInfo.Switches
                        .Where((switchInfo) => $"-{switchInfo.SwitchName}" == @switch)
                        .First().ConflictsWith
                        .Select((conflicting) => $"-{conflicting}")
                        .ToArray();

                    // Now, get the last switch and check to see if it's provided with the conflicting switch
                    string lastSwitch = processed.Count > 0 ? processed[^1] : "";
                    if (switchConflicts.Contains(lastSwitch))
                        conflicts.Add($"{@switch} vs. {lastSwitch}");
                    processed.Add(@switch);
                }
                conflictingSwitchesList = conflicts.ToArray();
            }

            // Install the parsed values to the new class instance
            ArgumentsList = EnclosedArgs;
            SwitchesList = EnclosedSwitches;
            ArgumentsText = strArgs;
            this.Command = Command;
            this.RequiredArgumentsProvided = RequiredArgumentsProvided;
            this.RequiredSwitchesProvided = RequiredSwitchesProvided;
            this.RequiredSwitchArgumentsProvided = RequiredSwitchArgumentsProvided;
        }

    }
}
