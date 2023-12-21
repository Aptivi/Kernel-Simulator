﻿using System;
using System.Data;
using System.Linq;

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

namespace KS.Misc.Text
{
	public static class CharManager
	{

		/// <summary>
		/// Gets all the letters and the numbers.
		/// </summary>
		public static char[] GetAllLettersAndNumbers()
		{
			return Enumerable.Range(0, Convert.ToInt32(char.MaxValue) + 1).Select(CharNum => Convert.ToChar(CharNum)).Where(c => char.IsLetterOrDigit(c)).ToArray();


		}

		/// <summary>
		/// Gets all the letters.
		/// </summary>
		public static char[] GetAllLetters()
		{
			return Enumerable.Range(0, Convert.ToInt32(char.MaxValue) + 1).Select(CharNum => Convert.ToChar(CharNum)).Where(c => char.IsLetter(c)).ToArray();


		}

		/// <summary>
		/// Gets all the numbers.
		/// </summary>
		public static char[] GetAllNumbers()
		{
			return Enumerable.Range(0, Convert.ToInt32(char.MaxValue) + 1).Select(CharNum => Convert.ToChar(CharNum)).Where(c => char.IsNumber(c)).ToArray();


		}

	}
}