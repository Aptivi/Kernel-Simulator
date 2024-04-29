﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Network;
using KS.Network.Transfer;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class Get_Command : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int RetryCount = 1;
            string URL = ListArgs[0];
            DebugWriter.Wdbg(DebugLevel.I, "URL: {0}", URL);
            while (!(RetryCount > NetworkTools.DownloadRetries))
            {
                try
                {
                    if (!(URL.StartsWith("ftp://") | URL.StartsWith("ftps://") | URL.StartsWith("ftpes://")))
                    {
                        if (!URL.StartsWith(" "))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Downloading from {0}..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), URL);
                            if (NetworkTransfer.DownloadFile(ListArgs[0]))
                            {
                                TextWriterColor.Write(Translate.DoTranslation("Download has completed."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Specify the address"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Please use \"ftp\" if you are going to download files from the FTP server."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                    }
                    return;
                }
                catch (Exception ex)
                {
                    NetworkTools.TransferFinished = false;
                    TextWriterColor.Write(Translate.DoTranslation("Download failed in try {0}: {1}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), RetryCount, ex.Message);
                    RetryCount += 1;
                    DebugWriter.Wdbg(DebugLevel.I, "Try count: {0}", RetryCount);
                    DebugWriter.WStkTrc(ex);
                }
            }
        }

    }
}