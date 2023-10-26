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

using KS.Kernel.Power;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shuts down your computer
    /// </summary>
    /// <remarks>
    /// If you're finished with everything and don't want to do something else on your computer, instead of leaving it on to consume energy and pay high energy bills, you have to use this command to shutdown your computer and conserve power.
    /// </remarks>
    class ShutdownCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!(parameters.ArgumentsList.Length == 0))
            {
                if (parameters.ArgumentsList.Length == 1)
                {
                    PowerManager.PowerManage(PowerMode.RemoteShutdown, parameters.ArgumentsList[0]);
                }
                else
                {
                    PowerManager.PowerManage(PowerMode.RemoteShutdown, parameters.ArgumentsList[0], Convert.ToInt32(parameters.ArgumentsList[1]));
                }
            }
            else
            {
                PowerManager.PowerManage(PowerMode.Shutdown);
            }
            return 0;
        }

    }
}
