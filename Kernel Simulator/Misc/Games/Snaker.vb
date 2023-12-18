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

Imports System.Threading
Imports KS.Misc.Screensaver
Imports KS.Misc.Screensaver.Displays

Namespace Misc.Games
    Module Snaker

        ''' <summary>
        ''' Initializes the game
        ''' </summary>
        Sub InitializeSnaker(Simulation As Boolean)
            'Variables
            Dim RandomDriver As New Random()
            Dim CurrentWindowWidth As Integer = ConsoleWrapper.WindowWidth
            Dim CurrentWindowHeight As Integer = ConsoleWrapper.WindowHeight
            Dim ResizeSyncing As Boolean
            Dim SnakeLength As Integer = 1
            Dim SnakeMassPositions As New List(Of String)
            Dim Direction As SnakeDirection = SnakeDirection.Bottom
            ConsoleWrapper.CursorVisible = False
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            ConsoleWrapper.Clear()

            'Get the floor color ready
            Dim FloorColor As Color = ChangeSnakeColor()

            'Draw the floor
            If Not ResizeSyncing Then
                Dim FloorTopLeftEdge As Integer = 2
                Dim FloorBottomLeftEdge As Integer = 2
                Wdbg(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", FloorTopLeftEdge, FloorBottomLeftEdge)

                Dim FloorTopRightEdge As Integer = ConsoleWrapper.WindowWidth - 3
                Dim FloorBottomRightEdge As Integer = ConsoleWrapper.WindowWidth - 3
                Wdbg(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", FloorTopRightEdge, FloorBottomRightEdge)

                Dim FloorTopEdge As Integer = 2
                Dim FloorBottomEdge As Integer = ConsoleWrapper.WindowHeight - 2
                Wdbg(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", FloorTopEdge, FloorBottomEdge)

                Dim FloorLeftEdge As Integer = 2
                Dim FloorRightEdge As Integer = ConsoleWrapper.WindowWidth - 4
                Wdbg(DebugLevel.I, "Left edge: {0}, Right edge: {1}", FloorLeftEdge, FloorRightEdge)
                SetConsoleColor(FloorColor, True)

                'First, draw the floor top edge
                For x As Integer = FloorTopLeftEdge To FloorTopRightEdge
                    ConsoleWrapper.SetCursorPosition(x, 1)
                    Wdbg(DebugLevel.I, "Drawing floor top edge ({0}, {1})", x, 1)
                    WritePlain(" ", False)
                Next

                'Second, draw the floor bottom edge
                For x As Integer = FloorBottomLeftEdge To FloorBottomRightEdge
                    ConsoleWrapper.SetCursorPosition(x, FloorBottomEdge)
                    Wdbg(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", x, FloorBottomEdge)
                    WritePlain(" ", False)
                Next

                'Third, draw the floor left edge
                For y As Integer = FloorTopEdge To FloorBottomEdge
                    ConsoleWrapper.SetCursorPosition(FloorLeftEdge, y)
                    Wdbg(DebugLevel.I, "Drawing floor left edge ({0}, {1})", FloorLeftEdge, y)
                    WritePlain("  ", False)
                Next

                'Finally, draw the floor right edge
                For y As Integer = FloorTopEdge To FloorBottomEdge
                    ConsoleWrapper.SetCursorPosition(FloorRightEdge, y)
                    Wdbg(DebugLevel.I, "Drawing floor right edge ({0}, {1})", FloorRightEdge, y)
                    WritePlain("  ", False)
                Next
            End If

            'Get the snake color ready
            Dim SnakeColor As Color = ChangeSnakeColor()

            'A typical snake usually starts in the middle.
            If Not ResizeSyncing Then
                Dim Dead As Boolean = False
                Dim FloorTopEdge As Integer = 1
                Dim FloorBottomEdge As Integer = ConsoleWrapper.WindowHeight - 2
                Dim FloorLeftEdge As Integer = 3
                Dim FloorRightEdge As Integer = ConsoleWrapper.WindowWidth - 4
                Wdbg(DebugLevel.I, "Floor top edge {0}", FloorTopEdge)
                Wdbg(DebugLevel.I, "Floor bottom edge {0}", FloorBottomEdge)
                Wdbg(DebugLevel.I, "Floor left edge {0}", FloorLeftEdge)
                Wdbg(DebugLevel.I, "Floor right edge {0}", FloorRightEdge)

                Dim SnakeCurrentX As Integer = ConsoleWrapper.WindowWidth / 2
                Dim SnakeCurrentY As Integer = ConsoleWrapper.WindowHeight / 2
                Wdbg(DebugLevel.I, "Initial snake position ({0}, {1})", SnakeCurrentX, SnakeCurrentY)

                Dim SnakeAppleX As Integer = RandomDriver.Next(FloorLeftEdge + 1, FloorRightEdge - 1)
                Dim SnakeAppleY As Integer = RandomDriver.Next(FloorTopEdge + 1, FloorBottomEdge - 1)
                Wdbg(DebugLevel.I, "Initial snake apple position ({0}, {1})", SnakeAppleX, SnakeAppleY)

                Dim DidHorizontal As Boolean = False
                Dim DidVertical As Boolean = False
                Dim AppleDrawn As Boolean = False
                Dim SnakePreviousX As Integer
                Dim SnakePreviousY As Integer
                Dim SnakeLastTailToWipeX As Integer
                Dim SnakeLastTailToWipeY As Integer

                Do Until Dead
                    'Delay
                    If Simulation Then SleepNoBlock(SnakerDelay, ScreensaverDisplayerThread) Else Thread.Sleep(SnakerDelay)
                    If CurrentWindowHeight <> ConsoleWrapper.WindowHeight Or CurrentWindowWidth <> ConsoleWrapper.WindowWidth Then ResizeSyncing = True
                    If ResizeSyncing Then Exit Do

                    'Remove excess mass
                    Console.BackgroundColor = ConsoleColor.Black
                    ConsoleWrapper.SetCursorPosition(SnakeLastTailToWipeX, SnakeLastTailToWipeY)
                    WritePlain(" ", False)

                    'Set the snake color
                    SetConsoleColor(SnakeColor, True)

                    'Draw an apple
                    If Not AppleDrawn Then
                        AppleDrawn = True
                        ConsoleWrapper.SetCursorPosition(SnakeAppleX, SnakeAppleY)
                        WritePlain("+", False)
                        Wdbg(DebugLevel.I, "Drawn apple at ({0}, {1})", SnakeAppleX, SnakeAppleY)
                    End If

                    'Make a snake
                    For PositionIndex As Integer = SnakeMassPositions.Count - 1 To 0 Step -1
                        Dim PositionStrings() As String = SnakeMassPositions(PositionIndex).Split("/")
                        Dim PositionX As Integer = PositionStrings(0)
                        Dim PositionY As Integer = PositionStrings(1)
                        ConsoleWrapper.SetCursorPosition(PositionX, PositionY)
                        WritePlain(" ", False)
                        ConsoleWrapper.SetCursorPosition(PositionX, PositionY)
                        Wdbg(DebugLevel.I, "Drawn snake at ({0}, {1}) for mass {2}/{3}", PositionX, PositionY, PositionIndex + 1, SnakeMassPositions.Count)
                    Next

                    'Set the previous positions
                    SnakePreviousX = SnakeCurrentX
                    SnakePreviousY = SnakeCurrentY

                    If Simulation Then
                        'Change the snake direction
                        Dim PossibilityToChange As Single = RandomDriver.NextDouble
                        If CInt(PossibilityToChange) = 1 Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Change guaranteed. {0}", PossibilityToChange)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Horizontal? {0}, Vertical? {1}", DidHorizontal, DidVertical)
                            If DidHorizontal Then
                                Direction = [Enum].Parse(GetType(SnakeDirection), RandomDriver.Next(2))
                            ElseIf DidVertical Then
                                Direction = [Enum].Parse(GetType(SnakeDirection), RandomDriver.Next(2, 4))
                            End If
                        End If
                        Select Case Direction
                            Case SnakeDirection.Bottom
                                SnakeCurrentY += 1
                                DidHorizontal = False
                                DidVertical = True
                                Wdbg(DebugLevel.I, "Increased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY)
                            Case SnakeDirection.Top
                                SnakeCurrentY -= 1
                                DidHorizontal = False
                                DidVertical = True
                                Wdbg(DebugLevel.I, "Decreased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY)
                            Case SnakeDirection.Left
                                SnakeCurrentX -= 1
                                DidHorizontal = True
                                DidVertical = False
                                Wdbg(DebugLevel.I, "Decreased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX)
                            Case SnakeDirection.Right
                                SnakeCurrentX += 1
                                DidHorizontal = True
                                DidVertical = False
                                Wdbg(DebugLevel.I, "Increased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX)
                        End Select
                    Else
                        'User pressed the arrow button to move the snake
                        If Console.KeyAvailable Then
                            Dim Pressed As ConsoleKey = DetectKeypress().Key
                            Select Case Pressed
                                Case ConsoleKey.DownArrow
                                    If DidHorizontal Then
                                        Direction = SnakeDirection.Bottom
                                    End If
                                Case ConsoleKey.UpArrow
                                    If DidHorizontal Then
                                        Direction = SnakeDirection.Top
                                    End If
                                Case ConsoleKey.LeftArrow
                                    If DidVertical Then
                                        Direction = SnakeDirection.Left
                                    End If
                                Case ConsoleKey.RightArrow
                                    If DidVertical Then
                                        Direction = SnakeDirection.Right
                                    End If
                            End Select
                        End If
                        Select Case Direction
                            Case SnakeDirection.Bottom
                                SnakeCurrentY += 1
                                DidHorizontal = False
                                DidVertical = True
                                Wdbg(DebugLevel.I, "Increased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY)
                            Case SnakeDirection.Top
                                SnakeCurrentY -= 1
                                DidHorizontal = False
                                DidVertical = True
                                Wdbg(DebugLevel.I, "Decreased vertical snake position from {0} to {1}", SnakePreviousY, SnakeCurrentY)
                            Case SnakeDirection.Left
                                SnakeCurrentX -= 1
                                DidHorizontal = True
                                DidVertical = False
                                Wdbg(DebugLevel.I, "Decreased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX)
                            Case SnakeDirection.Right
                                SnakeCurrentX += 1
                                DidHorizontal = True
                                DidVertical = False
                                Wdbg(DebugLevel.I, "Increased horizontal snake position from {0} to {1}", SnakePreviousX, SnakeCurrentX)
                        End Select
                    End If
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Snake is facing {0}.", Direction.ToString)

                    'Check death using mass position check
                    Dead = SnakeMassPositions.Contains($"{SnakeCurrentX}/{SnakeCurrentY}")
                    Wdbg(DebugLevel.I, "Mass position contains the current position ({0}, {1})? {2}", SnakeCurrentX, SnakeCurrentY, Dead)

                    'Add the mass position
                    If Not Dead Then SnakeMassPositions.Add($"{SnakeCurrentX}/{SnakeCurrentY}")
                    If SnakeMassPositions.Count > SnakeLength Then
                        Wdbg(DebugLevel.I, "Mass position count {0} exceeds snake length of {1}. Removing index 0...", SnakeMassPositions.Count, SnakeLength)
                        Dim LastTailPositionStrings() As String = SnakeMassPositions(0).Split("/")
                        SnakeLastTailToWipeX = LastTailPositionStrings(0)
                        SnakeLastTailToWipeY = LastTailPositionStrings(1)
                        SnakeMassPositions.RemoveAt(0)
                    End If

                    'Check death state
                    If Not Dead Then Dead = SnakeCurrentY = FloorTopEdge
                    Wdbg(DebugLevel.I, "Dead? {0} because current Y is {1} and top edge is {2}", Dead, SnakeCurrentY, FloorTopEdge)
                    If Not Dead Then Dead = SnakeCurrentY = FloorBottomEdge
                    Wdbg(DebugLevel.I, "Dead? {0} because current Y is {1} and bottom edge is {2}", Dead, SnakeCurrentY, FloorBottomEdge)
                    If Not Dead Then Dead = SnakeCurrentX = FloorLeftEdge
                    Wdbg(DebugLevel.I, "Dead? {0} because current X is {1} and left edge is {2}", Dead, SnakeCurrentX, FloorLeftEdge)
                    If Not Dead Then Dead = SnakeCurrentX = FloorRightEdge
                    Wdbg(DebugLevel.I, "Dead? {0} because current X is {1} and right edge is {2}", Dead, SnakeCurrentX, FloorRightEdge)

                    'If dead, show dead face
                    If Dead Then
                        ConsoleWrapper.SetCursorPosition(SnakePreviousX, SnakePreviousY)
                        WritePlain("X", False)
                        Wdbg(DebugLevel.I, "Snake dead at {0}/{1}.", SnakePreviousX, SnakePreviousY)
                    End If

                    'If the snake ate the apple, grow it up
                    If SnakeCurrentX = SnakeAppleX And SnakeCurrentY = SnakeAppleY Then
                        SnakeLength += 1
                        Wdbg(DebugLevel.I, "Snake grew up to {0}.", SnakeLength)

                        'Relocate the apple
                        SnakeAppleX = RandomDriver.Next(FloorLeftEdge + 1, FloorRightEdge - 1)
                        SnakeAppleY = RandomDriver.Next(FloorTopEdge + 1, FloorBottomEdge - 1)
                        AppleDrawn = False
                        Wdbg(DebugLevel.I, "New snake apple position ({0}, {1})", SnakeAppleX, SnakeAppleY)
                    End If
                Loop
            End If

            'Show the stage for few seconds before wiping
            If Simulation Then SleepNoBlock(SnakerStageDelay, ScreensaverDisplayerThread) Else Thread.Sleep(SnakerStageDelay)

            'Reset mass and console display
            SnakeMassPositions.Clear()
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            ConsoleWrapper.Clear()
        End Sub

        ''' <summary>
        ''' Changes the snake color
        ''' </summary>
        Function ChangeSnakeColor() As Color
            Dim RandomDriver As New Random()
            If SnakerTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(SnakerMinimumRedColorLevel, SnakerMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(SnakerMinimumGreenColorLevel, SnakerMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(SnakerMinimumBlueColorLevel, SnakerMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                Return New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}")
            ElseIf Snaker255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(SnakerMinimumColorLevel, SnakerMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                Return New Color(ColorNum)
            Else
                Console.BackgroundColor = colors(RandomDriver.Next(SnakerMinimumColorLevel, SnakerMaximumColorLevel))
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
            End If
        End Function

        ''' <summary>
        ''' Where would the snake go?
        ''' </summary>
        Enum SnakeDirection
            Top
            Bottom
            Left
            Right
        End Enum

    End Module
End Namespace
