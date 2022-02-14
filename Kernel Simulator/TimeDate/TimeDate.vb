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

Imports System.ComponentModel
Imports System.Threading
Imports KS.Misc.Screensaver

Namespace TimeDate
    Public Module TimeDate

        'Variables
        Public KernelDateTime As New Date
        Public KernelDateTimeUtc As New Date
        Friend TimeDateChange As New Thread(AddressOf TimeDateChange_DoWork) With {.Name = "Time/date updater thread", .IsBackground = True}

        ''' <summary>
        ''' Specifies the time/date format type.
        ''' </summary>
        Public Enum FormatType
            ''' <summary>
            ''' Long time/date format
            ''' </summary>
            [Long]
            ''' <summary>
            ''' Short time/date format
            ''' </summary>
            [Short]
        End Enum

        ''' <summary>
        ''' Updates the time and date. Also updates the time and date corner if it was enabled in kernel configuration.
        ''' </summary>
        Sub TimeDateChange_DoWork()
            Dim oldWid, oldTop As Integer
            Do While True
                Dim TimeString As String = $"{RenderDate()} - {RenderTime()}"
                KernelDateTime = Date.Now
                KernelDateTimeUtc = Date.UtcNow
                If CornerTimeDate = True And Not InSaver Then
                    oldWid = Console.WindowWidth - TimeString.Length - 1
                    oldTop = Console.WindowTop
                    WriteWhere(TimeString, Console.WindowWidth - TimeString.Length - 1, Console.WindowTop, True, ColTypes.Neutral)
                End If
                Thread.Sleep(1000)
                If oldWid <> 0 Then WriteWhere(" ".Repeat(TimeString.Length), oldWid, oldTop, True, ColTypes.Neutral)
            Loop
        End Sub

        ''' <summary>
        ''' Updates the KernelDateTime so it reflects the current time, and runs the updater.
        ''' </summary>
        Sub InitTimeDate()
            If Not TimeDateChange.IsAlive Then
                KernelDateTime = Date.Now
                KernelDateTimeUtc = Date.UtcNow
                TimeDateChange.Start()
            End If
        End Sub

        ''' <summary>
        ''' Shows current time, date, and timezone.
        ''' </summary>
        Public Sub ShowCurrentTimes()
            Write("datetime: ", False, ColTypes.ListEntry) : Write(DoTranslation("Current time is {0}"), True, ColTypes.ListValue, RenderTime)
            Write("datetime: ", False, ColTypes.ListEntry) : Write(DoTranslation("Today is {0}"), True, ColTypes.ListValue, RenderDate)
            Write("datetime: ", False, ColTypes.ListEntry) : Write(DoTranslation("Time and date in UTC: {0}"), True, ColTypes.ListValue, RenderUtc)
            Write("datetime: ", False, ColTypes.ListEntry) : Write(DoTranslation("Time Zone:") + " {0} ({1})", True, ColTypes.ListValue, TimeZone.CurrentTimeZone.StandardName, TimeZone.CurrentTimeZone.GetUtcOffset(KernelDateTime).ToString(If(TimeZone.CurrentTimeZone.GetUtcOffset(KernelDateTime) < TimeSpan.Zero, "\-", "\+") + "hh\:mm\:ss"))
        End Sub

        ''' <summary>
        ''' Gets the remaining time from now
        ''' </summary>
        ''' <param name="Milliseconds">The milliseconds interval</param>
        Public Function GetRemainingTimeFromNow(Milliseconds As Integer) As String
            Dim RemainingTime As TimeSpan = (Date.Now.AddMilliseconds(Milliseconds) - Date.Now)
            Dim RemainingTimeString As String = RemainingTime.ToString("d\.hh\:mm\:ss\.fff", CurrentCult)
            Return RemainingTimeString
        End Function

    End Module
End Namespace
