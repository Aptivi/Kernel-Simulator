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

using KS.Login;

using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Login
{

    [TestFixture]
    public class UserManagementTests
    {

        /// <summary>
        /// Tests user addition
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddUser()
        {
            UserManagement.AddUser("Account1").ShouldBeTrue();
            UserManagement.AddUser("Account2", "password").ShouldBeTrue();
        }

        /// <summary>
        /// Tests username change
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestChangeUser()
        {
            UserManagement.TryChangeUsername("Account2", "Account3").ShouldBeTrue();
        }

        /// <summary>
        /// Tests username change
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetUserProperty()
        {
            UserManagement.GetUserProperty("Account3", UserManagement.UserProperty.Username).ShouldBe("Account3");
            ((JArray)UserManagement.GetUserProperty("Account3", UserManagement.UserProperty.Permissions)).ShouldBeEmpty();
        }

        /// <summary>
        /// Tests removing user
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestRemoveUser()
        {
            UserManagement.TryRemoveUser("Account1").ShouldBeTrue();
            UserManagement.TryRemoveUser("Account3").ShouldBeTrue();
        }

    }
}