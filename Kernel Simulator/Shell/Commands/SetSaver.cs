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
using KS.Files;

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

using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class SetSaverCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string modPath = Paths.GetKernelPath(KernelPathType.Mods);
            StringArgs = StringArgs.ToLower();
            if (Screensaver.Screensavers.ContainsKey(StringArgs) | CustomSaverTools.CustomSavers.ContainsKey(StringArgs))
            {
                Screensaver.SetDefaultScreensaver(StringArgs);
                TextWriterColor.Write(Translate.DoTranslation("{0} is set to default screensaver."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), StringArgs);
            }
            else if (Checking.FileExists($"{modPath}{StringArgs}") & !Flags.SafeMode)
            {
                Screensaver.SetDefaultScreensaver(StringArgs);
                TextWriterColor.Write(Translate.DoTranslation("{0} is set to default screensaver."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), StringArgs);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Screensaver {0} not found."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), StringArgs);
            }
        }

        public override void HelpHelper()
        {
            if (CustomSaverTools.CustomSavers.Count > 0)
            {
                TextWriterColor.Write(Translate.DoTranslation("where customsaver will be") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), string.Join(", ", CustomSaverTools.CustomSavers.Keys));
            }
            TextWriterColor.Write(Translate.DoTranslation("where builtinsaver will be") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), string.Join(", ", Screensaver.Screensavers.Keys));
        }

    }
}