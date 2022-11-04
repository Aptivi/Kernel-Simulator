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

using System;
using System.Data;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using UnitsNet;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Unit conversion command
    /// </summary>
    /// <remarks>
    /// This command allows you to convert numbers from one unit to another compatible unit, provided that you've specified the unit type, like Length, Area, and so on.
    /// <br></br>
    /// If you want to see the full list of all supported units by the UnitsNet library, check out its help command where it lists all possible units.
    /// </remarks>
    class UnitConvCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string UnitType = ListArgsOnly[0];
            int QuantityNum = Convert.ToInt32(ListArgsOnly[1]);
            string SourceUnit = ListArgsOnly[2];
            string TargetUnit = ListArgsOnly[3];
            var QuantityInfos = Quantity.Infos.Where(x => (x.Name ?? "") == (UnitType ?? "")).ToArray();
            var TargetUnitInstance = UnitParser.Default.Parse(TargetUnit, QuantityInfos[0].UnitType);
            var ConvertedUnit = Quantity.Parse(QuantityInfos[0].ValueType, $"{QuantityNum} {SourceUnit}").ToUnit(TargetUnitInstance);
            TextWriterColor.Write("- {0} => {1}: ", false, ColorTools.ColTypes.ListEntry, SourceUnit, TargetUnit);
            TextWriterColor.Write(ConvertedUnit.ToString(CultureManager.CurrentCult.NumberFormat), true, ColorTools.ColTypes.ListValue);
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("Available unit types and their units:"));
            foreach (QuantityInfo QuantityInfo in Quantity.Infos)
            {
                TextWriterColor.Write("- {0}:", true, ColorTools.ColTypes.ListEntry, QuantityInfo.Name);
                foreach (Enum UnitValues in QuantityInfo.UnitInfos.Select(x => x.Value))
                {
                    TextWriterColor.Write("  - {0}: ", false, ColorTools.ColTypes.ListEntry, string.Join(", ", UnitAbbreviationsCache.Default.GetDefaultAbbreviation(UnitValues.GetType(), Convert.ToInt32(UnitValues))));
                    TextWriterColor.Write(UnitValues.ToString(), true, ColorTools.ColTypes.ListValue);
                }
            }
        }

    }
}