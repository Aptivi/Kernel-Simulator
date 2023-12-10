﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Kernel.Time.Renderers;
using KS.Languages;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using MimeKit.Cryptography;
using MimeKit.Text;
using Nitrocid.Extras.MailShell.Mail;
using Nitrocid.Extras.MailShell.Tools.Directory;
using Nitrocid.Extras.MailShell.Tools.PGP;

namespace Nitrocid.Extras.MailShell.Tools.Transfer
{
    /// <summary>
    /// Mail transfer module
    /// </summary>
    public static class MailTransfer
    {

        /// <summary>
        /// Prints content of message to console
        /// </summary>
        /// <param name="MessageNum">Message number</param>
        /// <param name="Decrypt">Whether to decrypt messages or not</param>
        public static void MailPrintMessage(int MessageNum, bool Decrypt = false)
        {
            int Message = MessageNum - 1;
            int MaxMessagesIndex = MailShellCommon.IMAP_Messages.Count() - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Message number {0}", Message);
            if (Message < 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Trying to access message 0 or less than 0.");
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Message number may not be negative or zero."), true, KernelColorType.Error);
                return;
            }
            else if (Message > MaxMessagesIndex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", Message, MaxMessagesIndex);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Message specified is not found."), true, KernelColorType.Error);
                return;
            }

