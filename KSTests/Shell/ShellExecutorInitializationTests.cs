﻿using System;

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

using KS.Shell.ShellBase.Shells;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

	[TestFixture]
	public class ShellExecutorInitializationTests
	{

		private static ShellExecutor ShellInstance;

		/// <summary>
		/// Tests initializing the shell instance from base
		/// </summary>
		[Test]
		[Description("Initialization")]
		[SetUp]
		public void TestInitializeShellExecutorFromBase()
		{
			// Create instance
			ShellInstance = new ShellTest();

			// Check for null
			ShellInstance.ShouldNotBeNull();
		}

		/// <summary>
		/// Tests initializing the shell instance from base
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializedShellExecution()
		{
			Should.NotThrow(new Action(() => ShellInstance.InitializeShell()));
			ShellInstance.Bail.ShouldBeTrue();
		}

		/// <summary>
		/// Tests initializing the shell instance from base
		/// </summary>
		[Test]
		[Description("Initialization")]
		public void TestInitializedShellExecutionWithArguments()
		{
			Should.NotThrow(new Action(() => ShellInstance.InitializeShell("Hello", "World")));
			ShellInstance.Bail.ShouldBeTrue();
		}

	}
}