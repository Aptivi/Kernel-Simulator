﻿using System;
using KS.ConsoleBase.Colors;
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

using KS.Network.SFTP.Transfer;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.SFTP.Commands
{
    /// <summary>
    /// Downloads a file from the current working directory
    /// </summary>
    /// <remarks>
    /// Downloads the binary or text file and saves it to the current working local directory for you to use the downloaded file that is provided in the SFTP server.
    /// </remarks>
    class SFTP_GetCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            TextWriterColor.Write(Translate.DoTranslation("Downloading file {0}..."), false, ColorTools.ColTypes.Progress, ListArgsOnly[0]);
            if (SFTPTransfer.SFTPGetFile(ListArgsOnly[0]))
            {
                Console.WriteLine();
                TextWriterColor.Write(Translate.DoTranslation("Downloaded file {0}."), true, ColorTools.ColTypes.Success, ListArgsOnly[0]);
            }
            else
            {
                Console.WriteLine();
                TextWriterColor.Write(Translate.DoTranslation("Download failed for file {0}."), true, ColorTools.ColTypes.Error, ListArgsOnly[0]);
            }
        }

    }
}