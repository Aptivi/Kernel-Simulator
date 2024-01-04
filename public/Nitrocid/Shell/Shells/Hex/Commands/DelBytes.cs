﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Editors.HexEdit;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using Textify.General;

namespace Nitrocid.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Deletes the bytes
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a extraneous bytes in a specified range. You can use the print command to review the changes.
    /// </remarks>
    class DelBytesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 1)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
                {
                    if (Convert.ToInt64(parameters.ArgumentsList[0]) <= HexEditShellCommon.FileBytes.LongLength)
                    {
                        HexEditTools.DeleteBytes(Convert.ToInt64(parameters.ArgumentsList[0]));
                        TextWriters.Write(Translate.DoTranslation("Deleted bytes."), true, KernelColorType.Success);
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.HexEditor;
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Specified Byte number {0} is not a valid number."), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", parameters.ArgumentsList[0]);
                    return 10000 + (int)KernelExceptionType.HexEditor;
                }
            }
            else if (parameters.ArgumentsList.Length > 1)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]) & TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
                {
                    if (Convert.ToInt64(parameters.ArgumentsList[0]) <= HexEditShellCommon.FileBytes.LongLength & Convert.ToInt64(parameters.ArgumentsList[1]) <= HexEditShellCommon.FileBytes.LongLength)
                    {
                        long ByteNumberStart = Convert.ToInt64(parameters.ArgumentsList[0]);
                        long ByteNumberEnd = Convert.ToInt64(parameters.ArgumentsList[1]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.DeleteBytes(ByteNumberStart, ByteNumberEnd);
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.HexEditor;
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorType.Error);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", parameters.ArgumentsList[1]);
                    return 10000 + (int)KernelExceptionType.HexEditor;
                }
            }
            return 0;
        }

    }
}
