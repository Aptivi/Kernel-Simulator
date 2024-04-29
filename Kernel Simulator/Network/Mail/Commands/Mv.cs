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
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Network.Mail.Directory;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Network.Mail.Commands
{
    class Mail_MvCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Message number is numeric? {0}", StringQuery.IsStringNumeric(ListArgs[0]));
            if (StringQuery.IsStringNumeric(ListArgs[0]))
            {
                MailManager.MailMoveMessage(Convert.ToInt32(ListArgs[0]), ListArgs[1]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Message number is not a numeric value."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
            }
        }

    }
}
