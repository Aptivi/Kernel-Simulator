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

using System;
using System.Collections.Generic;
using System.Linq;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase.Themes;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.WriterBase;

namespace KS.ConsoleBase.Colors
{
    /// <summary>
    /// Color tools module
    /// </summary>
    public static class ColorTools
    {

        /// <summary>
        /// Enumeration for color types
        /// </summary>
        public enum ColTypes : int
        {
            /// <summary>
            /// Input text
            /// </summary>
            Input,
            /// <summary>
            /// License color
            /// </summary>
            License,
            /// <summary>
            /// Continuable kernel panic text (usually sync'd with Warning)
            /// </summary>
            Continuable,
            /// <summary>
            /// Uncontinuable kernel panic text (usually sync'd with Error)
            /// </summary>
            Uncontinuable,
            /// <summary>
            /// Host name color
            /// </summary>
            HostName,
            /// <summary>
            /// User name color
            /// </summary>
            UserName,
            /// <summary>
            /// Background color
            /// </summary>
            Background,
            /// <summary>
            /// Neutral text (for general purposes)
            /// </summary>
            Neutral,
            /// <summary>
            /// List entry text
            /// </summary>
            ListEntry,
            /// <summary>
            /// List value text
            /// </summary>
            ListValue,
            /// <summary>
            /// Stage text
            /// </summary>
            Stage,
            /// <summary>
            /// Error text
            /// </summary>
            Error,
            /// <summary>
            /// Warning text
            /// </summary>
            Warning,
            /// <summary>
            /// Option text
            /// </summary>
            Option,
            /// <summary>
            /// Banner text
            /// </summary>
            Banner,
            /// <summary>
            /// Notification title text
            /// </summary>
            NotificationTitle,
            /// <summary>
            /// Notification description text
            /// </summary>
            NotificationDescription,
            /// <summary>
            /// Notification progress text
            /// </summary>
            NotificationProgress,
            /// <summary>
            /// Notification failure text
            /// </summary>
            NotificationFailure,
            /// <summary>
            /// Question text
            /// </summary>
            Question,
            /// <summary>
            /// Success text
            /// </summary>
            Success,
            /// <summary>
            /// User dollar sign on shell text
            /// </summary>
            UserDollar,
            /// <summary>
            /// Tip text
            /// </summary>
            Tip,
            /// <summary>
            /// Separator text
            /// </summary>
            SeparatorText,
            /// <summary>
            /// Separator color
            /// </summary>
            Separator,
            /// <summary>
            /// List title text
            /// </summary>
            ListTitle,
            /// <summary>
            /// Development warning text
            /// </summary>
            DevelopmentWarning,
            /// <summary>
            /// Stage time text
            /// </summary>
            StageTime,
            /// <summary>
            /// General progress text
            /// </summary>
            Progress,
            /// <summary>
            /// Back option text
            /// </summary>
            BackOption,
            /// <summary>
            /// Low priority notification border color
            /// </summary>
            LowPriorityBorder,
            /// <summary>
            /// Medium priority notification border color
            /// </summary>
            MediumPriorityBorder,
            /// <summary>
            /// High priority notification border color
            /// </summary>
            HighPriorityBorder,
            /// <summary>
            /// Table separator
            /// </summary>
            TableSeparator,
            /// <summary>
            /// Table header
            /// </summary>
            TableHeader,
            /// <summary>
            /// Table value
            /// </summary>
            TableValue,
            /// <summary>
            /// Selected option
            /// </summary>
            SelectedOption,
            /// <summary>
            /// Alternative option
            /// </summary>
            AlternativeOption,
            /// <summary>
            /// Gray color (for special purposes)
            /// </summary>
            Gray = -1,
        }

