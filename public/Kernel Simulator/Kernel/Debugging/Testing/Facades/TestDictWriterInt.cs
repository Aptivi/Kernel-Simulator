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

using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Collections.Generic;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestDictWriterInt : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the dictionary writer with the integer and integer array");
        public override void Run()
        {
            var NormalIntegerDict = new Dictionary<string, int>() { { "One", 1 }, { "Two", 2 }, { "Three", 3 } };
            var ArrayIntegerDict = new Dictionary<string, int[]>() { { "One", new int[] { 1, 2, 3 } }, { "Two", new int[] { 1, 2, 3 } }, { "Three", new int[] { 1, 2, 3 } } };
            TextWriterColor.Write(Translate.DoTranslation("Normal integer dictionary:"));
            ListWriterColor.WriteList(NormalIntegerDict);
            TextWriterColor.Write(Translate.DoTranslation("Array integer dictionary:"));
            ListWriterColor.WriteList(ArrayIntegerDict);
        }
    }
}
