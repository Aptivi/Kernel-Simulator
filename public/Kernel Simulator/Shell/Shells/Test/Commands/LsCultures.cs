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

using System.Globalization;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you test the available cultures installed on the system.
    /// </summary>
    class Test_LsCulturesCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Cults = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo Cult in Cults)
            {
                if (ListArgsOnly.Length > 0)
                {
                    if (Cult.Name.ToLower().Contains(ListArgsOnly[0].ToLower()) | Cult.EnglishName.ToLower().Contains(ListArgsOnly[0].ToLower()))
                    {
                        TextWriterColor.Write("{0}: {1}", Cult.Name, Cult.EnglishName);
                    }
                }
                else
                {
                    TextWriterColor.Write("{0}: {1}", Cult.Name, Cult.EnglishName);
                }
            }
        }

    }
}