        // Variables for colors used by previous versions of the kernel.
        internal static readonly Dictionary<ColTypes, Color> KernelColors = new()
        {
            { ColTypes.Input, new((int)ConsoleColors.White) },
            { ColTypes.License, new((int)ConsoleColors.White) },
            { ColTypes.Continuable, new((int)ConsoleColors.Yellow) },
            { ColTypes.Uncontinuable, new((int)ConsoleColors.Red) },
            { ColTypes.HostName, new((int)ConsoleColors.DarkGreen) },
            { ColTypes.UserName, new((int)ConsoleColors.Green) },
            { ColTypes.Background, new((int)ConsoleColors.Black) },
            { ColTypes.Neutral, new((int)ConsoleColors.Gray) },
            { ColTypes.ListEntry, new((int)ConsoleColors.DarkYellow) },
            { ColTypes.ListValue, new((int)ConsoleColors.DarkGray) },
            { ColTypes.Stage, new((int)ConsoleColors.Green) },
            { ColTypes.Error, new((int)ConsoleColors.Red) },
            { ColTypes.Warning, new((int)ConsoleColors.Yellow) },
            { ColTypes.Option, new((int)ConsoleColors.DarkYellow) },
            { ColTypes.Banner, new((int)ConsoleColors.Green) },
            { ColTypes.NotificationTitle, new((int)ConsoleColors.White) },
            { ColTypes.NotificationDescription, new((int)ConsoleColors.Gray) },
            { ColTypes.NotificationProgress, new((int)ConsoleColors.DarkYellow) },
            { ColTypes.NotificationFailure, new((int)ConsoleColors.Red) },
            { ColTypes.Question, new((int)ConsoleColors.Yellow) },
            { ColTypes.Success, new((int)ConsoleColors.Green) },
            { ColTypes.UserDollar, new((int)ConsoleColors.Gray) },
            { ColTypes.Tip, new((int)ConsoleColors.Gray) },
            { ColTypes.SeparatorText, new((int)ConsoleColors.White) },
            { ColTypes.Separator, new((int)ConsoleColors.Gray) },
            { ColTypes.ListTitle, new((int)ConsoleColors.White) },
            { ColTypes.DevelopmentWarning, new((int)ConsoleColors.Yellow) },
            { ColTypes.StageTime, new((int)ConsoleColors.Gray) },
            { ColTypes.Progress, new((int)ConsoleColors.DarkYellow) },
            { ColTypes.BackOption, new((int)ConsoleColors.DarkRed) },
            { ColTypes.LowPriorityBorder, new((int)ConsoleColors.White) },
            { ColTypes.MediumPriorityBorder, new((int)ConsoleColors.Yellow) },
            { ColTypes.HighPriorityBorder, new((int)ConsoleColors.Red) },
            { ColTypes.TableSeparator, new((int)ConsoleColors.DarkGray) },
            { ColTypes.TableHeader, new((int)ConsoleColors.White) },
            { ColTypes.TableValue, new((int)ConsoleColors.Gray) },
            { ColTypes.SelectedOption, new((int)ConsoleColors.Yellow) },
            { ColTypes.AlternativeOption, new((int)ConsoleColors.DarkGreen) },
        };

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public static Color GetColor(ColTypes type) => KernelColors[type];

        /// <summary>
        /// Sets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        /// <param name="color">Color to be set</param>
        public static Color SetColor(ColTypes type, Color color) => KernelColors[type] = color;

        /// <summary>
        /// Resets all colors to default
        /// </summary>
        public static void ResetColors()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Resetting colors");
            var DefInfo = new ThemeInfo("_Default");

