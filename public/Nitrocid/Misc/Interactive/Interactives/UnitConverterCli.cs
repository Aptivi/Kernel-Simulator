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

using KS.Kernel.Debugging;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using VisualCard.Parts;
using System.Text;
using KS.Misc.Probers.Regexp;
using KS.Files.Querying;
using System.Collections;
using KS.Misc.Text;
using KS.ConsoleBase.Writers.FancyWriters;
using UnitsNet;
using FluentFTP.Helpers;

namespace KS.Misc.Interactive.Interactives
{
    /// <summary>
    /// Unit converter TUI class
    /// </summary>
    public class UnitConverterCli : BaseInteractiveTui, IInteractiveTui
    {
        /// <summary>
        /// Contact manager bindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } = new()
        {
            // Operations
            new InteractiveTuiBinding(/* Localizable */ "Convert...",  ConsoleKey.F1,  (_, _)     => OpenConvert(), true),

            // Misc bindings
            new InteractiveTuiBinding(/* Localizable */ "Switch",      ConsoleKey.Tab, (_, _)     => Switch(), true),
        };

        /// <inheritdoc/>
        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            Quantity.Infos.Select((qi) => qi.Name);

        /// <inheritdoc/>
        public override IEnumerable SecondaryDataSource =>
            GetUnits();

        /// <inheritdoc/>
        public override void RenderStatus(object item) =>
            Status = $"{GetUnits().OfType<string>().Count()} " + Translate.DoTranslation("units to convert");

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item) =>
            (string)item;

        private static void OpenConvert()
        {
            try
            {
                // Open a dialog box asking for number to convert
                string answer = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a number to convert..."), BoxForegroundColor, BoxBackgroundColor);
                if (string.IsNullOrEmpty(answer))
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("You haven't entered a number to convert."), BoxForegroundColor, BoxBackgroundColor);
                    return;
                }
                else if (!answer.IsNumeric())
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("The entered number is invalid."), BoxForegroundColor, BoxBackgroundColor);
                    return;
                }
                else
                {
                    var parser = UnitsNetSetup.Default.UnitParser;
                    var unitNames = Quantity.Infos.Select((qi) => qi.Name);
                    var units = GetUnits();
                    string UnitType = unitNames.ElementAt(FirstPaneCurrentSelection - 1);
                    int QuantityNum = Convert.ToInt32(answer);
                    string wholeUnit = units.OfType<string>().ElementAt(SecondPaneCurrentSelection - 1);
                    string SourceUnit = wholeUnit[..wholeUnit.IndexOf(' ')];
                    string TargetUnit = wholeUnit[(wholeUnit.LastIndexOf(' ') + 1)..];
                    var QuantityInfos = Quantity.Infos.Where(x => x.Name == UnitType).ToArray();
                    var TargetUnitInstance = parser.Parse(TargetUnit, QuantityInfos[0].UnitType);
                    var ConvertedUnit = Quantity.Parse(QuantityInfos[0].ValueType, $"{QuantityNum} {SourceUnit}").ToUnit(TargetUnitInstance);
                    InfoBoxColor.WriteInfoBox("{0} => {1}: {2}", BoxForegroundColor, BoxBackgroundColor,
                        SourceUnit, TargetUnit, ConvertedUnit.ToString(CultureManager.CurrentCult.NumberFormat));
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Can't convert unit.") + ex.Message, BoxForegroundColor, BoxBackgroundColor);
            }
            RedrawRequired = true;
        }

        private static void Switch()
        {
            CurrentPane++;
            if (CurrentPane > 2)
                CurrentPane = 1;
            RedrawRequired = true;
        }

        private static IEnumerable GetUnits()
        {
            var unitInfo = Quantity.Infos;
            var abbreviations = UnitsNetSetup.Default.UnitAbbreviations;
            for (int i = 0; i < unitInfo.Length; i++)
            {
                if (i != FirstPaneCurrentSelection - 1)
                    continue;

                QuantityInfo QuantityInfo = unitInfo[i];
                var unitValues = QuantityInfo.UnitInfos.Select(x => x.Value);
                foreach (Enum UnitValue in unitValues)
                {
                    var remainingUnitValues = unitValues.Except(new[] { UnitValue });
                    foreach (Enum remainingUnitValue in remainingUnitValues)
                    {
                        string abbreviationSource = abbreviations.GetDefaultAbbreviation(UnitValue.GetType(), Convert.ToInt32(UnitValue));
                        string abbreviationTarget = abbreviations.GetDefaultAbbreviation(remainingUnitValue.GetType(), Convert.ToInt32(remainingUnitValue));
                        yield return $"{abbreviationSource} => {abbreviationTarget}";
                    }
                }
                break;
            }
        }
    }
}