            lock (((ImapClient)((object[])MailShellCommon.Client.ConnectionInstance)[0]).SyncRoot)
            {
                // Get message
                DebugWriter.WriteDebug(DebugLevel.I, "Getting message...");
                MimeMessage Msg;
                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                {
                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                    Msg = Dir.GetMessage(MailShellCommon.IMAP_Messages.ElementAtOrDefault(Message), default, MailShellCommon.Progress);
                }
                else
                {
                    Msg = ((ImapClient)((object[])MailShellCommon.Client.ConnectionInstance)[0]).Inbox.GetMessage(MailShellCommon.IMAP_Messages.ElementAtOrDefault(Message), default, MailShellCommon.Progress);
                }

                // Prepare view
                TextWriterColor.Write();

                // Print all the addresses that sent the mail
                DebugWriter.WriteDebug(DebugLevel.I, "{0} senders.", Msg.From.Count);
                foreach (InternetAddress Address in Msg.From)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Address: {0} ({1})", Address.Name, Address.Encoding.EncodingName);
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("- From {0}"), true, KernelColorType.ListEntry, Address.ToString());
                }

                // Print all the addresses that received the mail
                DebugWriter.WriteDebug(DebugLevel.I, "{0} receivers.", Msg.To.Count);
                foreach (InternetAddress Address in Msg.To)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Address: {0} ({1})", Address.Name, Address.Encoding.EncodingName);
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("- To {0}"), true, KernelColorType.ListEntry, Address.ToString());
                }

                // Print the date and time when the user received the mail
                DebugWriter.WriteDebug(DebugLevel.I, "Rendering time and date of {0}.", Msg.Date.DateTime.ToString());
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("- Sent at {0} in {1}"), true, KernelColorType.ListEntry, TimeDateRenderers.RenderTime(Msg.Date.DateTime), TimeDateRenderers.RenderDate(Msg.Date.DateTime));

                // Prepare subject
                TextWriterColor.Write();
                DebugWriter.WriteDebug(DebugLevel.I, "Subject length: {0}, {1}", Msg.Subject.Length, Msg.Subject);
                TextWriterColor.WriteKernelColor($"- {Msg.Subject}", false, KernelColorType.ListEntry);

                // Write a sign after the subject if attachments are found
                DebugWriter.WriteDebug(DebugLevel.I, "Attachments count: {0}", Msg.Attachments.Count());
                if (Msg.Attachments.Any())
                {
                    TextWriterColor.WriteKernelColor(" - [*]", true, KernelColorType.ListEntry);
                }
                else
                {
                    TextWriterColor.Write();
                }

                // Prepare body
                TextWriterColor.Write();
                DebugWriter.WriteDebug(DebugLevel.I, "Displaying body...");
                var DecryptedMessage = default(Dictionary<string, MimeEntity>);
                DebugWriter.WriteDebug(DebugLevel.I, "To decrypt: {0}", Decrypt);
                if (Decrypt)
                {
                    DecryptedMessage = DecryptMessage(Msg);
                    DebugWriter.WriteDebug(DebugLevel.I, "Decrypted messages length: {0}", DecryptedMessage.Count);
                    var DecryptedEntity = DecryptedMessage["Body"];
                    var DecryptedStream = new MemoryStream();
                    DebugWriter.WriteDebug(DebugLevel.I, $"Decrypted message type: {(DecryptedEntity is Multipart ? "Multipart" : "Singlepart")}");
                    if (DecryptedEntity is Multipart)
                    {
                        Multipart MultiEntity = (Multipart)DecryptedEntity;
                        DebugWriter.WriteDebug(DebugLevel.I, $"Decrypted message entity is {(MultiEntity is not null ? "multipart" : "nothing")}");
                        if (MultiEntity is not null)
                        {
                            for (int EntityNumber = 0; EntityNumber <= MultiEntity.Count - 1; EntityNumber++)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, $"Entity number {EntityNumber} is {(MultiEntity[EntityNumber].IsAttachment ? "an attachment" : "not an attachment")}");
                                if (!MultiEntity[EntityNumber].IsAttachment)
                                {
                                    MultiEntity[EntityNumber].WriteTo(DecryptedStream, true);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Written {0} bytes to stream.", DecryptedStream.Length);
                                    DecryptedStream.Position = 0L;
                                    var DecryptedByte = new byte[(int)(DecryptedStream.Length + 1)];
                                    DecryptedStream.Read(DecryptedByte, 0, (int)DecryptedStream.Length);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Written {0} bytes to buffer.", DecryptedByte.Length);
                                    TextWriterColor.WriteKernelColor(Encoding.Default.GetString(DecryptedByte), true, KernelColorType.ListValue);
                                }
                            }
                        }
                    }
                    else
                    {
                        DecryptedEntity.WriteTo(DecryptedStream, true);
                        DebugWriter.WriteDebug(DebugLevel.I, "Written {0} bytes to stream.", DecryptedStream.Length);
                        DecryptedStream.Position = 0L;
                        var DecryptedByte = new byte[(int)(DecryptedStream.Length + 1)];
                        DecryptedStream.Read(DecryptedByte, 0, (int)DecryptedStream.Length);
                        DebugWriter.WriteDebug(DebugLevel.I, "Written {0} bytes to buffer.", DecryptedByte.Length);
                        TextWriterColor.WriteKernelColor(Encoding.Default.GetString(DecryptedByte), true, KernelColorType.ListValue);
                    }
                }
                else
                {
                    TextWriterColor.WriteKernelColor(Msg.GetTextBody(MailShellCommon.TextFormat), true, KernelColorType.ListValue);
                }
                TextWriterColor.Write();

                // Populate attachments
                if (Msg.Attachments.Any())
                {
                    TextWriterColor.Write(Translate.DoTranslation("Attachments:"));
                    var AttachmentEntities = new List<MimeEntity>();
                    if (Decrypt)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Parsing attachments...");
                        for (int DecryptedEntityNumber = 0; DecryptedEntityNumber <= DecryptedMessage.Count - 1; DecryptedEntityNumber++)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Is entity number {0} an attachment? {1}", DecryptedEntityNumber, DecryptedMessage.Keys.ElementAtOrDefault(DecryptedEntityNumber).Contains("Attachment"));
                            DebugWriter.WriteDebug(DebugLevel.I, "Is entity number {0} a body that is a multipart? {1}", DecryptedEntityNumber, DecryptedMessage.Keys.ElementAtOrDefault(DecryptedEntityNumber) == "Body" & DecryptedMessage["Body"] is Multipart);
                            if (DecryptedMessage.Keys.ElementAtOrDefault(DecryptedEntityNumber).Contains("Attachment"))
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Adding entity {0} to attachment entities...", DecryptedEntityNumber);
                                AttachmentEntities.Add(DecryptedMessage.Values.ElementAtOrDefault(DecryptedEntityNumber));
                            }
                            else if (DecryptedMessage.Keys.ElementAtOrDefault(DecryptedEntityNumber) == "Body" & DecryptedMessage["Body"] is Multipart)
                            {
                                Multipart MultiEntity = (Multipart)DecryptedMessage["Body"];
                                DebugWriter.WriteDebug(DebugLevel.I, $"Decrypted message entity is {(MultiEntity is not null ? "multipart" : "nothing")}");
                                if (MultiEntity is not null)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "{0} entities found.", MultiEntity.Count);
                                    for (int EntityNumber = 0; EntityNumber <= MultiEntity.Count - 1; EntityNumber++)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, $"Entity number {EntityNumber} is {(MultiEntity[EntityNumber].IsAttachment ? "an attachment" : "not an attachment")}");
                                        if (MultiEntity[EntityNumber].IsAttachment)
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.I, "Adding entity {0} to attachment list...", EntityNumber);
                                            AttachmentEntities.Add(MultiEntity[EntityNumber]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        AttachmentEntities = (List<MimeEntity>)Msg.Attachments;
                    }
                    foreach (MimeEntity Attachment in AttachmentEntities)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Attachment ID: {0}", Attachment.ContentId);
                        if (Attachment is MessagePart)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Attachment is a message.");
                            TextWriterColor.Write($"- {Attachment.ContentDisposition?.FileName}");
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Attachment is a file.");
                            MimePart AttachmentPart = (MimePart)Attachment;
                            TextWriterColor.Write($"- {AttachmentPart.FileName}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts a message
        /// </summary>
        /// <param name="Text">Text part</param>
        /// <returns>A decrypted message, or null if unsuccessful.</returns>
        public static Dictionary<string, MimeEntity> DecryptMessage(MimeMessage Text)
        {
            var EncryptedDict = new Dictionary<string, MimeEntity>();
            DebugWriter.WriteDebug(DebugLevel.I, $"Encrypted message type: {(Text.Body is MultipartEncrypted ? "Multipart" : "Singlepart")}");
            if (Text.Body is MultipartEncrypted)
            {
                MultipartEncrypted Encrypted = (MultipartEncrypted)Text.Body;
                DebugWriter.WriteDebug(DebugLevel.I, $"Message type: {(Encrypted is not null ? "MultipartEncrypted" : "Nothing")}");
                DebugWriter.WriteDebug(DebugLevel.I, "Decrypting...");
                EncryptedDict.Add("Body", Encrypted.Decrypt(new PGPContext()));
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to decrypt plain text. Returning body...");
                EncryptedDict.Add("Body", Text.Body);
            }
            int AttachmentNumber = 1;
            foreach (MimeEntity TextAttachment in Text.Attachments)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Attachment number {0}", AttachmentNumber);
                DebugWriter.WriteDebug(DebugLevel.I, $"Encrypted attachment type: {(TextAttachment is MultipartEncrypted ? "Multipart" : "Singlepart")}");
                if (TextAttachment is MultipartEncrypted)
                {
                    MultipartEncrypted Encrypted = (MultipartEncrypted)TextAttachment;
                    DebugWriter.WriteDebug(DebugLevel.I, $"Attachment type: {(Encrypted is not null ? "MultipartEncrypted" : "Nothing")}");
                    DebugWriter.WriteDebug(DebugLevel.I, "Decrypting...");
                    EncryptedDict.Add("Attachment " + AttachmentNumber, Encrypted.Decrypt(new PGPContext()));
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Trying to decrypt plain attachment. Returning body...");
                    EncryptedDict.Add("Attachment " + AttachmentNumber, TextAttachment);
                }
                AttachmentNumber += 1;
            }
            return EncryptedDict;
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body (only text. See <see cref="MailSendMessage(string, string, MimeEntity)"/> for more.)</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool MailSendMessage(string Recipient, string Subject, string Body)
        {
            // Construct a message
            var FinalMessage = new MimeMessage();
            FinalMessage.From.Add(MailboxAddress.Parse(MailLogin.Authentication.UserName));
            DebugWriter.WriteDebug(DebugLevel.I, "Added sender to FinalMessage.From.");
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient));
            DebugWriter.WriteDebug(DebugLevel.I, "Added address to FinalMessage.To.");
            FinalMessage.Subject = Subject;
            DebugWriter.WriteDebug(DebugLevel.I, "Added subject to FinalMessage.Subject.");
            FinalMessage.Body = new TextPart(TextFormat.Plain) { Text = Body };
            DebugWriter.WriteDebug(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...");

            // Send the message
            lock (((SmtpClient)((object[])MailShellCommon.Client.ConnectionInstance)[1]).SyncRoot)
            {
                try
                {
                    ((SmtpClient)((object[])MailShellCommon.Client.ConnectionInstance)[1]).Send(FinalMessage, default, MailShellCommon.Progress);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to send message: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// Sends a message with advanced features like attachments
        /// </summary>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool MailSendMessage(string Recipient, string Subject, MimeEntity Body)
        {
            // Construct a message
            var FinalMessage = new MimeMessage();
            FinalMessage.From.Add(MailboxAddress.Parse(MailLogin.Authentication.UserName));
            DebugWriter.WriteDebug(DebugLevel.I, "Added sender to FinalMessage.From.");
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient));
            DebugWriter.WriteDebug(DebugLevel.I, "Added address to FinalMessage.To.");
            FinalMessage.Subject = Subject;
            DebugWriter.WriteDebug(DebugLevel.I, "Added subject to FinalMessage.Subject.");
            FinalMessage.Body = Body;
            DebugWriter.WriteDebug(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...");

            // Send the message
            lock (((SmtpClient)((object[])MailShellCommon.Client.ConnectionInstance)[1]).SyncRoot)
            {
                try
                {
                    ((SmtpClient)((object[])MailShellCommon.Client.ConnectionInstance)[1]).Send(FinalMessage, default, MailShellCommon.Progress);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to send message: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// Sends an encrypted message with advanced features like attachments
        /// </summary>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool MailSendEncryptedMessage(string Recipient, string Subject, MimeEntity Body)
        {
            // Construct a message
            var FinalMessage = new MimeMessage();
            FinalMessage.From.Add(MailboxAddress.Parse(MailLogin.Authentication.UserName));
            DebugWriter.WriteDebug(DebugLevel.I, "Added sender to FinalMessage.From.");
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient));
            DebugWriter.WriteDebug(DebugLevel.I, "Added address to FinalMessage.To.");
            FinalMessage.Subject = Subject;
            DebugWriter.WriteDebug(DebugLevel.I, "Added subject to FinalMessage.Subject.");
            FinalMessage.Body = MultipartEncrypted.Encrypt(new PGPContext(), FinalMessage.To.Mailboxes, Body);
            DebugWriter.WriteDebug(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...");

            // Send the message
            lock (((SmtpClient)((object[])MailShellCommon.Client.ConnectionInstance)[1]).SyncRoot)
            {
                try
                {
                    ((SmtpClient)((object[])MailShellCommon.Client.ConnectionInstance)[1]).Send(FinalMessage, default, MailShellCommon.Progress);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to send message: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// Populates e-mail messages
        /// </summary>
        public static void PopulateMessages()
        {
            if (((ImapClient)((object[])MailShellCommon.Client.ConnectionInstance)[0]).IsConnected)
            {
                lock (((ImapClient)((object[])MailShellCommon.Client.ConnectionInstance)[0]).SyncRoot)
                {
                    if (string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) | MailShellCommon.IMAP_CurrentDirectory == "Inbox")
                    {
                        ((ImapClient)((object[])MailShellCommon.Client.ConnectionInstance)[0]).Inbox.Open(FolderAccess.ReadWrite);
                        DebugWriter.WriteDebug(DebugLevel.I, "Opened inbox");
                        MailShellCommon.IMAP_Messages = ((ImapClient)((object[])MailShellCommon.Client.ConnectionInstance)[0]).Inbox.Search(SearchQuery.All).Reverse();
                        DebugWriter.WriteDebug(DebugLevel.I, "Messages count: {0} messages", MailShellCommon.IMAP_Messages.LongCount());
                    }
                    else
                    {
                        var Folder = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                        DebugWriter.WriteDebug(DebugLevel.I, "Opened {0}", MailShellCommon.IMAP_CurrentDirectory);
                        MailShellCommon.IMAP_Messages = Folder.Search(SearchQuery.All).Reverse();
                        DebugWriter.WriteDebug(DebugLevel.I, "Messages count: {0} messages", MailShellCommon.IMAP_Messages.LongCount());
                    }
                }
            }
        }

    }
}
