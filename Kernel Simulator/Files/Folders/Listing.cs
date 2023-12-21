﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;

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

using KS.Files.Print;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;

namespace KS.Files.Folders
{
	public static class Listing
	{

		public static bool SortList = true;
		public static FilesystemSortOptions SortMode = FilesystemSortOptions.FullName;
		public static FilesystemSortDirection SortDirection = FilesystemSortDirection.Ascending;
		public static bool ShowFileDetailsList = true;
		public static bool ShowTotalSizeInList;

		/// <summary>
		/// Creates a list of files and directories
		/// </summary>
		/// <param name="folder">Full path to folder</param>
		/// <param name="Sorted">Whether the list is sorted or not</param>
		/// <returns>List of filesystem entries if any. Empty list if folder is not found or is empty.</returns>
		/// <exception cref="Exceptions.FilesystemException"></exception>
		public static List<FileSystemInfo> CreateList(string folder, bool Sorted = false)
		{
			Filesystem.ThrowOnInvalidPath(folder);
			DebugWriter.Wdbg(DebugLevel.I, "Folder {0} will be listed...", folder);
			var FilesystemEntries = new List<FileSystemInfo>();

			// List files and folders
			folder = Filesystem.NeutralizePath(folder);
			if (Checking.FolderExists(folder) | folder.ContainsAnyOf(["?", "*"]))
			{
				IEnumerable<string> enumeration;
				try
				{
					enumeration = GetFilesystemEntries(folder);
				}
				catch (Exception ex)
				{
					DebugWriter.Wdbg(DebugLevel.E, "Failed to make a list of filesystem entries for directory {0}: {1}", folder, ex.Message);
					DebugWriter.WStkTrc(ex);
					throw new Kernel.Exceptions.FilesystemException(Translate.DoTranslation("Failed to make a list of filesystem entries for directory") + " {0}", ex, folder);
				}
				foreach (string Entry in enumeration)
				{
					DebugWriter.Wdbg(DebugLevel.I, "Enumerating {0}...", Entry);
					try
					{
						if (Checking.FileExists(Entry))
						{
							DebugWriter.Wdbg(DebugLevel.I, "Entry is a file. Adding {0} to list...", Entry);
							FilesystemEntries.Add(new FileInfo(Entry));
						}
						else if (Checking.FolderExists(Entry))
						{
							DebugWriter.Wdbg(DebugLevel.I, "Entry is a folder. Adding {0} to list...", Entry);
							FilesystemEntries.Add(new DirectoryInfo(Entry));
						}
					}
					catch (Exception ex)
					{
						DebugWriter.Wdbg(DebugLevel.E, "Failed to enumerate {0} for directory {1}: {2}", Entry, folder, ex.Message);
						DebugWriter.WStkTrc(ex);
					}
				}
			}

			// Return the resulting list immediately if not sorted. Otherwise, sort it.
			if (Sorted & !(FilesystemEntries.Count == 0))
			{
				// We define the max string length for the largest size. This is to overcome the limitation of sorting when it comes to numbers.
				int MaxLength = FilesystemEntries.Max(x => x as FileInfo is not null ? (x as FileInfo).Length.GetDigits() : 1);

				// Select whether or not to sort descending.
				switch (SortDirection)
				{
					case FilesystemSortDirection.Ascending:
						{
							FilesystemEntries = [.. FilesystemEntries.OrderBy(x => SortSelector(x, MaxLength), StringComparer.OrdinalIgnoreCase)];
							break;
						}
					case FilesystemSortDirection.Descending:
						{
							FilesystemEntries = [.. FilesystemEntries.OrderByDescending(x => SortSelector(x, MaxLength), StringComparer.OrdinalIgnoreCase)];
							break;
						}
				}
			}
			return FilesystemEntries;
		}

		private static int GetDigits(this long Number)
		{
			return Number == 0L ? 1 : Convert.ToInt32(Math.Log10(Math.Abs(Number)) + 1d);
		}

