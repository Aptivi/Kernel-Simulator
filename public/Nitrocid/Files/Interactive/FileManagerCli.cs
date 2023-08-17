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

using FluentFTP.Helpers;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.IO;
using MimeKit;
using KS.Files.LineEndings;
using System.Text;
using KS.Misc.Interactive;
using System.Collections;
using KS.Misc.Text;
using KS.Kernel.Time.Renderers;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Configuration;

namespace KS.Files.Interactive
{
    /// <summary>
    /// File manager class relating to the interactive file manager planned back in 2018
    /// </summary>
    public class FileManagerCli : BaseInteractiveTui, IInteractiveTui
    {
        private static string firstPanePath = Paths.HomePath;
        private static string secondPanePath = Paths.HomePath;

        /// <summary>
        /// File manager bindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } = new()
        {
            // Operations
            new InteractiveTuiBinding(/* Localizable */ "Open",         ConsoleKey.Enter, (info, _) => Open((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Copy",         ConsoleKey.F1,    (info, _) => CopyFileOrDir((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Move",         ConsoleKey.F2,    (info, _) => MoveFileOrDir((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Delete",       ConsoleKey.F3,    (info, _) => RemoveFileOrDir((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Up",           ConsoleKey.F4,    (_, _)    => GoUp(), true),
            new InteractiveTuiBinding(/* Localizable */ "Info",         ConsoleKey.F5,    (info, _) => PrintFileSystemInfo((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Go To",        ConsoleKey.F6,    (_, _)    => GoTo(), true),
            new InteractiveTuiBinding(/* Localizable */ "Copy To",      ConsoleKey.F7,    (info, _) => CopyTo((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Move To",      ConsoleKey.F8,    (info, _) => MoveTo((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Rename",       ConsoleKey.F9,    (info, _) => Rename((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "New Folder",   ConsoleKey.F10,   (_, _)    => MakeDir(), true),

            // Misc bindings
            new InteractiveTuiBinding(/* Localizable */ "Switch",       ConsoleKey.Tab,   (_, _)    => Switch(), true),
        };

        /// <summary>
        /// Always true in the file manager as we want it to behave like Total Commander
        /// </summary>
        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource
        {
            get
            {
                try
                {
                    return Listing.CreateList(firstPanePath, true);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get current directory list for the first pane [{0}]: {1}", firstPanePath, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return new List<FileSystemInfo>();
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable SecondaryDataSource
        {
            get
            {
                try
                {
                    return Listing.CreateList(secondPanePath, true);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get current directory list for the second pane [{0}]: {1}", secondPanePath, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return new List<FileSystemInfo>();
                }
            }
        }

        /// <inheritdoc/>
        public override void RenderStatus(object item)
        {
            FileSystemInfo FileInfoCurrentPane = (FileSystemInfo)item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane == null)
            {
                Status = Translate.DoTranslation("No info.");
                return;
            }

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = Checking.FolderExists(FileInfoCurrentPane.FullName);
                if (Config.MainConfig.IfmShowFileSize)
                    Status =
                        // Name and directory indicator
                        $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.Name} | " +

                        // File size or directory size
                        $"{(!infoIsDirectory ? ((FileInfo)FileInfoCurrentPane).Length.FileSizeToString() : SizeGetter.GetAllSizesInFolder((DirectoryInfo)FileInfoCurrentPane).FileSizeToString())} | " +

                        // Modified date
                        $"{(!infoIsDirectory ? TimeDateRenderers.Render(((FileInfo)FileInfoCurrentPane).LastWriteTime) : "")}"
                    ;
                else
                    Status = $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.Name}";
            }
            catch (Exception ex)
            {
                Status = Translate.DoTranslation(ex.Message);
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            try
            {
                FileSystemInfo file = (FileSystemInfo)item;
                bool isDirectory = Checking.FolderExists(file.FullName);
                return $" [{(isDirectory ? "/" : "*")}] {file.Name}";
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return "???";
            }
        }

        private static void Open(FileSystemInfo currentFileSystemInfo)
        {
            try
            {
                if (!Checking.FolderExists(currentFileSystemInfo.FullName))
                    return;
                if (CurrentPane == 2)
                {
                    secondPanePath = Filesystem.NeutralizePath(currentFileSystemInfo.FullName + "/");
                    SecondPaneCurrentSelection = 1;
                }
                else
                {
                    firstPanePath = Filesystem.NeutralizePath(currentFileSystemInfo.FullName + "/");
                    FirstPaneCurrentSelection = 1;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't open folder") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
                RedrawRequired = true;
            }
        }

        private static void GoUp()
        {
            if (CurrentPane == 2)
            {
                secondPanePath = Filesystem.NeutralizePath(secondPanePath + "/..");
                SecondPaneCurrentSelection = 1;
            }
            else
            {
                firstPanePath = Filesystem.NeutralizePath(firstPanePath + "/..");
                FirstPaneCurrentSelection = 1;
            }
        }

        private static void Switch()
        {
            CurrentPane++;
            if (CurrentPane > 2)
                CurrentPane = 1;
            RedrawRequired = true;
        }

        private static void PrintFileSystemInfo(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            // Render the final information string
            try
            {
                var finalInfoRendered = new StringBuilder();
                string fullPath = currentFileSystemInfo.FullName;
                if (Checking.FolderExists(fullPath))
                {
                    // The file system info instance points to a folder
                    var DirInfo = new DirectoryInfo(fullPath);
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), DirInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name: {0}"), Filesystem.NeutralizePath(DirInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Size: {0}"), SizeGetter.GetAllSizesInFolder(DirInfo).FileSizeToString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Creation time: {0}"), TimeDateRenderers.Render(DirInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last access time: {0}"), TimeDateRenderers.Render(DirInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last write time: {0}"), TimeDateRenderers.Render(DirInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Attributes: {0}"), DirInfo.Attributes));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Parent directory: {0}"), Filesystem.NeutralizePath(DirInfo.Parent.FullName)));
                }
                else
                {
                    // The file system info instance points to a file
                    FileInfo fileInfo = new(fullPath);
                    var Style = LineEndingsTools.GetLineEndingFromFile(fullPath);
                    bool isBinary = Parsing.IsBinaryFile(fileInfo.FullName);
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), fileInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name: {0}"), Filesystem.NeutralizePath(fileInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("File size: {0}"), fileInfo.Length.FileSizeToString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Creation time: {0}"), TimeDateRenderers.Render(fileInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last access time: {0}"), TimeDateRenderers.Render(fileInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last write time: {0}"), TimeDateRenderers.Render(fileInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Attributes: {0}"), fileInfo.Attributes));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Where to find: {0}"), Filesystem.NeutralizePath(fileInfo.DirectoryName)));
                    if (!isBinary)
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Newline style:") + " {0}", Style.ToString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Binary file:") + " {0}", isBinary));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("MIME metadata:") + " {0}", MimeTypes.GetMimeType(Filesystem.NeutralizePath(fileInfo.FullName))));
                }
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));

                // Now, render the info box
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't get file system info") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            }
            RedrawRequired = true;
        }

        private static void CopyFileOrDir(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string dest = (CurrentPane == 2 ? firstPanePath : secondPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                DebugCheck.AssertNull(dest, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                Copying.CopyFileOrDir(currentFileSystemInfo.FullName, dest);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
                RedrawRequired = true;
            }
        }

        private static void MoveFileOrDir(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string dest = (CurrentPane == 2 ? firstPanePath : secondPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                DebugCheck.AssertNull(dest, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                Moving.MoveFileOrDir(currentFileSystemInfo.FullName, dest);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
                RedrawRequired = true;
            }
        }

        private static void RemoveFileOrDir(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                Removing.RemoveFileOrDir(currentFileSystemInfo.FullName);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't remove file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
                RedrawRequired = true;
            }
        }

        private static void GoTo()
        {
            // Now, render the search box
            string path = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path or a full path to a local folder."), BoxForegroundColor, BoxBackgroundColor);
            path = Filesystem.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath);
            if (Checking.FolderExists(path))
            {
                if (CurrentPane == 2)
                {
                    SecondPaneCurrentSelection = 1;
                    secondPanePath = path;
                }
                else
                {
                    FirstPaneCurrentSelection = 1;
                    firstPanePath = path;
                }
            }
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Folder doesn't exist. Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
            RedrawRequired = true;
        }

        private static void CopyTo(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string path = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path or a full path to a destination folder to copy the selected file to."), BoxForegroundColor, BoxBackgroundColor);
                path = Filesystem.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                        Copying.CopyFileOrDir(currentFileSystemInfo.FullName, path);
                    else
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
                RedrawRequired = true;
            }
        }

        private static void MoveTo(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string path = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path or a full path to a destination folder to move the selected file to."), BoxForegroundColor, BoxBackgroundColor);
                path = Filesystem.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                        Moving.MoveFileOrDir(currentFileSystemInfo.FullName, path);
                    else
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
                RedrawRequired = true;
            }
        }

        private static void Rename(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            try
            {
                string filename = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a new file name."), BoxForegroundColor, BoxBackgroundColor);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");
                if (!Checking.FileExists(filename))
                {
                    if (Parsing.TryParseFileName(filename))
                        Moving.MoveFileOrDir(currentFileSystemInfo.FullName, Path.GetDirectoryName(currentFileSystemInfo.FullName) + $"/{filename}");
                    else
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Make sure that you've written the correct file name."), BoxForegroundColor, BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File already exists. The name shouldn't be occupied by another file."), BoxForegroundColor, BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
                RedrawRequired = true;
            }
        }

        private static void MakeDir()
        {
            // Now, render the search box
            string path = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a new directory name."), BoxForegroundColor, BoxBackgroundColor);
            path = Filesystem.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath);
            if (!Checking.FolderExists(path))
                Making.TryMakeDirectory(path);
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Folder already exists. The name shouldn't be occupied by another folder."), BoxForegroundColor, BoxBackgroundColor);
            RedrawRequired = true;
        }
    }
}
