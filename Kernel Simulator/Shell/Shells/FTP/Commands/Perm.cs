﻿using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Network.FTP.Filesystem;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Sets file permissions
    /// </summary>
    /// <remarks>
    /// If you have administrative access to the FTP server, you can set the remote file permissions. The permnumber argument is inherited from CHMOD's permission number.
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class FTP_PermCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (FTPShellCommon.FtpConnected)
            {
                if (FTPFilesystem.FTPChangePermissions(ListArgsOnly[0], Conversions.ToInteger(ListArgsOnly[1])))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Permissions set successfully for file") + " {0}", true, ColorTools.ColTypes.Success, ListArgsOnly[0]);
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to set permissions of {0} to {1}."), true, ColorTools.ColTypes.Error, ListArgsOnly[0], ListArgsOnly[1]);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You must connect to server before performing filesystem operations."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}