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
using System.Linq;
using System.Text;
using KS.Shell.ShellBase.Switches;

namespace KS.Shell.ShellBase.Arguments
{
    /// <summary>
    /// Command argument info class
    /// </summary>
    public class CommandArgumentInfo
    {

        /// <summary>
        /// Does the command require arguments?
        /// </summary>
        public bool ArgumentsRequired =>
            Arguments.Any((part) => part.ArgumentRequired);
        /// <summary>
        /// User must specify at least this number of arguments
        /// </summary>
        public int MinimumArguments =>
            Arguments.Where((part) => part.ArgumentRequired).Count();
        /// <summary>
        /// Command arguments
        /// </summary>
        public CommandArgumentPart[] Arguments { get; private set; }
        /// <summary>
        /// Command switches
        /// </summary>
        public SwitchInfo[] Switches { get; private set; } = new[] {
            new SwitchInfo("set", /* Localizable */ "Sets the value of the output to the selected UESH variable", false, true)
        };
        /// <summary>
        /// Whether to accept the -set switch to set the UESH variable value
        /// </summary>
        public bool AcceptsSet { get; private set; }
        /// <summary>
        /// Rendered usage
        /// </summary>
        public string RenderedUsage
        {
            get
            {
                var usageBuilder = new StringBuilder();

                // Enumerate through the available switches first
                foreach (var Switch in Switches)
                {
                    bool required = Switch.IsRequired;
                    bool argRequired = Switch.ArgumentsRequired;
                    bool acceptsValue = Switch.AcceptsValues;
                    string switchName = Switch.SwitchName;
                    string renderedSwitchValue = argRequired ? $"=value" : acceptsValue ? $"[=value]" : "";
                    string renderedSwitch =
                        required ?
                        $"<-{switchName}{renderedSwitchValue}> " :
                        $"[-{switchName}{renderedSwitchValue}] ";
                    usageBuilder.Append(renderedSwitch);
                }

                // Enumerate through the available arguments
                foreach (var Argument in Arguments)
                {
                    bool required = Argument.ArgumentRequired;
                    bool justNumeric = Argument.IsNumeric;
                    string renderedArgument =
                        required ?
                        $"<{Argument.ArgumentExpression}{(justNumeric ? ":int" : "")}> " :
                        $"[{Argument.ArgumentExpression}{(justNumeric ? ":int" : "")}] ";
                    usageBuilder.Append(renderedArgument);
                }

                return usageBuilder.ToString().Trim();
            }
        }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        public CommandArgumentInfo()
            : this(Array.Empty<CommandArgumentPart>(), Array.Empty<SwitchInfo>(), false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="AcceptsSet">Whether to accept the -set switch or not</param>
        public CommandArgumentInfo(bool AcceptsSet)
            : this(Array.Empty<CommandArgumentPart>(), Array.Empty<SwitchInfo>(), AcceptsSet)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments)
            : this(Arguments, Array.Empty<SwitchInfo>(), false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="AcceptsSet">Whether to accept the -set switch or not</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments, bool AcceptsSet)
            : this(Arguments, Array.Empty<SwitchInfo>(), AcceptsSet)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Switches">Command switches</param>
        public CommandArgumentInfo(SwitchInfo[] Switches)
            : this(Array.Empty<CommandArgumentPart>(), Switches, false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Switches">Command switches</param>
        /// <param name="AcceptsSet">Whether to accept the -set switch or not</param>
        public CommandArgumentInfo(SwitchInfo[] Switches, bool AcceptsSet)
            : this(Array.Empty<CommandArgumentPart>(), Switches, AcceptsSet)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="Switches">Command switches</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments, SwitchInfo[] Switches)
            : this(Arguments, Switches, false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="Switches">Command switches</param>
        /// <param name="AcceptsSet">Whether to accept the -set switch or not</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments, SwitchInfo[] Switches, bool AcceptsSet)
        {
            this.Arguments = Arguments;
            if (AcceptsSet)
                this.Switches = this.Switches.Union(Switches).ToArray();
            else
                this.Switches = Switches;
            this.AcceptsSet = AcceptsSet;
        }

    }
}
