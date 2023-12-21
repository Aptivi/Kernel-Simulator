﻿using System;
using System.Diagnostics;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

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

using MailKit;
using MailKit.Net.Imap;

namespace KS.Network.Mail
{
	public static class MailHandlers
	{

		/// <summary>
        /// Initializes the CountChanged handlers. Currently, it only supports inbox.
        /// </summary>
		public static void InitializeHandlers()
		{
			MailLogin.IMAP_Client.Inbox.CountChanged += OnCountChanged;
		}

		/// <summary>
        /// Releases the CountChanged handlers. Currently, it only supports inbox.
        /// </summary>
		public static void ReleaseHandlers()
		{
			MailLogin.IMAP_Client.Inbox.CountChanged -= OnCountChanged;
		}

		/// <summary>
        /// Handles WebAlert sent by Gmail
        /// </summary>
		public static void HandleWebAlert(object sender, WebAlertEventArgs e)
		{
			DebugWriter.Wdbg(DebugLevel.I, "WebAlert URI: {0}", e.WebUri.AbsoluteUri);
			TextWriterColor.Write(e.Message, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
			TextWriterColor.Write(Translate.DoTranslation("Opening URL... Make sure to follow the steps shown on the screen."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
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
				Notifications.NotifySend(new Notification(Translate.DoTranslation("{0} new messages arrived in inbox.").FormatString(NewMessagesCount), Translate.DoTranslation("Open \"mail\" to see them."), Notifications.NotifPriority.Medium, Notifications.NotifType.Normal));
			}
		}

	}
}