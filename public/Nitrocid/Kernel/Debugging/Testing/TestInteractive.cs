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

using KS.ConsoleBase;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging.Testing.Facades;
using KS.Languages;
using System.Collections.Generic;
using System.Linq;

namespace KS.Kernel.Debugging.Testing
{
    internal static class TestInteractive
    {
        internal static Dictionary<string, TestFacade> facades = new()
        {
            { "Print",                          new Print() },
            { "PrintF",                         new PrintF() },
            { "PrintD",                         new PrintD() },
            { "PrintDF",                        new PrintDF() },
            { "PrintSep",                       new PrintSep() },
            { "PrintSepF",                      new PrintSepF() },
            { "PrintSepColor",                  new PrintSepColor() },
            { "PrintSepColorF",                 new PrintSepColorF() },
            { "PrintFiglet",                    new PrintFiglet() },
            { "PrintFigletF",                   new PrintFigletF() },
            { "PrintPlaces",                    new PrintPlaces() },
            { "PrintWithNewLines",              new PrintWithNewLines() },
            { "PrintWithNulls",                 new PrintWithNulls() },
            { "Debug",                          new Debug() },
            { "RDebug",                         new RDebug() },
            { "TestDictWriterStr",              new TestDictWriterStr() },
            { "TestDictWriterInt",              new TestDictWriterInt() },
            { "TestDictWriterChar",             new TestDictWriterChar() },
            { "TestListWriterStr",              new TestListWriterStr() },
            { "TestListWriterInt",              new TestListWriterInt() },
            { "TestListWriterChar",             new TestListWriterChar() },
            { "TestCRC32",                      new TestCRC32() },
            { "TestMD5",                        new TestMD5() },
            { "TestSHA1",                       new TestSHA1() },
            { "TestSHA256",                     new TestSHA256() },
            { "TestSHA384",                     new TestSHA384() },
            { "TestSHA512",                     new TestSHA512() },
            { "TestArgs",                       new TestArgs() },
            { "TestSwitches",                   new TestSwitches() },
            { "TestExecuteAssembly",            new TestExecuteAssembly() },
            { "TestEvent",                      new TestEvent() },
            { "TestTable",                      new TestTable() },
            { "ShowTime",                       new ShowTime() },
            { "ShowDate",                       new ShowDate() },
            { "ShowTimeDate",                   new ShowTimeDate() },
            { "ShowTimeUtc",                    new ShowTimeUtc() },
            { "ShowDateUtc",                    new ShowDateUtc() },
            { "ShowTimeDateUtc",                new ShowTimeDateUtc() },
            { "LoadMods",                       new LoadMods() },
            { "StopMods",                       new StopMods() },
            { "ReloadMods",                     new ReloadMods() },
            { "BlacklistMod",                   new BlacklistMod() },
            { "BlacklistModUndo",               new BlacklistModUndo() },
            { "Panic",                          new Panic() },
            { "PanicF",                         new PanicF() },
            { "CheckString",                    new CheckString() },
            { "CheckStrings",                   new CheckStrings() },
            { "CheckLocalizationLines",         new CheckLocalizationLines() },
            { "CheckSettingsEntries",           new CheckSettingsEntries() },
            { "ColorTest",                      new ColorTest() },
            { "ColorTrueTest",                  new ColorTrueTest() },
            { "ListCultures",                   new ListCultures() },
            { "ListCodepages",                  new ListCodepages() },
            { "BenchmarkSleepOne",              new BenchmarkSleepOne() },
            { "BenchmarkTickSleepOne",          new BenchmarkTickSleepOne() },
            { "ProbeHardware",                  new ProbeHardware() },
            { "EnableNotifications",            new EnableNotifications() },
            { "SendNotification",               new SendNotification() },
            { "SendNotificationSimple",         new SendNotificationSimple() },
            { "SendNotificationProg",           new SendNotificationProg() },
            { "SendNotificationProgF",          new SendNotificationProgF() },
            { "DismissNotifications",           new DismissNotifications() },
            { "TestTranslate",                  new TestTranslate() },
            { "TestRNG",                        new TestRNG() },
            { "TestCryptoRNG",                  new TestCryptoRNG() },
            { "TestInputSelection",             new TestInputSelection() },
            { "TestInputMultiSelection",        new TestInputMultiSelection() },
            { "TestPresentation",               new TestPresentation() },
            { "TestPresentationKiosk",          new TestPresentationKiosk() },
            { "TestPresentationRequired",       new TestPresentationRequired() },
            { "TestPresentationKioskRequired",  new TestPresentationKioskRequired() },
            { "InternetCheck",                  new InternetCheck() },
            { "NetworkCheck",                   new NetworkCheck() },
            { "ChangeLanguage",                 new ChangeLanguage() },
            { "KernelThreadTest",               new KernelThreadTest() },
            { "KernelThreadChildTest",          new KernelThreadChildTest() },
            { "CliInfoPaneTest",                new CliInfoPaneTest() },
            { "CliInfoPaneTestRefreshing",      new CliInfoPaneTestRefreshing() },
            { "CliDoublePaneTest",              new CliDoublePaneTest() },
            { "CliInfoPaneSlowTest",            new CliInfoPaneSlowTest() },
            { "CliInfoPaneSlowTestRefreshing",  new CliInfoPaneSlowTestRefreshing() },
            { "CliDoublePaneSlowTest",          new CliDoublePaneSlowTest() },
            { "FetchKernelUpdates",             new FetchKernelUpdates() },
        };
        internal static Dictionary<TestSection, string> sections = new()
        {
            { TestSection.ConsoleBase,          /* Localizable */ "Base console tests" },
            { TestSection.Drivers,              /* Localizable */ "Driver tests" },
            { TestSection.Files,                /* Localizable */ "Filesystem tests" },
            { TestSection.Kernel,               /* Localizable */ "Main kernel component tests" },
            { TestSection.Languages,            /* Localizable */ "Language and localization tests" },
            { TestSection.Misc,                 /* Localizable */ "Miscellaneous tests" },
            { TestSection.Modification,         /* Localizable */ "Kernel modification tests" },
            { TestSection.Network,              /* Localizable */ "Network system tests" },
            { TestSection.Shell,                /* Localizable */ "UESH shell tests" },
            { TestSection.Users,                /* Localizable */ "User tests" },
        };
        private static bool exiting;

