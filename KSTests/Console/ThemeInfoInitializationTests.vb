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
Imports KS

<TestClass()> Public Class ThemeInfoInitializationTests

    ''' <summary>
    ''' Tests initializing an instance of ThemeInfo from KS resources
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeThemeInfoFromResources()
        'Create instance
        Dim ThemeInfoInstance As New ThemeInfo("Hacker")

        'Check for null
        ThemeInfoInstance.ThemeBackgroundColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListValueColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListEntryColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeContKernelErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeHostNameShellColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeInputColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeLicenseColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNeutralTextColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeStageColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUncontKernelErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUserNameShellColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeWarningColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationTitleColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationDescriptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationProgressColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationFailureColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeQuestionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSuccessColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUserDollarColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTipColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSeparatorTextColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSeparatorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListTitleColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeDevelopmentWarningColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeStageTimeColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeProgressColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeBackOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeLowPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeMediumPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeHighPriorityBorderColor.ShouldNotBeNull
    End Sub

    ''' <summary>
    ''' Tests initializing an instance of ThemeInfo from all KS resources
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeThemeInfoFromAllResources()
        For Each ResourceName As String In Themes.Keys

            'Special naming cases
            Dim ThemeName As String = ResourceName
            Select Case ResourceName
                Case "Default"
                    ThemeName = "_Default"
                Case "NFSHP-Cop"
                    ThemeName = "NFSHP_Cop"
                Case "NFSHP-Racer"
                    ThemeName = "NFSHP_Racer"
                Case "3Y-Diamond"
                    ThemeName = "_3Y_Diamond"
            End Select
            'Create instance
            Dim ThemeInfoInstance As New ThemeInfo(ThemeName)

            'Check for null
            ThemeInfoInstance.ThemeBackgroundColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeListValueColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeListEntryColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeContKernelErrorColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeErrorColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeHostNameShellColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeInputColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeLicenseColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNeutralTextColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeOptionColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeStageColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeUncontKernelErrorColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeUserNameShellColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeWarningColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNotificationTitleColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNotificationDescriptionColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNotificationProgressColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNotificationFailureColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeQuestionColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeSuccessColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeUserDollarColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeTipColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeSeparatorTextColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeSeparatorColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeListTitleColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeDevelopmentWarningColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeStageTimeColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeProgressColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeBackOptionColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeLowPriorityBorderColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeMediumPriorityBorderColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeHighPriorityBorderColor.ShouldNotBeNull
        Next
    End Sub

    ''' <summary>
    ''' Tests initializing an instance of ThemeInfo from file
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeThemeInfoFromFile()
        'Create instance
        Dim SourcePath As String = Path.GetFullPath("Hacker.json")
        Dim ThemeInfoInstance As New ThemeInfo(New StreamReader(SourcePath))

        'Check for null
        ThemeInfoInstance.ThemeBackgroundColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListValueColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListEntryColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeContKernelErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeHostNameShellColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeInputColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeLicenseColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNeutralTextColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeStageColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUncontKernelErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUserNameShellColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeWarningColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationTitleColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationDescriptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationProgressColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationFailureColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeQuestionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSuccessColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUserDollarColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTipColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSeparatorTextColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSeparatorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListTitleColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeDevelopmentWarningColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeStageTimeColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeProgressColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeBackOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeLowPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeMediumPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeHighPriorityBorderColor.ShouldNotBeNull
    End Sub

End Class