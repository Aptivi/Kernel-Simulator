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

using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Mail.Directory;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Mail.Commands
{
    /// <summary>
    /// Lists all messages in the current folder
    /// </summary>
    /// <remarks>
    /// It allows you to list all the messages in the current working folder in pages. It lists 10 messages in a page, so you can optionally specify the page number.
    /// </remarks>
    class Mail_ListCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length > 0)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Page is numeric? {0}", StringQuery.IsStringNumeric(ListArgsOnly[0]));
                if (StringQuery.IsStringNumeric(ListArgsOnly[0]))
                {
                    MailManager.MailListMessages(Convert.ToInt32(ListArgsOnly[0]));
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Page is not a numeric value."), true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                MailManager.MailListMessages(1);
            }
        }

    }
}
