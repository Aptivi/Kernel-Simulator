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

using System;
using System.Linq;
using System.Threading;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Misc.Amusements.Quotes;
using KS.Misc.Text;
using Terminaux.Colors;

namespace KS.Misc.Splash.Splashes
{
    class SplashQuote : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "Quote";

        public int ProgressWritePositionX => 3;

        public int ProgressWritePositionY
        {
            get
            {
                return Config.SplashConfig.QuoteProgressTextLocation switch
                {
                    (int)TextLocation.Top    => 1,
                    (int)TextLocation.Bottom => ConsoleWrapper.WindowHeight - 2,
                    _                        => 1,
                };
            }
        }

        public int ProgressReportWritePositionX => 9;

        public int ProgressReportWritePositionY
        {
            get
            {
                return Config.SplashConfig.QuoteProgressTextLocation switch
                {
                    (int)TextLocation.Top    => 1,
                    (int)TextLocation.Bottom => ConsoleWrapper.WindowHeight - 2,
                    _                        => 1,
                };
            }
        }

        // Actual logic
        public override void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Display the quote
                Color quoteColor = KernelColorTools.GetRandomColor(ColorType.TrueColor);
                string renderedQuote = RandomQuotes.RenderQuote();
                string[] quoteSplit = renderedQuote.SplitNewLines();
                int maxLength = quoteSplit.Max((quote) => quote.Length);
                int halfConsoleY = (ConsoleWrapper.WindowHeight / 2) - (quoteSplit.Length / 2);
                int quotePosX = (ConsoleWrapper.WindowWidth / 2) - (maxLength / 2);
                for (int i = 0; i < quoteSplit.Length; i++)
                {
                    int currentY = halfConsoleY + i;
                    string str = quoteSplit[i];
                    TextWriterWhereColor.WriteWhere(str, quotePosX, currentY, quoteColor);
                }

                // Display the progress text
                UpdateProgressReport(SplashReport.Progress, false, false, SplashReport.ProgressText, ProgressWritePositionX, ProgressWritePositionY, ProgressReportWritePositionX, ProgressReportWritePositionY);

                // Loop until closing
                while (!SplashClosing)
                    Thread.Sleep(1);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public override void Report(int Progress, string ProgressReport, params object[] Vars) =>
            UpdateProgressReport(Progress, false, false, ProgressReport, Vars);

        public override void ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars) =>
            UpdateProgressReport(Progress, false, true, WarningReport, Vars);

        public override void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) =>
            UpdateProgressReport(Progress, true, false, ErrorReport, Vars);

        /// <summary>
        /// Updates the splash progress
        /// </summary>
        /// <param name="Progress">Progress percentage from 0 to 100</param>
        /// <param name="ProgressErrored">The progress error or not</param>
        /// <param name="ProgressWarning">The progress warning or not</param>
        /// <param name="ProgressReport">The progress text</param>
        /// <param name="Vars">Variables to be formatted in the text</param>
        public void UpdateProgressReport(int Progress, bool ProgressErrored, bool ProgressWarning, string ProgressReport, params object[] Vars)
        {
            string RenderedText = ProgressReport.Truncate(ConsoleWrapper.WindowWidth - ProgressReportWritePositionX - ProgressWritePositionX - 3);
            TextWriterWhereColor.WriteWhere("{0}%", ProgressWritePositionX, ProgressWritePositionY, true, KernelColorType.Progress, Progress.ToString().PadLeft(3));
            TextWriterWhereColor.WriteWhere($"{(ProgressErrored ? "[X] " : "")}{RenderedText}", ProgressReportWritePositionX, ProgressReportWritePositionY, false, KernelColorType.Error, Vars);
            TextWriterWhereColor.WriteWhere($"{(ProgressWarning ? "[!] " : "")}{RenderedText}", ProgressReportWritePositionX, ProgressReportWritePositionY, false, KernelColorType.Warning, Vars);
            ConsoleExtensions.ClearLineToRight();
        }

    }
}
