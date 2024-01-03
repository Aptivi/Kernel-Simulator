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

using Nitrocid.Extras.JsonShell.Tools;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;

#pragma warning disable CS0618
namespace Nitrocid.Extras.JsonShell.Json.Commands
{
    /// <summary>
    /// Removes an object
    /// </summary>
    /// <remarks>
    /// You can use this command to remove an object from the parent property. Note that the parent property must exist.
    /// </remarks>
    class RmObjectIndexedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string parent = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-parentProperty");
            JsonTools.RemoveObjectIndexed(parent, int.Parse(parameters.ArgumentsList[0]));
            return 0;
        }
    }
}
#pragma warning restore CS0618
