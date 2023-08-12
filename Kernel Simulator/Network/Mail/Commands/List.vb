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

Imports KS.Network.Mail.Directory
Imports KS.Misc.Reflection

Namespace Network.Mail.Commands
    Class Mail_ListCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs?.Length > 0 Then
                Wdbg(DebugLevel.I, "Page is numeric? {0}", IsStringNumeric(ListArgs(0)))
                If IsStringNumeric(ListArgs(0)) Then
                    MailListMessages(ListArgs(0))
                Else
                    TextWriterColor.Write(DoTranslation("Page is not a numeric value."), True, ColTypes.Error)
                End If
            Else
                MailListMessages(1)
            End If
        End Sub

    End Class
End Namespace