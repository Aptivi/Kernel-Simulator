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

using Nitrocid.ConsoleBase.Themes;
using Newtonsoft.Json.Linq;
using Nitrocid.Kernel.Debugging;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Modifications;
using Nitrocid.Misc.Reflection.Internal;
using Textify.General;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.ThemePacks
{
    internal class ThemePackInit : IAddon
    {
        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonThemePacks);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon()
        {
            // Add them all!
            string[] themeResNames = ResourcesManager.GetResourceNames(typeof(ThemePackInit).Assembly);
            foreach (string resource in themeResNames)
            {
                string key = resource.RemovePrefix("Themes.");
                string themeName = key.RemoveSuffix(".json");
                string? data = ResourcesManager.GetData(key, ResourcesType.Themes, typeof(ThemePackInit).Assembly) ??
                    throw new KernelException(KernelExceptionType.Reflection, Translate.DoTranslation("Can't get necessary data to initialize this addon."));
                var themeToken = JToken.Parse(data);
                bool result = ThemeTools.themes.TryAdd(themeName, new ThemeInfo(themeToken));
                DebugWriter.WriteDebug(DebugLevel.I, "Added {0}: {1}", themeName, result);
            }
        }

        void IAddon.StopAddon()
        {
            // Remove them all!
            string[] themeResNames = ResourcesManager.GetResourceNames(typeof(ThemePackInit).Assembly);
            foreach (string resource in themeResNames)
            {
                string key = resource.RemovePrefix("Themes.");
                string themeName = key.RemoveSuffix(".json");
                bool result = ThemeTools.themes.Remove(themeName);
                DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}: {1}", themeName, result);
            }
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
