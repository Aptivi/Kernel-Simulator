﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Paths;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text.Probers.Motd;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can change your message of the day after log-in
    /// </summary>
    /// <remarks>
    /// If you don't like the default message of the day after log-in that is generated by the kernel, then you can use this command to change the message and store it permanently on the config file.
    /// <br></br>
    /// It also has placeholder support, like if you have <c>&lt;shortdate&gt;</c> and <c>&lt;longtime&gt;</c> placeholders, the <c>&lt;shortdate&gt;</c> placeholder changes to the current system date in the MM/DD/YYYY form, and the <c>&lt;longtime&gt;</c> placeholder changes to the current system time in the HH:MM:SS AM/PM form.
    /// <br></br>
    /// If no arguments are specified, the text editor shell will open to the path of MAL text file.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ChMalCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length > 0)
            {
                if (string.IsNullOrEmpty(parameters.ArgumentsText))
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Blank MAL After Login."), true, KernelColorType.Error);
                    return 10000 + (int)KernelExceptionType.MOTD;
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Changing MAL..."));
                    MalParse.SetMal(parameters.ArgumentsText);
                    return 0;
                }
            }
            else
            {
                ShellManager.StartShell(ShellType.TextShell, PathsManagement.GetKernelPath(KernelPathType.MAL));
                TextWriterColor.Write(Translate.DoTranslation("Changing MAL..."));
                MalParse.ReadMal();
                return 0;
            }
        }

    }
}
