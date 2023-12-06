﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Drivers;
using KS.Kernel.Debugging;
using KS.Kernel.Extensions;
using KS.Misc.Splash;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.Legacy.InxiNet
{
    internal class InxiNetInit : IAddon
    {
        private static readonly FallbackHardwareProber singleton = new();

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.LegacyInxiNet);

        AddonType IAddon.AddonType => AddonType.Important;

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            // TODO: Inxi.NET parser will be removed starting from the beginning of the RC cycle.
            SplashReport.ReportProgressWarning("Inxi.NET parser will be removed starting from the beginning of the RC cycle.");
            DriverHandler.RegisterBaseDriver(DriverTypes.HardwareProber, singleton);
        }

        void IAddon.StopAddon()
        {
            // Reset hardware info
            singleton.ResetAll();
            DebugWriter.WriteDebug(DebugLevel.I, "Inxi.NET: Hardware info reset.");
            DriverHandler.UnregisterBaseDriver(DriverTypes.HardwareProber, "Fallback");
        }
    }
}
