﻿using System.Collections.Generic;

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

using System.IO;
using KS.Misc.Writers.DebugWriters;

namespace KS.Files.Read
{
	public static class FileRead
	{

		/// <summary>
		/// Reads the contents of a file and writes it to the array. This is blocking and will put a lock on the file until read.
		/// </summary>
		/// <param name="filename">Full path to file</param>
		/// <returns>An array full of file contents</returns>
		public static string[] ReadContents(string filename)
		{
			// Read the contents
			Filesystem.ThrowOnInvalidPath(filename);
			var FileContents = new List<string>();
			filename = Filesystem.NeutralizePath(filename);
			using (var FStream = new StreamReader(filename))
			{
				DebugWriter.Wdbg(DebugLevel.I, "Stream to file {0} opened.", filename);
				while (!FStream.EndOfStream)
					FileContents.Add(FStream.ReadLine());
			}
			return [.. FileContents];
		}

		/// <summary>
		/// Opens a file, reads all lines, and returns the array of lines
		/// </summary>
		/// <param name="path">Path to file</param>
		/// <returns>Array of lines</returns>
		public static string[] ReadAllLinesNoBlock(string path)
		{
			Filesystem.ThrowOnInvalidPath(path);

			// Read all the lines, bypassing the restrictions.
			path = Filesystem.NeutralizePath(path);
			var AllLnList = new List<string>();
			var FOpen = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			while (!FOpen.EndOfStream)
				AllLnList.Add(FOpen.ReadLine());
			FOpen.Close();
			return [.. AllLnList];
		}

	}
}