        internal static void Open()
        {
            exiting = false;

            // List sections and alt options
            int sectionCount = sections.Count;
            var listFacadesCodeNames = sections.Keys.Select((sec) => sec).ToArray();
            var listFacadesDescs = sections.Values.Select(Translate.DoTranslation).ToArray();
            var listFacadesAltOptionName = new string[] 
            { 
                Translate.DoTranslation("Stats"),
                Translate.DoTranslation("Shutdown"),
            };
            var listFacadesAltOptionDesc = new string[] 
            { 
                Translate.DoTranslation("Shows the current test statistics"),
                Translate.DoTranslation("Exits the testing mode and shuts down the kernel"),
            };

            // Prompt for section
            while (!exiting)
            {
                // Now, prompt for the selection of the section
                int sel = SelectionStyle.PromptSelection(Translate.DoTranslation("Choose a test section to run"), string.Join("/", listFacadesCodeNames), listFacadesDescs, string.Join("/", listFacadesAltOptionName), listFacadesAltOptionDesc, true);
                if (sel <= sectionCount)
                {
                    OpenSection(listFacadesCodeNames[sel - 1]);
                }
                else
                {
                    // Selected alternative option
                    if (sel == sectionCount + 1) 
                    {
                        // Stats
                        PrintTestStats();
                    }
                    else if (sel == sectionCount + 2 || sel == -1)
                    {
                        // Shutdown
                        exiting = true;
                    }
                }
            }
        }

        internal static void OpenSection(TestSection section)
        {
            bool sectionExiting = false;

            // List facades and alt options
            var facadesList = facades
                .Where((kvp) => kvp.Value.TestSection == section)
                .ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);
            int facadeCount = facadesList.Count;
            var listFacadesCodeNames = facadesList.Keys.Select((fac) => fac).ToArray();
            var listFacadesAltOptionName = new string[]
            {
                Translate.DoTranslation("Test All"),
                Translate.DoTranslation("Stats"),
                Translate.DoTranslation("Go Back..."),
            };
            var listFacadesAltOptionDesc = new string[]
            {
                Translate.DoTranslation("Tests all facades"),
                Translate.DoTranslation("Shows the current test statistics"),
                Translate.DoTranslation("Goes back to the section selection"),
            };

