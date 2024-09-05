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

using Nitrocid.Modifications;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.Kernel.Extensions
{
    internal interface IAddon
    {
        internal string AddonName { get; }
        internal ModLoadPriority AddonType { get; }
        internal void StartAddon();
        internal void FinalizeAddon();
        internal void StopAddon();
        internal ReadOnlyDictionary<string, Delegate>? PubliclyAvailableFunctions { get; }
        internal ReadOnlyDictionary<string, PropertyInfo>? PubliclyAvailableProperties { get; }
        internal ReadOnlyDictionary<string, FieldInfo>? PubliclyAvailableFields { get; }
    }
}
