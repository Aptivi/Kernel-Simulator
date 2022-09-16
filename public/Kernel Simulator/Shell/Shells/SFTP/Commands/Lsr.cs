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
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.SFTP.Filesystem;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.SFTP.Commands
{
    /// <summary>
    /// Lists the contents of the current folder or the folder provided
    /// </summary>
    /// <remarks>
    /// You can see the list of the files and sub-directories contained in the current working directory if no directories are specified, or in the specified directory, if specified.
    /// <br></br>
    /// You can also see the list of the files and sub-directories contained in the previous directory of your current position.
    /// <br></br>
    /// Unlike lsl, you should connect to the server to use this command, because it lists directories in the server, not in the local hard drive.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-showdetails</term>
    /// <description>Shows the details of the files and folders</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class SFTP_LsrCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            bool ShowFileDetails = ListSwitchesOnly.Contains("-showdetails") || SFTPShellCommon.SFTPShowDetailsInList;
            var Entries = new List<string>();
            if (!(ListArgsOnly.Length == 0))
            {
                foreach (string TargetDirectory in ListArgsOnly)
                    Entries = SFTPFilesystem.SFTPListRemote(TargetDirectory, ShowFileDetails);
            }
            else
            {
                Entries = SFTPFilesystem.SFTPListRemote("", ShowFileDetails);
            }
            Entries.Sort();
            foreach (string Entry in Entries)
                TextWriterColor.Write(Entry, true, ColorTools.ColTypes.ListEntry);
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, ColorTools.ColTypes.NeutralText);
            TextWriterColor.Write("  -showdetails: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Shows the file details in the list"), true, ColorTools.ColTypes.ListValue);
        }

    }
}