            // Prompt for facade
            while (!sectionExiting)
            {
                // We need to update the names in case the status updated
                var listFacadesNames = facadesList.Values.Select((fac) => $"[{(fac.TestStatus == TestStatus.Success ? "+" : fac.TestStatus == TestStatus.Failed ? "X" : "-")}] " + fac.TestName).ToArray();

                // Now, prompt for the selection of the facade
                int sel = SelectionStyle.PromptSelection(Translate.DoTranslation("Choose a test facade to run"), string.Join("/", listFacadesCodeNames), listFacadesNames, string.Join("/", listFacadesAltOptionName), listFacadesAltOptionDesc, true);
                if (sel <= facadeCount)
                {
                    RunFacade(facadesList[listFacadesCodeNames[sel - 1]]);
                }
                else
                {
                    // Selected alternative option
                    if (sel == facadeCount + 1)
                    {
                        // Test All
                        foreach (var facade in facadesList.Values)
                            RunFacade(facade);
                        ConsoleWrapper.Clear();
                        PrintTestStats();
                    }
                    else if (sel == facadeCount + 2)
                    {
                        // Stats
                        PrintTestStatsSection(section);
                    }
                    else if (sel == facadeCount + 3 || sel == -1)
                    {
                        // Go back
                        sectionExiting = true;
                    }
                }
            }
        }

        internal static void RunFacade(TestFacade facade)
        {
            // Try to...
            bool tested = false;
            try
            {
                while (!tested)
                {
                    // ...test the facade
                    ConsoleWrapper.Clear();
                    facade.status = TestStatus.Neutral;
                    facade.Run();

                    if (facade.TestInteractive)
                    {
                        // Prompt the user to check to see if the test ran as expected
                        ConsoleWrapper.SetCursorPosition(0, ConsoleWrapper.WindowHeight - 1);
                        string answer = ChoiceStyle.PromptChoice(Translate.DoTranslation("Did the test run as expected?"), "y/n/r");

                        // Set status or retry
                        switch (answer)
                        {
                            case "y":
                                facade.status = TestStatus.Success;
                                tested = true;
                                break;
                            case "r":
                                break;
                            default:
                                facade.status = TestStatus.Failed;
                                tested = true;
                                break;
                        }
                    }
                    else
                    {
                        // Compare the actual value with the expected value
                        if (!facade.TestActualValue.Equals(facade.TestExpectedValue))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("The test failed. Expected value is {0}, but actual value is {1}."), true, ConsoleBase.Colors.KernelColorType.Error, facade.TestExpectedValue.ToString(), facade.TestActualValue.ToString());
                            Input.DetectKeypress();
                            facade.status = TestStatus.Failed;
                            tested = true;
                        }
                        else
                        {
                            facade.status = TestStatus.Success;
                            tested = true;
                        }
                    }
                }
            }
            catch
            {
                // Facade failed unexpectedly
                facade.status = TestStatus.Failed;
            }
        }

        internal static void PrintTestStatsSection(TestSection section)
        {
            TextWriterColor.Write(Translate.DoTranslation("Successful tests:") + " {0}", facades.Values.Where((fac) => fac.TestStatus == TestStatus.Success && fac.TestSection == section).Count());
            TextWriterColor.Write(Translate.DoTranslation("Failed tests:") + " {0}", facades.Values.Where((fac) => fac.TestStatus == TestStatus.Failed && fac.TestSection == section).Count());
            TextWriterColor.Write(Translate.DoTranslation("Tests to be run:") + " {0}", facades.Values.Where((fac) => fac.TestStatus == TestStatus.Neutral && fac.TestSection == section).Count());
            Input.DetectKeypress();
        }

        internal static void PrintTestStats()
        {
            TextWriterColor.Write(Translate.DoTranslation("Successful tests:") + " {0}", facades.Values.Where((fac) => fac.TestStatus == TestStatus.Success).Count());
            TextWriterColor.Write(Translate.DoTranslation("Failed tests:") + " {0}", facades.Values.Where((fac) => fac.TestStatus == TestStatus.Failed).Count());
            TextWriterColor.Write(Translate.DoTranslation("Tests to be run:") + " {0}", facades.Values.Where((fac) => fac.TestStatus == TestStatus.Neutral).Count());
            Input.DetectKeypress();
        }
    }
}
