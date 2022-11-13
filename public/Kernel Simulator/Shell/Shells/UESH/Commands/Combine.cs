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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Combines the two text files or more into the output file.
    /// </summary>
    /// <remarks>
    /// If you have two or more fragments of a complete text file, you can combine them using this command to generate a complete text file.
    /// </remarks>
    class CombineCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string OutputPath = Filesystem.NeutralizePath(ListArgsOnly[0]);
            string InputPath = ListArgsOnly[1];
            var CombineInputPaths = ListArgsOnly.Skip(2).ToArray();

            // Check all inputs
            bool AreAllInputsBinary = false;
            bool AreAllInputsText = false;
            bool IsInputBinary = Parsing.IsBinaryFile(InputPath);

            // Get all the input states and make them true if all binary
            List<bool> InputStates = new();
            foreach (string CombineInputPath in CombineInputPaths)
                InputStates.Add(Parsing.IsBinaryFile(CombineInputPath));

            // Check to see if all inputs are either binary or text.
            // TODO: Mixed combination?
            AreAllInputsBinary = InputStates.Count == InputStates.Where((binary) => binary).Count();
            AreAllInputsText = InputStates.Count == InputStates.Where((binary) => !binary).Count();
            if (!AreAllInputsBinary && !AreAllInputsText)
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Can't combine a mix of text and binary files."));

            // Make a combined content array
            if (AreAllInputsText)
            {
                var CombinedContents = Combination.CombineTextFiles(InputPath, CombineInputPaths);
                Making.MakeFile(OutputPath, false);
                File.WriteAllLines(OutputPath, CombinedContents);
            }
            else
            {
                var CombinedContents = Combination.CombineBinaryFiles(InputPath, CombineInputPaths);
                Making.MakeFile(OutputPath, false);
                File.WriteAllBytes(OutputPath, CombinedContents);
            }
        }

    }
}
