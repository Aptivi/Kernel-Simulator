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

using System;
using System.Threading;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.RSS;
using KS.Network.RSS.Instance;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.RSS
{
    /// <summary>
    /// The RSS shell
    /// </summary>
    public class RSSShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "RSSShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
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
                                TextWriterColor.Write(PlaceParse.ProbePlaces(RSSShellCommon.RSSFeedUrlPromptStyle), false, ColorTools.ColTypes.Input);
                            }
                            else
                            {
                                TextWriterColor.Write(Translate.DoTranslation("Enter an RSS feed URL:") + " ", false, ColorTools.ColTypes.Input);
                            }
                            RSSShellCommon.RSSFeedLink = Input.ReadLine();
                            if (ReadLineReboot.ReadLine.ReadRanToCompletion)
                            {
                                // The user entered the feed URL
                                RSSShellCommon.RSSFeedInstance = new RSSFeed(RSSShellCommon.RSSFeedLink, RSSFeedType.Infer);
                                RSSShellCommon.RSSFeedLink = RSSShellCommon.RSSFeedInstance.FeedUrl;
                                OldRSSFeedLink = RSSShellCommon.RSSFeedLink;
                            }
                            else
                            {
                                // We're aborting.
                                Bail = true;
                            }
                            BailFromEnter = true;
                        }
                        catch (ThreadInterruptedException)
                        {
                            Flags.CancelRequested = false;
                            BailFromEnter = true;
                            Bail = true;
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse RSS feed URL {0}: {1}", FeedUrl, ex.Message);
                            DebugWriter.WriteDebugStackTrace(ex);
                            TextWriterColor.Write(Translate.DoTranslation("Failed to parse feed URL:") + " {0}", true, ColorTools.ColTypes.Error, ex.Message);
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
                    catch (ThreadInterruptedException)
                    {
                        Flags.CancelRequested = false;
                        BailFromEnter = true;
                        Bail = true;
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse RSS feed URL {0}: {1}", RSSShellCommon.RSSFeedLink, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        TextWriterColor.Write(Translate.DoTranslation("Failed to parse feed URL:") + " {0}", true, ColorTools.ColTypes.Error, ex.Message);
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
                    DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about RefreshFeeds()");

                    // Prompt for the command
                    Shell.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.Write(Translate.DoTranslation("There was an error in the shell.") + CharManager.NewLine + "Error {0}: {1}", true, ColorTools.ColTypes.Error, ex.GetType().FullName, ex.Message);
                    continue;
                }
            }

            // Disconnect the session
            if (RSSShellCommon.RSSKeepAlive)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Exit requested, but not disconnecting.");
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Exit requested. Disconnecting host...");
                if (RSSShellCommon.RSSRefreshFeeds)
                    RSSShellCommon.RSSRefresher.Stop();
                RSSShellCommon.RSSFeedLink = "";
                RSSShellCommon.RSSFeedInstance = null;
            }
        }

    }
}
