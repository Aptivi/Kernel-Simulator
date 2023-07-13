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

using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Presentation;
using KS.Misc.Presentation.Elements;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace KS.Kernel
{
    internal static class KernelFirstRun
    {
        internal static void PresentFirstRun()
        {
            try
            {
                // Some variables
                string user = "owner";
                string stepFailureReason = "";
                string langCode = "eng";
                bool supportsTrueColor = true;
                bool moveOn = false;

                // Prepare some arguments
                // TODO: They are hacks that should be dealt with before Beta 2 for choice elements.
                List<object> step1Args = new()
                {
                    Translate.DoTranslation("Select your language. By default, the kernel uses the English language, but you can select any other language here. Write the language code listed below:")
                };
                step1Args.AddRange(LanguageManager.Languages.Keys);

                // Populate the first run presentations in case language changed during the first start-up
                Presentation firstRunPresIntro = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    new List<PresentationPage>()
                    {
                        // First page - introduction
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Welcome!"),
                        
                            // Page elements
                            new List<IElement>()
                            {
                                new TextElement()
                                { 
                                    Arguments = new object[] 
                                    { 
                                        Translate.DoTranslation("Welcome to Nitrocid Kernel! Thank you for trying it out!") + "\n"
                                    }
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("To get started, press ENTER.")
                                    }
                                }
                            }
                        )
                    }
                );

                Presentation firstRunPresStep1 = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    new List<PresentationPage>()
                    {
                        // Third page - language selection
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Select your language"),
                        
                            // Page elements
                            new List<IElement>()
                            {
                                new ChoiceInputElement()
                                {
                                    Arguments = step1Args.ToArray(),
                                    InvokeActionInput =
                                        (args) => { 
                                            langCode = (string)args[0];
                                            LanguageManager.SetLang(langCode);
                                            moveOn = true;
                                        } 
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Press the ENTER key to continue.") + "\n"
                                    }
                                }
                            }
                        )
                    }
                );

                Presentation firstRunPresStep2 = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    new List<PresentationPage>()
                    {
                        // Second page - username creation
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Create your first user"),
                        
                            // Page elements
                            new List<IElement>()
                            {
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("We'll help you create your own username. Select any name you want. This could be your nickname or your short name, as long as your username doesn't contain spaces and special characters.")
                                    }
                                },
                                new DynamicTextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        () => stepFailureReason
                                    }
                                },
                                new InputElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Enter the username") + ": "
                                    },
                                    InvokeActionInput =
                                        (args) =>
                                            user = string.IsNullOrWhiteSpace((string)args[0]) ? "owner" : (string)args[0]
                                },
                                new MaskedInputElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Enter the password") + ": "
                                    },
                                    InvokeActionInput =
                                        (args) => {
                                            try
                                            {
                                                UserManagement.AddUser(user, (string)args[0]);
                                                DebugWriter.WriteDebug(DebugLevel.I, "We shall move on.");
                                                stepFailureReason = "";
                                                moveOn = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                DebugWriter.WriteDebug(DebugLevel.I, "We shouldn't move on. Failed to create username. {0}", ex.Message);
                                                DebugWriter.WriteDebugStackTrace(ex);
                                                stepFailureReason = Translate.DoTranslation("Failed to create username. Please ensure that your username doesn't contain spaces and special characters.");
                                            }
                                        }
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Press the ENTER key to continue.") + "\n"
                                    }
                                }
                            }
                        )
                    }
                );

                Presentation firstRunPresStep3 = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    new List<PresentationPage>()
                    {
                        // Fourth page - Console test
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Testing your console for true-color support"),
                        
                            // Page elements
                            new List<IElement>()
                            {
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Your terminal is {0} on {1}.") + "\n\n",
                                        KernelPlatform.GetTerminalType(),
                                        KernelPlatform.GetTerminalEmulator()
                                    }
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        RenderBandsForFirstRun()
                                    }
                                },
                                new ChoiceInputElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Do these ramps look right to you? They should transition smoothly."),
                                        "y", "n"
                                    },
                                    InvokeActionInput =
                                        (args) => {
                                            supportsTrueColor = (string)args[0] == "y";
                                            moveOn = true;
                                        } 
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Press the ENTER key to continue.") + "\n"
                                    }
                                }
                            }
                        )
                    }
                );

                Presentation firstRunPresStep4 = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    new List<PresentationPage>()
                    {
                        // Fifth page - Automatic updates
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Automatic updates"),
                        
                            // Page elements
                            new List<IElement>()
                            {
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Nitrocid KS currently updates itself to get the most recent version that includes general improvements and bug fixes. New major versions usually include breaking changes and new exciting features.")
                                    }
                                },
                                new ChoiceInputElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Do you want Nitrocid KS to automatically check for updates?"),
                                        "y", "n"
                                    },
                                    InvokeActionInput =
                                        (args) => {
                                            Config.MainConfig.CheckUpdateStart = (string)args[0] == "y";
                                        } 
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("In addition to automatically checking for updates, Nitrocid KS can also download the update file automatically.")
                                    }
                                },
                                new ChoiceInputElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Do you want Nitrocid KS to automatically download updates?"),
                                        "y", "n"
                                    },
                                    InvokeActionInput =
                                        (args) => {
                                            Config.MainConfig.AutoDownloadUpdate = (string)args[0] == "y";
                                            moveOn = true;
                                        }
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("You can always check for kernel updates using the \"update\" command.") + "\n"
                                    }
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Press the ENTER key to continue.") + "\n"
                                    }
                                }
                            }
                        )
                    }
                );

                Presentation firstRunPresOutro = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    new List<PresentationPage>()
                    {
                        // Third page - get started
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Get Started!"),
                        
                            // Page elements
                            new List<IElement>()
                            {
                                new DynamicTextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        () => string.Format(Translate.DoTranslation("Congratulations! You now have a user account, {0}!"), user) + "\n"
                                    }
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Press the ENTER key to get started using the kernel and log-in to your new account. Good luck!") + "\n"
                                    }
                                }
                            }
                        )
                    }
                );

                // Assign all first runs
                Presentation[] firstRuns = {
                    // Introduction
                    firstRunPresIntro,

                    // Steps
                    firstRunPresStep1, firstRunPresStep2, firstRunPresStep3, firstRunPresStep4,

                    // Outro
                    firstRunPresOutro 
                };

                // Present all presentations
                for (int step = 0; step < firstRuns.Length; step++)
                {
                    // Put in loop if the presentation contains input
                    var firstRun = firstRuns[step];
                    DebugWriter.WriteDebug(DebugLevel.I, "First run: step {0}", step);
                    if (PresentationTools.PresentationContainsInput(firstRun))
                    {
                        // Contains input.
                        DebugWriter.WriteDebug(DebugLevel.I, "Presentation contains input.");
                        while (!moveOn)
                            PresentationTools.Present(firstRun, true, true);
                        moveOn = false;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Presentation doesn't contain input.");
                        PresentationTools.Present(firstRun, true, true);
                    }
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Out of first run");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error in first run: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                ConsoleWrapper.Clear();
                TextWriterColor.Write(Translate.DoTranslation("We apologize for your inconvenience, but the out-of-box experience has crashed. If you're sure that this is a defect in the experience, please report the crash to us with debugging logs.") + " {0}", ex.Message);
                TextWriterColor.Write(Translate.DoTranslation("Press any key to start the shell anyways, but please note that you may have to create your new user manually."));
                Input.DetectKeypress();
            }
        }

        private static string RenderBandsForFirstRun()
        {
            // Show three color bands
            var band = new StringBuilder();
            int times = ConsoleWrapper.WindowWidth - (PresentationTools.PresentationUpperInnerBorderLeft * 2) - 1;
            double threshold = 255 / (double)times;
            for (double i = 0; i < 255; i += threshold)
                band.Append($"{new Color(Convert.ToInt32(i), 0, 0).VTSequenceBackground} ");
            band.AppendLine();
            for (double i = 0; i < 255; i += threshold)
                band.Append($"{new Color(0, Convert.ToInt32(i), 0).VTSequenceBackground} ");
            band.AppendLine();
            for (double i = 0; i < 255; i += threshold)
                band.Append($"{new Color(0, 0, Convert.ToInt32(i)).VTSequenceBackground} ");
            band.AppendLine();
            band.Append($"{CharManager.GetEsc() + $"[49m"}");
            return band.ToString();
        }
    }
}
