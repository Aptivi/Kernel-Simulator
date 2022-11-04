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

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows recent notifications
    /// </summary>
    /// <remarks>
    /// If you need to see recent notifications, you can see them using this command. Any sent notifications will be put to the list that can be shown using this command. This is useful for dismissnotif command.
    /// </remarks>
    class ShowNotifsCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int Count = 1;
            if (!(Notifications.NotifRecents.Count == 0))
            {
                foreach (Notification Notif in Notifications.NotifRecents)
                {
                    TextWriterColor.Write($"[{Count}/{Notifications.NotifRecents.Count}] {Notif.Title}: ", false, ColorTools.ColTypes.ListEntry);
                    TextWriterColor.Write(Notif.Desc, false, ColorTools.ColTypes.ListValue);
                    if (Notif.Type == Notifications.NotifType.Progress)
                    {
                        TextWriterColor.Write($" ({Notif.Progress}%)", false, Notif.ProgressFailed ? ColorTools.ColTypes.Error : ColorTools.ColTypes.Success);
                    }
                    TextWriterColor.Write();
                    Count += 1;
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("No recent notifications"));
            }
        }

    }
}
