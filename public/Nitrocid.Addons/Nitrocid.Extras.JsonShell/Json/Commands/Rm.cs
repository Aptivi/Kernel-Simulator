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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.JsonShell.Tools;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using System;

namespace Nitrocid.Extras.JsonShell.Json.Commands
{
    /// <summary>
    /// Removes a new object, property, or array
    /// </summary>
    /// <remarks>
    /// You can use this command to remove an object, a property, or an from the end of the parent token. Note that the parent token must exist.
    /// </remarks>
    class RmCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                JsonTools.Remove(parameters.ArgumentsList[0]);
            }
            catch (KernelException kex)
            {
                TextWriters.Write(kex.Message, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.JsonEditor;
            }
            catch (Exception ex)
            {
                TextWriters.Write(Translate.DoTranslation("The JSON shell failed to remove an item.") + $" {ex.Message}", KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.JsonEditor;
            }
            return 0;
        }
    }
}
