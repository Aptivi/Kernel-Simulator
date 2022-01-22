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

Imports System.IO

Namespace Misc.Writers.DebugWriters
    Public Module DebugWriter

        Public DebugStreamWriter As StreamWriter
        Public DebugStackTraces As New List(Of String)

        ''' <summary>
        ''' Outputs the text into the debugger file, and sets the time stamp.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub Wdbg(Level As DebugLevel, text As String, ParamArray vars() As Object)
            If DebugMode Then
                'Open debugging stream
                If DebugStreamWriter Is Nothing Or DebugStreamWriter?.BaseStream Is Nothing Then DebugStreamWriter = New StreamWriter(GetKernelPath(KernelPathType.Debugging), True) With {.AutoFlush = True}

                'Try to debug...
                Try
                    Dim STrace As New StackTrace(True)
                    Dim Source As String = Path.GetFileName(STrace.GetFrame(1).GetFileName)
                    Dim LineNum As String = STrace.GetFrame(1).GetFileLineNumber
                    Dim Func As String = STrace.GetFrame(1).GetMethod.Name
                    Dim OffendingIndex As New List(Of String)

                    'We could be calling this function by WdbgConditional, so descend a frame
                    If Func = "WdbgConditional" Then
                        Source = Path.GetFileName(STrace.GetFrame(2).GetFileName)
                        LineNum = STrace.GetFrame(2).GetFileLineNumber
                        Func = STrace.GetFrame(2).GetMethod.Name
                    End If

                    'Apparently, GetFileName on Mono in Linux doesn't work for MDB files made using pdb2mdb for PDB files that are generated by Visual Studio, so we take the last entry for the backslash to get the source file name.
                    If IsOnUnix() Then
                        If Not String.IsNullOrEmpty(Source) Then
                            Source = Source.Split("\")(Source.Split("\").Length - 1)
                        End If
                    End If

                    'Check for debug quota
                    If CheckDebugQuota Then CheckForDebugQuotaExceed()

                    'For contributors who are testing new code: Define ENABLEIMMEDIATEWINDOWDEBUG for immediate debugging (Immediate Window)
                    If Source IsNot Nothing And Not LineNum = 0 Then
                        'Debug to file and all connected debug devices (raw mode)
                        DebugStreamWriter.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} [{Level}] ({Func} - {Source}:{LineNum}): {text}", vars)
                        For i As Integer = 0 To DebugDevices.Count - 1
                            Try
                                DebugDevices(i).ClientStreamWriter.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} [{Level}] ({Func} - {Source}:{LineNum}): {text}", vars)
                            Catch ex As Exception
                                OffendingIndex.Add(i)
                                WStkTrc(ex)
                            End Try
                        Next
#If ENABLEIMMEDIATEWINDOWDEBUG Then
                Debug.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} [{Level}] ({Func} - {Source}:{LineNum}): {text}", vars)
#End If
                    Else 'Rare case, unless debug symbol is not found on archives.
                        DebugStreamWriter.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} [{Level}] {text}", vars)
                        For i As Integer = 0 To DebugDevices.Count - 1
                            Try
                                DebugDevices(i).ClientStreamWriter.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} [{Level}] {text}", vars)
                            Catch ex As Exception
                                OffendingIndex.Add(i)
                                WStkTrc(ex)
                            End Try
                        Next
#If ENABLEIMMEDIATEWINDOWDEBUG Then
                Debug.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString}: [{Level}] {text}", vars)
