﻿'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Namespace Misc.Screensaver.Displays
    Public Module FireworksSettings

        Private _fireworks255Colors As Boolean
        Private _fireworksTrueColor As Boolean = True
        Private _fireworksDelay As Integer = 10
        Private _fireworksRadius As Integer = 5
        Private _fireworksMinimumRedColorLevel As Integer = 0
        Private _fireworksMinimumGreenColorLevel As Integer = 0
        Private _fireworksMinimumBlueColorLevel As Integer = 0
        Private _fireworksMinimumColorLevel As Integer = 0
        Private _fireworksMaximumRedColorLevel As Integer = 255
        Private _fireworksMaximumGreenColorLevel As Integer = 255
        Private _fireworksMaximumBlueColorLevel As Integer = 255
        Private _fireworksMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Fireworks] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Fireworks255Colors As Boolean
            Get
                Return _fireworks255Colors
            End Get
            Set(value As Boolean)
                _fireworks255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property FireworksTrueColor As Boolean
            Get
                Return _fireworksTrueColor
            End Get
            Set(value As Boolean)
                _fireworksTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FireworksDelay As Integer
            Get
                Return _fireworksDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _fireworksDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The radius of the explosion
        ''' </summary>
        Public Property FireworksRadius As Integer
            Get
                Return _fireworksRadius
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 5
                _fireworksRadius = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The minimum red color level (true color)
        ''' </summary>
        Public Property FireworksMinimumRedColorLevel As Integer
            Get
                Return _fireworksMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _fireworksMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The minimum green color level (true color)
        ''' </summary>
        Public Property FireworksMinimumGreenColorLevel As Integer
            Get
                Return _fireworksMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _fireworksMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The minimum blue color level (true color)
        ''' </summary>
        Public Property FireworksMinimumBlueColorLevel As Integer
            Get
                Return _fireworksMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _fireworksMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FireworksMinimumColorLevel As Integer
            Get
                Return _fireworksMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_fireworks255Colors Or _fireworksTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _fireworksMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The maximum red color level (true color)
        ''' </summary>
        Public Property FireworksMaximumRedColorLevel As Integer
            Get
                Return _fireworksMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _fireworksMinimumRedColorLevel Then value = _fireworksMinimumRedColorLevel
                If value > 255 Then value = 255
                _fireworksMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The maximum green color level (true color)
        ''' </summary>
        Public Property FireworksMaximumGreenColorLevel As Integer
            Get
                Return _fireworksMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _fireworksMinimumGreenColorLevel Then value = _fireworksMinimumGreenColorLevel
                If value > 255 Then value = 255
                _fireworksMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The maximum blue color level (true color)
        ''' </summary>
        Public Property FireworksMaximumBlueColorLevel As Integer
            Get
                Return _fireworksMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _fireworksMinimumBlueColorLevel Then value = _fireworksMinimumBlueColorLevel
                If value > 255 Then value = 255
                _fireworksMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FireworksMaximumColorLevel As Integer
            Get
                Return _fireworksMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_fireworks255Colors Or _fireworksTrueColor, 255, 15)
                If value <= _fireworksMinimumColorLevel Then value = _fireworksMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _fireworksMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class FireworksDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "Fireworks" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            'Variables
            Dim HalfHeight As Integer = Console.WindowHeight / 2
            Dim LaunchPositionX As Integer = RandomDriver.Next(Console.WindowWidth)
            Dim LaunchPositionY As Integer = Console.WindowHeight - 1
            Dim IgnitePositionX As Integer = RandomDriver.Next(Console.WindowWidth)
            Dim IgnitePositionY As Integer = RandomDriver.Next(HalfHeight, HalfHeight * 1.5)
            LaunchPositionX.SwapIfSourceLarger(IgnitePositionX)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Launch position {0}, {1}", LaunchPositionX, LaunchPositionY)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Ignite position {0}, {1}", IgnitePositionX, IgnitePositionY)

            'Thresholds
            Dim FireworkThresholdX As Integer = IgnitePositionX - LaunchPositionX
            Dim FireworkThresholdY As Integer = Math.Abs(IgnitePositionY - LaunchPositionY)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Position thresholds (X: {0}, Y: {1})", FireworkThresholdX, FireworkThresholdY)
            Dim FireworkStepsX As Double = FireworkThresholdX / FireworkThresholdY
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "{0} steps", FireworkStepsX)
            Dim FireworkRadius As Integer = If(FireworksRadius >= 0 And FireworksRadius <= 10, FireworksRadius, 5)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Radius: {0} blocks", FireworkRadius)
            Dim IgniteColor As New Color(255, 255, 255)

            'Select a color
            Console.Clear()
            If FireworksTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(FireworksMinimumRedColorLevel, FireworksMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(FireworksMinimumGreenColorLevel, FireworksMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(FireworksMinimumBlueColorLevel, FireworksMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                IgniteColor = New Color(RedColorNum, GreenColorNum, BlueColorNum)
            ElseIf Fireworks255Colors Then
                Dim color As Integer = RandomDriver.Next(FireworksMinimumColorLevel, FireworksMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", color)
                IgniteColor = New Color(color)
            End If

            'Launch the rocket
            If Not ResizeSyncing Then
                Dim CurrentX As Double = LaunchPositionX
                Dim CurrentY As Integer = LaunchPositionY
                Do Until CurrentX >= IgnitePositionX And CurrentY <= IgnitePositionY
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If ResizeSyncing Then Exit Do
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current position: {0}, {1}", CurrentX, CurrentY)
                    Console.SetCursorPosition(CurrentX, CurrentY)
                    Console.Write(" ")

                    'Delay writing
                    SleepNoBlock(FireworksDelay, ScreensaverDisplayerThread)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()
                    SetConsoleColor(New Color(255, 255, 255), True)

                    'Change positions
                    CurrentX += FireworkStepsX
                    CurrentY -= 1
                Loop
            End If

            'Blow it up!
            If Not ResizeSyncing Then
                For Radius As Integer = 0 To FireworkRadius
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If ResizeSyncing Then Exit For

                    'Variables
                    Dim UpperParticleY As Integer = IgnitePositionY + 1 + Radius
                    Dim LowerParticleY As Integer = IgnitePositionY - 1 - Radius
                    Dim LeftParticleX As Integer = IgnitePositionX - 1 - (Radius * 2)
                    Dim RightParticleX As Integer = IgnitePositionX + 1 + (Radius * 2)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Upper particle position: {0}", UpperParticleY)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Lower particle position: {0}", LowerParticleY)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Left particle position: {0}", LeftParticleX)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Right particle position: {0}", RightParticleX)

                    'Draw the explosion
                    SetConsoleColor(IgniteColor, True)
                    If UpperParticleY < Console.WindowHeight Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Making upper particle at {0}, {1}", IgnitePositionX, UpperParticleY)
                        Console.SetCursorPosition(IgnitePositionX, UpperParticleY)
                        Console.Write(" ")
                    End If
                    If LowerParticleY < Console.WindowHeight Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Making lower particle at {0}, {1}", IgnitePositionX, LowerParticleY)
                        Console.SetCursorPosition(IgnitePositionX, LowerParticleY)
                        Console.Write(" ")
                    End If
                    If LeftParticleX < Console.WindowWidth Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Making left particle at {0}, {1}", LeftParticleX, IgnitePositionY)
                        Console.SetCursorPosition(LeftParticleX, IgnitePositionY)
                        Console.Write(" ")
                    End If
                    If RightParticleX < Console.WindowWidth Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Making right particle at {0}, {1}", RightParticleX, IgnitePositionY)
                        Console.SetCursorPosition(RightParticleX, IgnitePositionY)
                        Console.Write(" ")
                    End If

                    'Delay writing
                    SleepNoBlock(FireworksDelay, ScreensaverDisplayerThread)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()
                Next
            End If

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
        End Sub

    End Class
End Namespace
