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

using System;
using System.IO;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using KS.Network.RemoteDebug.Interface;

namespace KS.Network.RemoteDebug.Commands
{
    class Debug_TraceCommand : RemoteDebugCommandExecutor, IRemoteDebugCommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, StreamWriter SocketStreamWriter, string DeviceAddress)
        {
            if (DebugWriter.DebugStackTraces.Count != 0)
            {
                if ((ListArgs?.Length) is { } arg1 && arg1 != 0)
                {
                    try
                    {
                        SocketStreamWriter.WriteLine(DebugWriter.DebugStackTraces[Convert.ToInt32(ListArgs[0])]);
                    }
                    catch (Exception)
                    {
                        SocketStreamWriter.WriteLine(Translate.DoTranslation("Index {0} invalid. There are {1} stack traces. Index is zero-based, so try subtracting by 1."), ListArgs[0], DebugWriter.DebugStackTraces.Count);
                    }
                }
                else
                {
                    SocketStreamWriter.WriteLine(DebugWriter.DebugStackTraces[0]);
                }
            }
            else
            {
                SocketStreamWriter.WriteLine(Translate.DoTranslation("No stack trace"));
            }
        }

    }
}