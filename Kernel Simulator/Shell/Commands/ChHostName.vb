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

Imports KS.Network

Namespace Shell.Commands
    Class ChHostNameCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs(0) = "" Then
                Write(DoTranslation("Blank host name."), True, ColTypes.Error)
            ElseIf ListArgs(0).IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                Write(DoTranslation("Special characters are not allowed."), True, ColTypes.Error)
            Else
                Write(DoTranslation("Changing from: {0} to {1}..."), True, ColTypes.Neutral, HostName, ListArgs(0))
                ChangeHostname(ListArgs(0))
            End If
        End Sub

    End Class
End Namespace