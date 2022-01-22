﻿
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Namespace Misc.Notifications
    ''' <summary>
    ''' Notification holder with title, description, and priority
    ''' </summary>
    Public Class Notification

        Private _Progress As Integer

        ''' <summary>
        ''' Notification title
        ''' </summary>
        Property Title As String

        ''' <summary>
        ''' Notification description
        ''' </summary>
        Property Desc As String

        ''' <summary>
        ''' Notification priority
        ''' </summary>
        Property Priority As NotifPriority

        ''' <summary>
        ''' Notification type
        ''' </summary>
        Property Type As NotifType

        ''' <summary>
        ''' Whether the progress failed
        ''' </summary>
        Property ProgressFailed As Boolean

        ''' <summary>
        ''' Notification progress
        ''' </summary>
        Property Progress As Integer
            Get
                Return _Progress
            End Get
            Set
                If Value >= 100 Then
                    _Progress = 100
                ElseIf Value <= 0 Then
                    _Progress = 0
                Else
                    _Progress = Value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Whether the progress has been compeleted successfully or with failure
        ''' </summary>
        ReadOnly Property ProgressCompleted As Boolean
            Get
                Return _Progress >= 100 Or ProgressFailed
            End Get
        End Property

        ''' <summary>
        ''' The notification border color
        ''' </summary>
        Property NotificationBorderColor As Color

        ''' <summary>
        ''' Creates a new notification
        ''' </summary>
        ''' <param name="Title">Title of notification</param>
        ''' <param name="Desc">Description of notification</param>
        ''' <param name="Priority">Priority of notification</param>
        ''' <param name="Type">Notification type</param>
        Public Sub New(Title As String, Desc As String, Priority As NotifPriority, Type As NotifType)
            Me.Title = Title
            Me.Desc = Desc
            Me.Priority = Priority
            Me.Type = Type
        End Sub

    End Class
End Namespace