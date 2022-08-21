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

using KS.Network.SFTP;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.SFTP.Commands
{
    /// <summary>
    /// Prompts you to select an address to connect to
    /// </summary>
    /// <remarks>
    /// We have implemented speed dial to the SFTP client to quickly connect to the last-connected SFTP server. This is so you don't have to repeat the connect command to the same server over and over.
    /// <br></br>
    /// For this, we have implemented this command for easier access to SFTP servers.
    /// </remarks>
    class SFTP_QuickConnectCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!SFTPShellCommon.SFTPConnected)
            {
                SFTPTools.SFTPQuickConnect();
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You should disconnect from server before connecting to another server"), true, ColorTools.ColTypes.Error);
            }
        }

    }
}