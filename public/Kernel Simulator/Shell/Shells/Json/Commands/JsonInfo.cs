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

using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Colors;
using Newtonsoft.Json.Linq;
using KS.Misc.Writers.FancyWriters;

namespace KS.Shell.Shells.Json.Commands
{
    /// <summary>
    /// Gets information about the JSON file and its contents
    /// </summary>
    class JsonShell_JsonInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // Base info
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Base JSON token information"), true);
            TextWriterColor.Write(Translate.DoTranslation("Base type") + ": {0}", true, ColorTools.ColTypes.NeutralText, JsonShellCommon.JsonShell_FileToken.Type);
            TextWriterColor.Write(Translate.DoTranslation("Base has values") + ": {0}", true, ColorTools.ColTypes.NeutralText, JsonShellCommon.JsonShell_FileToken.HasValues);
            TextWriterColor.Write(Translate.DoTranslation("Base path") + ": {0}", true, ColorTools.ColTypes.NeutralText, JsonShellCommon.JsonShell_FileToken.Path);
            TextWriterColor.Write();

            // Individual properties
            foreach (var token in JsonShellCommon.JsonShell_FileToken)
            {
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Individual JSON token information") + " [{0}]", true, token.Path);
                TextWriterColor.Write(Translate.DoTranslation("Token type") + ": {0}", true, ColorTools.ColTypes.NeutralText, token.Type);
                TextWriterColor.Write(Translate.DoTranslation("Token has values") + ": {0}", true, ColorTools.ColTypes.NeutralText, token.HasValues);
                TextWriterColor.Write(Translate.DoTranslation("Token path") + ": {0}", true, ColorTools.ColTypes.NeutralText, token.Path);
                TextWriterColor.Write(Translate.DoTranslation("Token value") + ": {0}", true, ColorTools.ColTypes.NeutralText, token);
                TextWriterColor.Write();

                // Check to see if the token is a property
                if (token.Type == JTokenType.Property)
                {
                    SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Property information for") + " [{0}]", true, token.Path);
                    TextWriterColor.Write(Translate.DoTranslation("Property type") + ": {0}", true, ColorTools.ColTypes.NeutralText, ((JProperty)token).Value.Type);
                    TextWriterColor.Write(Translate.DoTranslation("Property count") + ": {0}", true, ColorTools.ColTypes.NeutralText, ((JProperty)token).Count);
                    TextWriterColor.Write(Translate.DoTranslation("Property name") + ": {0}", true, ColorTools.ColTypes.NeutralText, ((JProperty)token).Name);
                    TextWriterColor.Write(Translate.DoTranslation("Property value") + ": {0}", true, ColorTools.ColTypes.NeutralText, ((JProperty)token).Value);
                    TextWriterColor.Write(Translate.DoTranslation("Property path") + ": {0}", true, ColorTools.ColTypes.NeutralText, ((JProperty)token).Path);
                    TextWriterColor.Write();
                }
            }
        }

    }
}
