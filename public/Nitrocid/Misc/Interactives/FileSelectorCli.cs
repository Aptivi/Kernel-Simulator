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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using Terminaux.Sequences;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Drivers;
using Nitrocid.Misc.Reflection;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Folders;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Files.Operations.Printing;
using Nitrocid.Drivers.Encryption;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Files.Paths;
using Nitrocid.Files.Instances;
using Nitrocid.Files.LineEndings;
using Nitrocid.Files.Extensions;
using Nitrocid.Files.Operations.Querying;
using Textify.General;

namespace Nitrocid.Misc.Interactives
{
    /// <summary>
    /// File selector class, a descendant of the file manager
    /// </summary>
    public class FileSelectorCli : BaseInteractiveTui<FileSystemEntry>, IInteractiveTui<FileSystemEntry>
    {
        internal string selectedFile = "";
        internal string firstPanePath = PathsManagement.HomePath;
        internal bool refreshFirstPaneListing = true;
        private List<FileSystemEntry> firstPaneListing = [];

        /// <inheritdoc/>
        public override IEnumerable<FileSystemEntry> PrimaryDataSource
        {
            get
            {
                try
                {
                    if (refreshFirstPaneListing)
                    {
                        refreshFirstPaneListing = false;
                        firstPaneListing = Listing.CreateList(firstPanePath, true);
                    }
                    return firstPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get current directory list for the first pane [{0}]: {1}", firstPanePath, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(FileSystemEntry item)
        {
            FileSystemEntry FileInfoCurrentPane = item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane == null)
                return Translate.DoTranslation("No info.");

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = FileInfoCurrentPane.Type == FileSystemEntryType.Directory;
                string status = $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.BaseEntry.Name}";
                if (!string.IsNullOrEmpty(selectedFile))
                    status = $"{Translate.DoTranslation("Selected")} {selectedFile} - {status}";
                return status;
            }
            catch (Exception ex)
            {
                return Translate.DoTranslation(ex.Message);
            }
        }

