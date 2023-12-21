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

using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.ShellBase.Commands;

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

using Textify.Online.EnglishDictionary;

namespace KS.Shell.Commands
{
    class DictCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            DictionaryWord[] Words = DictionaryManager.GetWordInfo(ListArgsOnly[0]);

            // Iterate for each word
            foreach (DictionaryWord Word in Words)
            {
                // First, print the license out
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("License information"), true);
                TextWriterColor.Write("dictionaryapi.dev " + Translate.DoTranslation("API is licensed under") + $" {Word.LicenseInfo.Name}: {Word.LicenseInfo.Url}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));

                // Now, we can write the word information
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word information for") + $" {ListArgs[0]}", true);
                TextWriterColor.Write(Translate.DoTranslation("Word:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write($" {Word.Word}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));

                // Meanings...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word meanings for") + $" {ListArgs[0]}", true);
                foreach (DictionaryWord.Meaning MeaningBase in Word.Meanings)
                {
                    // Base part of speech
                    TextWriterColor.Write(Translate.DoTranslation("Part of Speech:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                    TextWriterColor.Write($" {MeaningBase.PartOfSpeech}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));

                    // Get the definitions
                    foreach (DictionaryWord.DefinitionType DefinitionBase in MeaningBase.Definitions)
                    {
                        // Write definition and, if applicable, example
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Definition:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                        TextWriterColor.Write($" {DefinitionBase.Definition}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Example in Sentence:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                        TextWriterColor.Write($" {DefinitionBase.Example}", true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));

                        // Now, write the specific synonyms (usually blank)
                        if (DefinitionBase.Synonyms.Any())
                        {
                            TextWriterColor.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                            ListWriterColor.WriteList(DefinitionBase.Synonyms);
                        }

                        // ...and the specific antonyms (usually blank)
                        if (DefinitionBase.Antonyms.Any())
                        {
                            TextWriterColor.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                            ListWriterColor.WriteList(DefinitionBase.Antonyms);
                        }
                    }

                    // Now, write the base synonyms (usually blank)
                    if (MeaningBase.Synonyms.Any())
                    {
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                        ListWriterColor.WriteList(MeaningBase.Synonyms);
                    }

                    // ...and the base antonyms (usually blank)
                    if (MeaningBase.Antonyms.Any())
                    {
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                        ListWriterColor.WriteList(MeaningBase.Antonyms);
                    }
                }

                // Sources...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Sources used to define") + $" {ListArgs[0]}", true);
                ListWriterColor.WriteList(Word.SourceUrls);
            }
        }

    }
}