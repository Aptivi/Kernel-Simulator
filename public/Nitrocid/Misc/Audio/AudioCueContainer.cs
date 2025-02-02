﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection.Internal;
using System.Diagnostics;
using System.IO;

namespace Nitrocid.Misc.Audio
{
    /// <summary>
    /// Audio cue container
    /// </summary>
    [DebuggerDisplay("{Name}: {DisplayName}")]
    public class AudioCueContainer
    {
        /// <summary>
        /// Name of the audio cue container
        /// </summary>
        public string Name { get; internal set; } = "the_mirage";
        
        /// <summary>
        /// Display name of the audio cue container
        /// </summary>
        public string DisplayName { get; internal set; } = "The Mirage";

        /// <summary>
        /// A stream containing audio data for the intense version of the alarm sound effect
        /// </summary>
        public Stream? AlarmStream { get; internal set; } = ResourcesManager.GetData("the_mirage.alarm.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the idle version of the alarm sound effect
        /// </summary>
        public Stream? AlarmIdleStream { get; internal set; } = ResourcesManager.GetData("the_mirage.alarm-idle.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the intense version of the ambience sound effect
        /// </summary>
        public Stream? AmbienceStream { get; internal set; } = ResourcesManager.GetData("the_mirage.ambience.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the idle version of the ambience sound effect
        /// </summary>
        public Stream? AmbienceIdleStream { get; internal set; } = ResourcesManager.GetData("the_mirage.ambience-idle.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the high-priority beep sound
        /// </summary>
        public Stream? BeepHighStream { get; internal set; } = ResourcesManager.GetData("the_mirage.beep-high.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the medium-priority beep sound
        /// </summary>
        public Stream? BeepMediumStream { get; internal set; } = ResourcesManager.GetData("the_mirage.beep-med.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the low-priority beep sound
        /// </summary>
        public Stream? BeepLowStream { get; internal set; } = ResourcesManager.GetData("the_mirage.beep-low.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the keyboard cue backspace sound
        /// </summary>
        public Stream? KeyboardCueBackspaceStream { get; internal set; } = ResourcesManager.GetData("the_mirage.keyboard-cue-backspace.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the keyboard cue enter sound
        /// </summary>
        public Stream? KeyboardCueEnterStream { get; internal set; } = ResourcesManager.GetData("the_mirage.keyboard-cue-enter.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the keyboard cue type sound
        /// </summary>
        public Stream? KeyboardCueTypeStream { get; internal set; } = ResourcesManager.GetData("the_mirage.keyboard-cue-type.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the high-priority notification sound
        /// </summary>
        public Stream? NotificationHighStream { get; internal set; } = ResourcesManager.GetData("the_mirage.notification-high.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the medium-priority notification sound
        /// </summary>
        public Stream? NotificationMediumStream { get; internal set; } = ResourcesManager.GetData("the_mirage.notification-medium.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the low-priority notification sound
        /// </summary>
        public Stream? NotificationLowStream { get; internal set; } = ResourcesManager.GetData("the_mirage.notification-low.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the shutdown sound
        /// </summary>
        public Stream? ShutdownStream { get; internal set; } = ResourcesManager.GetData("the_mirage.shutdown.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the special beep sound
        /// </summary>
        public Stream? SpecialBeepStream { get; internal set; } = ResourcesManager.GetData("the_mirage.special-beep.mp3", ResourcesType.Audio);

        /// <summary>
        /// A stream containing audio data for the startup sound
        /// </summary>
        public Stream? StartupStream { get; internal set; } = ResourcesManager.GetData("the_mirage.startup.mp3", ResourcesType.Audio);

        internal Stream? GetStream(AudioCueType cueType) =>
            cueType switch
            {
                AudioCueType.Alarm => AlarmStream,
                AudioCueType.AlarmIdle => AlarmIdleStream,
                AudioCueType.Ambience => AmbienceStream,
                AudioCueType.AmbienceIdle => AmbienceIdleStream,
                AudioCueType.BeepHigh => BeepHighStream,
                AudioCueType.BeepMedium => BeepMediumStream,
                AudioCueType.BeepLow => BeepLowStream,
                AudioCueType.KeyboardCueBackspace => KeyboardCueBackspaceStream,
                AudioCueType.KeyboardCueEnter => KeyboardCueEnterStream,
                AudioCueType.KeyboardCueType => KeyboardCueTypeStream,
                AudioCueType.NotificationHigh => NotificationHighStream,
                AudioCueType.NotificationMedium => NotificationMediumStream,
                AudioCueType.NotificationLow => NotificationLowStream,
                AudioCueType.Shutdown => ShutdownStream,
                AudioCueType.SpecialBeep => SpecialBeepStream,
                AudioCueType.Startup => StartupStream,
                _ => throw new KernelException(KernelExceptionType.AudioCue, Translate.DoTranslation("There is no such audio cue type.")),
            };

        internal AudioCueContainer()
            : this("the_mirage", "The Mirage")
        { }

        internal AudioCueContainer(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;

            // Populate streams
            AlarmStream = ResourcesManager.GetData($"{Name}.alarm.mp3", ResourcesType.Audio) ?? AlarmStream;
            AlarmIdleStream = ResourcesManager.GetData($"{Name}.alarm-idle.mp3", ResourcesType.Audio) ?? AlarmIdleStream;
            AmbienceStream = ResourcesManager.GetData($"{Name}.ambience.mp3", ResourcesType.Audio) ?? AmbienceStream;
            AmbienceIdleStream = ResourcesManager.GetData($"{Name}.ambience-idle.mp3", ResourcesType.Audio) ?? AmbienceIdleStream;
            BeepHighStream = ResourcesManager.GetData($"{Name}.beep-high.mp3", ResourcesType.Audio) ?? BeepHighStream;
            BeepMediumStream = ResourcesManager.GetData($"{Name}.beep-med.mp3", ResourcesType.Audio) ?? BeepMediumStream;
            BeepLowStream = ResourcesManager.GetData($"{Name}.beep-low.mp3", ResourcesType.Audio) ?? BeepLowStream;
            KeyboardCueBackspaceStream = ResourcesManager.GetData($"{Name}.keyboard-cue-backspace.mp3", ResourcesType.Audio) ?? KeyboardCueBackspaceStream;
            KeyboardCueEnterStream = ResourcesManager.GetData($"{Name}.keyboard-cue-enter.mp3", ResourcesType.Audio) ?? KeyboardCueEnterStream;
            KeyboardCueTypeStream = ResourcesManager.GetData($"{Name}.keyboard-cue-type.mp3", ResourcesType.Audio) ?? KeyboardCueTypeStream;
            NotificationHighStream = ResourcesManager.GetData($"{Name}.notification-high.mp3", ResourcesType.Audio) ?? NotificationHighStream;
            NotificationMediumStream = ResourcesManager.GetData($"{Name}.notification-medium.mp3", ResourcesType.Audio) ?? NotificationMediumStream;
            NotificationLowStream = ResourcesManager.GetData($"{Name}.notification-low.mp3", ResourcesType.Audio) ?? NotificationLowStream;
            ShutdownStream = ResourcesManager.GetData($"{Name}.shutdown.mp3", ResourcesType.Audio) ?? ShutdownStream;
            SpecialBeepStream = ResourcesManager.GetData($"{Name}.special-beep.mp3", ResourcesType.Audio) ?? SpecialBeepStream;
            StartupStream = ResourcesManager.GetData($"{Name}.startup.mp3", ResourcesType.Audio) ?? StartupStream;
        }
    }
}
