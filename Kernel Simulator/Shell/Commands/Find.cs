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

using KS.Files;
using KS.Files.Folders;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using Terminaux.Writer.ConsoleWriters;

namespace KS.Shell.Commands
{
    class FindCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string FileToSearch = ListArgsOnly[0];
            string DirectoryToSearch = CurrentDirectory.CurrentDir;
            if (ListArgsOnly.Length > 1)
            {
                DirectoryToSearch = Filesystem.NeutralizePath(ListArgsOnly[1]);
            }

            // Print the results if found
            string[] FileEntries = Listing.GetFilesystemEntries(DirectoryToSearch, FileToSearch);
            ListWriterColor.WriteList(FileEntries, true);
        }

    }
}
