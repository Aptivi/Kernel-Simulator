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

using System.IO;
using KS.Misc.Editors.JsonShell;
using KS.Shell.Shells.Json;
using NUnit.Framework;
using Shouldly;

namespace KSTests.MiscTests
{

    [TestFixture]
    public class JsonShellInitializationTests
    {

        /// <summary>
        /// Tests opening, saving, and closing a JSON file
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestOpenSaveCloseJsonFile()
        {
            string PathToTestJson = Path.GetFullPath("TestData/TestJson.json");
            JsonTools.JsonShell_OpenJsonFile(PathToTestJson).ShouldBeTrue();
            JsonTools.JsonShell_AddNewProperty("$", "HowText", "How are you today?");
            JsonShellCommon.JsonShell_FileToken["HowText"].ShouldNotBeNull();
            JsonTools.JsonShell_GetProperty("HelloText").ShouldNotBeNull();
            JsonTools.JsonShell_SerializeToString("HelloText").ShouldNotBeNullOrEmpty();
            JsonTools.JsonShell_RemoveProperty("HowText");
            JsonTools.JsonShell_SaveFile(false).ShouldBeTrue();
            JsonTools.JsonShell_CloseTextFile().ShouldBeTrue();
        }

    }
}
