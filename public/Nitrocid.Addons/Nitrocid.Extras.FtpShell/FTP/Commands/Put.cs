﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.FtpShell.Tools.Transfer;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.FtpShell.FTP.Commands
{
    /// <summary>
    /// Uploads the file to the server
    /// </summary>
    /// <remarks>
    /// If you need to add your local files in your current working directory to the current working server directory, you must have administrative privileges to add them.
    /// <br></br>
    /// For example, if you're adding the picture of the New Delhi city using the PNG format, you need to upload it to the server for everyone to see. Assuming that it's "NewDelhi.PNG", use "put NewDelhi.PNG."
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class PutCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string LocalFile = parameters.ArgumentsList[0];
            string RemoteFile = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : "";
            TextWriters.Write(Translate.DoTranslation("Uploading file {0}..."), false, KernelColorType.Progress, parameters.ArgumentsList[0]);
            bool Result = !string.IsNullOrWhiteSpace(LocalFile) ? FTPTransfer.FTPUploadFile(RemoteFile, LocalFile) : FTPTransfer.FTPUploadFile(RemoteFile);
            if (Result)
            {
                TextWriterRaw.Write();
                TextWriters.Write(Translate.DoTranslation("Uploaded file {0}"), true, KernelColorType.Success, LocalFile);
                return 0;
            }
            else
            {
                TextWriterRaw.Write();
                TextWriters.Write(Translate.DoTranslation("Failed to upload {0}"), true, KernelColorType.Error, LocalFile);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPFilesystem);
            }
        }

    }
}