		/// <summary>
		/// Helper for sorting filesystem entries
		/// </summary>
		/// <param name="FileSystemEntry">File system entry</param>
		/// <param name="MaxLength">For size, how many zeroes to pad the size string to the left?</param>
		private static string SortSelector(FileSystemInfo FileSystemEntry, int MaxLength)
		{
			switch (SortMode)
			{
				case FilesystemSortOptions.FullName:
					{
						return FileSystemEntry.FullName;
					}
				case FilesystemSortOptions.Length:
					{
						return (FileSystemEntry as FileInfo is not null ? (FileSystemEntry as FileInfo).Length : 0L).ToString().PadLeft(MaxLength, '0');
					}
				case FilesystemSortOptions.CreationTime:
					{
						return Convert.ToString(FileSystemEntry.CreationTime);
					}
				case FilesystemSortOptions.LastAccessTime:
					{
						return Convert.ToString(FileSystemEntry.LastAccessTime);
					}
				case FilesystemSortOptions.LastWriteTime:
					{
						return Convert.ToString(FileSystemEntry.LastWriteTime);
					}

				default:
					{
						return FileSystemEntry.FullName;
					}
			}
		}

		/// <summary>
		/// List all files and folders in a specified folder
		/// </summary>
		/// <param name="folder">Full path to folder</param>
		public static void List(string folder)
		{
			List(folder, ShowFileDetailsList, Flags.SuppressUnauthorizedMessages, SortList);
		}

		/// <summary>
		/// List all files and folders in a specified folder
		/// </summary>
		/// <param name="folder">Full path to folder</param>
		public static void List(string folder, bool Sort)
		{
			List(folder, ShowFileDetailsList, Flags.SuppressUnauthorizedMessages, Sort);
		}

		/// <summary>
		/// List all files and folders in a specified folder
		/// </summary>
		/// <param name="folder">Full path to folder</param>
		public static void List(string folder, bool ShowFileDetails, bool SuppressUnauthorizedMessage)
		{
			List(folder, ShowFileDetails, SuppressUnauthorizedMessage, SortList);
		}

