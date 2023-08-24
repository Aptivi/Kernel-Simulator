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

using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Splash;
using System;
using System.Threading;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class SendNotificationSimple : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Sends a notification as asterisk to test the receiver");
        public override void Run()
        {
            foreach (var value in Enum.GetValues(typeof(NotificationPriority)))
            {
                SplashReport._KernelBooted = true;
                Config.MainConfig.NotifyDisplayAsAsterisk = true;
                var Notif = new Notification(Translate.DoTranslation("Test notification"), Translate.DoTranslation("Description is here"), (NotificationPriority)value, NotificationType.Normal);
                NotificationManager.NotifySend(Notif);
                Thread.Sleep(500);
                Config.MainConfig.NotifyDisplayAsAsterisk = false;
                SplashReport._KernelBooted = false;
            }
        }
    }
}