        /// <inheritdoc/>
        public override string GetInfoFromItem(FileSystemEntry item)
        {
            try
            {
                FileSystemEntry file = item;
                bool isDirectory = file.Type == FileSystemEntryType.Directory;
                bool isSelected = SelectedFile == file.FilePath;
                var size = file.FileSize;
                var path = file.FilePath;
                string finalRenderedName = Translate.DoTranslation("File name") + $": {Path.GetFileName(file.FilePath)}";
                string finalRenderedDir = Translate.DoTranslation("Is a directory") + $": {isDirectory}";
                string finalRenderedSelected = Translate.DoTranslation("Is selected") + $": {isSelected}";
                string finalRenderedSize = Translate.DoTranslation("File size") + $": {size.SizeString()}";
                string finalRenderedPath = Translate.DoTranslation("File path") + $": {path}";
                return
                    finalRenderedName + CharManager.NewLine +
                    finalRenderedDir + CharManager.NewLine +
                    finalRenderedSelected + CharManager.NewLine +
                    finalRenderedSize + CharManager.NewLine +
                    finalRenderedPath
                ;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get file entry: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(FileSystemEntry item)
        {
            try
            {
                FileSystemEntry file = item;
                bool isDirectory = file.Type == FileSystemEntryType.Directory;
                bool isSelected = SelectedFile == file.FilePath;
                if (Config.MainConfig.IfmShowFileSize)
                    return
                        // Name and directory indicator
                        $"[{(isDirectory ? "/" : "*")}] [{(isSelected ? "+" : " ")}] {file.BaseEntry.Name} | " +

                        // File size or directory size
                        $"{(!isDirectory ? ((FileInfo)file.BaseEntry).Length.SizeString() : SizeGetter.GetAllSizesInFolder((DirectoryInfo)file.BaseEntry).SizeString())}"
                    ;
                else
                    return $"[{(isDirectory ? "/" : "*")}] [{(isSelected ? "+" : " ")}] {file.BaseEntry.Name}";
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <summary>
        /// Selected file. If not selected yet and bailed earlier, this string is empty.
        /// </summary>
        public string SelectedFile =>
            selectedFile;

        internal void SelectOrGoTo(FileSystemEntry currentFileSystemEntry)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (currentFileSystemEntry is null)
                    return;

                // Check for existence
                if (!currentFileSystemEntry.Exists)
                    return;

                // Now that the selected file or folder exists, check the type.
                if (currentFileSystemEntry.Type == FileSystemEntryType.Directory)
                {
                    // We're dealing with a folder. Open it in the selected pane.
                    firstPanePath = FilesystemTools.NeutralizePath(currentFileSystemEntry.FilePath + "/");
                    InteractiveTuiTools.SelectionMovement(this, 1);
                    refreshFirstPaneListing = true;
                }
                else if (currentFileSystemEntry.Type == FileSystemEntryType.File)
                {
                    // We're dealing with a file. Clear the screen and open the appropriate editor.
                    selectedFile = currentFileSystemEntry.FilePath;
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected '{0}'. Press ESC to exit and confirm selection."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor, selectedFile);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't open folder or select file") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void GoUp()
        {
            firstPanePath = FilesystemTools.NeutralizePath(firstPanePath + "/..");
            InteractiveTuiTools.SelectionMovement(this, 1);
            refreshFirstPaneListing = true;
        }

        internal void PrintFileSystemEntry(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // Render the final information string
            try
            {
                var finalInfoRendered = new StringBuilder();
                string fullPath = currentFileSystemEntry.FilePath;
                if (Checking.FolderExists(fullPath))
                {
                    // The file system info instance points to a folder
                    var DirInfo = new DirectoryInfo(fullPath);
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), DirInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name: {0}"), FilesystemTools.NeutralizePath(DirInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Size: {0}"), SizeGetter.GetAllSizesInFolder(DirInfo).SizeString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Creation time: {0}"), TimeDateRenderers.Render(DirInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last access time: {0}"), TimeDateRenderers.Render(DirInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last write time: {0}"), TimeDateRenderers.Render(DirInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Attributes: {0}"), DirInfo.Attributes));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Parent directory: {0}"), FilesystemTools.NeutralizePath(DirInfo.Parent.FullName)));
                }
                else
                {
                    // The file system info instance points to a file
                    FileInfo fileInfo = new(fullPath);
                    bool isBinary = Parsing.IsBinaryFile(fileInfo.FullName);
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), fileInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name: {0}"), FilesystemTools.NeutralizePath(fileInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("File size: {0}"), fileInfo.Length.SizeString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Creation time: {0}"), TimeDateRenderers.Render(fileInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last access time: {0}"), TimeDateRenderers.Render(fileInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last write time: {0}"), TimeDateRenderers.Render(fileInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Attributes: {0}"), fileInfo.Attributes));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Where to find: {0}"), FilesystemTools.NeutralizePath(fileInfo.DirectoryName)));
                    if (!isBinary)
                    {
                        var Style = LineEndingsTools.GetLineEndingFromFile(fullPath);
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Newline style:") + " {0}", Style.ToString()));
                    }
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Binary file:") + " {0}", isBinary));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("MIME metadata:") + " {0}\n", MimeTypes.GetMimeType(fileInfo.Extension)));

                    // .NET managed info
                    if (ReflectionCommon.IsDotnetAssemblyFile(fullPath, out AssemblyName asmName))
                    {
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), asmName.Name));
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name") + ": {0}", asmName.FullName));
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Version") + ": {0}", asmName.Version.ToString()));
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Culture name") + ": {0}", asmName.CultureName));
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Content type") + ": {0}\n", asmName.ContentType.ToString()));
                    }
                    else
                    {
                        finalInfoRendered.AppendLine(Translate.DoTranslation("File is not a valid .NET assembly.\n"));
                    }

                    // Other info handled by the extension handler
                    if (ExtensionHandlerTools.IsHandlerRegistered(fileInfo.Extension))
                    {
                        var handler = ExtensionHandlerTools.GetExtensionHandler(fileInfo.Extension);
                        finalInfoRendered.AppendLine(handler.InfoHandler(fullPath));
                    }
                }
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));

                // Now, render the info box
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't get file system info") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void RemoveFileOrDir(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                Removing.RemoveFileOrDir(currentFileSystemEntry.FilePath);
                refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't remove file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void GoTo()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a local folder."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            path = FilesystemTools.NeutralizePath(path, firstPanePath);
            if (Checking.FolderExists(path))
            {
                InteractiveTuiTools.SelectionMovement(this, 1);
                firstPanePath = path;
                refreshFirstPaneListing = true;
            }
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Folder doesn't exist. Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
        }

        internal void CopyTo(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a destination folder to copy the selected file to."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                path = FilesystemTools.NeutralizePath(path, firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Copying.CopyFileOrDir(currentFileSystemEntry.FilePath, path);
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void MoveTo(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a destination folder to move the selected file to."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                path = FilesystemTools.NeutralizePath(path, firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Moving.MoveFileOrDir(currentFileSystemEntry.FilePath, path);
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void Rename(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string filename = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a new file name."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");
                if (!Checking.FileExists(filename))
                {
                    if (Parsing.TryParseFileName(filename))
                    {
                        Moving.MoveFileOrDir(currentFileSystemEntry.FilePath, Path.GetDirectoryName(currentFileSystemEntry.FilePath) + $"/{filename}");
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Make sure that you've written the correct file name."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File already exists. The name shouldn't be occupied by another file."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void MakeDir()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a new directory name."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            path = FilesystemTools.NeutralizePath(path, firstPanePath);
            if (!Checking.FolderExists(path))
            {
                Making.TryMakeDirectory(path);
                refreshFirstPaneListing = true;
            }
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Folder already exists. The name shouldn't be occupied by another folder."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
        }

        internal void Hash(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!Checking.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected entry is not a file."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
            else
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Hash driver not found."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }
            InfoBoxColor.WriteInfoBoxColorBack(hash, Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
        }

        internal void Verify(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!Checking.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected entry is not a file."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
            else
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Hash driver not found."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }

            // Now, let the user write the expected hash
            string expectedHash = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter your expected hash"), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            if (expectedHash == hash)
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Two hashes match!"), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Two hashes don't match."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
        }

        internal void Preview(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!Checking.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected entry is not a file."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }

            // Render the preview box
            string preview = FileContentPrinter.RenderContents(currentFileSystemEntry.FilePath);
            string filtered = VtSequenceTools.FilterVTSequences(preview);
            InfoBoxColor.WriteInfoBoxColorBack(filtered, Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
        }
    }
}
