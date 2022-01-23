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

Imports System.Net.NetworkInformation
Imports KS.Network

Namespace Shell.Commands
    Class PingCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            'If the pinged address is actually a number of times
            Dim PingTimes As Integer = 4
            Dim StepsToSkip As Integer = 0
            If IsNumeric(ListArgs(0)) Then
                Wdbg(DebugLevel.I, "ListArgs(0) is numeric. Assuming number of times: {0}", ListArgs(0))
                PingTimes = ListArgs(0)
                StepsToSkip = 1
            End If
            For Each PingedAddress As String In ListArgs.Skip(StepsToSkip)
                If PingedAddress <> "" Then
                    WriteSeparator(PingedAddress, True)
                    For CurrentTime As Integer = 1 To PingTimes
                        Try
                            Dim PingReplied As PingReply = PingAddress(PingedAddress)
                            If PingReplied.Status = IPStatus.Success Then
                                Write("[{1}] " + DoTranslation("Ping succeeded in {0} ms."), True, ColTypes.Neutral, PingReplied.RoundtripTime, CurrentTime)
                            Else
                                Write("[{2}] " + DoTranslation("Failed to ping {0}: {1}"), True, ColTypes.Error, PingedAddress, PingReplied.Status, CurrentTime)
                            End If
                        Catch ex As Exception
                            Write("[{2}] " + DoTranslation("Failed to ping {0}: {1}"), True, ColTypes.Error, PingedAddress, ex.Message, CurrentTime)
                            WStkTrc(ex)
                        End Try
                    Next
                Else
                    Write(DoTranslation("Address may not be empty."), True, ColTypes.Error)
                End If
            Next
        End Sub

    End Class
End Namespace