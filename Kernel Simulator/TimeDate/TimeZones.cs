﻿using System;
using System.Collections.Generic;
using System.Linq;

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

using static System.TimeZoneInfo;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

namespace KS.TimeDate
{
	public static class TimeZones
	{

		/// <summary>
        /// Populates current time in all of the time zones (IANA on Unix).
        /// </summary>
		public static Dictionary<string, DateTime> GetTimeZones()
		{
			// Get all system time zones (IANA on Unix)
			TimeZoneInfo[] Zones = TimeZoneInfo.GetSystemTimeZones().ToArray();
			var ZoneTimes = new Dictionary<string, DateTime>();
			DebugWriter.Wdbg(DebugLevel.I, "Found {0} time zones.", Zones.Length);

			// Run a cleanup in the list
			ZoneTimes.Clear();
			DebugWriter.Wdbg(DebugLevel.I, "Cleaned up zoneTimes.");

			// Adds date and time to every single time zone to the list
			foreach (var Zone in Zones)
				ZoneTimes.Add(Zone.Id, ConvertTime(TimeDate.KernelDateTime, FindSystemTimeZoneById(Zone.Id)));

			// Return the populated array
			return ZoneTimes;
		}

		/// <summary>
        /// Shows current time in selected time zone
        /// </summary>
        /// <param name="zone">Time zone</param>
        /// <returns>True if found; False if not found</returns>
		public static bool ShowTimeZone(string Zone)
		{
			var ZoneTimes = GetTimeZones();
			bool ZoneFound = ZoneTimes.ContainsKey(Zone);
			if (ZoneFound)
			{
				TextWriterColor.Write(Translate.DoTranslation("- Time of {0}: {1}") + " ({2})", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), Zone, ZoneTimes[Zone].ToString(), FindSystemTimeZoneById(Zone).GetUtcOffset(TimeDate.KernelDateTime).ToString());
			}
			return ZoneFound;
		}

		/// <summary>
        /// Shows current time in selected time zone
        /// </summary>
        /// <param name="zone">Time zone to search</param>
        /// <returns>True if found; False if not found</returns>
		public static bool ShowTimeZones(string Zone)
		{
			var ZoneTimes = GetTimeZones();
			var ZoneFound = default(bool);
			foreach (string ZoneName in ZoneTimes.Keys)
			{
				if (ZoneName.Contains(Zone))
				{
					ZoneFound = true;
					TextWriterColor.Write(Translate.DoTranslation("- Time of {0}: {1}") + " ({2})", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), ZoneName, ZoneTimes[ZoneName].ToString(), FindSystemTimeZoneById(ZoneName).GetUtcOffset(TimeDate.KernelDateTime).ToString());
				}
			}
			return ZoneFound;
		}

		/// <summary>
        /// Shows current time in all time zones
        /// </summary>
		public static void ShowAllTimeZones()
		{
			var ZoneTimes = GetTimeZones();
			foreach (var TimeZone in ZoneTimes.Keys)
				TextWriterColor.Write(Translate.DoTranslation("- Time of {0}: {1}") + " ({2})", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), TimeZone, ZoneTimes[TimeZone].ToString(), FindSystemTimeZoneById(TimeZone).GetUtcOffset(TimeDate.KernelDateTime).ToString());
		}

	}
}