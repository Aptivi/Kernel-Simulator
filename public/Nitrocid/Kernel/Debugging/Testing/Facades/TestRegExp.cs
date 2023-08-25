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
using KS.Languages;
using System.Linq;
using System.Text.RegularExpressions;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestRegExp : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the regular expression facility");
        public override TestSection TestSection => TestSection.Drivers;
        public override void Run()
        {
            string Text = Input.ReadLine(Translate.DoTranslation("Write a string to check:") + " ");
            string Regex = Input.ReadLine(Translate.DoTranslation("Write a regular expression:") + " ");
            var Reg = new Regex(Regex);
            var Matches = Reg.Matches(Text);
            int MatchNum = 1;
            foreach (Match Mat in Matches.Cast<Match>())
            {
                TextWriterColor.Write(Translate.DoTranslation("Match {0} ({1}): {2}"), MatchNum, Regex, Mat);
                MatchNum += 1;
            }
        }
    }
}
