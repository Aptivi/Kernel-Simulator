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

using System;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Misc.Reflection;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Writer.FancyWriters;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can get a property value
    /// </summary>
    /// <remarks>
    /// This command lets you get a value from a specified property.
    /// </remarks>
    class GetPropertyValueCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // List all available properties on all the kernel types
            string propertyName = parameters.ArgumentsList[0];
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var property = PropertyManager.GetProperty(propertyName, type);
                    if (property is null || (!property.GetMethod?.IsStatic ?? false))
                        continue;

                    // Write the property name and its value
                    SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Property info for") + $" {type.Name}::{propertyName}", true);
                    TextWriters.Write(Translate.DoTranslation("Value") + $": ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{property.GetValue(null)}", KernelColorType.ListValue);
                }
                catch (Exception ex)
                {
                    TextWriters.Write(Translate.DoTranslation("Failed to get property info for") + $" {type.Name}::{propertyName}: {ex.Message}", KernelColorType.Error);
                }
            }
            return 0;
        }

    }
}
