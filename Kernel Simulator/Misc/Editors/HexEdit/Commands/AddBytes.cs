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

using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
namespace KS.Misc.Editors.HexEdit.Commands
{
    class HexEdit_AddBytesCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var FinalBytes = new List<byte>();
            string FinalByte = "";

            // Keep prompting for bytes until the user finishes
            TextWriters.Write(Translate.DoTranslation("Enter a byte on its own line that you want to append to the end of the file. When you're done, write \"EOF\" on its own line."), true, KernelColorTools.ColTypes.Neutral);
            while (FinalByte != "EOF")
            {
                TextWriters.Write(">> ", false, KernelColorTools.ColTypes.Input);
                FinalByte = Input.ReadLine(false);
                if (!(FinalByte == "EOF"))
                {
                    if (byte.TryParse(FinalByte, System.Globalization.NumberStyles.HexNumber, null, out byte ByteContent))
                    {
                        FinalBytes.Add(ByteContent);
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Not a valid byte."), true, KernelColorTools.ColTypes.Error);
                    }
                }
            }

            // Add the new bytes
            HexEditTools.HexEdit_AddNewBytes([.. FinalBytes]);
        }

    }
}