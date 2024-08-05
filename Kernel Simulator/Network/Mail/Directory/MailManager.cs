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
using System.Data;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Text;
using KS.ConsoleBase.Writers;
using KS.Misc.Writers.DebugWriters;
using MailKit;
using MimeKit;

namespace KS.Network.Mail.Directory
{
    public static class MailManager
    {

        public static bool ShowPreview;

        /// <summary>
        /// Lists messages
        /// </summary>
        /// <param name="PageNum">Page number</param>
        /// <exception cref="ArgumentException"></exception>
        public static void MailListMessages(int PageNum)
        {
            MailListMessages(PageNum, MailShellCommon.Mail_MaxMessagesInPage);
        }

        /// <summary>
        /// Lists messages
        /// </summary>
        /// <param name="PageNum">Page number</param>
        /// <param name="MessagesInPage">Max messages in one page</param>
        /// <exception cref="ArgumentException"></exception>
        public static void MailListMessages(int PageNum, int MessagesInPage)
        {
            // Sanity checks for the page number
            if (PageNum <= 0)
                PageNum = 1;
            DebugWriter.Wdbg(DebugLevel.I, "Page number {0}", PageNum);

            int MsgsLimitForPg = MessagesInPage;
            int FirstIndex = MsgsLimitForPg * PageNum - 10;
            int LastIndex = MsgsLimitForPg * PageNum - 1;
            int MaxMessagesIndex = MailShellCommon.IMAP_Messages.Count() - 1;
            DebugWriter.Wdbg(DebugLevel.I, "10 messages shown in each page. First message number in page {0} is {1} and last message number in page {0} is {2}", MsgsLimitForPg, FirstIndex, LastIndex);
            for (int i = FirstIndex, loopTo = LastIndex; i <= loopTo; i++)
            {
                if (!(i > MaxMessagesIndex))
                {
                    string MsgFrom = "";
                    string MsgSubject = "";
                    string MsgPreview = "";

                    // Getting information about the message is vital to display them.
                    DebugWriter.Wdbg(DebugLevel.I, "Getting message {0}...", i);
                    lock (MailLogin.IMAP_Client.SyncRoot)
                    {
                        MimeMessage Msg;
                        if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                        {
                            var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                            Msg = Dir.GetMessage(MailShellCommon.IMAP_Messages.ElementAtOrDefault(i), default, MailShellCommon.Mail_Progress);
                        }
                        else
                        {
                            Msg = MailLogin.IMAP_Client.Inbox.GetMessage(MailShellCommon.IMAP_Messages.ElementAtOrDefault(i), default, MailShellCommon.Mail_Progress);
                        }
                        MsgFrom = Msg.From.ToString();
                        MsgSubject = Msg.Subject;
                        MsgPreview = (Msg.GetTextBody(MimeKit.Text.TextFormat.Text) ?? "").Truncate(200);
                    }
                    DebugWriter.Wdbg(DebugLevel.I, "From {0}: {1}", MsgFrom, MsgSubject);

                    // Display them now.
                    TextWriters.Write($"- [{i + 1}/{MaxMessagesIndex + 1}] {MsgFrom}: ", false, KernelColorTools.ColTypes.ListEntry);
                    TextWriters.Write(MsgSubject, true, KernelColorTools.ColTypes.ListValue);
                    if (ShowPreview & !string.IsNullOrWhiteSpace(MsgPreview))
                    {
                        TextWriters.Write(MsgPreview, true, KernelColorTools.ColTypes.ListValue);
                    }
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Reached max message limit. Message number {0}", i);
                }
            }
        }

        /// <summary>
        /// Removes a message
        /// </summary>
        /// <param name="MsgNumber">Message number</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool MailRemoveMessage(int MsgNumber)
        {
            int Message = MsgNumber - 1;
            int MaxMessagesIndex = MailShellCommon.IMAP_Messages.Count() - 1;
            DebugWriter.Wdbg(DebugLevel.I, "Message number {0}", Message);
            if (Message < 0)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Trying to remove message 0 or less than 0.");
                throw new ArgumentException(Translate.DoTranslation("Message number may not be negative or zero."));
            }
            else if (Message > MaxMessagesIndex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", Message, MaxMessagesIndex);
                throw new Kernel.Exceptions.MailException(Translate.DoTranslation("Message specified is not found."));
            }

