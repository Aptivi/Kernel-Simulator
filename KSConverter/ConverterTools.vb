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

Imports KS.Misc.Platform

Module ConverterTools

    ''' <summary>
    ''' Gets all paths that are used in 0.0.15.x or earlier kernels.
    ''' </summary>
    ''' <param name="AppendedPath">Optional path to append</param>
    Function GetOldPaths(AppendedPath As String) As Dictionary(Of String, String)
        'Initialize all needed variables
        Dim OldPaths As New Dictionary(Of String, String)

        'Check to see if we're appending new path name
        If Not String.IsNullOrEmpty(AppendedPath) Then AppendedPath = $"/{AppendedPath}"

        'Populate our dictionary with old paths
        If IsOnUnix() Then
            OldPaths.Add("Configuration", Environment.GetEnvironmentVariable("HOME") + $"{AppendedPath}/kernelConfig.ini")
            OldPaths.Add("Aliases", Environment.GetEnvironmentVariable("HOME") + $"{AppendedPath}/aliases.csv")
            OldPaths.Add("Users", Environment.GetEnvironmentVariable("HOME") + $"{AppendedPath}/users.csv")
            OldPaths.Add("FTPSpeedDial", Environment.GetEnvironmentVariable("HOME") + $"{AppendedPath}/ftp_speeddial.csv")
            OldPaths.Add("BlockedDevices", Environment.GetEnvironmentVariable("HOME") + $"{AppendedPath}/blocked_devices.csv")
        Else
            OldPaths.Add("Configuration", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/kernelConfig.ini")
            OldPaths.Add("Aliases", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/aliases.csv")
            OldPaths.Add("Users", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/users.csv")
            OldPaths.Add("FTPSpeedDial", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/ftp_speeddial.csv")
            OldPaths.Add("BlockedDevices", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/blocked_devices.csv")
        End If

        'Return it.
        Return OldPaths
    End Function

    ''' <summary>
    ''' Gets all paths that are used in 0.0.16.x or later kernels.
    ''' </summary>
    Function GetNewPaths() As Dictionary(Of String, String)
        'Initialize all needed variables
        Dim NewPaths As New Dictionary(Of String, String)

        'Populate our dictionary with old paths
        If IsOnUnix() Then
            NewPaths.Add("Configuration", Environment.GetEnvironmentVariable("HOME") + "/KernelConfig.json")
            NewPaths.Add("Aliases", Environment.GetEnvironmentVariable("HOME") + "/Aliases.json")
            NewPaths.Add("Users", Environment.GetEnvironmentVariable("HOME") + "/Users.json")
            NewPaths.Add("FTPSpeedDial", Environment.GetEnvironmentVariable("HOME") + "/FTP_SpeedDial.json")
            NewPaths.Add("DebugDevNames", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + "/DebugDeviceNames.json")
        Else
            NewPaths.Add("Configuration", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + "/KernelConfig.json")
            NewPaths.Add("Aliases", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + "/Aliases.json")
            NewPaths.Add("Users", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + "/Users.json")
            NewPaths.Add("FTPSpeedDial", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + "/FTP_SpeedDial.json")
            NewPaths.Add("DebugDevNames", Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/") + "/DebugDeviceNames.json")
        End If

        'Return it.
        Return NewPaths
    End Function

    ''' <summary>
    ''' Gets home directory depending on platform
    ''' </summary>
    Function GetHomeDirectory() As String
        If IsOnUnix() Then
            Return Environment.GetEnvironmentVariable("HOME")
        Else
            Return Environment.GetEnvironmentVariable("USERPROFILE")
        End If
    End Function

End Module
