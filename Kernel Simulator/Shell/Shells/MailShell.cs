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

using System;
using KS.ConsoleBase.Inputs;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;
using KS.Network.Mail;
using KS.Network.Mail.Transfer;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells
{
    public class MailShell : ShellExecutor, IShell
    {

        public override ShellType ShellType => ShellType.MailShell;

        public override bool Bail { get; set; }

        public override void InitializeShell(params object[] ShellArgs)
        {
            // Send ping to keep the connection alive
            var IMAP_NoOp = new KernelThread("IMAP Keep Connection", false, MailPingers.IMAPKeepConnection);
            IMAP_NoOp.Start();
            DebugWriter.Wdbg(DebugLevel.I, "Made new thread about IMAPKeepConnection()");
            var SMTP_NoOp = new KernelThread("SMTP Keep Connection", false, MailPingers.SMTPKeepConnection);
            SMTP_NoOp.Start();
            DebugWriter.Wdbg(DebugLevel.I, "Made new thread about SMTPKeepConnection()");
            Kernel.Kernel.KernelEventManager.RaiseIMAPShellInitialized();

            while (!Bail)
            {
                // Populate messages
                MailTransfer.PopulateMessages();
                if (MailShellCommon.Mail_NotifyNewMail)
                    MailHandlers.InitializeHandlers();

                // See UESHShell.vb for more info
                lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                {
                    // Initialize prompt
                    if (Kernel.Kernel.DefConsoleOut is not null)
                    {
                        Console.SetOut(Kernel.Kernel.DefConsoleOut);
                    }
                    PromptPresetManager.WriteShellPrompt(ShellType);
                }

                // Listen for a command
                string cmd = Input.ReadLine();
                if ((string.IsNullOrEmpty(cmd) | (cmd?.StartsWithAnyOf([" ", "#"]))) == false)
                {
                    Kernel.Kernel.KernelEventManager.RaiseIMAPPreExecuteCommand(cmd);
                    Shell.GetLine(cmd, false, "", ShellType.MailShell);
                    Kernel.Kernel.KernelEventManager.RaiseIMAPPostExecuteCommand(cmd);
                }
            }

            // Disconnect the session
            MailShellCommon.IMAP_CurrentDirectory = "Inbox";
            if (MailShellCommon.KeepAlive)
            {
                DebugWriter.Wdbg(DebugLevel.W, "Exit requested, but not disconnecting.");
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.W, "Exit requested. Disconnecting host...");
                if (MailShellCommon.Mail_NotifyNewMail)
                    MailHandlers.ReleaseHandlers();
                MailLogin.IMAP_Client.Disconnect(true);
                MailLogin.SMTP_Client.Disconnect(true);
            }
        }

    }
}