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

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Network.FTP.Filesystem;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.FTP.Commands
{
    class FTP_CpCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (FTPShellCommon.FtpConnected)
            {
                TextWriters.Write(Translate.DoTranslation("Copying {0} to {1}..."), true, KernelColorTools.ColTypes.Neutral, ListArgs[0], ListArgs[1]);
                if (FTPFilesystem.FTPCopyItem(ListArgs[0], ListArgs[1]))
                {
                    TextWriters.Write(Kernel.Kernel.NewLine + Translate.DoTranslation("Copied successfully"), true, KernelColorTools.ColTypes.Success);
                }
                else
                {
                    TextWriters.Write(Kernel.Kernel.NewLine + Translate.DoTranslation("Failed to copy {0} to {1}."), true, KernelColorTools.ColTypes.Error, ListArgs[0], ListArgs[1]);
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("You must connect to server before performing transmission."), true, KernelColorTools.ColTypes.Error);
            }
        }

    }
}