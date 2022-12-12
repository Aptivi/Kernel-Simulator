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
using static Extensification.CharExts.Querying;
using Extensification.StringExts;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Text;
using KS.Drivers;

namespace KS.Files.LineEndings
{
    /// <summary>
    /// Line endings tools module
    /// </summary>
    public static class LineEndingsTools
    {

        /// <summary>
        /// The new line style used for the current platform
        /// </summary>
        public static FilesystemNewlineStyle NewlineStyle
        {
            get
            {
                if (CharManager.NewLine == $"{Convert.ToChar(13)}{Convert.ToChar(10)}")
                    return FilesystemNewlineStyle.CRLF;
                else if (CharManager.NewLine == Convert.ToChar(13).ToString())
                    return FilesystemNewlineStyle.CR;
                else if (CharManager.NewLine == Convert.ToChar(10).ToString())
                    return FilesystemNewlineStyle.LF;
                else
                    return FilesystemNewlineStyle.CRLF;
            }
        }

        /// <summary>
        /// Gets the line ending string from the specified line ending style
        /// </summary>
        /// <param name="LineEndingStyle">Line ending style</param>
        public static string GetLineEndingString(FilesystemNewlineStyle LineEndingStyle)
        {
            switch (LineEndingStyle)
            {
                case FilesystemNewlineStyle.CRLF:
                    {
                        return $"{Convert.ToChar(13)}{Convert.ToChar(10)}";
                    }
                case FilesystemNewlineStyle.LF:
                    {
                        return Convert.ToChar(10).ToString();
                    }
                case FilesystemNewlineStyle.CR:
                    {
                        return Convert.ToChar(13).ToString();
                    }
                default:
                    {
                        return Environment.NewLine;
                    }
            }
        }

        /// <summary>
        /// Gets the line ending style from file
        /// </summary>
        /// <param name="TextFile">Target text file</param>
        public static FilesystemNewlineStyle GetLineEndingFromFile(string TextFile) =>
            DriverHandler.CurrentFilesystemDriver.GetLineEndingFromFile(TextFile);

    }
}
