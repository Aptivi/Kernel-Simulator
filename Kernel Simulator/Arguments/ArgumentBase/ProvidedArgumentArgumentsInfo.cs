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

using System.Collections.Generic;
using System.Linq;
using Extensification.StringExts;
using KS.Kernel.Debugging;

namespace KS.Arguments.ArgumentBase
{
    public class ProvidedArgumentArgumentsInfo
    {

        /// <summary>
        /// Target kernel argument that the user executed in shell
        /// </summary>
        public string Argument { get; private set; }
        /// <summary>
        /// Text version of the provided arguments and switches
        /// </summary>
        public string ArgumentsText { get; private set; }
        /// <summary>
        /// List version of the provided arguments and switches
        /// </summary>
        public string[] FullArgumentsList { get; private set; }
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
        /// Makes a new instance of the kernel argument argument info with the user-provided command text
        /// </summary>
        /// <param name="ArgumentText">Kernel argument text that the user provided</param>
        /// <param name="ArgumentType">Kernel argument type. Consult the <see cref="ArgumentType"/> enum for information about supported shells.</param>
        internal ProvidedArgumentArgumentsInfo(string ArgumentText, ArgumentType ArgumentType)
        {
            string Argument;
            bool RequiredArgumentsProvided = true;
            var KernelArguments = ArgumentParse.AvailableArgs;

            // Change the available commands list according to command type
            switch (ArgumentType)
            {
                case ArgumentType.KernelArgs:
                    {
                        KernelArguments = ArgumentParse.AvailableArgs;
                        break;
                    }
                case ArgumentType.CommandLineArgs:
                    {
                        KernelArguments = CommandLineArgs.AvailableCMDLineArgs;
                        break;
                    }
                case ArgumentType.PreBootCommandLineArgs:
                    {
                        KernelArguments = PreBootCommandLineArgsParse.AvailablePreBootCMDLineArgs;
                        break;
                    }
            }

            // Get the index of the first space (Used for step 3)
            int index = ArgumentText.IndexOf(" ");
            if (index == -1)
                index = ArgumentText.Length;
            DebugWriter.WriteDebug(DebugLevel.I, "Index: {0}", index);

            // Split the requested command string into words
            var words = ArgumentText.Split(new[] { ' ' });
            for (int i = 0, loopTo = words.Length - 1; i <= loopTo; i++)
                DebugWriter.WriteDebug(DebugLevel.I, "Word {0}: {1}", i + 1, words[i]);
            Argument = words[0];

            // Get the string of arguments
            string strArgs = ArgumentText.Substring(index);
            DebugWriter.WriteDebug(DebugLevel.I, "Prototype strArgs: {0}", strArgs);
            if (!(index == ArgumentText.Length))
                strArgs = strArgs.Substring(1);
            DebugWriter.WriteDebug(DebugLevel.I, "Finished strArgs: {0}", strArgs);

            // Split the arguments with enclosed quotes and set the required boolean variable
            var EnclosedArgs = strArgs.SplitEncloseDoubleQuotes(" ")?.ToList();
            if (EnclosedArgs is not null)
            {
                RequiredArgumentsProvided = (bool)(KernelArguments[Argument].MinimumArguments is var arg2 && (EnclosedArgs?.Count) is { } arg1 ? arg1 >= arg2 : (bool?)null);
            }
            else if (KernelArguments[Argument].ArgumentsRequired & EnclosedArgs is null)
            {
                RequiredArgumentsProvided = false;
            }
            if (EnclosedArgs is not null)
                DebugWriter.WriteDebug(DebugLevel.I, "Arguments parsed: " + string.Join(", ", EnclosedArgs));

            // Separate the arguments from the switches
            var FinalArgs = new List<string>();
            var FinalSwitches = new List<string>();
            if (EnclosedArgs is not null)
            {
                foreach (string EnclosedArg in EnclosedArgs)
                {
                    if (EnclosedArg.StartsWith("-"))
                    {
                        FinalSwitches.Add(EnclosedArg);
                    }
                    else
                    {
                        FinalArgs.Add(EnclosedArg);
                    }
                }
            }

            // Install the parsed values to the new class instance
            FullArgumentsList = EnclosedArgs?.ToArray();
            ArgumentsList = FinalArgs.ToArray();
            SwitchesList = FinalSwitches.ToArray();
            ArgumentsText = strArgs;
            this.Argument = Argument;
            this.RequiredArgumentsProvided = RequiredArgumentsProvided;
        }

    }
}