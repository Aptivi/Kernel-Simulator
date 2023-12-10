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

using BassBoom.Basolia.File;
using BassBoom.Basolia.Format;
using BassBoom.Basolia.Playback;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using System;
using System.IO;
using System.Threading;

namespace Nitrocid.Extras.BassBoom.Commands
{
    /// <summary>
    /// Plays a sound file
    /// </summary>
    /// <remarks>
    /// This command allows you to play a sound file.
    /// </remarks>
    class PlaySoundCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = parameters.ArgumentsList[0];
            path = FilesystemTools.NeutralizePath(path);
            if (!Checking.FileExists(path))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't play sound because the file is not found."), KernelColorType.Error);
                return 29;
            }
            try
            {
                FileTools.OpenFile(path);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Opened music file successfully."), KernelColorType.Success);
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't open music file.") + $" {ex.Message}", KernelColorType.Error);
                return ex.HResult;
            }
            if (FileTools.IsOpened)
            {
                try
                {
                    // They must be done before playing
                    int total = AudioInfoTools.GetDuration(true);
                    AudioInfoTools.GetId3Metadata(out var managedV1, out var managedV2);

                    // Play now!
                    PlaybackTools.PlayAsync();
                    if (!SpinWait.SpinUntil(() => PlaybackTools.Playing, 15000))
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't play sound because of timeout."), KernelColorType.Error);
                        return 30;
                    }

                    // Print song info
                    string musicName =
                        !string.IsNullOrEmpty(managedV2.Title) ? managedV2.Title :
                        !string.IsNullOrEmpty(managedV1.Title) ? managedV1.Title :
                        Path.GetFileNameWithoutExtension(path);
                    string musicArtist =
                        !string.IsNullOrEmpty(managedV2.Artist) ? managedV2.Artist :
                        !string.IsNullOrEmpty(managedV1.Artist) ? managedV1.Artist :
                        Translate.DoTranslation("Unknown Artist");
                    string musicGenre =
                        !string.IsNullOrEmpty(managedV2.Genre) ? managedV2.Genre :
                        managedV1.GenreIndex >= 0 ? $"{managedV1.Genre} [{managedV1.GenreIndex}]" :
                        Translate.DoTranslation("Unknown Genre");
                    var totalSpan = AudioInfoTools.GetDurationSpanFromSamples(total);
                    string duration = totalSpan.ToString();
                    ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Name"), musicName);
                    ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Artist"), musicArtist);
                    ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Genre"), musicGenre);
                    ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Duration"), duration);

                    // Wait until the song stops or the user bails
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Press 'q' to stop playing."), KernelColorType.Tip);
                    while (PlaybackTools.Playing)
                    {
                        if (ConsoleWrapper.KeyAvailable)
                        {
                            var key = Input.DetectKeypress();
                            if (key.Key == ConsoleKey.Q)
                                PlaybackTools.Stop();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't play sound.") + $" {ex.Message}", KernelColorType.Error);
                    return ex.HResult;
                }
                finally
                {
                    FileTools.CloseFile();
                }
            }
            return 0;
        }

    }
}
