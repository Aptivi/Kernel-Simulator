﻿
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

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.FTP.Filesystem;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Moves a file or directory to another destination in the server
    /// </summary>
    /// <remarks>
    /// If you manage the FTP server and wanted to move a file or a directory from a remote directory to another remote directory, use this command.
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class FTP_MvCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (FTPShellCommon.FtpConnected)
            {
                TextWriterColor.Write(Translate.DoTranslation("Moving {0} to {1}..."), true, ColorTools.ColTypes.Progress, ListArgsOnly[0], ListArgsOnly[1]);
                if (FTPFilesystem.FTPMoveItem(ListArgsOnly[0], ListArgsOnly[1]))
                {
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Moved successfully"), true, ColorTools.ColTypes.Success);
                }
                else
                {
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Failed to move {0} to {1}."), true, ColorTools.ColTypes.Error, ListArgsOnly[0], ListArgsOnly[1]);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You must connect to server before performing transmission."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}
