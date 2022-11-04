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
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
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
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-showdetails</term>
    /// <description>Shows the details of the files and folders</description>
    /// </item>
    /// <item>
    /// <term>-suppressmessages</term>
    /// <description>Suppresses the "unauthorized" messages</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class SFTP_LslCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            bool ShowFileDetails = ListSwitchesOnly.Contains("-showdetails") || Listing.ShowFileDetailsList;
            bool SuppressUnauthorizedMessage = ListSwitchesOnly.Contains("-suppressmessages") || Flags.SuppressUnauthorizedMessages;
            if (((0 is var arg2 && (ListArgsOnly?.Length) is { } arg1 ? arg1 == arg2 : (bool?)null) | ListArgsOnly is null) == true)
            {
                Listing.List(SFTPShellCommon.SFTPCurrDirect, ShowFileDetails, SuppressUnauthorizedMessage);
            }
            else
            {
                foreach (string Directory in ListArgsOnly)
                {
                    string direct = Filesystem.NeutralizePath(Directory);
                    Listing.List(direct, ShowFileDetails, SuppressUnauthorizedMessage);
                }
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -showdetails: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Shows the file details in the list"), true, ColorTools.ColTypes.ListValue);
            TextWriterColor.Write("  -suppressmessages: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Suppresses the annoying \"permission denied\" messages"), true, ColorTools.ColTypes.ListValue);
        }

    }
}