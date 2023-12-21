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

namespace KS.Kernel
{
	/// <summary>
    /// One-lettered kernel error level enumeration
    /// </summary>
	public enum KernelErrorLevel
	{
		/// <summary>
        /// Continuable kernel error
        /// </summary>
		C,
		/// <summary>
        /// Serious kernel error
        /// </summary>
		S,
		/// <summary>
        /// Double kernel panic
        /// </summary>
		D,
		/// <summary>
        /// Fatal kernel error
        /// </summary>
		F,
		/// <summary>
        /// Unrecoverable kernel error
        /// </summary>
		U
	}
}