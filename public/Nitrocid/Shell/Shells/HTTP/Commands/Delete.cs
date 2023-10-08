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
using KS.Languages;
using KS.Network.HTTP;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Inputs.Styles;
using KS.Misc.Text;
using KS.ConsoleBase.Writers.ConsoleWriters;

namespace KS.Shell.Shells.HTTP.Commands
{
    /// <summary>
    /// Removes content from the HTTP server
    /// </summary>
    /// <remarks>
    /// If you want to test a DELETE function of the REST API, you can do so using this command.
    /// </remarks>
    class HTTP_DeleteCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Print a message
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Deleting {0}..."), true, KernelColorType.Progress, parameters.ArgumentsList[0]);

            // Make a confirmation message so user will not accidentally delete a file or folder
            string answer = ChoiceStyle.PromptChoice(TextTools.FormatString(Translate.DoTranslation("Are you sure you want to delete {0}?"), parameters.ArgumentsList[0]), "y/n");
            if (answer != "y")
                return 1;

            try
            {
                var DeleteTask = HTTPTools.HttpDelete(parameters.ArgumentsList[0]);
                DeleteTask.Wait();
                return 0;
            }
            catch (AggregateException aex)
            {
                TextWriterColor.WriteKernelColor(aex.Message + ":", true, KernelColorType.Error);
                foreach (Exception InnerException in aex.InnerExceptions)
                {
                    TextWriterColor.WriteKernelColor("- " + InnerException.Message, true, KernelColorType.Error);
                    if (InnerException.InnerException is not null)
                    {
                        TextWriterColor.WriteKernelColor("- " + InnerException.InnerException.Message, true, KernelColorType.Error);
                    }
                }
                return aex.GetHashCode();
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteKernelColor(ex.Message, true, KernelColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
