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

using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Sets data transfer type
    /// </summary>
    /// <remarks>
    /// If you need to change how the data transfer is made, you can use this command to switch between the ASCII transfer and the binary transfer. Please note that the ASCII transfer is highly discouraged in many conditions except if you're only transferring text.
    /// </remarks>
    class FTP_TypeCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly[0].ToLower() == "a")
            {
                FTPShellCommon.ClientFTP.DownloadDataType = FtpDataType.ASCII;
                FTPShellCommon.ClientFTP.ListingDataType = FtpDataType.ASCII;
                FTPShellCommon.ClientFTP.UploadDataType = FtpDataType.ASCII;
                TextWriterColor.Write(Translate.DoTranslation("Data type set to ASCII!"), true, ColorTools.ColTypes.Success);
                TextWriterColor.Write(Translate.DoTranslation("Beware that most files won't download or upload properly using this mode, so we highly recommend using the Binary mode on most situations."), true, ColorTools.ColTypes.Warning);
            }
            else if (ListArgsOnly[0].ToLower() == "b")
            {
                FTPShellCommon.ClientFTP.DownloadDataType = FtpDataType.Binary;
                FTPShellCommon.ClientFTP.ListingDataType = FtpDataType.Binary;
                FTPShellCommon.ClientFTP.UploadDataType = FtpDataType.Binary;
                TextWriterColor.Write(Translate.DoTranslation("Data type set to Binary!"), true, ColorTools.ColTypes.Success);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Invalid data type."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}