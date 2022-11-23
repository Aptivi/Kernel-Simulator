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
using KS.Misc.Splash;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Previews the splash
    /// </summary>
    /// <remarks>
    /// This command previews either the current splash as set in the kernel settings or the specified splash. Refer the current splash list found in <see cref="Misc.Splash.SplashManager.Splashes"/>.
    /// </remarks>
    class PreviewSplashCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!(ListArgsOnly.Length == 0))
                SplashManager.PreviewSplash(ListArgsOnly[0]);
            else
                SplashManager.PreviewSplash();
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("Available splashes:"));
            ListWriterColor.WriteList(SplashManager.Splashes.Keys);
        }

    }
}
