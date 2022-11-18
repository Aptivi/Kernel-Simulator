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
using KS.Drivers.Encryption;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestMD5 : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Encrypts a string using MD5");
        public override void Run()
        {
            var spent = new Stopwatch();
            spent.Start(); // Time when you're on a breakpoint is counted
            TextWriterColor.Write(Encryption.GetEncryptedString("Kernel Simulator", EncryptionAlgorithms.MD5));
            TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), spent.ElapsedMilliseconds);
            spent.Stop();
        }
    }
}
