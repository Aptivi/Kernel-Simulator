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
using KS.Files;
using KS.Misc.Probers;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

    [TestFixture]
    [Order(1)]
    public class MOTDSettingTests
    {

        /// <summary>
        /// Tests setting MOTD
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestSetMOTD()
        {
            MOTDParse.SetMOTD(PlaceParse.ProbePlaces("Hello, I am on <system>"), MOTDParse.MessageType.MOTD);
            var MOTDFile = new StreamReader(Paths.GetKernelPath(KernelPathType.MOTD));
            MOTDFile.ReadLine().ShouldBe(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            MOTDFile.Close();
        }

        /// <summary>
        /// Tests setting MAL
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestSetMAL()
        {
            MOTDParse.SetMOTD(PlaceParse.ProbePlaces("Hello, I am on <system>"), MOTDParse.MessageType.MAL);
            var MALFile = new StreamReader(Paths.GetKernelPath(KernelPathType.MAL));
            MALFile.ReadLine().ShouldBe(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            MALFile.Close();
        }

    }
}