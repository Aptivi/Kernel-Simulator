﻿using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Network.Mail.Transfer;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

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

using MimeKit;

namespace KS.Network.Mail.Commands
{
	class Mail_SendEncCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			string Receiver, Subject;
			var Body = new BodyBuilder();

			// Prompt for receiver e-mail address
			TextWriterColor.Write(Translate.DoTranslation("Enter recipient mail address:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
			Receiver = Input.ReadLine();
			DebugWriter.Wdbg(DebugLevel.I, "Recipient: {0}", Receiver);

			// Check for mail format
			if (Receiver.Contains("@") & Receiver.Substring(Receiver.IndexOf("@")).Contains("."))
			{
				DebugWriter.Wdbg(DebugLevel.I, "Mail format satisfied. Contains \"@\" and contains \".\" in the second part after the \"@\" symbol.");

				// Prompt for subject
				TextWriterColor.Write(Translate.DoTranslation("Enter the subject:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
				Subject = Input.ReadLine(false);
				DebugWriter.Wdbg(DebugLevel.I, "Subject: {0} ({1} chars)", Subject, Subject.Length);

				// Prompt for body
				TextWriterColor.Write(Translate.DoTranslation("Enter your message below. Write \"EOF\" to confirm."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
				string BodyLine = "";
				while (!(BodyLine.ToUpper() == "EOF"))
				{
					BodyLine = Input.ReadLine();
					if (!(BodyLine.ToUpper() == "EOF"))
					{
						DebugWriter.Wdbg(DebugLevel.I, "Body line: {0} ({1} chars)", BodyLine, BodyLine.Length);
						Body.TextBody += BodyLine + Kernel.Kernel.NewLine;
						DebugWriter.Wdbg(DebugLevel.I, "Body length: {0} chars", Body.TextBody.Length);
					}
				}

				TextWriterColor.Write(Translate.DoTranslation("Enter file paths to attachments. Press ENTER on a blank path to confirm."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
				string PathLine = " ";
				while (!string.IsNullOrEmpty(PathLine))
				{
					TextWriterColor.Write("> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
					PathLine = Input.ReadLine(false);
					if (!string.IsNullOrEmpty(PathLine))
					{
						PathLine = Filesystem.NeutralizePath(PathLine);
						DebugWriter.Wdbg(DebugLevel.I, "Path line: {0} ({1} chars)", PathLine, PathLine.Length);
						if (Checking.FileExists(PathLine))
						{
							Body.Attachments.Add(PathLine);
						}
					}
				}

				// Send the message
				TextWriterColor.Write(Translate.DoTranslation("Sending message..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Progress));
				if (Conversions.ToBoolean(MailTransfer.MailSendEncryptedMessage(Receiver, Subject, Body.ToMessageBody())))
				{
					DebugWriter.Wdbg(DebugLevel.I, "Message sent.");
					TextWriterColor.Write(Translate.DoTranslation("Message sent."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success));
				}
				else
				{
					DebugWriter.Wdbg(DebugLevel.E, "See debug output to find what's wrong.");
					TextWriterColor.Write(Translate.DoTranslation("Error sending message."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
				}
			}
			else
			{
				DebugWriter.Wdbg(DebugLevel.E, "Mail format unsatisfied." + Receiver);
				TextWriterColor.Write(Translate.DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:") + " john.s@example.com", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
			}
		}

	}
}