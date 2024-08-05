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
using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Network.FTP.Filesystem;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.FTP.Commands
{
    class FTP_SumFilesCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string RemoteDirectory = ListArgs[0];
            string Hash = ListArgs[1];

            // Check to see if hash is found
            if (Enum.IsDefined(typeof(FtpHashAlgorithm), Hash))
            {
                var HashResults = FTPHashing.FTPGetHashes(RemoteDirectory, (FtpHashAlgorithm)Convert.ToInt32(Enum.Parse(typeof(FtpHashAlgorithm), Hash)));
                foreach (string Filename in HashResults.Keys)
                {
                    TextWriters.Write("- " + Filename + ": ", false, KernelColorTools.ColTypes.ListEntry);
                    TextWriters.Write(HashResults[Filename].Value, true, KernelColorTools.ColTypes.ListValue);
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorTools.ColTypes.Error);
            }
        }

    }
}