#End If
                    End If

                    'Disconnect offending clients who are disconnected
                    For Each i As Integer In OffendingIndex
                        If i <> -1 Then
                            DebugDevices(i).ClientSocket.Disconnect(True)
                            Kernel.KernelEventManager.RaiseRemoteDebugConnectionDisconnected(DebugDevices(i).ClientIP)
                            Wdbg(DebugLevel.W, "Debug device {0} ({1}) disconnected.", DebugDevices(i).ClientName, DebugDevices(i).ClientIP)
                            DebugDevices.RemoveAt(i)
                        End If
                    Next
                    OffendingIndex.Clear()
                Catch ex As Exception
                    Wdbg(DebugLevel.F, "Debugger error: {0}", ex.Message)
                    WStkTrc(ex)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Conditionally outputs the text into the debugger file, and sets the time stamp.
        ''' </summary>
        ''' <param name="Condition">The condition that must be satisfied</param>
        ''' <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WdbgConditional(ByRef Condition As Boolean, Level As DebugLevel, text As String, ParamArray vars() As Object)
            If Condition Then Wdbg(Level, text, vars)
        End Sub

        ''' <summary>
        ''' Outputs the text into the debugger devices, and sets the time stamp. Note that it doesn't print where did the debugger debug in source files.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the the debugger devices. Supports {0}, {1}, ...</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WdbgDevicesOnly(Level As DebugLevel, text As String, ParamArray vars() As Object)
            If DebugMode Then
                Dim OffendingIndex As New List(Of String)

                'For contributors who are testing new code: Define ENABLEIMMEDIATEWINDOWDEBUG for immediate debugging (Immediate Window)
                For i As Integer = 0 To DebugDevices.Count - 1
                    Try
                        DebugDevices(i).ClientStreamWriter.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} [{Level}] {text}", vars)
                    Catch ex As Exception
                        OffendingIndex.Add(i)
                        WStkTrc(ex)
                    End Try
                Next
#If ENABLEIMMEDIATEWINDOWDEBUG Then
            Debug.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString}: [{Level}] {text}", vars)
#End If

                'Disconnect offending clients who are disconnected
                For Each i As Integer In OffendingIndex
                    If i <> -1 Then
                        DebugDevices(i).ClientSocket.Disconnect(True)
                        Kernel.KernelEventManager.RaiseRemoteDebugConnectionDisconnected(DebugDevices(i).ClientIP)
                        Wdbg(DebugLevel.W, "Debug device {0} ({1}) disconnected.", DebugDevices(i).ClientName, DebugDevices(i).ClientIP)
                        DebugDevices.RemoveAt(i)
                    End If
                Next
                OffendingIndex.Clear()
            End If
        End Sub

        ''' <summary>
        ''' Conditionally writes the exception's stack trace to the debugger
        ''' </summary>
        ''' <param name="Condition">The condition that must be satisfied</param>
        ''' <param name="Ex">An exception</param>
        Public Sub WStkTrcConditional(ByRef Condition As Boolean, Ex As Exception)
            If Condition Then WStkTrc(Ex)
        End Sub

        ''' <summary>
        ''' Writes the exception's stack trace to the debugger
        ''' </summary>
        ''' <param name="Ex">An exception</param>
        Public Sub WStkTrc(Ex As Exception)
            If DebugMode Then
                'These two vbNewLines are padding for accurate stack tracing.
                Dim Inner As Exception = Ex.InnerException
                Dim InnerNumber As Integer = 1
                Dim NewStackTraces As New List(Of String) From {
                $"{vbNewLine}{Ex.ToString.Substring(0, Ex.ToString.IndexOf(":"))}: {Ex.Message}{vbNewLine}{Ex.StackTrace}{vbNewLine}"
            }

                'Get all the inner exceptions
                Do Until Inner Is Nothing
                    NewStackTraces.Add($"[{InnerNumber}] {Inner.ToString.Substring(0, Inner.ToString.IndexOf(":"))}: {Inner.Message}{vbNewLine}{Inner.StackTrace}{vbNewLine}")
                    InnerNumber += 1
                    Inner = Inner.InnerException
                Loop

                'Print stack trace to debugger
                Dim StkTrcs As New List(Of String)
                For i As Integer = 0 To NewStackTraces.Count - 1
                    StkTrcs.AddRange(NewStackTraces(i).SplitNewLines)
                Next
                For i As Integer = 0 To StkTrcs.Count - 1
                    Wdbg(DebugLevel.E, StkTrcs(i))
                Next
                DebugStackTraces.AddRange(NewStackTraces)
            End If
        End Sub

    End Module
End Namespace