            // Set colors
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ColTypes)).Length - 2; typeIndex++)
            {
                ColTypes type = KernelColors.Keys.ElementAt(typeIndex);
                KernelColors[type] = DefInfo.ThemeColors[type];
            }
            LoadBack();

            // Raise event
            Kernel.Kernel.KernelEventManager.RaiseColorReset();
        }

        /// <summary>
        /// Loads the background
        /// </summary>
        public static void LoadBack()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Filling background with background color");
                SetConsoleColor(GetColor(ColTypes.Background), true);
                ConsoleWrapper.Clear();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to set background: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="InputColor">Input color</param>
        /// <param name="LicenseColor">License color</param>
        /// <param name="ContKernelErrorColor">Continuable kernel error color</param>
        /// <param name="UncontKernelErrorColor">Uncontinuable kernel error color</param>
        /// <param name="HostNameShellColor">Host name color</param>
        /// <param name="UserNameShellColor">User name color</param>
        /// <param name="BackgroundColor">Background color</param>
        /// <param name="NeutralTextColor">Neutral text color</param>
        /// <param name="ListEntryColor">Command list color</param>
        /// <param name="ListValueColor">Command definition color</param>
        /// <param name="StageColor">Stage color</param>
        /// <param name="ErrorColor">Error color</param>
        /// <param name="WarningColor">Warning color</param>
        /// <param name="OptionColor">Option color</param>
        /// <param name="BannerColor">Banner color</param>
        /// <param name="NotificationTitleColor">Notification title color</param>
        /// <param name="NotificationDescriptionColor">Notification description color</param>
        /// <param name="NotificationProgressColor">Notification progress color</param>
        /// <param name="NotificationFailureColor">Notification failure color</param>
        /// <param name="QuestionColor">Question color</param>
        /// <param name="SuccessColor">Success text color</param>
        /// <param name="UserDollarColor">User dollar color</param>
        /// <param name="TipColor">Tip color</param>
        /// <param name="SeparatorTextColor">Separator text color</param>
        /// <param name="SeparatorColor">Separator color</param>
        /// <param name="ListTitleColor">List title color</param>
        /// <param name="DevelopmentWarningColor">Development warning color</param>
        /// <param name="StageTimeColor">Stage time color</param>
        /// <param name="ProgressColor">Progress color</param>
        /// <param name="BackOptionColor">Back option color</param>
        /// <param name="LowPriorityBorderColor">Low priority notification border color</param>
        /// <param name="MediumPriorityBorderColor">Medium priority notification border color</param>
        /// <param name="HighPriorityBorderColor">High priority notification border color</param>
        /// <param name="TableSeparatorColor">Table separator color</param>
        /// <param name="TableHeaderColor">Table header color</param>
        /// <param name="TableValueColor">Table value color</param>
        /// <param name="SelectedOptionColor">Selected option color</param>
        /// <param name="AlternativeOptionColor">Alternative option color</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetColors(string InputColor, string LicenseColor, string ContKernelErrorColor, string UncontKernelErrorColor, string HostNameShellColor, string UserNameShellColor, string BackgroundColor, string NeutralTextColor, string ListEntryColor, string ListValueColor, string StageColor, string ErrorColor, string WarningColor, string OptionColor, string BannerColor, string NotificationTitleColor, string NotificationDescriptionColor, string NotificationProgressColor, string NotificationFailureColor, string QuestionColor, string SuccessColor, string UserDollarColor, string TipColor, string SeparatorTextColor, string SeparatorColor, string ListTitleColor, string DevelopmentWarningColor, string StageTimeColor, string ProgressColor, string BackOptionColor, string LowPriorityBorderColor, string MediumPriorityBorderColor, string HighPriorityBorderColor, string TableSeparatorColor, string TableHeaderColor, string TableValueColor, string SelectedOptionColor, string AlternativeOptionColor)
        {
            try
            {
                SetColors(InputColor, LicenseColor, ContKernelErrorColor, UncontKernelErrorColor, HostNameShellColor, UserNameShellColor, BackgroundColor, NeutralTextColor, ListEntryColor, ListValueColor, StageColor, ErrorColor, WarningColor, OptionColor, BannerColor, NotificationTitleColor, NotificationDescriptionColor, NotificationProgressColor, NotificationFailureColor, QuestionColor, SuccessColor, UserDollarColor, TipColor, SeparatorTextColor, SeparatorColor, ListTitleColor, DevelopmentWarningColor, StageTimeColor, ProgressColor, BackOptionColor, LowPriorityBorderColor, MediumPriorityBorderColor, HighPriorityBorderColor, TableSeparatorColor, TableHeaderColor, TableValueColor, SelectedOptionColor, AlternativeOptionColor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="InputColor">Input color</param>
        /// <param name="LicenseColor">License color</param>
        /// <param name="ContKernelErrorColor">Continuable kernel error color</param>
        /// <param name="UncontKernelErrorColor">Uncontinuable kernel error color</param>
        /// <param name="HostNameShellColor">Host name color</param>
        /// <param name="UserNameShellColor">User name color</param>
        /// <param name="BackgroundColor">Background color</param>
        /// <param name="NeutralTextColor">Neutral text color</param>
        /// <param name="ListEntryColor">Command list color</param>
        /// <param name="ListValueColor">Command definition color</param>
        /// <param name="StageColor">Stage color</param>
        /// <param name="ErrorColor">Error color</param>
        /// <param name="WarningColor">Warning color</param>
        /// <param name="OptionColor">Option color</param>
        /// <param name="BannerColor">Banner color</param>
        /// <param name="NotificationTitleColor">Notification title color</param>
        /// <param name="NotificationDescriptionColor">Notification description color</param>
        /// <param name="NotificationProgressColor">Notification progress color</param>
        /// <param name="NotificationFailureColor">Notification failure color</param>
        /// <param name="QuestionColor">Question color</param>
        /// <param name="SuccessColor">Success text color</param>
        /// <param name="UserDollarColor">User dollar color</param>
        /// <param name="TipColor">Tip color</param>
        /// <param name="SeparatorTextColor">Separator text color</param>
        /// <param name="SeparatorColor">Separator color</param>
        /// <param name="ListTitleColor">List title color</param>
        /// <param name="DevelopmentWarningColor">Development warning color</param>
        /// <param name="StageTimeColor">Stage time color</param>
        /// <param name="ProgressColor">Progress color</param>
        /// <param name="BackOptionColor">Back option color</param>
        /// <param name="LowPriorityBorderColor">Low priority notification border color</param>
        /// <param name="MediumPriorityBorderColor">Medium priority notification border color</param>
        /// <param name="HighPriorityBorderColor">High priority notification border color</param>
        /// <param name="TableSeparatorColor">Table separator color</param>
        /// <param name="TableHeaderColor">Table header color</param>
        /// <param name="TableValueColor">Table value color</param>
        /// <param name="SelectedOptionColor">Selected option color</param>
        /// <param name="AlternativeOptionColor">Alternative option color</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Kernel.Exceptions.ColorException"></exception>
        public static void SetColors(string InputColor, string LicenseColor, string ContKernelErrorColor, string UncontKernelErrorColor, string HostNameShellColor, string UserNameShellColor, string BackgroundColor, string NeutralTextColor, string ListEntryColor, string ListValueColor, string StageColor, string ErrorColor, string WarningColor, string OptionColor, string BannerColor, string NotificationTitleColor, string NotificationDescriptionColor, string NotificationProgressColor, string NotificationFailureColor, string QuestionColor, string SuccessColor, string UserDollarColor, string TipColor, string SeparatorTextColor, string SeparatorColor, string ListTitleColor, string DevelopmentWarningColor, string StageTimeColor, string ProgressColor, string BackOptionColor, string LowPriorityBorderColor, string MediumPriorityBorderColor, string HighPriorityBorderColor, string TableSeparatorColor, string TableHeaderColor, string TableValueColor, string SelectedOptionColor, string AlternativeOptionColor)
        {
            // Check colors for null and set them to "def" if found
            if (string.IsNullOrEmpty(OptionColor))
                OptionColor = "def";
            if (string.IsNullOrEmpty(WarningColor))
                WarningColor = "def";
            if (string.IsNullOrEmpty(ErrorColor))
                ErrorColor = "def";
            if (string.IsNullOrEmpty(StageColor))
                StageColor = "def";
            if (string.IsNullOrEmpty(ListValueColor))
                ListValueColor = "def";
            if (string.IsNullOrEmpty(ListEntryColor))
                ListEntryColor = "def";
            if (string.IsNullOrEmpty(NeutralTextColor))
                NeutralTextColor = "def";
            if (string.IsNullOrEmpty(BackgroundColor))
                BackgroundColor = "def";
            if (string.IsNullOrEmpty(UserNameShellColor))
                UserNameShellColor = "def";
            if (string.IsNullOrEmpty(HostNameShellColor))
                HostNameShellColor = "def";
            if (string.IsNullOrEmpty(UncontKernelErrorColor))
                UncontKernelErrorColor = "def";
            if (string.IsNullOrEmpty(ContKernelErrorColor))
                ContKernelErrorColor = "def";
            if (string.IsNullOrEmpty(LicenseColor))
                LicenseColor = "def";
            if (string.IsNullOrEmpty(InputColor))
                InputColor = "def";
            if (string.IsNullOrEmpty(BannerColor))
                BannerColor = "def";
            if (string.IsNullOrEmpty(NotificationTitleColor))
                NotificationTitleColor = "def";
            if (string.IsNullOrEmpty(NotificationDescriptionColor))
                NotificationDescriptionColor = "def";
            if (string.IsNullOrEmpty(NotificationProgressColor))
                NotificationProgressColor = "def";
            if (string.IsNullOrEmpty(NotificationFailureColor))
                NotificationFailureColor = "def";
            if (string.IsNullOrEmpty(QuestionColor))
                QuestionColor = "def";
            if (string.IsNullOrEmpty(SuccessColor))
                SuccessColor = "def";
            if (string.IsNullOrEmpty(UserDollarColor))
                UserDollarColor = "def";
            if (string.IsNullOrEmpty(TipColor))
                TipColor = "def";
            if (string.IsNullOrEmpty(SeparatorTextColor))
                SeparatorTextColor = "def";
            if (string.IsNullOrEmpty(SeparatorColor))
                SeparatorColor = "def";
            if (string.IsNullOrEmpty(ListTitleColor))
                ListTitleColor = "def";
            if (string.IsNullOrEmpty(DevelopmentWarningColor))
                DevelopmentWarningColor = "def";
            if (string.IsNullOrEmpty(StageTimeColor))
                StageTimeColor = "def";
            if (string.IsNullOrEmpty(ProgressColor))
                ProgressColor = "def";
            if (string.IsNullOrEmpty(BackOptionColor))
                BackOptionColor = "def";
            if (string.IsNullOrEmpty(LowPriorityBorderColor))
                LowPriorityBorderColor = "def";
            if (string.IsNullOrEmpty(MediumPriorityBorderColor))
                MediumPriorityBorderColor = "def";
            if (string.IsNullOrEmpty(HighPriorityBorderColor))
                HighPriorityBorderColor = "def";
            if (string.IsNullOrEmpty(TableSeparatorColor))
                TableSeparatorColor = "def";
            if (string.IsNullOrEmpty(TableHeaderColor))
                TableHeaderColor = "def";
            if (string.IsNullOrEmpty(TableValueColor))
                TableValueColor = "def";
            if (string.IsNullOrEmpty(SelectedOptionColor))
                SelectedOptionColor = "def";
            if (string.IsNullOrEmpty(AlternativeOptionColor))
                AlternativeOptionColor = "def";

            // Set colors
            if (Shell.Shell.ColoredShell == true)
            {
                // Check for defaults
                // We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
                if (InputColor == "def")
                    InputColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (LicenseColor == "def")
                    LicenseColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (ContKernelErrorColor == "def")
                    ContKernelErrorColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (UncontKernelErrorColor == "def")
                    UncontKernelErrorColor = new Color((int)ConsoleColors.Red).PlainSequence;
                if (HostNameShellColor == "def")
                    HostNameShellColor = new Color((int)ConsoleColors.DarkGreen).PlainSequence;
                if (UserNameShellColor == "def")
                    UserNameShellColor = new Color((int)ConsoleColors.Green).PlainSequence;
                if (NeutralTextColor == "def")
                    NeutralTextColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (ListEntryColor == "def")
                    ListEntryColor = new Color((int)ConsoleColors.DarkYellow).PlainSequence;
                if (ListValueColor == "def")
                    ListValueColor = new Color((int)ConsoleColors.DarkGray).PlainSequence;
                if (StageColor == "def")
                    StageColor = new Color((int)ConsoleColors.Green).PlainSequence;
                if (ErrorColor == "def")
                    ErrorColor = new Color((int)ConsoleColors.Red).PlainSequence;
                if (WarningColor == "def")
                    WarningColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (OptionColor == "def")
                    OptionColor = new Color((int)ConsoleColors.DarkYellow).PlainSequence;
                if (BannerColor == "def")
                    OptionColor = new Color((int)ConsoleColors.Green).PlainSequence;
                if (NotificationTitleColor == "def")
                    NotificationTitleColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (NotificationDescriptionColor == "def")
                    NotificationDescriptionColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (NotificationProgressColor == "def")
                    NotificationProgressColor = new Color((int)ConsoleColors.DarkYellow).PlainSequence;
                if (NotificationFailureColor == "def")
                    NotificationFailureColor = new Color((int)ConsoleColors.Red).PlainSequence;
                if (QuestionColor == "def")
                    QuestionColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (SuccessColor == "def")
                    SuccessColor = new Color((int)ConsoleColors.Green).PlainSequence;
                if (UserDollarColor == "def")
                    UserDollarColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (TipColor == "def")
                    TipColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (SeparatorTextColor == "def")
                    SeparatorTextColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (SeparatorColor == "def")
                    SeparatorColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (ListTitleColor == "def")
                    ListTitleColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (DevelopmentWarningColor == "def")
                    DevelopmentWarningColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (StageTimeColor == "def")
                    StageTimeColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (ProgressColor == "def")
                    ProgressColor = new Color((int)ConsoleColors.DarkYellow).PlainSequence;
                if (BackOptionColor == "def")
                    BackOptionColor = new Color((int)ConsoleColors.DarkRed).PlainSequence;
                if (LowPriorityBorderColor == "def")
                    LowPriorityBorderColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (MediumPriorityBorderColor == "def")
                    MediumPriorityBorderColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (HighPriorityBorderColor == "def")
                    HighPriorityBorderColor = new Color((int)ConsoleColors.Red).PlainSequence;
                if (TableSeparatorColor == "def")
                    TableSeparatorColor = new Color((int)ConsoleColors.DarkGray).PlainSequence;
                if (TableHeaderColor == "def")
                    TableHeaderColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (TableValueColor == "def")
                    TableValueColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (SelectedOptionColor == "def")
                    SelectedOptionColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (AlternativeOptionColor == "def")
                    AlternativeOptionColor = new Color((int)ConsoleColors.DarkGreen).PlainSequence;
                if (BackgroundColor == "def")
                {
                    BackgroundColor = new Color((int)ConsoleColors.Black).PlainSequence;
                    LoadBack();
                }

                // Set the colors
                try
                {
                    KernelColors[ColTypes.Input] = new Color(InputColor);
                    KernelColors[ColTypes.License] = new Color(LicenseColor);
                    KernelColors[ColTypes.Continuable] = new Color(ContKernelErrorColor);
                    KernelColors[ColTypes.Uncontinuable] = new Color(UncontKernelErrorColor);
                    KernelColors[ColTypes.HostName] = new Color(HostNameShellColor);
                    KernelColors[ColTypes.UserName] = new Color(UserNameShellColor);
                    KernelColors[ColTypes.Background] = new Color(BackgroundColor);
                    KernelColors[ColTypes.Neutral] = new Color(NeutralTextColor);
                    KernelColors[ColTypes.ListEntry] = new Color(ListEntryColor);
                    KernelColors[ColTypes.ListValue] = new Color(ListValueColor);
                    KernelColors[ColTypes.Stage] = new Color(StageColor);
                    KernelColors[ColTypes.Error] = new Color(ErrorColor);
                    KernelColors[ColTypes.Warning] = new Color(WarningColor);
                    KernelColors[ColTypes.Option] = new Color(OptionColor);
                    KernelColors[ColTypes.Banner] = new Color(BannerColor);
                    KernelColors[ColTypes.NotificationTitle] = new Color(NotificationTitleColor);
                    KernelColors[ColTypes.NotificationDescription] = new Color(NotificationDescriptionColor);
                    KernelColors[ColTypes.NotificationProgress] = new Color(NotificationProgressColor);
                    KernelColors[ColTypes.NotificationFailure] = new Color(NotificationFailureColor);
                    KernelColors[ColTypes.Question] = new Color(QuestionColor);
                    KernelColors[ColTypes.Success] = new Color(SuccessColor);
                    KernelColors[ColTypes.UserDollar] = new Color(UserDollarColor);
                    KernelColors[ColTypes.Tip] = new Color(TipColor);
                    KernelColors[ColTypes.SeparatorText] = new Color(SeparatorTextColor);
                    KernelColors[ColTypes.Separator] = new Color(SeparatorColor);
                    KernelColors[ColTypes.ListTitle] = new Color(ListTitleColor);
                    KernelColors[ColTypes.DevelopmentWarning] = new Color(DevelopmentWarningColor);
                    KernelColors[ColTypes.StageTime] = new Color(StageTimeColor);
                    KernelColors[ColTypes.Progress] = new Color(ProgressColor);
                    KernelColors[ColTypes.BackOption] = new Color(BackOptionColor);
                    KernelColors[ColTypes.LowPriorityBorder] = new Color(LowPriorityBorderColor);
                    KernelColors[ColTypes.MediumPriorityBorder] = new Color(MediumPriorityBorderColor);
                    KernelColors[ColTypes.HighPriorityBorder] = new Color(HighPriorityBorderColor);
                    KernelColors[ColTypes.TableSeparator] = new Color(TableSeparatorColor);
                    KernelColors[ColTypes.TableHeader] = new Color(TableHeaderColor);
                    KernelColors[ColTypes.TableValue] = new Color(TableValueColor);
                    KernelColors[ColTypes.SelectedOption] = new Color(SelectedOptionColor);
                    KernelColors[ColTypes.AlternativeOption] = new Color(AlternativeOptionColor);
                    LoadBack();
                    Config.CreateConfig();

                    // Raise event
                    Kernel.Kernel.KernelEventManager.RaiseColorSet();
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
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
        /// Gets the gray color according to the brightness of the background color
        /// </summary>
        public static Color GetGray()
        {
            if (GetColor(ColTypes.Background).IsBright)
            {
                return GetColor(ColTypes.Neutral);
            }
            else
            {
                return new Color((int)ConsoleColors.Gray);
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        public static void SetConsoleColor(ColTypes colorType) => SetConsoleColor(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <param name="ForceSet">Force set color</param>
        public static void SetConsoleColor(ColTypes colorType, bool Background, bool ForceSet = false)
        {
            switch (colorType)
            {
                case ColTypes.Gray:
                    {
                        SetConsoleColor(GetGray(), Background, ForceSet);
                        break;
                    }
                default:
                    {
                        SetConsoleColor(ColorTools.GetColor(colorType), Background, ForceSet);
                        break;
                    }
            }
            if (!Background)
                SetConsoleColor(GetColor(ColTypes.Background), true);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false)
        {
            if (Shell.Shell.ColoredShell)
            {
                if (ColorSequence is null)
                    throw new ArgumentNullException(nameof(ColorSequence));

                // Define reset background sequence
                string resetSequence = CharManager.GetEsc() + $"[49m";

                // Set background
                if (Background)
                {
                    if (Flags.SetBackground | ForceSet)
                        WriterPlainManager.CurrentPlain.WritePlain(ColorSequence.VTSequenceBackground, false);
                    else
                        WriterPlainManager.CurrentPlain.WritePlain(resetSequence, false);
                }
                else
                {
                    WriterPlainManager.CurrentPlain.WritePlain(ColorSequence.VTSequenceForeground, false);
                }
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(ColTypes colorType) => TrySetConsoleColor(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(ColTypes colorType, bool Background)
        {
            try
            {
                SetConsoleColor(colorType, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(Color ColorSequence, bool Background)
        {
            try
            {
                SetConsoleColor(ColorSequence, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(string ColorSpecifier)
        {
            try
            {
                var ColorInstance = new Color(ColorSpecifier);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int ColorNum)
        {
            try
            {
                var ColorInstance = new Color(ColorNum);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int R, int G, int B)
        {
            try
            {
                var ColorInstance = new Color(R, G, B);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the RGB sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHexToRGB(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex.RemoveLetter(0), 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                return $"{R};{G};{B}";
            }
            else
            {
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid hex color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(string RGBSequence)
        {
            if (RGBSequence.Contains(Convert.ToString(';')))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    return $"#{R:X2}{G:X2}{B:X2}";
                }
                else
                {
                    throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid RGB color specifier."));
                }
            }
            else
            {
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid RGB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(int R, int G, int B)
        {
            if (R < 0 | R > 255)
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid red color specifier."));
            if (G < 0 | G > 255)
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid green color specifier."));
            if (B < 0 | B > 255)
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid blue color specifier."));
            return $"#{R:X2}{G:X2}{B:X2}";
        }

    }
}
