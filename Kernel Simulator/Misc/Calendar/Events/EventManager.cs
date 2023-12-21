﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using KS.ConsoleBase.Colors;
using KS.Files;

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

using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Calendar.Events
{
	public static class EventManager
	{

		public static List<EventInfo> CalendarEvents = new();
		public static KernelThread EventThread = new("Event Thread", false, EventListen);
		internal static object EventManagerLock = new();

		/// <summary>
        /// Listens for events and notifies the user if the date is due to the event
        /// </summary>
		private static void EventListen()
		{
			while (!Flags.KernelShutdown)
			{
				try
				{
					Thread.Sleep(100);
					lock (EventManagerLock)
					{
						for (int EventIndex = 0, loopTo = CalendarEvents.Count - 1; EventIndex <= loopTo; EventIndex++)
						{
							var EventInstance = CalendarEvents[EventIndex];
							if (DateTime.Today == EventInstance.EventDate.Date)
							{
								EventInstance.NotifyEvent();
							}
						}
					}
				}
				catch (ThreadInterruptedException ex)
				{
					DebugWriter.Wdbg(DebugLevel.I, "Aborting event listener...");
					return;
				}
			}
		}

		/// <summary>
        /// Adds the event to the list (calendar will mark the day with color)
        /// </summary>
        /// <param name="EventDate">Event date and time</param>
        /// <param name="EventTitle">Event title</param>
		public static void AddEvent(DateTime EventDate, string EventTitle)
		{
			if (string.IsNullOrWhiteSpace(EventTitle))
				EventTitle = Translate.DoTranslation("Untitled event");
			var EventInstance = new EventInfo()
			{
				EventTitle = EventTitle,
				EventDate = EventDate
			};
			AddEvent(EventInstance);
		}

		/// <summary>
        /// Adds the event to the list (calendar will mark the day with color)
        /// </summary>
        /// <param name="EventInstance">Event info instance</param>
		internal static void AddEvent(EventInfo EventInstance)
		{
			CalendarEvents.Add(EventInstance);
		}

		/// <summary>
        /// Removes the event from the list
        /// </summary>
        /// <param name="EventDate">Event date and time</param>
        /// <param name="EventId">Event ID</param>
		public static void RemoveEvent(DateTime EventDate, int EventId)
		{
			int EventIndex = EventId - 1;
			var EventInstance = CalendarEvents[EventIndex];
			if (EventInstance.EventDate == EventDate)
			{
				CalendarEvents.Remove(EventInstance);
			}
		}

		/// <summary>
        /// List all the events
        /// </summary>
		public static void ListEvents()
		{
			foreach (EventInfo EventInstance in CalendarEvents)
			{
				TextWriterColor.Write("- {0}: ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), EventInstance.EventDate);
				TextWriterColor.Write(EventInstance.EventTitle, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
			}
		}

		/// <summary>
        /// Loads all the events from the KSEvents directory and adds them to the event list
        /// </summary>
		public static void LoadEvents()
		{
			Making.MakeDirectory(Paths.GetKernelPath(KernelPathType.Events), false);
			var EventFiles = Directory.EnumerateFileSystemEntries(Paths.GetKernelPath(KernelPathType.Events), "*", SearchOption.AllDirectories).ToList();
			DebugWriter.Wdbg(DebugLevel.I, "Got {0} events.", EventFiles.Count);

			// Load all the events
			foreach (string EventFile in EventFiles)
			{
				var LoadedEvent = LoadEvent(EventFile);
				if (LoadedEvent is not null)
					AddEvent(LoadedEvent);
			}
		}

		/// <summary>
        /// Loads an event file
        /// </summary>
        /// <param name="EventFile">Event file</param>
        /// <returns>A converted event info instance. null if unsuccessful.</returns>
		public static EventInfo LoadEvent(string EventFile)
		{
			lock (EventManagerLock)
			{
				Filesystem.ThrowOnInvalidPath(EventFile);
				EventFile = Filesystem.NeutralizePath(EventFile);
				DebugWriter.Wdbg(DebugLevel.I, "Loading event {0}...", EventFile);

				// If file exists, convert the file to the event instance
				if (Checking.FileExists(EventFile))
				{
					var Converter = new XmlSerializer(typeof(EventInfo));
					var EventFileStream = new FileStream(EventFile, FileMode.Open);
					DebugWriter.Wdbg(DebugLevel.I, "Opened stream [{0}]. Converting...", EventFileStream.Length);
					EventInfo ConvertedEvent = (EventInfo)Converter.Deserialize(EventFileStream);
					DebugWriter.Wdbg(DebugLevel.I, "Converted!");
					EventFileStream.Close();
					return ConvertedEvent;
				}
				else
				{
					DebugWriter.Wdbg(DebugLevel.E, "File doesn't exist!");
				}
				return null;
			}
		}

		/// <summary>
        /// Saves all the events from the event list to their individual files
        /// </summary>
		public static void SaveEvents()
		{
			SaveEvents(Paths.GetKernelPath(KernelPathType.Events), Flags.SaveEventsRemindersDestructively);
		}

		/// <summary>
        /// Saves all the events from the event list to their individual files
        /// </summary>
		public static void SaveEvents(string Path, bool Destructive)
		{
			Filesystem.ThrowOnInvalidPath(Path);
			Path = Filesystem.NeutralizePath(Path);
			DebugWriter.Wdbg(DebugLevel.I, "Saving events to {0}...", Path);

			// Remove all events from path, if running destructively
			if (Destructive)
			{
				string[] EventFiles = Directory.EnumerateFiles(Path, "*", SearchOption.AllDirectories).ToArray();
				string[] EventFolders = Directory.EnumerateDirectories(Path, "*", SearchOption.AllDirectories).ToArray();

				// First, remove all files
				foreach (string FilePath in EventFiles)
					Removing.RemoveFile(FilePath);

				// Then, remove all empty folders
				foreach (string FolderPath in EventFolders)
					Removing.RemoveDirectory(FolderPath);
			}

			// Enumerate through every event and save them
			for (int EventIndex = 0, loopTo = CalendarEvents.Count - 1; EventIndex <= loopTo; EventIndex++)
			{
				var EventInstance = CalendarEvents[EventIndex];
				string EventFileName = $"[{EventIndex}] {EventInstance.EventTitle}.ksevent";
				DebugWriter.Wdbg(DebugLevel.I, "Event file name: {0}...", EventFileName);
				string EventFilePath = Filesystem.NeutralizePath(EventFileName, Path);
				DebugWriter.Wdbg(DebugLevel.I, "Event file path: {0}...", EventFilePath);
				SaveEvent(EventInstance, EventFilePath);
			}
		}

		/// <summary>
        /// Saves an event to a file
        /// </summary>
		public static void SaveEvent(EventInfo EventInstance)
		{
			SaveEvent(EventInstance, Paths.GetKernelPath(KernelPathType.Events));
		}

		/// <summary>
        /// Saves an event to a file
        /// </summary>
		public static void SaveEvent(EventInfo EventInstance, string File)
		{
			Filesystem.ThrowOnInvalidPath(File);
			File = Filesystem.NeutralizePath(File);
			DebugWriter.Wdbg(DebugLevel.I, "Saving event to {0}...", File);
			var Converter = new XmlSerializer(typeof(EventInfo));
			var EventFileStream = new FileStream(File, FileMode.OpenOrCreate);
			DebugWriter.Wdbg(DebugLevel.I, "Opened stream with length {0}", EventFileStream.Length);
			Converter.Serialize(EventFileStream, EventInstance);
			EventFileStream.Close();
		}

	}
}