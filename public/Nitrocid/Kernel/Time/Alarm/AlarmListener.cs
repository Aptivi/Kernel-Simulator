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

using KS.Kernel.Debugging;
using KS.Kernel.Power;
using KS.Kernel.Threading;
using KS.Languages;
using KS.Misc.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Kernel.Time.Alarm
{
    internal static class AlarmListener
    {
        internal static bool hasRemovedAlarm = false;
        private static readonly KernelThread alarmThread = new("Alarm Listener Thread", true, HandleAlarms);

        internal static void StartListener()
        {
            if (!alarmThread.IsAlive)
                alarmThread.Start();
        }

        internal static void StopListener()
        {
            if (alarmThread.IsAlive)
                alarmThread.Stop();
        }

        private static void HandleAlarms()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"Alarm listener started");

                // Loop through all the alarms
                List<(string, string)> notifiedAlarms = [];
                while (!PowerManager.RebootRequested)
                {
                    ThreadManager.SleepNoBlock(1);

                    // Fetch all alarms
                    for (int i = 0; i < AlarmTools.alarms.Count; i++)
                    {
                        // An alarm has been removed and we need to skip this iteration of the loop
                        if (hasRemovedAlarm)
                        {
                            hasRemovedAlarm = false;
                            break;
                        }

                        // Get an alarm key and value pair
                        var alarm = AlarmTools.alarms.ElementAt(i);
                        var alarmId = alarm.Key.id;

                        // Get the current date and time for comparison
                        var date = TimeDateTools.KernelDateTime;
                        if (date >= alarm.Value && !notifiedAlarms.Any((tuple) => tuple.Item1 == alarmId))
                        {
                            // The alarm has been fired! Send a notification
                            notifiedAlarms.Add(alarm.Key);
                            var alarmNotif = new Notification(
                                Translate.DoTranslation("Alarm fired!"),
                                alarm.Key.name,
                                NotificationPriority.High, NotificationType.Normal
                            );
                            NotificationManager.NotifySend(alarmNotif);
                        }
                    }

                    // Clear all notified alarms
                    foreach (var notifiedAlarm in notifiedAlarms)
                        AlarmTools.StopAlarm(notifiedAlarm.Item1);
                    notifiedAlarms.Clear();
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Stopping the alarm listener: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }
    }
}
