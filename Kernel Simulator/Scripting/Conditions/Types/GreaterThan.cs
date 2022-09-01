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

namespace KS.Scripting.Conditions.Types
{
    /// <summary>
    /// Checks to see if one of the two UESH variables is greater than the other
    /// </summary>
    public class GreaterThanCondition : BaseCondition, ICondition
    {

        /// <inheritdoc/>
        public override string ConditionName
        {
            get
            {
                return "gre";
            }
        }

        /// <inheritdoc/>
        public override int ConditionPosition { get; } = 2;

        /// <inheritdoc/>
        public override int ConditionRequiredArguments { get; } = 3;

        /// <inheritdoc/>
        public override bool IsConditionSatisfied(string FirstVariable, string SecondVariable)
        {
            return UESHOperators.UESHVariableGreaterThan(FirstVariable, SecondVariable);
        }

    }
}
