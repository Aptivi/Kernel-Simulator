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

using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.FtpShell.FTP.Commands
{
    /// <summary>
    /// Executes a server command
    /// </summary>
    /// <remarks>
    /// If you want to go advanced and execute a server command to the FTP server, you can use this command.
    /// </remarks>
    class ExecuteCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write("<<< C: {0}", parameters.ArgumentsText);
            var ExecutedReply = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Execute(parameters.ArgumentsText);
            if (ExecutedReply.Success)
            {
                TextWriters.Write(">>> [{0}] M: {1}", true, KernelColorType.Success, ExecutedReply.Code, ExecutedReply.Message);
                TextWriters.Write(">>> [{0}] I: {1}", true, KernelColorType.Success, ExecutedReply.Code, ExecutedReply.InfoMessages);
                return 0;
            }
            else
            {
                TextWriters.Write(">>> [{0}] M: {1}", true, KernelColorType.Error, ExecutedReply.Code, ExecutedReply.Message);
                TextWriters.Write(">>> [{0}] I: {1}", true, KernelColorType.Error, ExecutedReply.Code, ExecutedReply.InfoMessages);
                TextWriters.Write(">>> [{0}] E: {1}", true, KernelColorType.Error, ExecutedReply.Code, ExecutedReply.ErrorMessage);
                return 10000 + (int)KernelExceptionType.FTPShell;
            }
        }

    }
}
