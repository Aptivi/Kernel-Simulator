﻿using System;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Network.RSS;
using KS.Network.RSS.Instance;

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

using KS.Shell.Prompts;
using KS.Shell.ShellBase;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells
{
	public class RSSShell : ShellExecutor, IShell
	{

		public override ShellType ShellType
		{
			get
			{
				return ShellType.RSSShell;
			}
		}

		public override bool Bail { get; set; }

		public override void InitializeShell(params object[] ShellArgs)
		{
			// Handle the RSS feed link provided by user
			bool BailFromEnter = false;
			string OldRSSFeedLink = "";
			string FeedUrl = "";
			if (ShellArgs.Length > 0)
			{
				FeedUrl = Convert.ToString(ShellArgs[0]);
			}
			RSSShellCommon.RSSFeedLink = FeedUrl;

			while (!BailFromEnter)
			{
				if (string.IsNullOrWhiteSpace(RSSShellCommon.RSSFeedLink))
				{
					while (string.IsNullOrWhiteSpace(RSSShellCommon.RSSFeedLink))
					{
						try
						{
							if (!string.IsNullOrWhiteSpace(RSSShellCommon.RSSFeedUrlPromptStyle))
							{
								TextWriterColor.Write(PlaceParse.ProbePlaces(RSSShellCommon.RSSFeedUrlPromptStyle), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
							}
							else
							{
								TextWriterColor.Write(Translate.DoTranslation("Enter an RSS feed URL:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
							}
							RSSShellCommon.RSSFeedLink = Input.ReadLine();

							// The user entered the feed URL
							RSSShellCommon.RSSFeedInstance = new RSSFeed(RSSShellCommon.RSSFeedLink, RSSFeedType.Infer);
							RSSShellCommon.RSSFeedLink = RSSShellCommon.RSSFeedInstance.FeedUrl;
							OldRSSFeedLink = RSSShellCommon.RSSFeedLink;
							BailFromEnter = true;
						}
						catch (ThreadInterruptedException taex)
						{
							Flags.CancelRequested = false;
							BailFromEnter = true;
							Bail = true;
						}
						catch (Exception ex)
						{
							DebugWriter.Wdbg(DebugLevel.E, "Failed to parse RSS feed URL {0}: {1}", FeedUrl, ex.Message);
							DebugWriter.WStkTrc(ex);
							TextWriterColor.Write(Translate.DoTranslation("Failed to parse feed URL:") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
							RSSShellCommon.RSSFeedLink = "";
						}
					}
				}
				else
				{
					// Make a new RSS feed instance
					try
					{
						if ((OldRSSFeedLink ?? "") != (RSSShellCommon.RSSFeedLink ?? ""))
						{
							if (RSSShellCommon.RSSFeedLink == "select")
							{
								RSSTools.OpenFeedSelector();
							}
							RSSShellCommon.RSSFeedInstance = new RSSFeed(RSSShellCommon.RSSFeedLink, RSSFeedType.Infer);
							RSSShellCommon.RSSFeedLink = RSSShellCommon.RSSFeedInstance.FeedUrl;
						}
						OldRSSFeedLink = RSSShellCommon.RSSFeedLink;
						BailFromEnter = true;
					}
					catch (ThreadInterruptedException taex)
					{
						Flags.CancelRequested = false;
						BailFromEnter = true;
						Bail = true;
					}
					catch (Exception ex)
					{
						DebugWriter.Wdbg(DebugLevel.E, "Failed to parse RSS feed URL {0}: {1}", RSSShellCommon.RSSFeedLink, ex.Message);
						DebugWriter.WStkTrc(ex);
						TextWriterColor.Write(Translate.DoTranslation("Failed to parse feed URL:") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
						RSSShellCommon.RSSFeedLink = "";
					}
				}
			}

			while (!Bail)
			{
				try
				{
					// Send ping to keep the connection alive
					if (!RSSShellCommon.RSSKeepAlive & !RSSShellCommon.RSSRefresher.IsAlive & RSSShellCommon.RSSRefreshFeeds)
						RSSShellCommon.RSSRefresher.Start();
					DebugWriter.Wdbg(DebugLevel.I, "Made new thread about RefreshFeeds()");

					// See UESHShell.vb for more info
					lock (CancellationHandlers.GetCancelSyncLock(ShellType))
					{
						// Prepare for prompt
						if (Kernel.Kernel.DefConsoleOut is not null)
						{
							Console.SetOut(Kernel.Kernel.DefConsoleOut);
						}
						PromptPresetManager.WriteShellPrompt(ShellType);

						// Raise the event
						Kernel.Kernel.KernelEventManager.RaiseRSSShellInitialized(RSSShellCommon.RSSFeedLink);
					}

					// Prompt for command
					string WrittenCommand = Input.ReadLine();
					if ((string.IsNullOrEmpty(WrittenCommand) | (WrittenCommand?.StartsWithAnyOf([" ", "#"]))) == false)
					{
						Kernel.Kernel.KernelEventManager.RaiseRSSPreExecuteCommand(RSSShellCommon.RSSFeedLink, WrittenCommand);
						Shell.GetLine(WrittenCommand, false, "", ShellType.RSSShell);
						Kernel.Kernel.KernelEventManager.RaiseRSSPostExecuteCommand(RSSShellCommon.RSSFeedLink, WrittenCommand);
					}
				}
				catch (ThreadInterruptedException taex)
				{
					Flags.CancelRequested = false;
					Bail = true;
				}
				catch (Exception ex)
				{
					DebugWriter.WStkTrc(ex);
					TextWriterColor.Write(Translate.DoTranslation("There was an error in the shell.") + Kernel.Kernel.NewLine + "Error {0}: {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.GetType().FullName, ex.Message);
					continue;
				}
			}

			// Disconnect the session
			if (RSSShellCommon.RSSKeepAlive)
			{
				DebugWriter.Wdbg(DebugLevel.W, "Exit requested, but not disconnecting.");
			}
			else
			{
				DebugWriter.Wdbg(DebugLevel.W, "Exit requested. Disconnecting host...");
				if (RSSShellCommon.RSSRefreshFeeds)
					RSSShellCommon.RSSRefresher.Stop();
				RSSShellCommon.RSSFeedLink = "";
				RSSShellCommon.RSSFeedInstance = null;
			}
		}

	}
}