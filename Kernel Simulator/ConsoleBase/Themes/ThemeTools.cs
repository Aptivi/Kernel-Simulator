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

using System;
using System.Collections.Generic;
using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

namespace KS.ConsoleBase.Themes
{
    public static class ThemeTools
    {

        /// <summary>
        /// All the available built-in themes
        /// </summary>
        public static readonly Dictionary<string, ThemeInfo> Themes = new()
        {
            { "Default", new ThemeInfo("Default") },
            { "3Y-Diamond", new ThemeInfo("3Y-Diamond") },
            { "Aquatic", new ThemeInfo("Aquatic") },
            { "AyuDark", new ThemeInfo("AyuDark") },
            { "AyuLight", new ThemeInfo("AyuLight") },
            { "AyuMirage", new ThemeInfo("AyuMirage") },
            { "BlackOnWhite", new ThemeInfo("BlackOnWhite") },
            { "BedOS", new ThemeInfo("BedOS") },
            { "Bloody", new ThemeInfo("Bloody") },
            { "Blue Power", new ThemeInfo("Blue Power") },
            { "Bluesome", new ThemeInfo("Bluesome") },
            { "Bluespire", new ThemeInfo("Bluespire") },
            { "BreezeDark", new ThemeInfo("BreezeDark") },
            { "Breeze", new ThemeInfo("Breeze") },
            { "Dawn Aurora", new ThemeInfo("Dawn Aurora") },
            { "Darcula", new ThemeInfo("Darcula") },
            { "Debian", new ThemeInfo("Debian") },
            { "Fire", new ThemeInfo("Fire") },
            { "Grape", new ThemeInfo("Grape") },
            { "Grape Kiwi", new ThemeInfo("Grape Kiwi") },
            { "GTASA", new ThemeInfo("GTASA") },
            { "Grays", new ThemeInfo("Grays") },
            { "GrayOnYellow", new ThemeInfo("GrayOnYellow") },
            { "Green Mix", new ThemeInfo("Green Mix") },
            { "Grink", new ThemeInfo("Grink") },
            { "Gruvbox", new ThemeInfo("Gruvbox") },
            { "Hacker", new ThemeInfo("Hacker") },
            { "Lemon", new ThemeInfo("Lemon") },
            { "Light Planks", new ThemeInfo("Light Planks") },
            { "LinuxColoredDef", new ThemeInfo("LinuxColoredDef") },
            { "LinuxUncolored", new ThemeInfo("LinuxUncolored") },
            { "Materialistic", new ThemeInfo("Materialistic") },
            { "Melange", new ThemeInfo("Melange") },
            { "MelangeDark", new ThemeInfo("MelangeDark") },
            { "Metallic", new ThemeInfo("Metallic") },
            { "Mint", new ThemeInfo("Mint") },
            { "Mint Gum", new ThemeInfo("Mint Gum") },
            { "Mintback", new ThemeInfo("Mintback") },
            { "Mintish", new ThemeInfo("Mintish") },
            { "NeonBreeze", new ThemeInfo("NeonBreeze") },
            { "Neutralized", new ThemeInfo("Neutralized") },
            { "NFSHP-Cop", new ThemeInfo("NFSHP-Cop") },
            { "NFSHP-Racer", new ThemeInfo("NFSHP-Racer") },
            { "NoFrilsAcme", new ThemeInfo("NoFrilsAcme") },
            { "NoFrilsDark", new ThemeInfo("NoFrilsDark") },
            { "NoFrilsLight", new ThemeInfo("NoFrilsLight") },
            { "NoFrilsSepia", new ThemeInfo("NoFrilsSepia") },
            { "Orange Sea", new ThemeInfo("Orange Sea") },
            { "Pastel 1", new ThemeInfo("Pastel 1") },
            { "Pastel 2", new ThemeInfo("Pastel 2") },
            { "Pastel 3", new ThemeInfo("Pastel 3") },
            { "Papercolor", new ThemeInfo("Papercolor") },
            { "PapercolorDark", new ThemeInfo("PapercolorDark") },
            { "PhosphoricBG", new ThemeInfo("PhosphoricBG") },
            { "PhosphoricFG", new ThemeInfo("PhosphoricFG") },
            { "Planted Wood", new ThemeInfo("Planted Wood") },
            { "Purlow", new ThemeInfo("Purlow") },
            { "Red Breeze", new ThemeInfo("Red Breeze") },
            { "RedConsole", new ThemeInfo("RedConsole") },
            { "Reddish", new ThemeInfo("Reddish") },
            { "Rigel", new ThemeInfo("Rigel") },
            { "SolarizedDark", new ThemeInfo("SolarizedDark") },
            { "SolarizedLight", new ThemeInfo("SolarizedLight") },
            { "SpaceCamp", new ThemeInfo("SpaceCamp") },
            { "SpaceDuck", new ThemeInfo("SpaceDuck") },
            { "Tealed", new ThemeInfo("Tealed") },
            { "TealerOS", new ThemeInfo("TealerOS") },
            { "TrafficLight", new ThemeInfo("TrafficLight") },
            { "Ubuntu", new ThemeInfo("Ubuntu") },
            { "VisualStudioDark", new ThemeInfo("VisualStudioDark") },
            { "VisualStudioLight", new ThemeInfo("VisualStudioLight") },
            { "Windows11", new ThemeInfo("Windows11") },
            { "Windows11Light", new ThemeInfo("Windows11Light") },
            { "Windows95", new ThemeInfo("Windows95") },
            { "Wood", new ThemeInfo("Wood") },
            { "YellowBG", new ThemeInfo("YellowBG") },
            { "YellowFG", new ThemeInfo("YellowFG") }
        };

