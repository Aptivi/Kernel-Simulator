﻿
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports KS.Misc.Notifications

Namespace Misc.Calendar.Reminders
    Public Class ReminderInfo

        Private ReminderNotified As Boolean
        ''' <summary>
        ''' Reminder date
        ''' </summary>
        Public Property ReminderDate As Date
        ''' <summary>
        ''' Reminder title
        ''' </summary>
        Public Property ReminderTitle As String
        ''' <summary>
        ''' Reminder importance
        ''' </summary>
        Public Property ReminderImportance As NotifPriority

        ''' <summary>
        ''' Notifies the user about the reminder
        ''' </summary>
        Protected Friend Sub NotifyReminder()
            If Not ReminderNotified Then
                Dim ReminderNotification As New Notification(ReminderTitle, DoTranslation("Don't miss this!"), ReminderImportance, NotifType.Normal)
                NotifySend(ReminderNotification)
                ReminderNotified = True
            End If
        End Sub

    End Class
End Namespace