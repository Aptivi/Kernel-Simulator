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
Imports System.Reflection

Public Module HardwareList

    ''' <summary>
    ''' Lists simple information about hardware
    ''' </summary>
    Public Sub ListHardware()
        'CPU Info
        For Each ProcessorInfo As String In HardwareInfo.Hardware.CPU.Keys
            If ProcessorInfo.Contains("@") And ProcessorInfo.EndsWith("GHz") Then
                Write("CPU: {0}", False, ColTypes.Neutral, ProcessorInfo)
            Else
                Write("CPU: {0} @ {1}", False, ColTypes.Neutral, ProcessorInfo, HardwareInfo.Hardware.CPU(ProcessorInfo).Speed)
            End If

            'SSE2 availability
            If HardwareInfo.Hardware.CPU(ProcessorInfo).Flags.Contains("sse2") Or HardwareInfo.Hardware.CPU(ProcessorInfo).Flags.Contains("SSE2") Then 'After SSE2 requirement addition, remove the check.
                Write(" : SSE2 @ {0}-bit", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(ProcessorInfo).Bits)
            ElseIf HardwareInfo.Hardware.CPU(ProcessorInfo).Bits = 32 Then
                Write(vbNewLine + DoTranslation("CPU: WARNING: SSE2 will be required in future development commits."), True, ColTypes.Error)
            End If
        Next
        Write(DoTranslation("CPU: Total number of processors: {0}"), True, ColTypes.Neutral, Environment.ProcessorCount)

        'Print RAM info
        Write(If(IsNumeric(HardwareInfo.Hardware.RAM.TotalMemory), "RAM: {0} MB", "RAM: {0}"), True, ColTypes.Neutral, If(IsNumeric(HardwareInfo.Hardware.RAM.TotalMemory), FormatNumber(HardwareInfo.Hardware.RAM.TotalMemory / 1024, 2), HardwareInfo.Hardware.RAM.TotalMemory))

        'GPU info
        For Each GPUInfo In HardwareInfo.Hardware.GPU.Keys
            Write("GPU: {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.GPU(GPUInfo).Name)
        Next

        'Drive Info
        For Each driveinfo In HardwareInfo.Hardware.HDD.Keys
            If HardwareInfo.Hardware.HDD(driveinfo).Vendor = "(Standard disk drives)" Then
                Write(If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Size), "HDD: {0} {1} GB", "HDD: {0} {1}"), True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(driveinfo).Model, If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Size), FormatNumber(HardwareInfo.Hardware.HDD(driveinfo).Size / 1024 / 1024 / 1024, 2), HardwareInfo.Hardware.HDD(driveinfo).Size))
            Else
                Write(If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Size), "HDD: {0} {1} {2} GB", "HDD: {0} {1} {2}"), True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(driveinfo).Vendor, HardwareInfo.Hardware.HDD(driveinfo).Model, If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Size), FormatNumber(HardwareInfo.Hardware.HDD(driveinfo).Size / 1024 / 1024 / 1024, 2), HardwareInfo.Hardware.HDD(driveinfo).Size))
            End If
            For Each PartInfo In HardwareInfo.Hardware.HDD(driveinfo).Partitions.Keys
                Write(If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).Size), "HDD ({2}): {0} {1} GB", "HDD ({2}): {0} {1}"), True, ColTypes.Neutral,
                  HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).FileSystem, If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).Size), FormatNumber(HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).Size / 1024 / 1024 / 1024, 2), HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).Size), HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).ID)
            Next
        Next
    End Sub

    ''' <summary>
    ''' Lists information about hardware
    ''' </summary>
    ''' <param name="HardwareType">Hadrware type defined by Inxi.NET. If "all", prints all information.</param>
    Public Sub ListHardware(HardwareType As String)
        Dim HardwareField As FieldInfo = GetField(HardwareType, GetField(NameOf(HardwareInfo.Hardware), GetType(InxiFrontend.Inxi)).FieldType)
        Wdbg("I", "Got hardware field {0}.", If(HardwareField IsNot Nothing, HardwareField.Name, "unknown"))
        If HardwareField IsNot Nothing Then
            ListHardwareProperties(HardwareField)
        ElseIf HardwareType.ToLower = "all" Then
            Dim HardwareFields As FieldInfo() = GetField(NameOf(HardwareInfo.Hardware), GetType(InxiFrontend.Inxi)).FieldType.GetFields
            For Each HardwareFieldType As FieldInfo In HardwareFields
                ListHardwareProperties(HardwareFieldType)
            Next
        Else
            Write(DoTranslation("Either the hardware type {0} is not probed, or is not valid."), True, ColTypes.Error, HardwareType)
        End If
    End Sub

    Private Sub ListHardwareProperties(Field As FieldInfo)
        Wdbg("I", "Got hardware field {0}.", Field.Name)
        WriteSeparator(Field.Name, True, ColTypes.Stage)
        Dim FieldValue As Object = Field.GetValue(HardwareInfo.Hardware)
        If FieldValue IsNot Nothing Then
            Dim FieldValueDict As IDictionary = TryCast(FieldValue, IDictionary)
            If FieldValueDict IsNot Nothing Then
                For Each HardwareKey As String In FieldValueDict.Keys
                    Write("- {0}: ", True, ColTypes.ListEntry, HardwareKey)
                    For Each HardwareValueFieldInfo As FieldInfo In FieldValueDict(HardwareKey).GetType.GetFields
                        Write("  - {0}: ", False, ColTypes.ListEntry, HardwareValueFieldInfo.Name)
                        If Field.Name = "HDD" And HardwareValueFieldInfo.Name = "Partitions" Then
                            Console.WriteLine()
                            Dim Partitions As IDictionary = TryCast(HardwareValueFieldInfo.GetValue(FieldValueDict(HardwareKey)), IDictionary)
                            If Partitions IsNot Nothing Then
                                For Each PartitionKey As String In Partitions.Keys
                                    Write("    - {0}: ", True, ColTypes.ListEntry, PartitionKey)
                                    For Each PartitionValueFieldInfo As FieldInfo In Partitions(PartitionKey).GetType.GetFields
                                        Write("      - {0}: ", False, ColTypes.ListEntry, PartitionValueFieldInfo.Name)
                                        Write(PartitionValueFieldInfo.GetValue(Partitions(PartitionKey)), True, ColTypes.ListValue)
                                    Next
                                Next
                            Else
                                Write(DoTranslation("Partitions not parsed to list."), True, ColTypes.Error)
                            End If
                        ElseIf Field.Name = "CPU" And HardwareValueFieldInfo.Name = "Flags" Then
                            Write(String.Join(", ", TryCast(HardwareValueFieldInfo.GetValue(FieldValueDict(HardwareKey)), String())), True, ColTypes.ListValue)
                        Else
                            Write(HardwareValueFieldInfo.GetValue(FieldValueDict(HardwareKey)), True, ColTypes.ListValue)
                        End If
                    Next
                Next
            Else
                For Each HardwareFieldInfo As FieldInfo In Field.FieldType.GetFields()
                    Write("- {0}: ", False, ColTypes.ListEntry, HardwareFieldInfo.Name)
                    Write(HardwareFieldInfo.GetValue(FieldValue), True, ColTypes.ListValue)
                Next
            End If
        Else
            Write(DoTranslation("The hardware type {0} is not probed yet. If you're sure that it's probed, restart the kernel with debugging enabled."), True, ColTypes.Error, Field.Name)
        End If
    End Sub

End Module
