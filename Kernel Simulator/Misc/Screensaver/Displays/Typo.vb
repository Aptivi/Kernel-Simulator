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

Imports System.ComponentModel

Module TypoDisplay

    Public WithEvents Typo As New BackgroundWorker

    Sub Typo_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Typo.DoWork
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Dim CpmSpeedMin As Integer = TypoWritingSpeedMin * 5
        Dim CpmSpeedMax As Integer = TypoWritingSpeedMax * 5
        Dim Strikes As New List(Of String) From {"q`12wsa", "r43edfgt5", "u76yhjki8", "p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa "}
        Dim CapStrikes As New List(Of String) From {" ~!@   ", " $#     %", " &^     *", " )(  :""{_+}|", "?"":> ", " <     "}
        Do While True
            SleepNoBlock(TypoDelay, Typo)
            If Typo.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(New Color(InputColor).VTSequenceForeground)
                Console.Write(New Color(BackgroundColor).VTSequenceBackground)
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Typo screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                'Prepare display (make a paragraph indentation)
                Console.WriteLine()
                Console.Write("    ")

                'Get struck character and write it
                For Each StruckChar As Char In TypoWrite
                    'Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                    Dim SelectedCpm As Integer = RandomDriver.Next(CpmSpeedMin, CpmSpeedMax)
                    Dim WriteMs As Integer = (60 / SelectedCpm) * 1000

                    'See if the typo is guaranteed
                    Dim Probability As Double = If(TypoMissStrikePossibility >= 80, 80, TypoMissStrikePossibility) / 100
                    Dim TypoGuaranteed As Boolean = Rnd() < Probability
                    If TypoGuaranteed Then
                        'Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                        Dim StrikeCharsIndex As Integer
                        Dim StruckFound As Boolean = False
                        Dim CappedStrike As Boolean = False
                        Dim StrikesString As String = ""
                        Do Until StruckFound
                            StrikeCharsIndex = RandomDriver.Next(0, Strikes.Count - 1)
                            CappedStrike = Char.IsUpper(StruckChar)
                            StrikesString = If(CappedStrike, Strikes(StrikeCharsIndex).ToUpper, Strikes(StrikeCharsIndex))
                            StruckFound = Not String.IsNullOrEmpty(StrikesString) AndAlso StrikesString.Contains(StruckChar)
                        Loop

                        'Select a random character that is a typo from the selected strike index
                        Dim RandomStrikeIndex As Integer = RandomDriver.Next(0, StrikesString.Count - 1)
                        Dim MistypedChar As Char = StrikesString(RandomStrikeIndex)
                        If "`-=\][';/.,".Contains(MistypedChar) And CappedStrike Then
                            'The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                            MistypedChar = CapStrikes(StrikeCharsIndex)(RandomStrikeIndex)
                        End If
                        StruckChar = MistypedChar
                    End If

                    'Write the final character to the console and wait
                    Console.Write(StruckChar)
                    SleepNoBlock(WriteMs, Typo)
                Next

                'Wait until retry
                Console.WriteLine()
                SleepNoBlock(TypoWriteAgainDelay, Typo)
            End If
        Loop
    End Sub

End Module
