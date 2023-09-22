﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Threading;
using KS.ConsoleBase;
using KS.Shell.ShellBase.Commands;
using Nitrocid.Extras.BassBoom.Animations.Lyrics;

namespace Nitrocid.Extras.BassBoom.Commands
{
    /// <summary>
    /// Plays a lyric file
    /// </summary>
    /// <remarks>
    /// This command allows you to play a lyric file by showing you the basic lyrics visualizer.
    /// </remarks>
    class PlayLyricCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            string pathToLyrics = ListArgsOnly[0];

            // Visualize it!
            Lyrics.VisualizeLyric(pathToLyrics);
            Thread.Sleep(10000);
            ConsoleWrapper.CursorVisible = true;
            return 0;
        }

    }
}
