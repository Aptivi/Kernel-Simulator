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

Imports System.IO
Imports KS.Network.RemoteDebug.Interface

Namespace Network.RemoteDebug.Commands
    Class Debug_RegisterCommand
        Inherits RemoteDebugCommandExecutor
        Implements IRemoteDebugCommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly() As String, ListSwitchesOnly() As String, SocketStreamWriter As StreamWriter, DeviceAddress As String) Implements IRemoteDebugCommand.Execute
            If String.IsNullOrWhiteSpace(GetDeviceProperty(DeviceAddress, DeviceProperty.Name)) Then
                If ListArgsOnly.Length <> 0 Then
                    SetDeviceProperty(DeviceAddress, DeviceProperty.Name, ListArgsOnly(0))
                    DebugDevices.Where(Function(Device As RemoteDebugDevice) Device.ClientIP = DeviceAddress)(0).ClientName = ListArgsOnly(0)
                    SocketStreamWriter.WriteLine(DoTranslation("Hi, {0}!").FormatString(ListArgsOnly(0)))
                Else
                    SocketStreamWriter.WriteLine(DoTranslation("You need to write your name."))
                End If
            Else
                SocketStreamWriter.WriteLine(DoTranslation("You're already registered."))
            End If
        End Sub

    End Class
End Namespace
