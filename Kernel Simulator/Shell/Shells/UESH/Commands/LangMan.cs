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

using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages your custom languages
    /// </summary>
    /// <remarks>
    /// You can manage all your custom languages installed in Kernel Simulator by this command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class LangManCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!Flags.SafeMode)
            {
                string CommandMode = ListArgsOnly[0].ToLower();
                string TargetLanguage = "";
                string TargetLanguagePath = "";
                string LanguageListTerm = "";

                // These command modes require two arguments to be passed, so re-check here and there. Optional arguments also lie there.
                switch (CommandMode ?? "")
                {
                    case "reload":
                    case "load":
                    case "unload":
                        {
                            if (ListArgsOnly.Length > 1)
                            {
                                TargetLanguage = ListArgsOnly[1];
                                TargetLanguagePath = Filesystem.NeutralizePath(TargetLanguage + ".json", Paths.GetKernelPath(KernelPathType.CustomLanguages));
                                if (!(Parsing.TryParsePath(TargetLanguagePath) && Checking.FileExists(TargetLanguagePath)) & !LanguageManager.Languages.ContainsKey(TargetLanguage))
                                {
                                    TextWriterColor.Write(Translate.DoTranslation("Language not found or file has invalid characters."), true, ColorTools.ColTypes.Error);
                                    return;
                                }
                            }
                            else
                            {
                                TextWriterColor.Write(Translate.DoTranslation("Language is not specified."), true, ColorTools.ColTypes.Error);
                                return;
                            }

                            break;
                        }
                    case "list":
                        {
                            if (ListArgsOnly.Length > 1)
                            {
                                LanguageListTerm = ListArgsOnly[1];
                            }

                            break;
                        }
                }

                // Now, the actual logic
                switch (CommandMode ?? "")
                {
                    case "reload":
                        {
                            LanguageManager.UninstallCustomLanguage(TargetLanguagePath);
                            LanguageManager.InstallCustomLanguage(TargetLanguagePath);
                            break;
                        }
                    case "load":
                        {
                            LanguageManager.InstallCustomLanguage(TargetLanguage);
                            break;
                        }
                    case "unload":
                        {
                            LanguageManager.UninstallCustomLanguage(TargetLanguage);
                            break;
                        }
                    case "list":
                        {
                            foreach (string Language in LanguageManager.ListLanguages(LanguageListTerm).Keys)
                            {
                                SeparatorWriterColor.WriteSeparator(Language, true);
                                TextWriterColor.Write("- " + Translate.DoTranslation("Language short name:") + " ", false, ColorTools.ColTypes.ListEntry);
                                TextWriterColor.Write(LanguageManager.Languages[Language].ThreeLetterLanguageName, true, ColorTools.ColTypes.ListValue);
                                TextWriterColor.Write("- " + Translate.DoTranslation("Language full name:") + " ", false, ColorTools.ColTypes.ListEntry);
                                TextWriterColor.Write(LanguageManager.Languages[Language].FullLanguageName, true, ColorTools.ColTypes.ListValue);
                                TextWriterColor.Write("- " + Translate.DoTranslation("Language transliterable:") + " ", false, ColorTools.ColTypes.ListEntry);
                                TextWriterColor.Write($"{LanguageManager.Languages[Language].Transliterable}", true, ColorTools.ColTypes.ListValue);
                                TextWriterColor.Write("- " + Translate.DoTranslation("Custom language:") + " ", false, ColorTools.ColTypes.ListEntry);
                                TextWriterColor.Write($"{LanguageManager.Languages[Language].Custom}", true, ColorTools.ColTypes.ListValue);
                            }

                            break;
                        }
                    case "reloadall":
                        {
                            LanguageManager.UninstallCustomLanguages();
                            LanguageManager.InstallCustomLanguages();
                            break;
                        }

                    default:
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Invalid command {0}. Check the usage below:"), true, ColorTools.ColTypes.Error, CommandMode);
                            HelpSystem.ShowHelp("langman");
                            break;
                        }
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Language management is disabled in safe mode."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}