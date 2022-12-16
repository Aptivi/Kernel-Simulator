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
using System.Collections.Generic;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.Shells.RSS;

namespace KS.Network.RSS.Bookmarks
{
    /// <summary>
    /// RSS bookmark management routines
    /// </summary>
    public static class RSSBookmarkManager
    {

        private readonly static List<string> RssBookmarks = new();

        /// <summary>
        /// Adds the current RSS feed to the bookmarks
        /// </summary>
        public static void AddRSSFeedToBookmark()
        {
            if (!string.IsNullOrEmpty(RSSShellCommon.RSSFeedLink))
            {
                AddRSSFeedToBookmark(RSSShellCommon.RSSFeedLink);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to add null feed link to bookmarks. Ignored.");
            }
        }

        /// <summary>
        /// Adds the RSS feed URL to the bookmarks
        /// </summary>
        /// <param name="FeedURL">The feed URL to parse</param>
        public static void AddRSSFeedToBookmark(string FeedURL)
        {
            if (!string.IsNullOrEmpty(FeedURL))
            {
                try
                {
                    // Form a URI of feed
                    var FinalFeedUri = new Uri(FeedURL);
                    string FinalFeedUriString = FinalFeedUri.AbsoluteUri;

                    // Add the feed to bookmarks if not found
                    if (!RssBookmarks.Contains(FinalFeedUriString))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Adding {0} to feed bookmark list...", FinalFeedUriString);
                        RssBookmarks.Add(FinalFeedUriString);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to add {0} to RSS bookmarks: {1}", FeedURL, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    if (ex.GetType().Name == nameof(UriFormatException))
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Verify that {0} is actually valid.", FeedURL);
                        throw new KernelException(KernelExceptionType.InvalidFeedLink, Translate.DoTranslation("Failed to parse feed URL:") + " {0}", ex.Message);
                    }
                    else
                    {
                        throw new KernelException(KernelExceptionType.InvalidFeed, Translate.DoTranslation("Failed to parse feed URL:") + " {0}", ex.Message);
                    }
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to add null feed link to bookmarks. Ignored.");
            }
        }

        /// <summary>
        /// Removes the current RSS feed from the bookmarks
        /// </summary>
        public static void RemoveRSSFeedFromBookmark()
        {
            if (!string.IsNullOrEmpty(RSSShellCommon.RSSFeedLink))
            {
                RemoveRSSFeedFromBookmark(RSSShellCommon.RSSFeedLink);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to remove null feed link from bookmarks. Ignored.");
            }
        }

        /// <summary>
        /// Removes the RSS feed URL from the bookmarks
        /// </summary>
        /// <param name="FeedURL">The feed URL to parse</param>
        public static void RemoveRSSFeedFromBookmark(string FeedURL)
        {
            if (!string.IsNullOrEmpty(FeedURL))
            {
                try
                {
                    // Form a URI of feed
                    var FinalFeedUri = new Uri(FeedURL);
                    string FinalFeedUriString = FinalFeedUri.AbsoluteUri;

                    // Remove the feed from bookmarks if found
                    if (RssBookmarks.Contains(FinalFeedUriString))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Removing {0} from feed bookmark list...", FinalFeedUriString);
                        RssBookmarks.Remove(FinalFeedUriString);
                    }
                    else
                    {
                        throw new KernelException(KernelExceptionType.InvalidFeedLink, Translate.DoTranslation("The feed doesn't exist in bookmarks."));
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove {0} from RSS bookmarks: {1}", FeedURL, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    if (ex.GetType().Name == nameof(UriFormatException))
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Verify that {0} is actually valid.", FeedURL);
                        throw new KernelException(KernelExceptionType.InvalidFeedLink, Translate.DoTranslation("Failed to parse feed URL:") + " {0}", ex.Message);
                    }
                    else
                    {
                        throw new KernelException(KernelExceptionType.InvalidFeed, Translate.DoTranslation("Failed to parse feed URL:") + " {0}", ex.Message);
                    }
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to remove null feed link from bookmarks. Ignored.");
            }
        }

        /// <summary>
        /// Gets all RSS bookmarks
        /// </summary>
        public static List<string> GetBookmarks() => RssBookmarks;

        /// <summary>
        /// Gets the bookmark URL from number
        /// </summary>
        public static string GetBookmark(int Num)
        {
            // Return nothing if there are no bookmarks
            if (RssBookmarks.Count == 0)
                return "";

            // Get the bookmark
            if (Num <= 0)
                Num = 1;
            return RssBookmarks[Num - 1];
        }

    }
}
