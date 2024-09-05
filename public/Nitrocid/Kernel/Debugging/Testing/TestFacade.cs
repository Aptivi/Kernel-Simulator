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

namespace Nitrocid.Kernel.Debugging.Testing
{
    internal abstract class TestFacade
    {
        internal TestStatus status;
        public virtual TestStatus TestStatus =>
            status;
        public virtual string TestName { get; } = "";
        public virtual bool TestInteractive { get; } = true;
        public virtual object? TestExpectedValue { get; }
        public virtual object? TestActualValue { get; set; }
        public virtual TestSection TestSection { get; } = TestSection.Misc;
        public virtual int TestOptionalParameters { get; }

        public virtual void Run(params string[] args) { }
    }
}
