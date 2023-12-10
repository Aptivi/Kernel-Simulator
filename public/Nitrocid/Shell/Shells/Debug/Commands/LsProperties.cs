﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Misc.Reflection;
using System;
using KS.ConsoleBase.Colors;
using KS.Shell.ShellBase.Switches;

namespace KS.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the available properties
    /// </summary>
    /// <remarks>
    /// This command lets you list all the available properties that Nitrocid KS registered.
    /// </remarks>
    class LsPropertiesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // List all available properties on all the kernel types
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var properties = PropertyManager.GetProperties(type);
                    if (properties.Count > 0)
                    {
                        // Write the property names and their values
                        SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("List of properties for") + $" {type.Name}", true);
                        ListWriterColor.WriteList(properties);
                    }
                }
                catch (Exception ex)
                {
                    if (!SwitchManager.ContainsSwitch(parameters.SwitchesList, "-suppress"))
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Failed to get property info for") + $" {type.Name}: {ex.Message}", KernelColorType.Error);
                }
            }
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            // List all available properties on all the kernel types
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var properties = PropertyManager.GetProperties(type);
                    if (properties.Count > 0)
                    {
                        // Write the property names and their values
                        SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("List of properties for") + $" {type.Name}", true);
                        foreach (var property in properties)
                            TextWriterColor.Write($"  - {property.Key} [{property.Value}]");
                    }
                }
                catch (Exception ex)
                {
                    if (!SwitchManager.ContainsSwitch(parameters.SwitchesList, "-suppress"))
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Failed to get property info for") + $" {type.Name}: {ex.Message}", KernelColorType.Error);
                }
            }
            return 0;
        }

    }
}
