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

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Configuration;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class SettingsCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var SettingsType = Misc.Configuration.SettingsType.Normal;
            if (ListSwitchesOnly.Length > 0)
            {
                if (ListSwitchesOnly[0] == "-saver")
                    SettingsType = Misc.Configuration.SettingsType.Screensaver;
                if (ListSwitchesOnly[0] == "-splash")
                    SettingsType = Misc.Configuration.SettingsType.Splash;
            }
            SettingsApp.OpenMainPage(SettingsType);
        }

        public override void HelpHelper()
        {
            TextWriters.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, KernelColorTools.ColTypes.Neutral);
            TextWriters.Write("  -saver: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Opens the screensaver settings"), true, KernelColorTools.ColTypes.ListValue);
            TextWriters.Write("  -splash: ", false, KernelColorTools.ColTypes.ListEntry);
            TextWriters.Write(Translate.DoTranslation("Opens the splash settings"), true, KernelColorTools.ColTypes.ListValue);
        }

    }
}