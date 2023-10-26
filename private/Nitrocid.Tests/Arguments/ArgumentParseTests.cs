﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Shell.ShellBase.Arguments;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Arguments
{
    [TestFixture]
    public class ArgumentParseTests
    {
        /// <summary>
        /// Tests initializing <see cref="ProvidedArgumentArgumentsInfo"/> instance from a command line argument
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeProvidedArgumentArgumentInfoInstanceFromCommandLineArgNoArg()
        {
            // Create instance
            var argArginfo = ArgumentsParser.ParseArgumentArguments("lang").total[0];

            // Test for null
            argArginfo.ShouldNotBeNull();
            argArginfo.Command.ShouldNotBeNullOrEmpty();
            argArginfo.ArgumentsList.ShouldBeEmpty();
            argArginfo.ArgumentsText.ShouldBeNullOrEmpty();
            argArginfo.SwitchesList.ShouldBeEmpty();

            // Test for correctness
            argArginfo.Command.ShouldBe("lang");
            argArginfo.RequiredArgumentsProvided.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedArgumentArgumentsInfo"/> instance from a command line argument
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeProvidedArgumentArgumentInfoInstanceFromCommandLineArgWithArg()
        {
            // Create instance
            var argArginfo = ArgumentsParser.ParseArgumentArguments("lang eng").total[0];

            // Test for null
            argArginfo.ShouldNotBeNull();
            argArginfo.Command.ShouldNotBeNullOrEmpty();
            argArginfo.ArgumentsList.ShouldNotBeEmpty();
            argArginfo.ArgumentsText.ShouldNotBeNullOrEmpty();
            argArginfo.SwitchesList.ShouldBeEmpty();

            // Test for correctness
            argArginfo.Command.ShouldBe("lang");
            argArginfo.ArgumentsList.ShouldHaveSingleItem();
            argArginfo.ArgumentsList.ShouldContain("eng");
            argArginfo.ArgumentsText.ShouldBe("eng");
            argArginfo.RequiredArgumentsProvided.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedArgumentArgumentsInfo"/> instance from a command line argument
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeProvidedArgumentArgumentInfoInstanceFromCommandLineArgWithSwitch()
        {
            // Create instance
            var argArginfo = ArgumentsParser.ParseArgumentArguments("lang -switch").total[0];

            // Test for null
            argArginfo.ShouldNotBeNull();
            argArginfo.Command.ShouldNotBeNullOrEmpty();
            argArginfo.ArgumentsList.ShouldBeEmpty();
            argArginfo.ArgumentsText.ShouldBeNullOrEmpty();
            argArginfo.SwitchesList.ShouldNotBeEmpty();

            // Test for correctness
            argArginfo.Command.ShouldBe("lang");
            argArginfo.RequiredArgumentsProvided.ShouldBeFalse();
            argArginfo.SwitchesList.ShouldHaveSingleItem();
            argArginfo.SwitchesList.ShouldContain("-switch");
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedArgumentArgumentsInfo"/> instance from a command line argument
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeProvidedArgumentArgumentInfoInstanceFromCommandLineArgFull()
        {
            // Create instance
            var argArginfo = ArgumentsParser.ParseArgumentArguments("lang -switch eng").total[0];

            // Test for null
            argArginfo.ShouldNotBeNull();
            argArginfo.Command.ShouldNotBeNullOrEmpty();
            argArginfo.ArgumentsList.ShouldNotBeEmpty();
            argArginfo.ArgumentsText.ShouldNotBeNullOrEmpty();
            argArginfo.SwitchesList.ShouldNotBeEmpty();

            // Test for correctness
            argArginfo.Command.ShouldBe("lang");
            argArginfo.ArgumentsList.ShouldHaveSingleItem();
            argArginfo.ArgumentsList.ShouldContain("eng");
            argArginfo.ArgumentsText.ShouldBe("eng");
            argArginfo.RequiredArgumentsProvided.ShouldBeTrue();
            argArginfo.SwitchesList.ShouldHaveSingleItem();
            argArginfo.SwitchesList.ShouldContain("-switch");
        }
    }
}
