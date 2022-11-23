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

using KS.ConsoleBase.Inputs;
using KS.Files.Read;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class CheckStrings : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Checks to see if the translatable strings exist in the KS resources");
        public override void Run()
        {
            string TextPath = Input.ReadLine(Translate.DoTranslation("Write a translatable string list file path to check:") + " ", "");
            var LocalizedStrings = Translate.PrepareDict("eng");
            var Texts = FileRead.ReadContents(TextPath);
            foreach (string Text in Texts)
            {
                if (!LocalizedStrings.ContainsKey(Text))
                {
                    TextWriterColor.Write("[-] {0}", Text);
                }
            }
        }
    }
}
