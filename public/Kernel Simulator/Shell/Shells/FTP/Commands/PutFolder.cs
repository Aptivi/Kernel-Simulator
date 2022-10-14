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
using KS.Network.FTP.Transfer;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Uploads the folder to the server
    /// </summary>
    /// <remarks>
    /// If you need to add your local folders in your current working directory to the current working server directory, you must have administrative privileges to add them.
    /// <br></br>
    /// For example, if you're adding the group of pictures, you need to upload it to the server for everyone to see. Assuming that it's "NewDelhi", use "putfolder NewDelhi."
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class FTP_PutFolderCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string LocalFolder = ListArgsOnly[0];
            string RemoteFolder = ListArgsOnly.Length > 1 ? ListArgsOnly[1] : "";
            TextWriterColor.Write(Translate.DoTranslation("Uploading folder {0}..."), true, ColorTools.ColTypes.Progress, ListArgsOnly[0]);
            bool Result = !string.IsNullOrWhiteSpace(LocalFolder) ? FTPTransfer.FTPUploadFolder(RemoteFolder, LocalFolder) : FTPTransfer.FTPUploadFolder(RemoteFolder);
            if (Result)
            {
                TextWriterColor.Write();
                TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Uploaded folder {0}"), true, ColorTools.ColTypes.Success, ListArgsOnly[0]);
            }
            else
            {
                TextWriterColor.Write();
                TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Failed to upload {0}"), true, ColorTools.ColTypes.Error, ListArgsOnly[0]);
            }
        }

    }
}
