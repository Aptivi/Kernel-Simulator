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

using KS.Misc.Calendar.Events;
using NUnit.Framework;
using Shouldly;
using System;

namespace KSTests
{

    [TestFixture]
    public class EventManagementTests
    {

        /// <summary>
        /// Tests adding the event
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddEvent()
        {
            EventManager.AddEvent(new DateTime(2022, 2, 22), "Kernel Simulator second-gen release");
            EventManager.CalendarEvents.ShouldNotBeNull();
            EventManager.CalendarEvents.ShouldNotBeEmpty();
            EventManager.CalendarEvents[0].EventDate.Day.ShouldBe(22);
            EventManager.CalendarEvents[0].EventDate.Month.ShouldBe(2);
            EventManager.CalendarEvents[0].EventDate.Year.ShouldBe(2022);
        }

        /// <summary>
        /// Tests adding the event
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestRemoveEvent()
        {
            EventManager.RemoveEvent(new DateTime(2022, 2, 22), 1);
            EventManager.CalendarEvents.ShouldNotBeNull();
            EventManager.CalendarEvents.ShouldBeEmpty();
        }

    }
}
