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

Namespace Misc.Threading
    Public Module ThreadManager

        ''' <summary>
        ''' Sleeps until either the time specified, or the thread has finished or cancelled.
        ''' </summary>
        ''' <param name="Time">Time in milliseconds</param>
        ''' <param name="ThreadWork">The working thread</param>
        Public Sub SleepNoBlock(Time As Long, ThreadWork As BackgroundWorker)
            Dim WorkFinished As Boolean
            Dim TimeCount As Long
            AddHandler ThreadWork.RunWorkerCompleted, Sub() WorkFinished = True
            Do Until WorkFinished Or TimeCount = Time
                Thread.Sleep(1)
                If ThreadWork.CancellationPending Then WorkFinished = True
                TimeCount += 1
            Loop
        End Sub

        ''' <summary>
        ''' Sleeps until either the time specified, or the thread is no longer alive.
        ''' </summary>
        ''' <param name="Time">Time in milliseconds</param>
        ''' <param name="ThreadWork">The working thread</param>
        Public Sub SleepNoBlock(Time As Long, ThreadWork As Thread)
            Dim WorkFinished As Boolean
            Dim TimeCount As Long
            Do Until WorkFinished Or TimeCount = Time
                Thread.Sleep(1)
                If Not ThreadWork.IsAlive Then WorkFinished = True
                TimeCount += 1
            Loop
        End Sub

        ''' <summary>
        ''' Gets the actual milliseconds time from the sleep time provided
        ''' </summary>
        ''' <param name="Time">Sleep time</param>
        ''' <returns>How many milliseconds did it really sleep?</returns>
        Public Function GetActualMilliseconds(Time As Integer) As Integer
            Dim SleepStopwatch As New Stopwatch
            SleepStopwatch.Start()
            Thread.Sleep(Time)
            SleepStopwatch.Stop()
            Return SleepStopwatch.ElapsedMilliseconds
        End Function

        ''' <summary>
        ''' Gets the actual ticks from the sleep time provided
        ''' </summary>
        ''' <param name="Time">Sleep time</param>
        ''' <returns>How many ticks did it really sleep?</returns>
        Public Function GetActualTicks(Time As Integer) As Long
            Dim SleepStopwatch As New Stopwatch
            SleepStopwatch.Start()
            Thread.Sleep(Time)
            SleepStopwatch.Stop()
            Return SleepStopwatch.ElapsedTicks
        End Function

    End Module
End Namespace