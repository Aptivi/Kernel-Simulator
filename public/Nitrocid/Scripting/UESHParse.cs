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
using System.IO;
using Extensification.StringExts;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Kernel.Events;
using KS.Scripting.Conditions;
using System.Linq;

namespace KS.Scripting
{
    /// <summary>
    /// UESH script parser
    /// </summary>
    public static class UESHParse
    {

        /// <summary>
        /// Executes the UESH script
        /// </summary>
        /// <param name="ScriptPath">Full path to script</param>
        /// <param name="ScriptArguments">Script arguments</param>
        public static void Execute(string ScriptPath, string ScriptArguments)
        {
            try
            {
                // Raise event
                EventsManager.FireEvent(EventType.UESHPreExecute, ScriptPath, ScriptArguments);

                // Open the script file for reading
                var FileStream = new StreamReader(ScriptPath);
                int LineNo = 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Stream opened. Parsing script");

                // Look for $variables and initialize them
                while (!FileStream.EndOfStream)
                {
                    // Get line
                    string Line = FileStream.ReadLine();
                    DebugWriter.WriteDebug(DebugLevel.I, "Line {0}: \"{1}\"", LineNo, Line);

                    // If $variable is found in string, initialize it
                    var SplitWords = Line.Split(' ');
                    for (int i = 0; i <= SplitWords.Length - 1; i++)
                        if (!UESHVariables.ShellVariables.ContainsKey(SplitWords[i]) & SplitWords[i].StartsWith("$"))
                            UESHVariables.InitializeVariable(SplitWords[i]);
                }

                // Seek to the beginning
                FileStream.BaseStream.Seek(0L, SeekOrigin.Begin);

                // Get all lines and parse comments, commands, and arguments
                string[] commandBlocks = new string[] { "if" };
                int lineNum = 1;
                int commandStackNum = 0;
                bool newCommandStackRequired = false;
                while (!FileStream.EndOfStream)
                {
                    // Get line
                    string Line = FileStream.ReadLine();
                    DebugWriter.WriteDebug(DebugLevel.I, "Line {0}: \"{1}\"", LineNo, Line);

                    // First, trim the line from the left after checking the stack
                    string stackIndicator = new('|', commandStackNum);
                    if (Line.StartsWith(stackIndicator))
                    {
                        newCommandStackRequired = false;

                        // Get the actual command
                        Line = Line[commandStackNum..];

                        // If it still starts with the new stack indicator, throw an error
                        if (Line.StartsWith('|'))
                            throw new KernelException(KernelExceptionType.UESHScript, Translate.DoTranslation("You can't declare the new block before you place expressions that support the creation, like conditions or loops. The stack number is {0}.") + " {1}:{2}", commandStackNum, ScriptPath, lineNum);
                    }
                    else if (!Line.StartsWith(stackIndicator) && newCommandStackRequired)
                        throw new KernelException(KernelExceptionType.UESHScript, Translate.DoTranslation("When starting a new block, make sure that you've indented the stack correctly. The stack number is {0}.") + " {1}:{2}", commandStackNum, ScriptPath, lineNum);
                    else
                        commandStackNum = 0;

                    // See if the line contains variable, and replace every instance of it with its value
                    var SplitWords = Line.SplitEncloseDoubleQuotes(" ");
                    if (SplitWords is not null)
                        // Iterate every word
                        for (int i = 0; i <= SplitWords.Length - 1; i++)
                            // Every word that start with the $ sign means it's a variable that should be replaced with the
                            // value from the UESH variable manager.
                            if (SplitWords[i].StartsWith("$"))
                                Line = UESHVariables.GetVariableCommand(SplitWords[i], Line);

                    // See if the line contains argument placeholder, and replace every instance of it with its value
                    var SplitArguments = ScriptArguments.SplitEncloseDoubleQuotes(" ");
                    if (SplitArguments is not null)
                        // Iterate every word
                        for (int i = 0; i <= SplitWords.Length - 1; i++)
                            // Iterate every script argument
                            for (int j = 0; j <= SplitArguments.Length - 1; j++)
                                // If there is a placeholder variable like so:
                                //     echo Hello, {0}
                                // ...then proceed to replace the placeholder that contains an index of argument with the
                                // actual value
                                if (SplitWords[i] == $"{{{j}}}")
                                    Line = Line.Replace(SplitWords[i], SplitArguments[j]);

                    // See if the line is a command that starts with the if statement
                    if (SplitWords is not null)
                    {
                        string Command = SplitWords[0];
                        string Arguments = Line.TrimStart($"{Command} ".ToCharArray());
                        bool isBlock = commandBlocks.Contains(Command);
                        if (isBlock)
                        {
                            bool satisfied = false;
                            switch (Command)
                            {
                                case "if":
                                    satisfied = UESHConditional.ConditionSatisfied(Arguments);
                                    break;
                            }
                            if (satisfied)
                            {
                                // New stack required
                                newCommandStackRequired = true;
                                commandStackNum++;
                                continue;
                            }
                            else
                            {
                                // Skip all the if block until we reach our stack
                                while (true)
                                {
                                    Line = FileStream.ReadLine();
                                    string blockStackIndicator = new('|', commandStackNum + 1);
                                    if (!Line.StartsWith(blockStackIndicator))
                                        break;
                                }
                                Line = Line.TrimStart('|');
                            }
                        }
                    }

                    // See if the line is a comment or command
                    if (!Line.StartsWith("#") & !Line.StartsWith(" "))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Line {0} is not a comment.", Line);
                        Shell.Shell.GetLine(Line);
                    }
                    else
                        // For debugging purposes
                        DebugWriter.WriteDebug(DebugLevel.I, "Line {0} is a comment.", Line);

                    // Increment the new line number
                    lineNum++;
                }

                // Close the stream
                FileStream.Close();
                EventsManager.FireEvent(EventType.UESHPostExecute, ScriptPath, ScriptArguments);
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.UESHError, ScriptPath, ScriptArguments, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to execute script {0} with arguments {1}: {2}", ScriptPath, ScriptArguments, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.UESHScript, Translate.DoTranslation("The script is malformed. Check the script and resolve any errors: {0}"), ex, ex.Message);
            }
        }

    }
}
