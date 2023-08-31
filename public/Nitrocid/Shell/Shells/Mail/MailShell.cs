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

using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Network.Base.Connections;
using KS.Network.Mail;
using KS.Network.Mail.Transfer;
using KS.Shell.ShellBase.Shells;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using System.Threading;
using System;
using KS.Kernel.Threading;
using KS.Network.SpeedDial;

namespace KS.Shell.Shells.Mail
{
    /// <summary>
    /// The mail shell
    /// </summary>
    public class MailShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "MailShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            NetworkConnection connection = (NetworkConnection)ShellArgs[0];
            ImapClient imapLink = (ImapClient)((object[])connection.ConnectionInstance)[0];
            SmtpClient smtpLink = (SmtpClient)((object[])connection.ConnectionInstance)[1];
            MailShellCommon.Client = connection;

            // Send ping to keep the connection alive
            var IMAP_NoOp = new KernelThread("IMAP Keep Connection", false, MailPingers.IMAPKeepConnection);
            IMAP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about IMAPKeepConnection()");
            var SMTP_NoOp = new KernelThread("SMTP Keep Connection", false, MailPingers.SMTPKeepConnection);
            SMTP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about SMTPKeepConnection()");

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(connection.ConnectionUri.AbsoluteUri, connection.ConnectionUri.Port, NetworkConnectionType.Mail, false, MailLogin.Mail_Authentication.UserName);

            while (!Bail)
            {
                try
                {
                    // Populate messages
                    MailTransfer.PopulateMessages();
                    if (MailShellCommon.Mail_NotifyNewMail)
                        MailHandlers.InitializeHandlers();

                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.HTTPShell, Translate.DoTranslation("There was an error in the HTTP shell:") + " {0}", ex, ex.Message);
                }

                // Exiting, so reset the site
                if (Bail)
                {
                    MailShellCommon.IMAP_CurrentDirectory = "Inbox";
                    if (!detaching)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Exit requested. Disconnecting host...");
                        if (MailShellCommon.Mail_NotifyNewMail)
                            MailHandlers.ReleaseHandlers();
                        IMAP_NoOp.Stop();
                        SMTP_NoOp.Stop();
                        imapLink.Disconnect(true);
                        smtpLink.Disconnect(true);
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(MailShellCommon.Client);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        MailShellCommon.Client = null;
                    }
                    detaching = false;
                }
            }
        }

    }
}
