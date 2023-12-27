﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.Languages;

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

using KS.Misc.Notifiers;

namespace KS.Misc.Calendar.Events
{
    public class EventInfo
    {

        private bool EventNotified;
        /// <summary>
        /// Event date
        /// </summary>
        public DateTime EventDate { get; set; }
        /// <summary>
        /// Event title
        /// </summary>
        public string EventTitle { get; set; }

        /// <summary>
        /// Notifies the user about the event
        /// </summary>
        protected internal void NotifyEvent()
        {
            if (!EventNotified)
            {
                var EventNotification = new Notification(EventTitle, Translate.DoTranslation("Now it's an event day!"), Notifications.NotifPriority.Medium, Notifications.NotifType.Normal);
                Notifications.NotifySend(EventNotification);
                EventNotified = true;
            }
        }

    }
}