		/// <summary>
		/// List all files and folders in a specified folder
		/// </summary>
		/// <param name="folder">Full path to folder</param>
		public static void List(string folder, bool ShowFileDetails, bool SuppressUnauthorizedMessage, bool Sort)
		{
			Filesystem.ThrowOnInvalidPath(folder);
			DebugWriter.Wdbg(DebugLevel.I, "Folder {0} will be listed...", folder);

			// List files and folders
			folder = Filesystem.NeutralizePath(folder);
			if (Checking.FolderExists(folder) | folder.ContainsAnyOf(["?", "*"]))
			{
				List<FileSystemInfo> enumeration;
				SeparatorWriterColor.WriteSeparator(folder, true);

				// Try to create a list
				try
				{
					enumeration = CreateList(folder, Sort);
					if (enumeration.Count == 0)
						TextWriterColor.Write(Translate.DoTranslation("Folder is empty."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));

					// Enumerate each entry
					long TotalSize = 0L;
					foreach (FileSystemInfo Entry in enumeration)
					{
						DebugWriter.Wdbg(DebugLevel.I, "Enumerating {0}...", Entry.FullName);
						try
						{
							if (Checking.FileExists(Entry.FullName))
							{
								TotalSize += ((FileInfo)Entry).Length;
								FileInfoPrinter.PrintFileInfo(Entry);
							}
							else if (Checking.FolderExists(Entry.FullName))
							{
								DirectoryInfoPrinter.PrintDirectoryInfo(Entry);
							}
						}
						catch (UnauthorizedAccessException ex)
						{
							if (!SuppressUnauthorizedMessage)
								TextWriterColor.Write("- " + Translate.DoTranslation("You are not authorized to get info for {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), Entry.Name);
							DebugWriter.WStkTrc(ex);
						}
					}

					// Show total size in list optionally
					if (ShowTotalSizeInList)
						TextWriterColor.Write(Kernel.Kernel.NewLine + Translate.DoTranslation("Total size in folder:") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), TotalSize.FileSizeToString());
				}
				catch (Exception ex)
				{
					TextWriterColor.Write(Translate.DoTranslation("Unknown error while listing in directory: {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
					DebugWriter.WStkTrc(ex);
				}
			}
			else if (Checking.FileExists(folder))
			{
				try
				{
					FileInfoPrinter.PrintFileInfo(new FileInfo(folder), ShowFileDetails);
				}
				catch (UnauthorizedAccessException ex)
				{
					if (!SuppressUnauthorizedMessage)
						TextWriterColor.Write("- " + Translate.DoTranslation("You are not authorized to get info for {0}."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), folder);
					DebugWriter.WStkTrc(ex);
				}
			}
			else
			{
				TextWriterColor.Write(Translate.DoTranslation("Directory {0} not found"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), folder);
				DebugWriter.Wdbg(DebugLevel.I, "IO.FolderExists = {0}", Checking.FolderExists(folder));
			}
		}

		/// <summary>
		/// Gets the filesystem entries of the parent with the specified pattern (wildcards, ...)
		/// </summary>
		/// <param name="Path">The path, including the pattern</param>
		/// <returns>The array of full paths</returns>
		public static string[] GetFilesystemEntries(string Path, bool IsFile = false)
		{
			string[] Entries = [];
			try
			{
				Filesystem.ThrowOnInvalidPath(Path);

				// Select the pattern index
				int SelectedPatternIndex = 0;
				string[] SplitPath = Path.Split('/').Skip(1).ToArray();
				var SplitParent = new List<string>() { Path.Split('/')[0] };
				for (int PatternIndex = 0, loopTo = SplitPath.Length - 1; PatternIndex <= loopTo; PatternIndex++)
				{
					if (SplitPath[PatternIndex].ContainsAnyOf(System.IO.Path.GetInvalidFileNameChars().Select(Character => Character.ToString()).ToArray()))
					{
						SelectedPatternIndex = PatternIndex;
						break;
					}
					SplitParent.Add(SplitPath[PatternIndex]);
				}

				// Split the path and the pattern
				string Parent = Filesystem.NeutralizePath(System.IO.Path.GetDirectoryName(Path) + "/" + System.IO.Path.GetFileName(Path));
				string Pattern = IsFile ? "" : "*";
				if (Parent.ContainsAnyOf(Parsing.GetInvalidPathChars().Select(Character => Character.ToString()).ToArray()))
				{
					Parent = System.IO.Path.GetDirectoryName(Path);
					Pattern = System.IO.Path.GetFileName(Path);
				}
				if (SelectedPatternIndex != 0)
				{
					Parent = string.Join("/", SplitParent);
					Pattern = string.Join("/", SplitPath.Skip(SelectedPatternIndex));
				}

				// Split the path and the pattern and return the final result
				Entries = GetFilesystemEntries(Parent, Pattern);
			}
			catch (Exception ex)
			{
				DebugWriter.WStkTrc(ex);
				DebugWriter.Wdbg(DebugLevel.E, "Failed to combine files: {0}", ex.Message);
			}
			return Entries;
		}

		/// <summary>
		/// Gets the filesystem entries of the parent with the specified pattern (wildcards, ...)
		/// </summary>
		/// <param name="Parent">The parent path. It can be neutralized if necessary</param>
		/// <param name="Pattern">The pattern</param>
		/// <returns>The array of full paths</returns>
		public static string[] GetFilesystemEntries(string Parent, string Pattern)
		{
			string[] Entries = [];
			try
			{
				Filesystem.ThrowOnInvalidPath(Parent);
				Filesystem.ThrowOnInvalidPath(Pattern);
				Parent = Filesystem.NeutralizePath(Parent);

				// Get the entries
				if (Directory.Exists(Parent))
				{
					Entries = Directory.EnumerateFileSystemEntries(Parent, Pattern).ToArray();
					DebugWriter.Wdbg(DebugLevel.I, "Enumerated {0} entries from parent {1} using pattern {2}", Entries.Length, Parent, Pattern);
				}
				else
				{
					Entries = [Parent];
				}
			}
			catch (Exception ex)
			{
				DebugWriter.WStkTrc(ex);
				DebugWriter.Wdbg(DebugLevel.E, "Failed to combine files: {0}", ex.Message);
			}
			return Entries;
		}

	}
}