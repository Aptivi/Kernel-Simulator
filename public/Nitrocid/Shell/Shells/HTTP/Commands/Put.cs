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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Network.HTTP;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.HTTP.Commands
{
    class HTTP_PutCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            // Print a message
            TextWriterColor.Write(Translate.DoTranslation("Uploading file {0}..."), true, KernelColorType.Progress, ListArgsOnly[1]);

            try
            {
                var ResponseTask = HTTPTools.HttpPutFile(ListArgsOnly[0], ListArgsOnly[1]);
                ResponseTask.Wait();
                var Response = ResponseTask.Result;
                string ResponseContent = Response.Content.ReadAsStringAsync().Result;
                TextWriterColor.Write("[{0}] {1}", (int)Response.StatusCode, Response.StatusCode.ToString());
                TextWriterColor.Write(ResponseContent);
                TextWriterColor.Write(Response.ReasonPhrase);
                return 0;
            }
            catch (AggregateException aex)
            {
                TextWriterColor.Write(aex.Message + ":", true, KernelColorType.Error);
                foreach (Exception InnerException in aex.InnerExceptions)
                {
                    TextWriterColor.Write("- " + InnerException.Message, true, KernelColorType.Error);
                    if (InnerException.InnerException is not null)
                    {
                        TextWriterColor.Write("- " + InnerException.InnerException.Message, true, KernelColorType.Error);
                    }
                }
                return aex.GetHashCode();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
