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

Imports System.IO

Public Module Input

    ''' <summary>
    ''' Reads the next line of characters from the standard input stream without showing input being written by user.
    ''' </summary>
    ''' <param name="MaskChar">Specifies the password mask character</param>
    Public Function ReadLineNoInput(MaskChar As Char) As String
        Dim Final As String = ""
        While True
            Dim KeyInfo As ConsoleKeyInfo = Console.ReadKey(True)
            Dim KeyCharacter As Char = KeyInfo.KeyChar
            If KeyCharacter = vbCr Or KeyCharacter = vbLf Then
                Exit While
            ElseIf KeyInfo.Key = ConsoleKey.Backspace Then
                If Not Final.Length = 0 Then
                    Final = Final.Remove(Final.Length - 1)
                    If Not MaskChar = vbNullChar Then
                        Console.Write(GetEsc() + "D") 'Cursor backwards by one character
                        Console.Write(GetEsc() + "[1X") 'Remove a character
                    End If
                End If
            Else
                Final += KeyCharacter
                If Not MaskChar = vbNullChar Then Console.Write(MaskChar)
            End If
        End While
        Return Final
    End Function

    ''' <summary>
    ''' Reads the next key from the console input stream with the timeout
    ''' </summary>
    ''' <param name="Intercept"></param>
    ''' <param name="Timeout"></param>
    Public Function ReadKeyTimeout(Intercept As Boolean, Timeout As TimeSpan) As ConsoleKeyInfo
        Dim CurrentMilliseconds As Double
        While Not Console.KeyAvailable
            If Not CurrentMilliseconds = Timeout.TotalMilliseconds Then
                CurrentMilliseconds += 1
            Else
                Throw New Exceptions.ConsoleReadTimeoutException(DoTranslation("User didn't provide any input in a timely fashion."))
            End If
            Threading.Thread.Sleep(1)
        End While
        Return Console.ReadKey(Intercept)
    End Function

    ''' <summary>
    ''' Reads the next line of characters until the condition is met or the user pressed ENTER
    ''' </summary>
    ''' <param name="Condition">The condition to be met</param>
    Public Function ReadLineUntil(ByRef Condition As Boolean) As String
        Dim Final As String = ""
        Dim Finished As Boolean
        While Not Finished
            Dim KeyInfo As ConsoleKeyInfo
            Dim KeyCharacter As Char
            While Not Console.KeyAvailable
                If Condition Then Finished = True
                Threading.Thread.Sleep(1)
            End While
            KeyInfo = Console.ReadKey(True)
            KeyCharacter = KeyInfo.KeyChar
            If KeyCharacter = vbCr Or KeyCharacter = vbLf Then
                Finished = True
            ElseIf KeyInfo.Key = ConsoleKey.Backspace Then
                If Not Final.Length = 0 Then
                    Final = Final.Remove(Final.Length - 1)
                    Console.Write(GetEsc() + "D") 'Cursor backwards by one character
                    Console.Write(GetEsc() + "[1X") 'Remove a character
                End If
            Else
                Final += KeyCharacter
                Console.Write(KeyCharacter)
            End If
        End While
        Return Final
    End Function

    ''' <summary>
    ''' Reads the next line of characters that exceed the 256-character limit up to 65536 characters
    ''' </summary>
    Public Function ReadLineLong() As String
        Console.SetIn(New StreamReader(Console.OpenStandardInput(65536), Console.InputEncoding, False, 65536))
        Return Console.ReadLine()
    End Function

End Module
