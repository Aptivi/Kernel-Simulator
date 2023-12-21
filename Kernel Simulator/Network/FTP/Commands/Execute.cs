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
using KS.Shell.ShellBase.Commands;

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

namespace KS.Network.FTP.Commands
{
    class FTP_ExecuteCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (FTPShellCommon.FtpConnected)
            {
                TextWriterColor.Write("<<< C: {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), StringArgs);
                var ExecutedReply = FTPShellCommon.ClientFTP.Execute(StringArgs);
                if (ExecutedReply.Success)
                {
                    TextWriterColor.Write(">>> [{0}] M: {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success), ExecutedReply.Code, ExecutedReply.Message);
                    TextWriterColor.Write(">>> [{0}] I: {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success), ExecutedReply.Code, ExecutedReply.InfoMessages);
                }
                else
                {
                    TextWriterColor.Write(">>> [{0}] M: {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ExecutedReply.Code, ExecutedReply.Message);
                    TextWriterColor.Write(">>> [{0}] I: {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ExecutedReply.Code, ExecutedReply.InfoMessages);
                    TextWriterColor.Write(">>> [{0}] E: {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ExecutedReply.Code, ExecutedReply.ErrorMessage);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You haven't connected to any server yet"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
            }
        }

    }
}