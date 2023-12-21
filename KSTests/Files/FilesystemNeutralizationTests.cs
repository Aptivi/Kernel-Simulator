﻿using KS.Files;

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

using KS.Files.Folders;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

	[TestFixture]
	public class FilesystemNeutralizationTests
	{

		/// <summary>
    /// Tests path neutralization on a folder in home directory
    /// </summary>
		[Test]
		[Description("Neutralization")]
		public void TestNeutralizePaths()
		{
			CurrentDirectory.CurrentDir = Paths.HomePath;
			string TestPath = "Documents";
			string ExpectedPath = Paths.HomePath + "/" + TestPath;
			string NeutPath = Filesystem.NeutralizePath(TestPath);
			NeutPath.ShouldBe(ExpectedPath);
		}

		/// <summary>
    /// Tests path neutralization on a folder in a custom directory
    /// </summary>
		[Test]
		[Description("Neutralization")]
		public void TestNeutralizePathsCustom()
		{
			string TestPath = "sources.list";
			string TargetPath = "/etc/apt";
			string NeutPath = Filesystem.NeutralizePath(TestPath, TargetPath);
			NeutPath.ShouldBe(TargetPath + "/" + TestPath);
		}

	}
}