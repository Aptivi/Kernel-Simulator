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

using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Scripting.Conditions;

namespace KS.Shell.ShellBase.Scripting.Conditions.Types
{
    /// <summary>
    /// Checks to see if a UESH variable is not of the correct type
    /// </summary>
    public class IsNotCondition : BaseCondition, ICondition
    {

        /// <inheritdoc/>
        public override string ConditionName => "isnot";

        /// <inheritdoc/>
        public override int ConditionPosition { get; } = 2;

        /// <inheritdoc/>
        public override int ConditionRequiredArguments { get; } = 3;

        /// <inheritdoc/>
        public override bool IsConditionSatisfied(string FirstVariable, string SecondVariable)
        {
            // SecondVariable is actually a data type needed for parsing.
            if (!IsCondition.DataTypes.ContainsKey(SecondVariable))
                throw new KernelException(KernelExceptionType.UESHConditionParse, Translate.DoTranslation("Data type {0} specified is invalid."), SecondVariable);

            // Get the action needed to get the comparer and test the condition defined above
            return !IsCondition.DataTypes[SecondVariable](FirstVariable);
        }

    }
}
