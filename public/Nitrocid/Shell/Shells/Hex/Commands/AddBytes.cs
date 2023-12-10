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

using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Editors.HexEdit;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Adds new bytes at the end of the file
    /// </summary>
    /// <remarks>
    /// You can use this command to add new bytes at the end of the file.
    /// </remarks>
    class AddBytesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var FinalBytes = new List<byte>();
            string FinalByte = "";

            // Keep prompting for bytes until the user finishes
            TextWriterColor.Write(Translate.DoTranslation("Enter a byte on its own line that you want to append to the end of the file. When you're done, write \"EOF\" on its own line."));
            while (FinalByte != "EOF")
            {
                TextWriterColor.WriteKernelColor(">> ", false, KernelColorType.Input);
                FinalByte = Input.ReadLine();
                if (FinalByte != "EOF")
                {
                    if (byte.TryParse(FinalByte, System.Globalization.NumberStyles.HexNumber, null, out byte ByteContent))
                    {
                        FinalBytes.Add(ByteContent);
                    }
                    else
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Not a valid byte."), true, KernelColorType.Error);
                    }
                }
            }

            // Add the new bytes
            HexEditTools.AddNewBytes([.. FinalBytes]);
            return 0;
        }

    }
}
