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

using System.Reflection;
using Figgle;
using KS.Languages;
using KS.Misc.Reflection;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

    [TestFixture]
    public class PropertyManagementTests
    {

        /// <summary>
    /// Tests checking field
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckProperty()
        {
            PropertyManager.CheckProperty("PersonLookupDelay").ShouldBeTrue();
        }

        /// <summary>
    /// Tests getting value
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetPropertyValue()
        {
            string Value = Conversions.ToString(PropertyManager.GetPropertyValue("PersonLookupDelay"));
            Value.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
    /// Tests setting value
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestSetPropertyValue()
        {
            PropertyManager.SetPropertyValue("PersonLookupDelay", 100);
            int Value = Conversions.ToInteger(PropertyManager.GetPropertyValue("PersonLookupDelay"));
            Value.ShouldBe(100);
        }

        /// <summary>
    /// Tests getting variable
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetConfigProperty()
        {
            var PropertyInfo = PropertyManager.GetProperty("PersonLookupDelay");
            PropertyInfo.Name.ShouldBe("PersonLookupDelay");
        }

        /// <summary>
    /// Tests getting properties
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetProperties()
        {
            var Properties = PropertyManager.GetProperties(typeof(FiggleFonts));
            Properties.ShouldNotBeNull();
            Properties.ShouldNotBeEmpty();
        }

        /// <summary>
    /// Tests getting properties
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetPropertiesNoEvaluation()
        {
            var Properties = PropertyManager.GetPropertiesNoEvaluation(typeof(FiggleFonts));
            Properties.ShouldNotBeNull();
            Properties.ShouldNotBeEmpty();
        }

        /// <summary>
    /// Tests getting property value from variable
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetPropertyValueInVariable()
        {
            string Value = Conversions.ToString(PropertyManager.GetPropertyValueInVariable(nameof(CultureManager.CurrentCult), nameof(CultureManager.CurrentCult.Name)));
            Value.ShouldNotBeNullOrEmpty();
        }

    }
}