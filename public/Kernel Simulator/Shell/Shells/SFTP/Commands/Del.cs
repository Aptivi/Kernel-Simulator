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

using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.SFTP.Filesystem;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Inputs;

namespace KS.Shell.Shells.SFTP.Commands
{
    /// <summary>
    /// Removes files or folders
    /// </summary>
    /// <remarks>
    /// If you have logged in to a user that has administrative privileges, you can remove unwanted files, or extra folders, from the server.
    /// <br></br>
    /// If you deleted a file while there are transmissions going on in the server, people who tries to get the deleted file will never be able to download it again after their download fails.
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class SFTP_DelCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (SFTPShellCommon.SFTPConnected)
            {
                // Print a message
                TextWriterColor.Write(Translate.DoTranslation("Deleting {0}..."), true, ColorTools.ColTypes.Progress, ListArgsOnly[0]);

                // Make a confirmation message so user will not accidentally delete a file or folder
                TextWriterColor.Write(Translate.DoTranslation("Are you sure you want to delete {0} <y/n>?") + " ", false, ColorTools.ColTypes.Input, ListArgsOnly[0]);
                string answer = Convert.ToString(Input.DetectKeypress().KeyChar);
                TextWriterColor.Write();

                try
                {
                    SFTPFilesystem.SFTPDeleteRemote(ListArgsOnly[0]);
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(ex.Message, true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You must connect to server with administrative privileges before performing the deletion."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}
