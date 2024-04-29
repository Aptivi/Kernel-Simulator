﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Misc.Writers.DebugWriters;
namespace KS.Scripting.Conditions
{
    public abstract class BaseCondition : ICondition
    {

        public virtual string ConditionName => "none";

        public virtual int ConditionPosition { get; } = 1;

        public virtual int ConditionRequiredArguments { get; } = 1;

        public virtual bool IsConditionSatisfied(string FirstVariable, string SecondVariable)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Doing nothing because the condition is undefined. Returning true...");
            return true;
        }

        public bool IsConditionSatisfied(string[] Variables)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Doing nothing because the condition is undefined. Returning true...");
            return true;
        }

    }
}
