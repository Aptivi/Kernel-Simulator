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

using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lets you unblock a blocked debug device
    /// </summary>
    /// <remarks>
    /// If you wanted to let a device whose IP address is blocked join the remote debugging again, you can unblock it using this command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class UnblockDbgDevCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string address = parameters.ArgumentsList[0];
            var device = RemoteDebugTools.GetDeviceFromIp(address);
            if (device.Blocked)
            {
                if (RemoteDebugTools.TryRemoveFromBlockList(address))
                {
                    TextWriterColor.Write(Translate.DoTranslation("{0} can now join remote debug again."), address);
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to unblock {0}."), address);
                    return 10000 + (int)KernelExceptionType.Debug;
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("{0} is not blocked yet."), address);
                return 10000 + (int)KernelExceptionType.Debug;
            }
        }

    }
}
