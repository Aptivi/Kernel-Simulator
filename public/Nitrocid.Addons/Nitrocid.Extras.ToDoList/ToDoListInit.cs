﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Extras.ToDoList.ToDoList;
using Nitrocid.Extras.ToDoList.ToDoList.Commands;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Modifications;
using System.Linq;

namespace Nitrocid.Extras.ToDoList
{
    internal class ToDoListInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("todo", /* Localizable */ "To-do task manager",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "add"),
                        new CommandArgumentPart(true, "taskname"),
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "remove"),
                        new CommandArgumentPart(true, "taskname"),
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "done"),
                        new CommandArgumentPart(true, "taskname"),
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "undone"),
                        new CommandArgumentPart(true, "taskname"),
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "list"),
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "save"),
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "load"),
                    }),
                ], new TodoCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasToDoList);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            // Initialize to-do tasks
            ToDoManager.LoadTasks();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded tasks.");
        }

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
    }
}
