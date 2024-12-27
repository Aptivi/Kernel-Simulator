﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can check your file to see if it's locked
    /// </summary>
    /// <remarks>
    /// If you want to know that your file is locked, you can point this command to your file.
    /// </remarks>
    class ChkLockCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = parameters.ArgumentsList[0];
            bool locked = FilesystemTools.IsLocked(path);
            bool waitForUnlock = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-waitforunlock");
            string waitForUnlockMsStr = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-waitforunlock");
            bool waitForUnlockTimed = !string.IsNullOrEmpty(waitForUnlockMsStr);
            int waitForUnlockMs = waitForUnlockTimed ? int.Parse(waitForUnlockMsStr) : 0;
            if (locked)
            {
                TextWriters.Write(Translate.DoTranslation("File or folder is already in use."), true, KernelColorType.Warning);
                if (waitForUnlock)
                {
                    TextWriters.Write(Translate.DoTranslation("Waiting until the file or the folder is unlocked..."), true, KernelColorType.Progress);
                    if (waitForUnlockTimed)
                        FilesystemTools.WaitForLockRelease(path, waitForUnlockMs);
                    else
                        FilesystemTools.WaitForLockReleaseIndefinite(path);
                    TextWriters.Write(Translate.DoTranslation("File or folder is not in use."), true, KernelColorType.Success);
                    return 0;
                }
                else
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            else
                TextWriters.Write(Translate.DoTranslation("File or folder is not in use."), true, KernelColorType.Success);
            return 0;
        }
    }
}
