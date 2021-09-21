﻿
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.Globalization

Public Module TimeDateRenderersUtc

    ''' <summary>
    ''' Renders the current time based on kernel config (long or short) and current culture
    ''' </summary>
    ''' <returns>A long or short time</returns>
    Public Function RenderTimeUtc() As String
        If LongTimeDate Then
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.LongTimePattern, CurrentCult)
        Else
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current time based on kernel config (long or short) and current culture
    ''' </summary>
    ''' <param name="FormatType">Date/time format type</param>
    ''' <returns>A long or short time</returns>
    Public Function RenderTimeUtc(FormatType As FormatType) As String
        If FormatType = FormatType.Long Then
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.LongTimePattern, CurrentCult)
        Else
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current time based on specified culture
    ''' </summary>
    ''' <param name="Cult">A culture.</param>
    ''' <returns>A time</returns>
    Public Function RenderTimeUtc(Cult As CultureInfo) As String
        If LongTimeDate Then
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
        Else
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current time based on specified culture
    ''' </summary>
    ''' <param name="Cult">A culture.</param>
    ''' <param name="FormatType">Date/time format type</param>
    ''' <returns>A time</returns>
    Public Function RenderTimeUtc(Cult As CultureInfo, FormatType As FormatType) As String
        If FormatType = FormatType.Long Then
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
        Else
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current date based on kernel config (long or short) and current culture
    ''' </summary>
    ''' <returns>A long or short date</returns>
    Public Function RenderDateUtc() As String
        If LongTimeDate Then
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.LongDatePattern, CurrentCult)
        Else
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current date based on kernel config (long or short) and current culture
    ''' </summary>
    ''' <param name="FormatType">Date/time format type</param>
    ''' <returns>A long or short date</returns>
    Public Function RenderDateUtc(FormatType As FormatType) As String
        If FormatType = FormatType.Long Then
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.LongDatePattern, CurrentCult)
        Else
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current date based on specified culture
    ''' </summary>
    ''' <param name="Cult">A culture.</param>
    ''' <returns>A date</returns>
    Public Function RenderDateUtc(Cult As CultureInfo) As String
        If LongTimeDate Then
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
        Else
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current date based on specified culture
    ''' </summary>
    ''' <param name="Cult">A culture.</param>
    ''' <param name="FormatType">Date/time format type</param>
    ''' <returns>A date</returns>
    Public Function RenderDateUtc(Cult As CultureInfo, FormatType As FormatType) As String
        If FormatType = FormatType.Long Then
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
        Else
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current time and date based on kernel config (long or short) and current culture
    ''' </summary>
    ''' <returns>A long or short time and date</returns>
    Public Function RenderUtc() As String
        If LongTimeDate Then
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.FullDateTimePattern, CurrentCult)
        Else
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult) + " - " + KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current time and date based on kernel config (long or short) and current culture
    ''' </summary>
    ''' <param name="FormatType">Date/time format type</param>
    ''' <returns>A long or short time and date</returns>
    Public Function RenderUtc(FormatType As FormatType) As String
        If FormatType = FormatType.Long Then
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.FullDateTimePattern, CurrentCult)
        Else
            Return KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult) + " - " + KernelDateTimeUtc.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current time and date based on specified culture
    ''' </summary>
    ''' <param name="Cult">A culture.</param>
    ''' <returns>A time and date</returns>
    Public Function RenderUtc(Cult As CultureInfo) As String
        If LongTimeDate Then
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
        Else
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
        End If
    End Function

    ''' <summary>
    ''' Renders the current time and date based on specified culture
    ''' </summary>
    ''' <param name="Cult">A culture.</param>
    ''' <param name="FormatType">Date/time format type</param>
    ''' <returns>A time and date</returns>
    Public Function RenderUtc(Cult As CultureInfo, FormatType As FormatType) As String
        If FormatType = FormatType.Long Then
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
        Else
            Return KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + KernelDateTimeUtc.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
        End If
    End Function

End Module
