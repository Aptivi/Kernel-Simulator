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
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Shells;

namespace KS.Misc.Writers.MiscWriters
{
    public static class Decisive
    {

        /// <summary>
        /// Decides where to write the text
        /// </summary>
        /// <param name="CommandType">A specified command type</param>
        /// <param name="DebugDeviceSocket">Only for remote debug shell. Specifies the debug device socket.</param>
        /// <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void DecisiveWrite(ShellType CommandType, StreamWriter DebugDeviceSocket, string Text, bool Line, ColorTools.ColTypes colorType, params object[] vars)
        {
            if (!(CommandType == ShellType.RemoteDebugShell))
            {
                TextWriterColor.Write(Text, Line, colorType, vars);
            }
            else if (DebugDeviceSocket is not null)
            {
                if (Line)
                {
                    DebugDeviceSocket.WriteLine(Text, vars);
                }
                else
                {
                    DebugDeviceSocket.Write(Text, vars);
                }
            }
        }

    }
}