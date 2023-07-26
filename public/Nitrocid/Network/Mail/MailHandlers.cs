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

using System;
using System.Diagnostics;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Text;
using KS.Shell.Shells.Mail;
using MailKit;
using MailKit.Net.Imap;

namespace KS.Network.Mail
{
    /// <summary>
    /// Mail event handlers
    /// </summary>
    public static class MailHandlers
    {

        /// <summary>
        /// Initializes the CountChanged handlers. Currently, it only supports inbox.
        /// </summary>
        public static void InitializeHandlers() => ((ImapClient)((object[])MailShellCommon.Client.ConnectionInstance)[0]).Inbox.CountChanged += OnCountChanged;

        /// <summary>
        /// Releases the CountChanged handlers. Currently, it only supports inbox.
        /// </summary>
        public static void ReleaseHandlers() => ((ImapClient)((object[])MailShellCommon.Client.ConnectionInstance)[0]).Inbox.CountChanged -= OnCountChanged;

        /// <summary>
        /// Handles WebAlert sent by Gmail
        /// </summary>
        public static void HandleWebAlert(object sender, WebAlertEventArgs e)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "WebAlert URI: {0}", e.WebUri.AbsoluteUri);
            TextWriterColor.Write(e.Message, true, KernelColorType.Warning);
            TextWriterColor.Write(Translate.DoTranslation("Opening URL... Make sure to follow the steps shown on the screen."));
            Process.Start(e.WebUri.AbsoluteUri).WaitForExit();
        }

        /// <summary>
        /// Executed when the CountChanged event is fired.
        /// </summary>
        /// <param name="Sender">A folder</param>
        /// <param name="e">Event arguments</param>
        public static void OnCountChanged(object Sender, EventArgs e)
        {
            ImapFolder Folder = (ImapFolder)Sender;
            if (Folder.Count > MailShellCommon.IMAP_Messages.Count())
            {
                int NewMessagesCount = Folder.Count - MailShellCommon.IMAP_Messages.Count();
                NotificationManager.NotifySend(new Notification(TextTools.FormatString(Translate.DoTranslation("{0} new messages arrived in inbox."), NewMessagesCount), Translate.DoTranslation("Open \"mail\" to see them."), NotificationManager.NotifPriority.Medium, NotificationManager.NotifType.Normal));
            }
        }

    }
}
