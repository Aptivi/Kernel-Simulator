﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Misc.Calendar;
using System;

namespace KS.Kernel.Time.Renderers
{
    /// <summary>
    /// Miscellaneous time and date renderers
    /// </summary>
    public static class TimeDateMiscRenderers
    {
        /// <summary>
        /// Gets the remaining time from now
        /// </summary>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <returns>Remaining time from now in a string representation</returns>
        public static string RenderRemainingTimeFromNow(int Milliseconds) =>
            RenderRemainingTimeFromNow(Milliseconds, TimeDateRenderConstants.FullTimeFormat);

        /// <summary>
        /// Gets the remaining time from now
        /// </summary>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <param name="format">A format which will be used to render the time remaining</param>
        /// <returns>Remaining time from now in a string representation</returns>
        public static string RenderRemainingTimeFromNow(int Milliseconds, string format) =>
            RenderRemainingTimeFrom(TimeDateTools.KernelDateTime, Milliseconds, format);

        /// <summary>
        /// Gets the remaining time from the specified date and time
        /// </summary>
        /// <param name="moment">A moment in time in which will be compared into how many milliseconds remaining</param>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <returns>Remaining time from the specified date and time in a string representation</returns>
        public static string RenderRemainingTimeFrom(DateTime moment, int Milliseconds) =>
            RenderRemainingTimeFrom(moment, Milliseconds, TimeDateRenderConstants.FullTimeFormat);

        /// <summary>
        /// Gets the remaining time from the specified date and time
        /// </summary>
        /// <param name="moment">A moment in time in which will be compared into how many milliseconds remaining</param>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <param name="format">A format which will be used to render the time remaining</param>
        /// <returns>Remaining time from the specified date and time in a string representation</returns>
        public static string RenderRemainingTimeFrom(DateTime moment, int Milliseconds, string format)
        {
            var RemainingTime = moment.AddMilliseconds(Milliseconds) - moment;
            string RemainingTimeString = RemainingTime.ToString(format, CultureManager.CurrentCult);
            return RemainingTimeString;
        }

        /// <summary>
        /// Shows current time, date, and timezone.
        /// </summary>
        public static void ShowCurrentTimes()
        {
            TextWriterColor.Write(Translate.DoTranslation("Current time is {0}"), TimeDateRenderers.RenderTime());
            TextWriterColor.Write(Translate.DoTranslation("Today is {0}"), TimeDateRenderers.RenderDate());
            if (CalendarTools.EnableAltCalendar)
                TextWriterColor.Write(Translate.DoTranslation("Current time in {0} is {1}"), CalendarTools.AltCalendar.ToString(), TimeDateRenderers.Render(CalendarTools.GetCultureFromCalendar(CalendarTools.AltCalendar)));
            TextWriterColor.Write(Translate.DoTranslation("Time and date in UTC: {0}"), TimeDateRenderersUtc.RenderUtc());
            TextWriterColor.Write(Translate.DoTranslation("Time Zone:") + " {0} ({1})", TimeZoneInfo.Local.StandardName, TimeZoneRenderers.ShowTimeZoneUtcOffsetStringLocal());
        }
    }
}
