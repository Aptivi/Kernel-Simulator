﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Buffered;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Textify.Sequences.Builder.Types;
using Textify.Sequences.Tools;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.ConsoleBase.Writers;

namespace KS.ConsoleBase.Interactive
{
    /// <summary>
    /// Tools for the interactive TUI implementation
    /// </summary>
    public static class InteractiveTuiTools
    {

        private static string _finalInfoRendered = "";
        private static readonly object _interactiveTuiLock = new();

        /// <summary>
        /// Opens the interactive TUI
        /// </summary>
        /// <param name="interactiveTui">The inherited class instance of the interactive TUI</param>
        /// <exception cref="KernelException"></exception>
        public static void OpenInteractiveTui(BaseInteractiveTui interactiveTui)
        {
            lock (_interactiveTuiLock)
            {
                if (interactiveTui is null)
                    throw new KernelException(KernelExceptionType.InteractiveTui, Translate.DoTranslation("Please provide a base Interactive TUI class and try again."));
                BaseInteractiveTui.instances.Add(interactiveTui);

                // First, check to see if the interactive TUI has no data source
                if (interactiveTui.PrimaryDataSource is null && interactiveTui.SecondaryDataSource is null ||
                    EnumerableTools.CountElements(interactiveTui.PrimaryDataSource) == 0 && EnumerableTools.CountElements(interactiveTui.SecondaryDataSource) == 0 && !interactiveTui.AcceptsEmptyData)
                {
                    TextWriters.Write(Translate.DoTranslation("The interactive TUI {0} doesn't contain any data source. This program can't continue."), true, KernelColorType.Error, interactiveTui.GetType().Name);
                    TextWriterColor.Write();
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to exit this program..."));
                    Input.DetectKeypress();
                    return;
                }
                bool notifyCrash = false;
                string crashReason = "";

                // Make the screen
                var screen = new Screen();
                ScreenTools.SetCurrent(screen);
                interactiveTui.screen = screen;

                // Now, run the application
                try
                {
                    // Loop until the user requests to exit
                    while (!interactiveTui.isExiting)
                    {
                        // Check the selection
                        interactiveTui.LastOnOverflow();
                        CheckSelectionForUnderflow(interactiveTui);
                        DebugWriter.WriteDebug(DebugLevel.I, "Went to the last element on overflow.");

                        // Draw the boxes
                        DrawInteractiveTui(interactiveTui);
                        DebugWriter.WriteDebug(DebugLevel.I, "Interactive TUI drawn.");

                        // Draw the first pane
                        DrawInteractiveTuiItems(interactiveTui, 1);
                        DebugWriter.WriteDebug(DebugLevel.I, "Interactive TUI items (first pane) drawn.");

                        // Draw the second pane
                        if (interactiveTui.SecondPaneInteractable)
                        {
                            DrawInteractiveTuiItems(interactiveTui, 2);
                            DebugWriter.WriteDebug(DebugLevel.I, "Interactive TUI items (second pane) drawn.");
                        }
                        else
                        {
                            DrawInformationOnSecondPane(interactiveTui);
                            DebugWriter.WriteDebug(DebugLevel.I, "Info drawn.");
                        }

                        DrawStatus(interactiveTui);
                        DebugWriter.WriteDebug(DebugLevel.I, "Status drawn.");

                        // Wait for user input
                        ScreenTools.Render(screen);
                        RespondToUserInput(interactiveTui);
                        DebugWriter.WriteDebug(DebugLevel.I, "Responded to user input.");

                        // Reset, in case selection changed
                        screen.RemoveBufferedParts();
                    }
                }
                catch (Exception ex)
                {
                    notifyCrash = true;
                    crashReason = TextTools.FormatString(Translate.DoTranslation("The interactive TUI, {0}, has crashed for the following reason:"), interactiveTui.GetType().Name) + $" {ex.Message}";
                    DebugWriter.WriteDebug(DebugLevel.E, "Interactive TUI {0} crashed! {1}", interactiveTui.GetType().Name, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                finally
                {
                    BaseInteractiveTui.instances.Remove(interactiveTui);
                }
                ScreenTools.UnsetCurrent(screen);

                // Clear the console to clean up
                KernelColorTools.LoadBack();

                // If there is a crash, notify the user about it
                if (notifyCrash)
                {
                    notifyCrash = false;
                    TextWriters.Write(crashReason, true, KernelColorType.Error);
                    TextWriterColor.Write();
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to exit this program..."));
                    Input.DetectKeypress();
                }

                // Reset some static variables
                BaseInteractiveTui.CurrentPane = 1;
                BaseInteractiveTui.FirstPaneCurrentSelection = 1;
                BaseInteractiveTui.SecondPaneCurrentSelection = 1;
                BaseInteractiveTui.Status = "";
            }
        }

        /// <summary>
        /// Initiates the selection movement
        /// </summary>
        /// <param name="interactiveTui">Interactive TUI to deal with</param>
        /// <param name="pos">Position to move the pane selection to</param>
        public static void SelectionMovement(BaseInteractiveTui interactiveTui, int pos)
        {
            // Check the position
            var data = BaseInteractiveTui.CurrentPane == 2 ?
                       interactiveTui.SecondaryDataSource :
                       interactiveTui.PrimaryDataSource;
            int elements = EnumerableTools.CountElements(data);
            if (pos < 1)
                pos = 1;
            if (pos > elements)
                pos = elements;

            // Now, process the movement
            if (BaseInteractiveTui.CurrentPane == 2)
                BaseInteractiveTui.SecondPaneCurrentSelection = pos;
            else
                BaseInteractiveTui.FirstPaneCurrentSelection = pos;
        }

        private static void DrawInteractiveTui(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui and screen
            DebugCheck.AssertNull(interactiveTui,
                "attempted to render TUI items on null");
            DebugCheck.AssertNull(interactiveTui.Screen,
                "attempted to render TUI items on no screen");

            // Make a screen part
            var part = new ScreenPart();

            // Prepare the console
            ConsoleWrapper.CursorVisible = false;
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMinimumHeight = 1;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Redraw the entire TUI screen
            DebugWriter.WriteDebug(DebugLevel.I, "We're redrawing.");
            part.BackgroundColor(BaseInteractiveTui.BackgroundColor);
            part.AddText(CsiSequences.GenerateCsiEraseInDisplay(2));

            // Make a separator that separates the two panes to make it look like Total Commander or Midnight Commander. We need information in the upper and the
            // lower part of the console, so we need to render the entire program to look like this: (just a concept mockup)
            //
            //       |  vvvvvvvvvvvvvvvvvvvv  (SeparatorHalfConsoleWidthInterior)
            //       | v                    v (SeparatorHalfConsoleWidth)
            // H: 0  |
            // H: 1  | a--------------------+c---------------------+ < ----> (SeparatorMinimumHeight)
            // H: 2  | |b                   ||d                    |  < ----> (SeparatorMinimumHeightInterior)
            // H: 3  | |                    ||                     |  <
            // H: 4  | |                    ||                     |  <
            // H: 5  | |                    ||                     |  <
            // H: 6  | |                    ||                     |  <
            // H: 7  | |                    ||                     |  <
            // H: 8  | |                    ||                     |  < ----> (SeparatorMaximumHeightInterior)
            // H: 9  | +--------------------++---------------------+ < ----> (SeparatorMaximumHeight)
            // H: 10 |
            //       | where a is the dimension for the first pane upper left corner           (0, SeparatorMinimumHeight                                     (usually 1))
            //       |   and b is the dimension for the first pane interior upper left corner  (1, SeparatorMinimumHeightInterior                             (usually 2))
            //       |   and c is the dimension for the second pane upper left corner          (SeparatorHalfConsoleWidth, SeparatorMinimumHeight             (usually 1))
            //       |   and d is the dimension for the second pane interior upper left corner (SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior (usually 2))

            // First, the horizontal and vertical separators
            var finalForeColorFirstPane = BaseInteractiveTui.CurrentPane == 1 ? BaseInteractiveTui.PaneSelectedSeparatorColor : BaseInteractiveTui.PaneSeparatorColor;
            var finalForeColorSecondPane = BaseInteractiveTui.CurrentPane == 2 ? BaseInteractiveTui.PaneSelectedSeparatorColor : BaseInteractiveTui.PaneSeparatorColor;
            DebugWriter.WriteDebug(DebugLevel.I, "0, {0}, {1}, {2}, {3}, {4}", SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalForeColorFirstPane.PlainSequence, BaseInteractiveTui.PaneBackgroundColor.PlainSequence);
            part.AddDynamicText(new(() =>
            {
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var builder = new StringBuilder();
                builder.Append(finalForeColorFirstPane.VTSequenceForeground);
                builder.Append(BaseInteractiveTui.PaneBackgroundColor.VTSequenceBackground);
                builder.Append(BorderColor.RenderBorderPlain(0, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior));
                return builder.ToString();
            }));
            DebugWriter.WriteDebug(DebugLevel.I, "{0}, {1}, {2}, {3}, {4}, {5}", SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalForeColorSecondPane.PlainSequence, BaseInteractiveTui.PaneBackgroundColor.PlainSequence);
            part.AddDynamicText(new(() =>
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var builder = new StringBuilder();
                builder.Append(finalForeColorSecondPane.VTSequenceForeground);
                builder.Append(BaseInteractiveTui.PaneBackgroundColor.VTSequenceBackground);
                builder.Append(BorderColor.RenderBorderPlain(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0), SeparatorMaximumHeightInterior));
                return builder.ToString();
            }));

            // Populate appropriate bindings, depending on the SecondPaneInteractable value
            List<InteractiveTuiBinding> finalBindings;
            if (interactiveTui.Bindings is null || interactiveTui.Bindings.Count == 0)
                finalBindings =
                [
                    new InteractiveTuiBinding(/* Localizable */ "Exit", ConsoleKey.Escape, null, true)
                ];
            else
                finalBindings = new(interactiveTui.Bindings)
                {
                    new InteractiveTuiBinding(/* Localizable */ "Exit", ConsoleKey.Escape, null, true),
                    new InteractiveTuiBinding(/* Localizable */ "Keybindings", ConsoleKey.K, null, true),
                };
            if (interactiveTui.SecondPaneInteractable)
                finalBindings.Add(
                    new InteractiveTuiBinding(/* Localizable */ "Switch", ConsoleKey.Tab, null, true)
                );

            // Render the key bindings
            part.AddDynamicText(() =>
            {
                var bindingsBuilder = new StringBuilder(CsiSequences.GenerateCsiCursorPosition(1, ConsoleWrapper.WindowHeight));
                foreach (InteractiveTuiBinding binding in finalBindings)
                {
                    // First, check to see if the rendered binding info is going to exceed the console window width
                    string renderedBinding = $"{GetBindingKeyShortcut(binding, false)} {binding.BindingName}  ";
                    int actualLength = VtSequenceTools.FilterVTSequences(bindingsBuilder.ToString()).Length;
                    bool canDraw = renderedBinding.Length + actualLength < ConsoleWrapper.WindowWidth - 3;
                    if (canDraw)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Drawing binding {0} with description {1}...", GetBindingKeyShortcut(binding, false), binding.BindingName);
                        bindingsBuilder.Append(
                            $"{BaseInteractiveTui.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{BaseInteractiveTui.OptionBackgroundColor.VTSequenceBackground}" +
                            GetBindingKeyShortcut(binding, false) +
                            $"{BaseInteractiveTui.OptionForegroundColor.VTSequenceForeground}" +
                            $"{BaseInteractiveTui.BackgroundColor.VTSequenceBackground}" +
                            $" {(binding._localizable ? Translate.DoTranslation(binding.BindingName) : binding.BindingName)}  "
                        );
                    }
                    else
                    {
                        // We can't render anymore, so just break and write a binding to show more
                        DebugWriter.WriteDebug(DebugLevel.I, "Bailing because of no space...");
                        bindingsBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight)}" +
                            $"{BaseInteractiveTui.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{BaseInteractiveTui.OptionBackgroundColor.VTSequenceBackground}" +
                            " K "
                        );
                        break;
                    }
                }
                return bindingsBuilder.ToString();
            });

            interactiveTui.Screen.AddBufferedPart($"Interactive TUI - Main - {interactiveTui.GetType().Name}", part);
        }

        private static void DrawInteractiveTuiItems(BaseInteractiveTui interactiveTui, int paneNum)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            DebugCheck.AssertNull(interactiveTui,
                "attempted to render TUI items on null");
            DebugCheck.AssertNull(interactiveTui.Screen,
                "attempted to render TUI items on no screen");

            // Check to make sure that we're not rendering the second pane on the first-pane-only interactive TUI
            DebugCheck.Assert(!interactiveTui.SecondPaneInteractable && paneNum == 1 || interactiveTui.SecondPaneInteractable,
                "tried to render interactive TUI items for the secondary pane on an interactive TUI that only allows interaction from one pane.");

            // Make a screen part
            var part = new ScreenPart();

            // Check the pane number
            if (paneNum < 1)
                paneNum = 1;
            if (paneNum > 2)
                paneNum = 2;

            // Get how many data are there in the chosen data source
            var data = paneNum == 2 ? interactiveTui.SecondaryDataSource : interactiveTui.PrimaryDataSource;
            int dataCount = EnumerableTools.CountElements(data);

            // Render the pane right away
            part.AddDynamicText(() =>
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeightInterior = 2;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                int answersPerPage = SeparatorMaximumHeightInterior;
                int paneCurrentSelection = paneNum == 2 ? BaseInteractiveTui.SecondPaneCurrentSelection : BaseInteractiveTui.FirstPaneCurrentSelection;
                int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                int startIndex = answersPerPage * currentPage;
                var builder = new StringBuilder();
                for (int i = 0; i <= answersPerPage - 1; i++)
                {
                    // Populate the first pane
                    string finalEntry = "";
                    int finalIndex = i + startIndex;
                    object dataObject = null;
                    if (finalIndex <= dataCount - 1)
                    {
                        // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
                        int steppedItems = 0;
                        foreach (var item in data)
                        {
                            steppedItems++;
                            if (steppedItems == startIndex + i + 1)
                            {
                                // We found the item that we need! Assign it to dataObject so GetEntryFromItem() can formulate a string.
                                DebugWriter.WriteDebug(DebugLevel.I, "Found required item index {0} [{1}, offset {2}, final {3}].", steppedItems, i, startIndex, finalIndex);
                                dataObject = item;
                                break;
                            }
                        }

                        // Render an entry
                        var finalForeColor = finalIndex == paneCurrentSelection - 1 ? BaseInteractiveTui.PaneSelectedItemForeColor : BaseInteractiveTui.PaneItemForeColor;
                        var finalBackColor = finalIndex == paneCurrentSelection - 1 ? BaseInteractiveTui.PaneSelectedItemBackColor : BaseInteractiveTui.PaneItemBackColor;
                        int leftPos = paneNum == 2 ? SeparatorHalfConsoleWidth + 1 : 1;
                        int top = SeparatorMinimumHeightInterior + finalIndex - startIndex;
                        finalEntry = interactiveTui.GetEntryFromItem(dataObject).Truncate(SeparatorHalfConsoleWidthInterior - 4);
                        string text =
                            $"{CsiSequences.GenerateCsiCursorPosition(leftPos + 1, top + 1)}" +
                            $"{finalForeColor.VTSequenceForeground}" +
                            $"{finalBackColor.VTSequenceBackground}" +
                            finalEntry +
                            new string(' ', SeparatorHalfConsoleWidthInterior - finalEntry.Length - (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? 0 : 1)) +
                            $"{BaseInteractiveTui.PaneItemBackColor.VTSequenceBackground}";
                        builder.Append(text);
                    }
                }

                // Render the vertical bar
                if (Config.MainConfig.EnableScrollBarInSelection)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing scroll bar.");
                    int left = paneNum == 2 ? (SeparatorHalfConsoleWidthInterior * 2) + (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? 2 : 1) : SeparatorHalfConsoleWidthInterior - 1;
                    builder.Append(ProgressBarVerticalColor.RenderVerticalProgress(100 * ((double)paneCurrentSelection / dataCount), left, 1, 2, 2, BaseInteractiveTui.PaneSeparatorColor, BaseInteractiveTui.PaneBackgroundColor, false));
                }
                return builder.ToString();
            });

            interactiveTui.Screen.AddBufferedPart($"Interactive TUI - Items - {interactiveTui.GetType().Name}", part);
        }

        private static void DrawInformationOnSecondPane(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            DebugCheck.AssertNull(interactiveTui,
                "attempted to render TUI items on null");
            DebugCheck.AssertNull(interactiveTui.Screen,
                "attempted to render TUI items on no screen");

            // Check to make sure that we're not rendering the information pane on the both-panes interactive TUI
            DebugCheck.Assert(!interactiveTui.SecondPaneInteractable,
                "tried to render information the secondary pane on an interactive TUI that allows interaction from two panes, messing the selection rendering up there.");

            // Make a screen part
            var part = new ScreenPart();

            // Populate some positions
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Populate some colors
            var ForegroundColor = BaseInteractiveTui.ForegroundColor;
            var PaneItemBackColor = BaseInteractiveTui.PaneItemBackColor;

            // Now, do the job!
            string finalInfoRendered;
            try
            {
                // Populate data source and its count
                int paneCurrentSelection = BaseInteractiveTui.CurrentPane == 2 ?
                                           BaseInteractiveTui.SecondPaneCurrentSelection :
                                           BaseInteractiveTui.FirstPaneCurrentSelection;
                var data = BaseInteractiveTui.CurrentPane == 2 ?
                           interactiveTui.SecondaryDataSource :
                           interactiveTui.PrimaryDataSource;
                int dataCount = EnumerableTools.CountElements(data);

                // Populate selected data
                DebugWriter.WriteDebug(DebugLevel.I, "{0} elements.", dataCount);
                if (dataCount > 0)
                {
                    object selectedData = EnumerableTools.GetElementFromIndex(data, paneCurrentSelection - 1);
                    DebugCheck.AssertNull(selectedData,
                        "attempted to render info about null data");
                    finalInfoRendered = interactiveTui.GetInfoFromItem(selectedData);
                    DebugWriter.WriteDebug(DebugLevel.I, "Rendered info: {0}", finalInfoRendered);
                }
                else
                {
                    finalInfoRendered = Translate.DoTranslation("No info.");
                    DebugWriter.WriteDebug(DebugLevel.W, "There is no info!");
                }
            }
            catch (Exception ex)
            {
                finalInfoRendered = Translate.DoTranslation("Failed to get information.");
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to get information in interactive TUI: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }

            // Now, write info
            var finalForeColorSecondPane = BaseInteractiveTui.CurrentPane == 2 ? BaseInteractiveTui.PaneSelectedSeparatorColor : BaseInteractiveTui.PaneSeparatorColor;
            part.AddDynamicText(() =>
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMinimumHeightInterior = 2;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var builder = new StringBuilder();
                builder.Append(finalForeColorSecondPane.VTSequenceForeground);
                builder.Append(BaseInteractiveTui.PaneBackgroundColor.VTSequenceBackground);
                builder.Append(BorderColor.RenderBorderPlain(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0), SeparatorMaximumHeightInterior));

                _finalInfoRendered = finalInfoRendered;
                string[] finalInfoStrings = TextTools.GetWrappedSentences(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
                for (int infoIndex = 0; infoIndex < finalInfoStrings.Length; infoIndex++)
                {
                    // Check to see if the info is overpopulated
                    if (infoIndex >= SeparatorMaximumHeightInterior - 1)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Info is overflowing! {0} >= {1}", infoIndex, SeparatorMaximumHeightInterior - 1);
                        string truncated = Translate.DoTranslation("Shift+I = more info");
                        builder.Append(ForegroundColor.VTSequenceForeground);
                        builder.Append(PaneItemBackColor.VTSequenceBackground);
                        builder.Append(TextWriterWhereColor.RenderWherePlain(truncated + new string(' ', SeparatorHalfConsoleWidthInterior - truncated.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + infoIndex));
                        break;
                    }

                    // Now, render the info
                    string finalInfo = finalInfoStrings[infoIndex];
                    DebugWriter.WriteDebug(DebugLevel.I, "Rendering final info {0}...", finalInfo);
                    builder.Append(ForegroundColor.VTSequenceForeground);
                    builder.Append(PaneItemBackColor.VTSequenceBackground);
                    builder.Append(TextWriterWhereColor.RenderWherePlain(finalInfo + new string(' ', SeparatorHalfConsoleWidthInterior - finalInfo.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + infoIndex));
                }
                return builder.ToString();
            });

            interactiveTui.Screen.AddBufferedPart($"Interactive TUI - Info (2nd pane) - {interactiveTui.GetType().Name}", part);
        }

        private static void DrawStatus(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            DebugCheck.AssertNull(interactiveTui.Screen,
                "attempted to render TUI items on no screen");

            // Make a screen part
            var part = new ScreenPart();

            // Populate some necessary variables
            int paneCurrentSelection = BaseInteractiveTui.CurrentPane == 2 ?
                                       BaseInteractiveTui.SecondPaneCurrentSelection :
                                       BaseInteractiveTui.FirstPaneCurrentSelection;
            var data = BaseInteractiveTui.CurrentPane == 2 ?
                       interactiveTui.SecondaryDataSource :
                       interactiveTui.PrimaryDataSource;
            object selectedData = EnumerableTools.GetElementFromIndex(data, paneCurrentSelection - 1);
            interactiveTui.RenderStatus(selectedData);
            DebugWriter.WriteDebug(DebugLevel.I, "Status rendered. {0}", BaseInteractiveTui.Status);

            // Now, write info
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                builder.Append(BaseInteractiveTui.ForegroundColor.VTSequenceForeground);
                builder.Append(BaseInteractiveTui.BackgroundColor.VTSequenceBackground);
                builder.Append(TextWriterWhereColor.RenderWherePlain(BaseInteractiveTui.Status.Truncate(ConsoleWrapper.WindowWidth - 3), 0, 0));
                builder.Append(ConsoleExtensions.GetClearLineToRightSequence());
                return builder.ToString();
            });
            interactiveTui.Screen.AddBufferedPart($"Interactive TUI - Status - {interactiveTui.GetType().Name}", part);
        }

        private static void RespondToUserInput(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            DebugCheck.AssertNull(interactiveTui,
                "attempted to respond to user input on null");

            // Populate some necessary variables
            int paneCurrentSelection = BaseInteractiveTui.CurrentPane == 2 ?
                                       BaseInteractiveTui.SecondPaneCurrentSelection :
                                       BaseInteractiveTui.FirstPaneCurrentSelection;
            var data = BaseInteractiveTui.CurrentPane == 2 ?
                       interactiveTui.SecondaryDataSource :
                       interactiveTui.PrimaryDataSource;
            int dataCount = EnumerableTools.CountElements(data);
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Populate selected data
            object selectedData = EnumerableTools.GetElementFromIndex(data, paneCurrentSelection - 1);

            // Wait for key
            try
            {
                ConsoleKeyInfo pressedKey;
                if (interactiveTui.RefreshInterval == 0 || interactiveTui.SecondPaneInteractable)
                    pressedKey = Input.DetectKeypress();
                else
                    pressedKey = Input.ReadKeyTimeout(true, TimeSpan.FromMilliseconds(interactiveTui.RefreshInterval));

                // Handle the key
                DebugWriter.WriteDebug(DebugLevel.I, "Pressed key. Handling...");
                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (BaseInteractiveTui.CurrentPane == 2)
                        {
                            SelectionMovement(interactiveTui, BaseInteractiveTui.SecondPaneCurrentSelection - 1);
                            DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", BaseInteractiveTui.SecondPaneCurrentSelection);
                        }
                        else
                        {
                            SelectionMovement(interactiveTui, BaseInteractiveTui.FirstPaneCurrentSelection - 1);
                            DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", BaseInteractiveTui.FirstPaneCurrentSelection);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (BaseInteractiveTui.CurrentPane == 2)
                        {
                            SelectionMovement(interactiveTui, BaseInteractiveTui.SecondPaneCurrentSelection + 1);
                            DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", BaseInteractiveTui.SecondPaneCurrentSelection);
                        }
                        else
                        {
                            SelectionMovement(interactiveTui, BaseInteractiveTui.FirstPaneCurrentSelection + 1);
                            DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", BaseInteractiveTui.FirstPaneCurrentSelection);
                        }
                        break;
                    case ConsoleKey.Home:
                        SelectionMovement(interactiveTui, 1);
                        DebugWriter.WriteDebug(DebugLevel.I, "Selection: 1");
                        break;
                    case ConsoleKey.End:
                        SelectionMovement(interactiveTui, dataCount);
                        DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", dataCount);
                        break;
                    case ConsoleKey.PageUp:
                        {
                            int answersPerPage = SeparatorMaximumHeightInterior;
                            int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                            int startIndex = answersPerPage * currentPage;
                            SelectionMovement(interactiveTui, startIndex);
                        }
                        break;
                    case ConsoleKey.PageDown:
                        {
                            int answersPerPage = SeparatorMaximumHeightInterior;
                            int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                            int startIndex = answersPerPage * (currentPage + 1) + 1;
                            SelectionMovement(interactiveTui, startIndex);
                        }
                        break;
                    case ConsoleKey.I:
                        if (pressedKey.Modifiers.HasFlag(ConsoleModifiers.Shift) && !string.IsNullOrEmpty(_finalInfoRendered))
                        {
                            // User needs more information in the infobox
                            DebugWriter.WriteDebug(DebugLevel.I, "Rendering information in the infobox...");
                            InfoBoxColor.WriteInfoBoxColorBack(_finalInfoRendered, BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);
                        }
                        break;
                    case ConsoleKey.K:
                        // First, check the bindings length
                        var bindings = interactiveTui.Bindings;
                        if (bindings is null || bindings.Count == 0)
                            break;

                        // User needs an infobox that shows all available keys
                        string section = Translate.DoTranslation("Available keys");
                        int maxBindingLength = bindings
                            .Max((itb) => GetBindingKeyShortcut(itb).Length);
                        string[] bindingRepresentations = bindings
                            .Select((itb) => $"{GetBindingKeyShortcut(itb) + new string(' ', maxBindingLength - GetBindingKeyShortcut(itb).Length) + $" | {(itb._localizable ? Translate.DoTranslation(itb.BindingName) : itb.BindingName)}"}")
                            .ToArray();
                        InfoBoxColor.WriteInfoBoxColorBack(
                            $"{section}{CharManager.NewLine}" +
                            $"{new string('=', section.Length)}{CharManager.NewLine}{CharManager.NewLine}" +
                            $"{string.Join('\n', bindingRepresentations)}"
                        , BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);
                        break;
                    case ConsoleKey.Escape:
                        // User needs to exit
                        DebugWriter.WriteDebug(DebugLevel.I, "Exiting...");
                        interactiveTui.HandleExit();
                        interactiveTui.isExiting = true;
                        break;
                    case ConsoleKey.Tab:
                        // User needs to switch sides
                        DebugWriter.WriteDebug(DebugLevel.I, "Switching sides...");
                        SwitchSides(interactiveTui);
                        break;
                    default:
                        // First, check the bindings
                        var allBindings = interactiveTui.Bindings;
                        if (allBindings is null || allBindings.Count == 0)
                            break;

                        // Now, get the implemented bindings from the pressed key
                        var implementedBindings = allBindings.Where((binding) =>
                            binding.BindingKeyName == pressedKey.Key && binding.BindingKeyModifiers == pressedKey.Modifiers);
                        foreach (var implementedBinding in implementedBindings)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Executing implemented binding {0}...", implementedBinding.BindingKeyName.ToString(), implementedBinding.BindingName);
                            implementedBinding.BindingAction.Invoke(selectedData, paneCurrentSelection - 1);
                        }
                        break;
                }
            }
            catch (KernelException kex) when (kex.ExceptionType == KernelExceptionType.ConsoleReadTimeout)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Refreshing...");
            }
        }

        private static void CheckSelectionForUnderflow(BaseInteractiveTui interactiveTui)
        {
            if (BaseInteractiveTui.FirstPaneCurrentSelection <= 0 && EnumerableTools.CountElements(interactiveTui.PrimaryDataSource) > 0)
                BaseInteractiveTui.FirstPaneCurrentSelection = 1;
            if (BaseInteractiveTui.SecondPaneCurrentSelection <= 0 && EnumerableTools.CountElements(interactiveTui.SecondaryDataSource) > 0)
                BaseInteractiveTui.SecondPaneCurrentSelection = 1;
        }

        private static string GetBindingKeyShortcut(InteractiveTuiBinding bind, bool mark = true)
        {
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingKeyModifiers != 0 ? $"{bind.BindingKeyModifiers} + " : "")}{bind.BindingKeyName}{markEnd}";
        }

        private static void SwitchSides(BaseInteractiveTui interactiveTui)
        {
            if (!interactiveTui.SecondPaneInteractable)
                return;
            BaseInteractiveTui.CurrentPane++;
            if (BaseInteractiveTui.CurrentPane > 2)
                BaseInteractiveTui.CurrentPane = 1;
        }
    }
}
