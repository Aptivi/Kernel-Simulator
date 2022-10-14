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

using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Collections.Generic;
using System.IO;

namespace KS.Files.Interactive
{
    /// <summary>
    /// File manager class relating to the interactive file manager planned back in 2018
    /// </summary>
    public static class FileManagerCli
    {
        private static Dictionary<string, FileInfo> cachedFileInfos = new();

        /// <summary>
        /// File manager background color
        /// </summary>
        public static Color FileManagerBackgroundColor = new(Convert.ToInt32(ConsoleColors.Blue));
        /// <summary>
        /// File manager pane separator color
        /// </summary>
        public static Color FileManagerPaneSeparatorColor = ColorTools.GetColor(ColorTools.ColTypes.Separator);
        /// <summary>
        /// File manager option background color
        /// </summary>
        public static Color FileManagerOptionBackgroundColor = new(Convert.ToInt32(ConsoleColors.DarkCyan));
        /// <summary>
        /// File manager key binding in option color
        /// </summary>
        public static Color FileManagerKeyBindingOptionColor = new(Convert.ToInt32(ConsoleColors.Black));
        /// <summary>
        /// File manager option foreground color
        /// </summary>
        public static Color FileManagerOptionForegroundColor = new(Convert.ToInt32(ConsoleColors.Cyan));

        /// <summary>
        /// Opens the file manager to the current path
        /// </summary>
        public static void OpenMain() =>
            OpenMain(CurrentDirectory.CurrentDir);

        /// <summary>
        /// Opens the file manager to the specified path
        /// </summary>
        /// <param name="path">(Non)neutralized path to the folder</param>
        public static void OpenMain(string path)
        {
            // Set the background color
            ColorTools.SetConsoleColor(FileManagerBackgroundColor, true, true);
            ConsoleWrapper.Clear();

            // Make a separator that separates the two panes to make it look like Total Commander or Midnight Commander. We need information in the upper and the
            // lower part of the console, so we need to render the entire program to look like this: (just a concept mockup)
            //
            // H: 0  |
            // H: 1  | ----------------------+----------------------
            // H: 2  |                       |
            // H: 3  |                       |
            // H: 4  |                       |
            // H: 5  |                       |
            // H: 6  |                       |
            // H: 7  |                       |
            // H: 8  |                       |
            // H: 9  | ----------------------+----------------------
            // H: 10 |
            int    SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int    SeparatorMinimumHeight    = 1;
            int    SeparatorMaximumHeight    = ConsoleWrapper.WindowHeight - 2;
            string SeparatorHorizontal       = "═";
            string SeparatorVertical         = "║";
            string SeparatorIntersectDown    = "╦";
            string SeparatorIntersectUp      = "╩";

            // First, the horizontal separators
            for (int i = 0; i < ConsoleWrapper.WindowWidth; i++)
            {
                TextWriterWhereColor.WriteWhere(SeparatorHorizontal, i, SeparatorMinimumHeight, FileManagerPaneSeparatorColor, FileManagerBackgroundColor);
                TextWriterWhereColor.WriteWhere(SeparatorHorizontal, i, SeparatorMaximumHeight, FileManagerPaneSeparatorColor, FileManagerBackgroundColor);
            }

            // Second, the vertical separators
            for (int i = SeparatorMinimumHeight; i < SeparatorMaximumHeight; i++)
                TextWriterWhereColor.WriteWhere(SeparatorVertical, SeparatorHalfConsoleWidth, i, FileManagerPaneSeparatorColor, FileManagerBackgroundColor);

            // Finally, the intersections
            TextWriterWhereColor.WriteWhere(SeparatorIntersectDown, SeparatorHalfConsoleWidth, SeparatorMinimumHeight, FileManagerPaneSeparatorColor, FileManagerBackgroundColor);
            TextWriterWhereColor.WriteWhere(SeparatorIntersectUp, SeparatorHalfConsoleWidth, SeparatorMaximumHeight, FileManagerPaneSeparatorColor, FileManagerBackgroundColor);

            // As we're not done yet, write this message
            TextWriterWhereColor.WriteWhere("TBD. It'll be hopefully finished by Beta 1.", 0, 0, ColorTools.GetColor(ColorTools.ColTypes.Warning), FileManagerBackgroundColor);
            TextWriterWhereColor.WriteWhere("ESC ", 0, ConsoleWrapper.WindowHeight - 1, FileManagerKeyBindingOptionColor, FileManagerOptionBackgroundColor);
            TextWriterWhereColor.WriteWhere("Exit", 5, ConsoleWrapper.WindowHeight - 1, FileManagerOptionForegroundColor, FileManagerBackgroundColor);
            
            // Wait for key
            while (ConsoleWrapper.ReadKey(true).Key != ConsoleKey.Escape)
                ;

            // Clear the console to clean up
            ColorTools.LoadBack();
        }
    }
}