        /// <summary>
        /// Sets system colors according to the programmed templates
        /// </summary>
        /// <param name="theme">A specified theme</param>
        public static void ApplyThemeFromResources(string theme)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Theme: {0}", theme);
            if (Themes.ContainsKey(theme))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Theme found.");

                // Populate theme info
                if (theme == "Default")
                    KernelColorTools.ResetColors();
                else
                {
                    var ThemeInfo = new ThemeInfo(theme);
                    SetColorsTheme(ThemeInfo);
                }

                // Raise event
                Kernel.Kernel.KernelEventManager.RaiseThemeSet(theme);
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.E, "Theme not found.");

                // Raise event
                Kernel.Kernel.KernelEventManager.RaiseThemeSetError(theme, ThemeSetErrorReasons.NotFound);
                throw new Kernel.Exceptions.NoSuchThemeException(Translate.DoTranslation("Invalid color template {0}"), theme);
            }
        }

        /// <summary>
        /// Sets system colors according to the template file
        /// </summary>
        /// <param name="ThemeFile">Theme file</param>
        public static void ApplyThemeFromFile(string ThemeFile)
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Theme file name: {0}", ThemeFile);
                ThemeFile = Filesystem.NeutralizePath(ThemeFile, true);
                DebugWriter.Wdbg(DebugLevel.I, "Theme file path: {0}", ThemeFile);

                // Populate theme info
                var ThemeStream = new StreamReader(ThemeFile);
                var ThemeInfo = new ThemeInfo(ThemeStream);
                ThemeStream.Close();

                if (!(ThemeFile == "Default"))
                {
                    // Set colors as appropriate
                    SetColorsTheme(ThemeInfo);
                }

                // Raise event
                Kernel.Kernel.KernelEventManager.RaiseThemeSet(ThemeFile);
            }
            catch (FileNotFoundException)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Theme not found.");

                // Raise event
                Kernel.Kernel.KernelEventManager.RaiseThemeSetError(ThemeFile, ThemeSetErrorReasons.NotFound);
                throw new Kernel.Exceptions.NoSuchThemeException(Translate.DoTranslation("Invalid color template {0}"), ThemeFile);
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="ThemeInfo">Theme information</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetColorsTheme(ThemeInfo ThemeInfo)
        {
            if (ThemeInfo is null)
                throw new ArgumentNullException(nameof(ThemeInfo));

            // Set the colors
            if (Shell.Shell.ColoredShell == true)
            {
                try
                {
                    KernelColorTools.InputColor = ThemeInfo.ThemeInputColor;
                    KernelColorTools.LicenseColor = ThemeInfo.ThemeLicenseColor;
                    KernelColorTools.ContKernelErrorColor = ThemeInfo.ThemeContKernelErrorColor;
                    KernelColorTools.UncontKernelErrorColor = ThemeInfo.ThemeUncontKernelErrorColor;
                    KernelColorTools.HostNameShellColor = ThemeInfo.ThemeHostNameShellColor;
                    KernelColorTools.UserNameShellColor = ThemeInfo.ThemeUserNameShellColor;
                    KernelColorTools.BackgroundColor = ThemeInfo.ThemeBackgroundColor;
                    KernelColorTools.NeutralTextColor = ThemeInfo.ThemeNeutralTextColor;
                    KernelColorTools.ListEntryColor = ThemeInfo.ThemeListEntryColor;
                    KernelColorTools.ListValueColor = ThemeInfo.ThemeListValueColor;
                    KernelColorTools.StageColor = ThemeInfo.ThemeStageColor;
                    KernelColorTools.ErrorColor = ThemeInfo.ThemeErrorColor;
                    KernelColorTools.WarningColor = ThemeInfo.ThemeWarningColor;
                    KernelColorTools.OptionColor = ThemeInfo.ThemeOptionColor;
                    KernelColorTools.BannerColor = ThemeInfo.ThemeBannerColor;
                    KernelColorTools.NotificationTitleColor = ThemeInfo.ThemeNotificationTitleColor;
                    KernelColorTools.NotificationDescriptionColor = ThemeInfo.ThemeNotificationDescriptionColor;
                    KernelColorTools.NotificationProgressColor = ThemeInfo.ThemeNotificationProgressColor;
                    KernelColorTools.NotificationFailureColor = ThemeInfo.ThemeNotificationFailureColor;
                    KernelColorTools.QuestionColor = ThemeInfo.ThemeQuestionColor;
                    KernelColorTools.SuccessColor = ThemeInfo.ThemeSuccessColor;
                    KernelColorTools.UserDollarColor = ThemeInfo.ThemeUserDollarColor;
                    KernelColorTools.TipColor = ThemeInfo.ThemeTipColor;
                    KernelColorTools.SeparatorTextColor = ThemeInfo.ThemeSeparatorTextColor;
                    KernelColorTools.SeparatorColor = ThemeInfo.ThemeSeparatorColor;
                    KernelColorTools.ListTitleColor = ThemeInfo.ThemeListTitleColor;
                    KernelColorTools.DevelopmentWarningColor = ThemeInfo.ThemeDevelopmentWarningColor;
                    KernelColorTools.StageTimeColor = ThemeInfo.ThemeStageTimeColor;
                    KernelColorTools.ProgressColor = ThemeInfo.ThemeProgressColor;
                    KernelColorTools.BackOptionColor = ThemeInfo.ThemeBackOptionColor;
                    KernelColorTools.LowPriorityBorderColor = ThemeInfo.ThemeLowPriorityBorderColor;
                    KernelColorTools.MediumPriorityBorderColor = ThemeInfo.ThemeMediumPriorityBorderColor;
                    KernelColorTools.HighPriorityBorderColor = ThemeInfo.ThemeHighPriorityBorderColor;
                    KernelColorTools.TableSeparatorColor = ThemeInfo.ThemeTableSeparatorColor;
                    KernelColorTools.TableHeaderColor = ThemeInfo.ThemeTableHeaderColor;
                    KernelColorTools.TableValueColor = ThemeInfo.ThemeTableValueColor;
                    KernelColorTools.SelectedOptionColor = ThemeInfo.ThemeSelectedOptionColor;
                    KernelColorTools.AlternativeOptionColor = ThemeInfo.ThemeAlternativeOptionColor;
                    KernelColorTools.LoadBack();
                    KernelColorTools.MakePermanent();

                    // Raise event
                    Kernel.Kernel.KernelEventManager.RaiseColorSet();
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    Kernel.Kernel.KernelEventManager.RaiseColorSetError(ColorSetErrorReasons.InvalidColors);
                    throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("One or more of the colors is invalid.") + " {0}", ex, ex.Message);
                }
            }
            else
            {
                Kernel.Kernel.KernelEventManager.RaiseColorSetError(ColorSetErrorReasons.NoColors);
                throw new InvalidOperationException(Translate.DoTranslation("Colors are not available. Turn on colored shell in the kernel config."));
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="ThemeInfo">Theme information</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool TrySetColorsTheme(ThemeInfo ThemeInfo)
        {
            try
            {
                SetColorsTheme(ThemeInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
