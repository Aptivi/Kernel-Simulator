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

using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Misc.Text.Probers.Placeholder;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class PrintPlaces : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Prints a string to console and parses the placeholders found");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override int TestOptionalParameters => 1;
        public override void Run(params string[] args)
        {
            string rendered = args.Length > 0 ? PlaceParse.ProbePlaces(args[0]) : "Date: " + PlaceParse.ProbePlaces("<shortdate>");
            TextWriterColor.Write(rendered, true);
        }
    }
}
