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
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.Shells.SFTP;
using KS.Kernel.Events;

namespace KS.Network.SFTP.Transfer
{
    /// <summary>
    /// SFTP transfer module
    /// </summary>
    public static class SFTPTransfer
    {

        /// <summary>
        /// Downloads a file from the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPGetFile(string File)
        {
            if (SFTPShellCommon.SFTPConnected)
            {
                try
                {
                    // Show a message to download
                    EventsManager.FireEvent(EventType.SFTPPreDownload, File);
                    DebugWriter.WriteDebug(DebugLevel.I, "Downloading file {0}...", File);

                    // Try to download
                    var DownloadFileStream = new System.IO.FileStream($"{SFTPShellCommon.SFTPCurrDirect}/{File}", System.IO.FileMode.OpenOrCreate);
                    SFTPShellCommon.ClientSFTP.DownloadFile($"{SFTPShellCommon.SFTPCurrentRemoteDir}/{File}", DownloadFileStream);

                    // Show a message that it's downloaded
                    DebugWriter.WriteDebug(DebugLevel.I, "Downloaded file {0}.", File);
                    EventsManager.FireEvent(EventType.SFTPPostDownload, File);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Download failed for file {0}: {1}", File, ex.Message);
                    EventsManager.FireEvent(EventType.SFTPDownloadError, File, ex);
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("You must connect to server before performing transmission."));
            }
            return false;
        }

        /// <summary>
        /// Uploads a file to the currently connected SFTP server
        /// </summary>
        /// <param name="File">A local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPUploadFile(string File)
        {
            if (SFTPShellCommon.SFTPConnected)
            {
                try
                {
                    // Show a message to download
                    EventsManager.FireEvent(EventType.SFTPPreUpload, File);
                    DebugWriter.WriteDebug(DebugLevel.I, "Uploading file {0}...", File);

                    // Try to upload
                    var UploadFileStream = new System.IO.FileStream($"{SFTPShellCommon.SFTPCurrDirect}/{File}", System.IO.FileMode.Open);
                    SFTPShellCommon.ClientSFTP.UploadFile(UploadFileStream, $"{SFTPShellCommon.SFTPCurrentRemoteDir}/{File}");
                    DebugWriter.WriteDebug(DebugLevel.I, "Uploaded file {0}", File);
                    EventsManager.FireEvent(EventType.SFTPPostUpload, File);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Upload failed for file {0}: {1}", File, ex.Message);
                    EventsManager.FireEvent(EventType.SFTPUploadError, File, ex);
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("You must connect to server before performing transmission."));
            }
            return false;
        }

    }
}
