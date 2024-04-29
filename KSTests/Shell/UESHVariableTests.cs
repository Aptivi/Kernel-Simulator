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
using KS.Scripting;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Shell
{

    [TestFixture]
    public class UESHVariableTests
    {

        /// <summary>
        /// Tests initializing, setting, and getting $variable
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestVariables()
        {
            UESHVariables.InitializeVariable("$test_var");
            UESHVariables.GetVariables().ShouldNotBeEmpty();
            UESHVariables.SetVariable("$test_var", "test").ShouldBeTrue();
            UESHVariables.GetVariable("$test_var").ShouldBe("test");
            string ExpectedCommand = "echo test";
            string ActualCommand = UESHVariables.GetVariableCommand("$test_var", "echo $test_var");
            ActualCommand.ShouldBe(ExpectedCommand);
        }

    }
}