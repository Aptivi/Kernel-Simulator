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
using System.IO;
using System.IO.Compression;
using System.Linq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Folders;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Extracts a ZIP file
    /// </summary>
    /// <remarks>
    /// If you wanted to extract the contents of a ZIP file, you can use this command to gain access to the compressed files stored inside it.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-createdir</term>
    /// <description>Extracts the archive to the new directory that has the same name as the archive</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class UnZipCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length == 1)
            {
                string ZipArchiveName = Filesystem.NeutralizePath(ListArgsOnly[0]);
                ZipFile.ExtractToDirectory(ZipArchiveName, CurrentDirectory.CurrentDir);
            }
            else if (ListArgsOnly.Length > 1)
            {
                string ZipArchiveName = Filesystem.NeutralizePath(ListArgsOnly[0]);
                string Destination = !(ListSwitchesOnly[0] == "-createdir") ? Filesystem.NeutralizePath(ListArgsOnly[1]) : "";
                if (ListSwitchesOnly.Contains("-createdir"))
                {
                    Destination = $"{(!(ListSwitchesOnly[0] == "-createdir") ? Filesystem.NeutralizePath(ListArgsOnly[1]) : "")}/{(!(ListSwitchesOnly[0] == "-createdir") ? Path.GetFileNameWithoutExtension(ZipArchiveName) : Filesystem.NeutralizePath(Path.GetFileNameWithoutExtension(ZipArchiveName)))}";
                    if (Convert.ToString(Destination[0]) == "/")
                        Destination = Destination.RemoveLetter(0);
                }
                ZipFile.ExtractToDirectory(ZipArchiveName, Destination);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, ColorTools.ColTypes.Neutral);
            TextWriterColor.Write("  -createdir: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Creates a directory that contains the contents of the ZIP file"), true, ColorTools.ColTypes.ListValue);
        }

    }
}