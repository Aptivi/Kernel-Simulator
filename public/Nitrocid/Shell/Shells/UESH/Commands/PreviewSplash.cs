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

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Misc.Splash;
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
            bool splashOut = SwitchManager.ContainsSwitch(ListSwitchesOnly, "-splashout");
            if (!(ListArgsOnly.Length == 0))
                SplashManager.PreviewSplash(ListArgsOnly[0], splashOut);
            else
                SplashManager.PreviewSplash(splashOut);
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("Available splashes:"));
            ListWriterColor.WriteList(SplashManager.Splashes.Keys);
        }

    }
}
