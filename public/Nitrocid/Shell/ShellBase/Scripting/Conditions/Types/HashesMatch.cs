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

using KS.Shell.ShellBase.Scripting;
using KS.Shell.ShellBase.Scripting.Conditions;

namespace KS.Shell.ShellBase.Scripting.Conditions.Types
{
    /// <summary>
    /// Checks to see if a UESH variable matches its hash specified on the second
    /// </summary>
    public class HashesMatchCondition : BaseCondition, ICondition
    {

        /// <inheritdoc/>
        public override string ConditionName => "sane";

        /// <inheritdoc/>
        public override int ConditionPosition { get; } = 3;

        /// <inheritdoc/>
        public override int ConditionRequiredArguments { get; } = 3;

        /// <inheritdoc/>
        public override bool IsConditionSatisfied(string FirstVariable, string SecondVariable) => UESHOperators.UESHVariableHashMatch(FirstVariable, SecondVariable);

    }
}
