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

using System;
using KS.Arguments.ArgumentBase;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Argument
{

    [TestFixture]
    public class ArgumentInfoInitializationTests
    {

        private static ArgumentExecutor ArgumentInstance;

        /// <summary>
        /// Tests initializing ArgumentInfo instance from a command line argument
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeArgumentInfoInstanceFromCommandLineArg()
        {
            // Create instance
            var ArgumentInstance = new ArgumentInfo("help", ArgumentType.CommandLineArgs, "Help page", "", false, 0, null);

            // Check for null
            ArgumentInstance.ShouldNotBeNull();
            ArgumentInstance.Argument.ShouldNotBeNullOrEmpty();
            ArgumentInstance.HelpDefinition.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ArgumentInstance.Argument.ShouldBe("help");
            ArgumentInstance.ArgumentsRequired.ShouldBeFalse();
            ArgumentInstance.HelpDefinition.ShouldBe("Help page");
            ArgumentInstance.MinimumArguments.ShouldBe(0);
            ArgumentInstance.Obsolete.ShouldBeFalse();
            ArgumentInstance.Type.ShouldBe(ArgumentType.CommandLineArgs);
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeArgumentFromBase()
        {
            // Create instance
            ArgumentInstance = new ArgumentTest();

            // Check for null
            ArgumentInstance.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedArgumentExecution()
        {
            Should.NotThrow(new Action(() => ArgumentInstance.Execute("", [], [], [])));
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedArgumentExecutionWithArguments()
        {
            Should.NotThrow(new Action(() => ArgumentInstance.Execute("Hello World", ["Hello", "World"], ["Hello", "World"], [])));
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedArgumentExecutionWithSwitches()
        {
            Should.NotThrow(new Action(() => ArgumentInstance.Execute("-s", ["-s"], [], ["-s"])));
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedArgumentExecutionWithArgumentsAndSwitches()
        {
            Should.NotThrow(new Action(() => ArgumentInstance.Execute("-s Hello!", ["-s", "Hello!"], ["Hello!"], ["-s"])));
        }

    }
}