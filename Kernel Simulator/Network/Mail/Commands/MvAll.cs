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
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Mail.Directory;
using KS.Shell.ShellBase.Commands;

namespace KS.Network.Mail.Commands
{
    class Mail_MvAllCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (MailManager.MailMoveAllBySender(ListArgs[0], ListArgs[1]))
            {
                TextWriterColor.Write(Translate.DoTranslation("All mail made by {0} are moved successfully."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success), ListArgs[0]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Failed to move all mail made by {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ListArgs[0]);
            }
        }

    }
}