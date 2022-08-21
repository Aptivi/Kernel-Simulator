﻿using System;
using FluentFTP;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
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

namespace KS.Network.FTP.Transfer
{
    public static class FTPTransferProgress
    {

        /// <summary>
        /// Action of file progress. You can make your own handler by mods
        /// </summary>
        public static Action<FtpProgress> FileProgress = new Action<FtpProgress>(FileProgressHandler);
        /// <summary>
        /// Action of folder/multiple file progress. You can make your own handler by mods
        /// </summary>
        public static Action<FtpProgress> MultipleProgress = new Action<FtpProgress>(MultipleProgressHandler);

        /// <summary>
        /// Handles the individual file download/upload progress
        /// </summary>
        private static void FileProgressHandler(FtpProgress Percentage)
        {
            // If the progress is not defined, disable progress bar
            if (Percentage.Progress < 0d)
            {
                FTPTransfer.progressFlag = false;
            }
            else
            {
                FTPTransfer.ConsoleOriginalPosition_LEFT = Console.CursorLeft;
                FTPTransfer.ConsoleOriginalPosition_TOP = Console.CursorTop;
                if (FTPTransfer.progressFlag == true & Percentage.Progress != 100d)
                {
                    TextWriterColor.Write(" {0}% (ETA: {1}d {2}:{3}:{4} @ {5})", false, ColorTools.ColTypes.Progress, Percentage.Progress.ToString("N2"), Percentage.ETA.Days, Percentage.ETA.Hours, Percentage.ETA.Minutes, Percentage.ETA.Seconds, Percentage.TransferSpeedToString());
                    ConsoleExtensions.ClearLineToRight();
                }
                Console.SetCursorPosition(FTPTransfer.ConsoleOriginalPosition_LEFT, FTPTransfer.ConsoleOriginalPosition_TOP);
            }
        }

        /// <summary>
        /// Handles the multiple files/folder download/upload progress
        /// </summary>
        private static void MultipleProgressHandler(FtpProgress Percentage)
        {
            // If the progress is not defined, disable progress bar
            if (Percentage.Progress < 0d)
            {
                FTPTransfer.progressFlag = false;
            }
            else
            {
                FTPTransfer.ConsoleOriginalPosition_LEFT = Console.CursorLeft;
                FTPTransfer.ConsoleOriginalPosition_TOP = Console.CursorTop;
                if (FTPTransfer.progressFlag == true & Percentage.Progress != 100d)
                {
                    TextWriterColor.Write("- [{0}/{1}] {2}: ", false, ColorTools.ColTypes.ListEntry, Percentage.FileIndex + 1, Percentage.FileCount, Percentage.RemotePath);
                    TextWriterColor.Write("{0}% (ETA: {1}d {2}:{3}:{4} @ {5})", false, ColorTools.ColTypes.Progress, Percentage.Progress.ToString("N2"), Percentage.ETA.Days, Percentage.ETA.Hours, Percentage.ETA.Minutes, Percentage.ETA.Seconds, Percentage.TransferSpeedToString());
                    ConsoleExtensions.ClearLineToRight();
                }
                Console.SetCursorPosition(FTPTransfer.ConsoleOriginalPosition_LEFT, FTPTransfer.ConsoleOriginalPosition_TOP);
            }
        }

    }
}