﻿using System;
using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;

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

using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Changes attributes of file
    /// </summary>
    /// <remarks>
    /// You can use this command to change attributes of a file.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Attribute</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>Normal</term>
    /// <description>The file is a normal file</description>
    /// </item>
    /// <item>
    /// <term>ReadOnly</term>
    /// <description>The file is a read-only file</description>
    /// </item>
    /// <item>
    /// <term>Hidden</term>
    /// <description>The file is a hidden file</description>
    /// </item>
    /// <item>
    /// <term>Archive</term>
    /// <description>The file is an archive. Used for backups.</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ChAttrCommand : CommandExecutor, ICommand
    {

        // Warning: Don't use ListSwitchesOnly to replace ListArgsOnly(1); the removal signs of ChAttr are treated as switches and will cause unexpected behavior if changed.
        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string NeutralizedFilePath = Filesystem.NeutralizePath(ListArgsOnly[0]);
            if (Checking.FileExists(NeutralizedFilePath))
            {
                if (ListArgsOnly[1].EndsWith("Normal") | ListArgsOnly[1].EndsWith("ReadOnly") | ListArgsOnly[1].EndsWith("Hidden") | ListArgsOnly[1].EndsWith("Archive"))
                {
                    if (ListArgsOnly[1].StartsWith("+"))
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), ListArgsOnly[1].Remove(0, 1)));
                        if (AttributeManager.TryAddAttributeToFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Attribute has been added successfully."), true, ColorTools.ColTypes.Neutral, ListArgsOnly[1]);
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Failed to add attribute."), true, ColorTools.ColTypes.Neutral, ListArgsOnly[1]);
                        }
                    }
                    else if (ListArgsOnly[1].StartsWith("-"))
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), ListArgsOnly[1].Remove(0, 1)));
                        if (AttributeManager.TryRemoveAttributeFromFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Attribute has been removed successfully."), true, ColorTools.ColTypes.Neutral, ListArgsOnly[1]);
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Failed to remove attribute."), true, ColorTools.ColTypes.Neutral, ListArgsOnly[1]);
                        }
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Attribute \"{0}\" is invalid."), true, ColorTools.ColTypes.Error, ListArgsOnly[1]);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File not found."), true, ColorTools.ColTypes.Error);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("where <attributes> is one of the following:"), true, ColorTools.ColTypes.Neutral);
            TextWriterColor.Write("- Normal: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("The file is a normal file"), true, ColorTools.ColTypes.ListValue);                   // Normal   = 128
            TextWriterColor.Write("- ReadOnly: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("The file is a read-only file"), true, ColorTools.ColTypes.ListValue);              // ReadOnly = 1
            TextWriterColor.Write("- Hidden: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("The file is a hidden file"), true, ColorTools.ColTypes.ListValue);                   // Hidden   = 2
            TextWriterColor.Write("- Archive: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("The file is an archive. Used for backups."), true, ColorTools.ColTypes.ListValue);  // Archive  = 32
        }

    }
}