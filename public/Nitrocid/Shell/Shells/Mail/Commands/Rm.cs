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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Network.Mail.Directory;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Mail.Commands
{
    /// <summary>
    /// Removes a mail
    /// </summary>
    /// <remarks>
    /// If you no longer want a message in your mail account, use this command to remove a mail permanently.
    /// </remarks>
    class RmCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Message number is numeric? {0}", TextTools.IsStringNumeric(parameters.ArgumentsList[0]));
            if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
            {
                MailManager.MailRemoveMessage(Convert.ToInt32(parameters.ArgumentsList[0]));
                return 0;
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Message number is not a numeric value."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Mail;
            }
        }

    }
}