            lock (MailLogin.IMAP_Client.SyncRoot)
            {
                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                {
                    // Remove message
                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                    DebugWriter.Wdbg(DebugLevel.I, "Opened {0}. Removing {1}...", MailShellCommon.IMAP_CurrentDirectory, MsgNumber);
                    Dir.Store(MailShellCommon.IMAP_Messages.ElementAtOrDefault(Message), new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                    DebugWriter.Wdbg(DebugLevel.I, "Removed.");
                    Dir.Expunge();
                }
                else
                {
                    // Remove message
                    MailLogin.IMAP_Client.Inbox.Open(FolderAccess.ReadWrite);
                    DebugWriter.Wdbg(DebugLevel.I, "Removing {0}...", MsgNumber);
                    MailLogin.IMAP_Client.Inbox.Store(MailShellCommon.IMAP_Messages.ElementAtOrDefault(Message), new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                    DebugWriter.Wdbg(DebugLevel.I, "Removed.");
                    MailLogin.IMAP_Client.Inbox.Expunge();
                }
            }
            return true;
        }

        /// <summary>
        /// Removes all mail that the specified sender has sent
        /// </summary>
        /// <param name="Sender">The sender name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool MailRemoveAllBySender(string Sender)
        {
            DebugWriter.Wdbg(DebugLevel.I, "All mail by {0} will be removed.", Sender);
            int DeletedMsgNumber = 1;
            int SteppedMsgNumber = 0;
            for (int i = 0, loopTo = MailShellCommon.IMAP_Messages.Count(); i <= loopTo; i++)
            {
                try
                {
                    lock (MailLogin.IMAP_Client.SyncRoot)
                    {
                        var MessageId = MailShellCommon.IMAP_Messages.ElementAtOrDefault(i);
                        MimeMessage Msg;
                        if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                        {
                            var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                            Msg = Dir.GetMessage(MessageId, default, MailShellCommon.Mail_Progress);
                        }
                        else
                        {
                            Msg = MailLogin.IMAP_Client.Inbox.GetMessage(MessageId, default, MailShellCommon.Mail_Progress);
                        }
                        SteppedMsgNumber += 1;

                        foreach (var address in Msg.From)
                        {
                            if ((address.Name ?? "") == (Sender ?? ""))
                            {
                                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                                {
                                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);

                                    // Remove message
                                    DebugWriter.Wdbg(DebugLevel.I, "Opened {0}. Removing {1}...", MailShellCommon.IMAP_CurrentDirectory, Sender);
                                    Dir.Store(MessageId, new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                                    DebugWriter.Wdbg(DebugLevel.I, "Removed.");
                                    Dir.Expunge();
                                    DebugWriter.Wdbg(DebugLevel.I, "Message {0} from {1} deleted from {2}. {3} messages remaining to parse.", DeletedMsgNumber, Sender, MailShellCommon.IMAP_CurrentDirectory, MailShellCommon.IMAP_Messages.Count() - SteppedMsgNumber);
                                    TextWriters.Write(Translate.DoTranslation("Message {0} from {1} deleted from {2}. {3} messages remaining to parse."), true, KernelColorTools.ColTypes.Neutral, DeletedMsgNumber, Sender, MailShellCommon.IMAP_CurrentDirectory, MailShellCommon.IMAP_Messages.Count() - SteppedMsgNumber);
                                }
                                else
                                {
                                    // Remove message
                                    MailLogin.IMAP_Client.Inbox.Open(FolderAccess.ReadWrite);
                                    DebugWriter.Wdbg(DebugLevel.I, "Removing {0}...", Sender);
                                    MailLogin.IMAP_Client.Inbox.Store(MessageId, new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                                    DebugWriter.Wdbg(DebugLevel.I, "Removed.");
                                    MailLogin.IMAP_Client.Inbox.Expunge();
                                    DebugWriter.Wdbg(DebugLevel.I, "Message {0} from {1} deleted from inbox. {2} messages remaining to parse.", DeletedMsgNumber, Sender, MailShellCommon.IMAP_Messages.Count() - SteppedMsgNumber);
                                    TextWriters.Write(Translate.DoTranslation("Message {0} from {1} deleted from inbox. {2} messages remaining to parse."), true, KernelColorTools.ColTypes.Neutral, DeletedMsgNumber, Sender, MailShellCommon.IMAP_Messages.Count() - SteppedMsgNumber);
                                }
                                DeletedMsgNumber += 1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Moves a message
        /// </summary>
        /// <param name="MsgNumber">Message number</param>
        /// <param name="TargetFolder">Target folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool MailMoveMessage(int MsgNumber, string TargetFolder)
        {
            int Message = MsgNumber - 1;
            int MaxMessagesIndex = MailShellCommon.IMAP_Messages.Count() - 1;
            DebugWriter.Wdbg(DebugLevel.I, "Message number {0}", Message);
            if (Message < 0)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Trying to move message 0 or less than 0.");
                throw new ArgumentException(Translate.DoTranslation("Message number may not be negative or zero."));
            }
            else if (Message > MaxMessagesIndex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", Message, MaxMessagesIndex);
                throw new Kernel.Exceptions.MailException(Translate.DoTranslation("Message specified is not found."));
            }

            lock (MailLogin.IMAP_Client.SyncRoot)
            {
                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                {
                    // Move message
                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                    var TargetF = MailDirectory.OpenFolder(TargetFolder);
                    DebugWriter.Wdbg(DebugLevel.I, "Opened {0}. Moving {1}...", MailShellCommon.IMAP_CurrentDirectory, MsgNumber);
                    Dir.MoveTo(MailShellCommon.IMAP_Messages.ElementAtOrDefault(Message), TargetF);
                    DebugWriter.Wdbg(DebugLevel.I, "Moved.");
                }
                else
                {
                    // Move message
                    var TargetF = MailDirectory.OpenFolder(TargetFolder);
                    DebugWriter.Wdbg(DebugLevel.I, "Moving {0}...", MsgNumber);
                    MailLogin.IMAP_Client.Inbox.Open(FolderAccess.ReadWrite);
                    MailLogin.IMAP_Client.Inbox.MoveTo(MailShellCommon.IMAP_Messages.ElementAtOrDefault(Message), TargetF);
                    DebugWriter.Wdbg(DebugLevel.I, "Moved.");
                }
            }
            return true;
        }

        /// <summary>
        /// Moves all mail that the specified sender has sent
        /// </summary>
        /// <param name="Sender">The sender name</param>
        /// <param name="TargetFolder">Target folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool MailMoveAllBySender(string Sender, string TargetFolder)
        {
            DebugWriter.Wdbg(DebugLevel.I, "All mail by {0} will be moved.", Sender);
            int DeletedMsgNumber = 1;
            int SteppedMsgNumber = 0;
            for (int i = 0, loopTo = MailShellCommon.IMAP_Messages.Count(); i <= loopTo; i++)
            {
                try
                {
                    lock (MailLogin.IMAP_Client.SyncRoot)
                    {
                        var MessageId = MailShellCommon.IMAP_Messages.ElementAtOrDefault(i);
                        MimeMessage Msg;
                        if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                        {
                            var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                            Msg = Dir.GetMessage(MessageId, default, MailShellCommon.Mail_Progress);
                        }
                        else
                        {
                            Msg = MailLogin.IMAP_Client.Inbox.GetMessage(MessageId, default, MailShellCommon.Mail_Progress);
                        }
                        SteppedMsgNumber += 1;

                        foreach (var address in Msg.From)
                        {
                            if ((address.Name ?? "") == (Sender ?? ""))
                            {
                                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                                {
                                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                                    var TargetF = MailDirectory.OpenFolder(TargetFolder);
                                    // Remove message
                                    DebugWriter.Wdbg(DebugLevel.I, "Opened {0}. Moving {1}...", MailShellCommon.IMAP_CurrentDirectory, Sender);
                                    Dir.MoveTo(MessageId, TargetF);
                                    DebugWriter.Wdbg(DebugLevel.I, "Moved.");
                                    DebugWriter.Wdbg(DebugLevel.I, "Message {0} from {1} moved from {2}. {3} messages remaining to parse.", DeletedMsgNumber, Sender, MailShellCommon.IMAP_CurrentDirectory, MailShellCommon.IMAP_Messages.Count() - SteppedMsgNumber);
                                    TextWriters.Write(Translate.DoTranslation("Message {0} from {1} moved from {2}. {3} messages remaining to parse."), true, KernelColorTools.ColTypes.Neutral, DeletedMsgNumber, Sender, MailShellCommon.IMAP_CurrentDirectory, MailShellCommon.IMAP_Messages.Count() - SteppedMsgNumber);
                                }
                                else
                                {
                                    // Remove message
                                    var TargetF = MailDirectory.OpenFolder(TargetFolder);
                                    DebugWriter.Wdbg(DebugLevel.I, "Moving {0}...", Sender);
                                    MailLogin.IMAP_Client.Inbox.Open(FolderAccess.ReadWrite);
                                    MailLogin.IMAP_Client.Inbox.MoveTo(MessageId, TargetF);
                                    DebugWriter.Wdbg(DebugLevel.I, "Moved.");
                                    DebugWriter.Wdbg(DebugLevel.I, "Message {0} from {1} moved. {2} messages remaining to parse.", DeletedMsgNumber, Sender, MailShellCommon.IMAP_Messages.Count() - SteppedMsgNumber);
                                    TextWriters.Write(Translate.DoTranslation("Message {0} from {1} moved. {2} messages remaining to parse."), true, KernelColorTools.ColTypes.Neutral, DeletedMsgNumber, Sender, MailShellCommon.IMAP_Messages.Count() - SteppedMsgNumber);
                                }
                                DeletedMsgNumber += 1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    return false;
                }
            }
            return true;
        }

    }
}
