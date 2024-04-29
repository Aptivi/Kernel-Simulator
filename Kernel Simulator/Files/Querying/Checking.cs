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
using System.IO;

namespace KS.Files.Querying
{
    public static class Checking
    {

        /// <summary>
        /// Checks to see if the file exists. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="File">Target file</param>
        /// <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        public static bool FileExists(string File, bool Neutralize = false)
        {
            Filesystem.ThrowOnInvalidPath(File);
            if (Neutralize)
                File = Filesystem.NeutralizePath(File);
            return System.IO.File.Exists(File);
        }

        /// <summary>
        /// Checks to see if the folder exists. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="Folder">Target folder</param>
        /// <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        public static bool FolderExists(string Folder, bool Neutralize = false)
        {
            Filesystem.ThrowOnInvalidPath(Folder);
            if (Neutralize)
                Folder = Filesystem.NeutralizePath(Folder);
            return Directory.Exists(Folder);
        }

    }
}