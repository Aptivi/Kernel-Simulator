﻿using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

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

using KS.TimeDate;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows time information
    /// </summary>
    /// <remarks>
    /// This shows you the detailed time information, including the time analysis, binary representation, and even the Unix time.
    /// </remarks>
    class GetTimeInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            DateTime DateTimeInfo;
            if (DateTime.TryParse(ListArgsOnly[0], out DateTimeInfo))
            {
                TextWriterColor.Write("-- " + Translate.DoTranslation("Information for") + " {0} --" + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Neutral, TimeDateRenderers.Render(DateTimeInfo));
                TextWriterColor.Write(Translate.DoTranslation("Milliseconds:") + " {0}", true, ColorTools.ColTypes.Neutral, DateTimeInfo.Millisecond);
                TextWriterColor.Write(Translate.DoTranslation("Seconds:") + " {0}", true, ColorTools.ColTypes.Neutral, DateTimeInfo.Second);
                TextWriterColor.Write(Translate.DoTranslation("Minutes:") + " {0}", true, ColorTools.ColTypes.Neutral, DateTimeInfo.Minute);
                TextWriterColor.Write(Translate.DoTranslation("Hours:") + " {0}", true, ColorTools.ColTypes.Neutral, DateTimeInfo.Hour);
                TextWriterColor.Write(Translate.DoTranslation("Days:") + " {0}", true, ColorTools.ColTypes.Neutral, DateTimeInfo.Day);
                TextWriterColor.Write(Translate.DoTranslation("Months:") + " {0}", true, ColorTools.ColTypes.Neutral, DateTimeInfo.Month);
                TextWriterColor.Write(Translate.DoTranslation("Year:") + " {0}" + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Neutral, DateTimeInfo.Year);
                TextWriterColor.Write(Translate.DoTranslation("Date:") + " {0}", true, ColorTools.ColTypes.Neutral, TimeDateRenderers.RenderDate(DateTimeInfo));
                TextWriterColor.Write(Translate.DoTranslation("Time:") + " {0}" + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Neutral, TimeDateRenderers.RenderTime(DateTimeInfo));
                TextWriterColor.Write(Translate.DoTranslation("Day of Year:") + " {0}", true, ColorTools.ColTypes.Neutral, DateTimeInfo.DayOfYear);
                TextWriterColor.Write(Translate.DoTranslation("Day of Week:") + " {0}" + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Neutral, DateTimeInfo.DayOfWeek.ToString());
                TextWriterColor.Write(Translate.DoTranslation("Binary:") + " {0}", true, ColorTools.ColTypes.Neutral, DateTimeInfo.ToBinary());
                TextWriterColor.Write(Translate.DoTranslation("Local Time:") + " {0}", true, ColorTools.ColTypes.Neutral, TimeDateRenderers.Render(DateTimeInfo.ToLocalTime()));
                TextWriterColor.Write(Translate.DoTranslation("Universal Time:") + " {0}", true, ColorTools.ColTypes.Neutral, TimeDateRenderers.Render(DateTimeInfo.ToUniversalTime()));
                TextWriterColor.Write(Translate.DoTranslation("Unix Time:") + " {0}", true, ColorTools.ColTypes.Neutral, TimeDateConverters.DateToUnix(DateTimeInfo));
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Failed to parse date information for") + " {0}. " + Translate.DoTranslation("Ensure that the format is correct."), true, ColorTools.ColTypes.Error, ListArgsOnly[0]);
            }
        }

    }
}