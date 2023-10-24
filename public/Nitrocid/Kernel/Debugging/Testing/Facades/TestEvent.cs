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

using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Events;
using KS.Languages;
using System;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestEvent : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests an event");
        public override TestSection TestSection => TestSection.Kernel;
        public override void Run(params string[] args)
        {
            string Text = Input.ReadLine(Translate.DoTranslation("Write an event name:") + " ");
            string[] eventArgs = new string[] { "RanByTest" };
            if (Enum.TryParse(Text, out EventType eventType))
                EventsManager.FireEvent(eventType, eventArgs);
            else
                TextWriterColor.Write(Translate.DoTranslation("Event {0} not found."), Text);
        }
    }
}
