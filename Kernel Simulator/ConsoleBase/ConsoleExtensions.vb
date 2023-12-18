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

Imports Terminaux.Base
Imports TermConsoleExtensions = Terminaux.Base.ConsoleExtensions

Namespace ConsoleBase
    Public Module ConsoleExtensions

        ''' <summary>
        ''' Clears the console buffer, but keeps the current cursor position
        ''' </summary>
        Public Sub ClearKeepPosition()
            TermConsoleExtensions.ClearKeepPosition()
        End Sub

        ''' <summary>
        ''' Clears the line to the right
        ''' </summary>
        Public Sub ClearLineToRight()
            TermConsoleExtensions.ClearLineToRight()
        End Sub

        ''' <summary>
        ''' Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        ''' </summary>
        ''' <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        ''' <param name="MaximumNumber">The maximum number.</param>
        ''' <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
        ''' <returns>How many times to repeat the character</returns>
        Public Function PercentRepeat(CurrentNumber As Integer, MaximumNumber As Integer, WidthOffset As Integer) As Integer
            Return TermConsoleExtensions.PercentRepeat(CurrentNumber, MaximumNumber, WidthOffset)
        End Function

        ''' <summary>
        ''' Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        ''' </summary>
        ''' <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        ''' <param name="MaximumNumber">The maximum number.</param>
        ''' <param name="TargetWidth">The target width</param>
        ''' <returns>How many times to repeat the character</returns>
        Public Function PercentRepeatTargeted(CurrentNumber As Integer, MaximumNumber As Integer, TargetWidth As Integer) As Integer
            Return TermConsoleExtensions.PercentRepeatTargeted(CurrentNumber, MaximumNumber, TargetWidth)
        End Function

        ''' <summary>
        ''' Filters the VT sequences that matches the regex
        ''' </summary>
        ''' <param name="Text">The text that contains the VT sequences</param>
        ''' <returns>The text that doesn't contain the VT sequences</returns>
        Public Function FilterVTSequences(Text As String) As String
            Return TermConsoleExtensions.FilterVTSequences(Text)
        End Function

        ''' <summary>
        ''' Get the filtered cursor positions (by filtered means filtered from the VT escape sequences that matches the regex in the routine)
        ''' </summary>
        ''' <param name="Text">The text that contains the VT sequences</param>
        ''' <param name="Left">The filtered left position</param>
        ''' <param name="Top">The filtered top position</param>
        Public Sub GetFilteredPositions(Text As String, ByRef Left As Integer, ByRef Top As Integer, ParamArray Vars() As Object)
            Dim pos As (Integer, Integer) = TermConsoleExtensions.GetFilteredPositions(Text, False, Vars)
            Left = pos.Item1
            Top = pos.Item2
        End Sub

        ''' <summary>
        ''' Polls $TERM_PROGRAM to get terminal emulator
        ''' </summary>
        Public Function GetTerminalEmulator() As String
            Return ConsolePlatform.GetTerminalEmulator()
        End Function

        ''' <summary>
        ''' Polls $TERM to get terminal type (vt100, dumb, ...)
        ''' </summary>
        Public Function GetTerminalType() As String
            Return ConsolePlatform.GetTerminalType()
        End Function

        Public Sub SetTitle(Text As String)
            TermConsoleExtensions.SetTitle(Text)
        End Sub

    End Module
End Namespace
