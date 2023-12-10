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

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.Encryption;
using KS.Languages;
using System.Diagnostics;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestCRC32 : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Encrypts a string using CRC32");
        public override TestSection TestSection => TestSection.Drivers;
        public override void Run(params string[] args)
        {
            // Time when you're on a breakpoint is counted
            var spent = new Stopwatch();
            spent.Start();
            TextWriterColor.Write(Encryption.GetEncryptedString("Nitrocid KS", "CRC32"));
            TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), spent.ElapsedMilliseconds);
            spent.Stop();
        }
    }
}
