﻿
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Misc.Splash;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Misc.Splash
{

    [TestFixture]
    public class SplashTests
    {

        /// <summary>
        /// Tests getting splash names
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestGetSplashNames()
        {
            var names = SplashManager.GetNamesOfSplashes();
            names.ShouldNotBeNull();
            names.ShouldNotBeEmpty();
            names.ShouldContain("openrc");
        }

        /// <summary>
        /// Tests getting splash from name
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestGetSplashFromName()
        {
            var names = SplashManager.GetSplashFromName("openrc");
            names.ShouldNotBeNull();
            names.SplashName.ShouldBe("openrc");
        }

    }
}
