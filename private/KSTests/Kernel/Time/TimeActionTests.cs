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

using System.Globalization;
using NUnit.Framework;
using Shouldly;
using System;
using KS.Kernel.Time;
using KS.Kernel.Time.Renderers;

namespace KSTests.Kernel.Time
{

    [TestFixture]
    public class TimeActionTests
    {

        /// <summary>
        /// Tests rendering kernel date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDate() =>
            TimeDateRenderers.RenderDate().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel date with specific format type
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateType()
        {
            TimeDateRenderers.RenderDate(FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderDate(FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderDate(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture and format type
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderDate(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderDate(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateUtc() =>
            TimeDateRenderersUtc.RenderDateUtc().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel date with specific format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateUtcType()
        {
            TimeDateRenderersUtc.RenderDateUtc(FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderDateUtc(FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateUtcCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderDateUtc(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture and format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateUtcCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderDateUtc(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderDateUtc(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDate()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderers.RenderDate(TargetDate).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderers.RenderDate(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderDate(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateCult()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderDate(TargetDate, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateCultType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderDate(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderDate(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateUtc()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderersUtc.RenderDateUtc(TargetDate).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateUtcType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateUtcCult()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateUtcCultType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTime() =>
            TimeDateRenderers.RenderTime().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel time
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeType()
        {
            TimeDateRenderers.RenderTime(FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderTime(FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderTime(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderTime(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderTime(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeUtc() =>
            TimeDateRenderersUtc.RenderTimeUtc().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel time with specific format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeUtcType()
        {
            TimeDateRenderersUtc.RenderTimeUtc(FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderTimeUtc(FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeUtcCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderTimeUtc(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time with specified culture and format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeUtcCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderTimeUtc(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderTimeUtc(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTime()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            TimeDateRenderers.RenderTime(TargetTime).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeType()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            TimeDateRenderers.RenderTime(TargetTime, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderTime(TargetTime, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeCult()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderTime(TargetTime, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeCultType()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderTime(TargetTime, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderTime(TargetTime, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeUtc()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeUtcType()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeUtcCult()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeUtcCultType()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernel() =>
            TimeDateRenderers.Render().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelType()
        {
            TimeDateRenderers.Render(FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.Render(FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with custom format
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelCustomFormat()
        {
            string formatted = "";
            Should.NotThrow(() => formatted = TimeDateRenderers.Render("yyyy-MM-dd--hh-mm-ss"));
            formatted.ShouldNotBeNullOrEmpty();
            formatted.ShouldMatch( /* lang=regex */ @"[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]--[0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.Render(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture and format
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.Render(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.Render(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture and custom format
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelCultCustomFormat()
        {
            string formatted = "";
            var TargetCult = new CultureInfo("es-ES");
            Should.NotThrow(() => formatted = TimeDateRenderers.Render(TargetCult, "yyyy-MM-dd--hh-mm-ss"));
            formatted.ShouldNotBeNullOrEmpty();
            formatted.ShouldMatch( /* lang=regex */ @"[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]--[0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
        }

        /// <summary>
        /// Tests rendering kernel date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtc() =>
            TimeDateRenderersUtc.RenderUtc().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel date with specific format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtcType()
        {
            TimeDateRenderersUtc.RenderUtc(FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderUtc(FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with custom format (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtcCustomFormat()
        {
            string formatted = "";
            Should.NotThrow(() => formatted = TimeDateRenderersUtc.RenderUtc("yyyy-MM-dd--hh-mm-ss"));
            formatted.ShouldNotBeNullOrEmpty();
            formatted.ShouldMatch( /* lang=regex */ @"[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]--[0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtcCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderUtc(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture and format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtcCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderUtc(TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderUtc(TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture and custom format (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtcCultCustomFormat()
        {
            string formatted = "";
            var TargetCult = new CultureInfo("es-ES");
            Should.NotThrow(() => formatted = TimeDateRenderersUtc.RenderUtc(TargetCult, "yyyy-MM-dd--hh-mm-ss"));
            formatted.ShouldNotBeNullOrEmpty();
            formatted.ShouldMatch( /* lang=regex */ @"[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]--[0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
        }

        /// <summary>
        /// Tests rendering custom date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustom()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderers.Render(TargetDate).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderers.Render(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.Render(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with custom format
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomFormat()
        {
            string formatted = "";
            var TargetDate = new DateTime(2018, 2, 22);
            Should.NotThrow(() => formatted = TimeDateRenderers.Render(TargetDate, "yyyy-MM-dd--hh-mm-ss"));
            formatted.ShouldNotBeNullOrEmpty();
            formatted.ShouldMatch( /* lang=regex */ @"[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]--[0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
        }

        /// <summary>
        /// Tests rendering custom date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomCult()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.Render(TargetDate, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomCultType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.Render(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.Render(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with custom format
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomCultFormat()
        {
            string formatted = "";
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            Should.NotThrow(() => formatted = TimeDateRenderers.Render(TargetDate, TargetCult, "yyyy-MM-dd--hh-mm-ss"));
            formatted.ShouldNotBeNullOrEmpty();
            formatted.ShouldMatch( /* lang=regex */ @"[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]--[0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
        }

        /// <summary>
        /// Tests rendering custom date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtc()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderersUtc.RenderUtc(TargetDate).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtcType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderersUtc.RenderUtc(TargetDate, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderUtc(TargetDate, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with custom format (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtcFormat()
        {
            string formatted = "";
            var TargetDate = new DateTime(2018, 2, 22);
            Should.NotThrow(() => formatted = TimeDateRenderersUtc.RenderUtc(TargetDate, "yyyy-MM-dd--hh-mm-ss"));
            formatted.ShouldNotBeNullOrEmpty();
            formatted.ShouldMatch( /* lang=regex */ @"[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]--[0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
        }

        /// <summary>
        /// Tests rendering custom date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtcCult()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderUtc(TargetDate, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtcCultType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderUtc(TargetDate, TargetCult, FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderUtc(TargetDate, TargetCult, FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        // TODO: Public API doesn't implement below RenderUtc overload. Implement in Beta 3.
#if UNDONE
        /// <summary>
        /// Tests rendering custom date with specified culture and custom format (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtcCultFormat()
        {
            string formatted = "";
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            Should.NotThrow(() => formatted = TimeDateRenderersUtc.RenderUtc(TargetDate, TargetCult, "yyyy-MM-dd--hh-mm-ss"));
            formatted.ShouldNotBeNullOrEmpty();
            formatted.ShouldMatch( /* lang=regex */ @"[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]--[0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
        }
#endif

    }
}
