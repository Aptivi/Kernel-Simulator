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

using KS.Languages;

namespace KS.Modifications
{
    /// <summary>
    /// Mod part information
    /// </summary>
    public class PartInfo
    {

        /// <summary>
        /// The mod name. If no name is specified, or if it only consists of whitespaces (space), the file name is taken.
        /// </summary>
        public string ModName { get; private set; }
        /// <summary>
        /// The part name.
        /// </summary>
        public string PartName { get; private set; }
        /// <summary>
        /// The mod part file name
        /// </summary>
        public string PartFileName { get; private set; }
        /// <summary>
        /// The mod part file path
        /// </summary>
        public string PartFilePath { get; private set; }
        /// <summary>
        /// The mod part script
        /// </summary>
        public IScript PartScript { get; private set; }

        /// <summary>
        /// Creates new mod info instance
        /// </summary>
        internal PartInfo(string ModName, string PartName, string PartFileName, string PartFilePath, IScript PartScript)
        {
            // Validate values. Check to see if the name is null. If so, it will take the mod file name.
            if (string.IsNullOrWhiteSpace(ModName))
            {
                ModName = PartFileName;
            }

            // Check to see if the part script is null. If so, throw exception.
            if (PartScript is null)
            {
                throw new Kernel.Exceptions.ModNoPartsException(Translate.DoTranslation("Mod part is nothing."));
            }

            // Install values to new instance
            this.ModName = ModName;
            this.PartName = PartName;
            this.PartFileName = PartFileName;
            this.PartFilePath = PartFilePath;
            this.PartScript = PartScript;
        }

    }
}
