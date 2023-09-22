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

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Hardware;
using KS.Shell.ShellBase.Commands;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows hard disks (for scripts)
    /// </summary>
    /// <remarks>
    /// This shows you a list of hard disks.
    /// </remarks>
    class LsDisksCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            var hardDrives = HardwareProbe.HardwareInfo.Hardware.HDD.Keys.ToArray();
            ListWriterColor.WriteList(hardDrives);
            variableValue = $"[{string.Join(", ", hardDrives)}]";
            return 0;
        }

        public override int ExecuteDumb(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            var hardDrives = HardwareProbe.HardwareInfo.Hardware.HDD.Keys.ToArray();
            for (int i = 0; i < hardDrives.Length; i++)
            {
                string hardDrive = hardDrives[i];
                TextWriterColor.Write($"- [{i + 1}] {hardDrive}");
            }
            variableValue = $"[{string.Join(", ", hardDrives)}]";
            return 0;
        }

    }
}
