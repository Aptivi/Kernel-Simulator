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
Imports System.Net.NetworkInformation
Imports System.Text
Imports Newtonsoft.Json.Linq

Public Module NetworkTools

    'Variables
    Public adapterNumber As Long
    Public DRetries As Integer = 3
    Public URetries As Integer = 3
    Friend DFinish As Boolean
    Friend UFinish As Boolean

    Public Enum SpeedDialType
        ''' <summary>
        ''' FTP speed dial
        ''' </summary>
        FTP
        ''' <summary>
        ''' SFTP speed dial
        ''' </summary>
        SFTP
    End Enum

    ''' <summary>
    ''' Print each of adapters' properties to the console.
    ''' </summary>
    Public Sub PrintAdapterProperties()
        Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces
        Dim NoV4, NoV6, Failed As Boolean
        Dim gp As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties
        Dim gs6 As IPGlobalStatistics = gp.GetIPv6GlobalStatistics
        Dim gs4 As IPGlobalStatistics = gp.GetIPv4GlobalStatistics

        'Probe for adapter capabilities
        For Each adapter As NetworkInterface In adapters
            adapterNumber += 1
            W("==========================================", True, ColTypes.Neutral)

            'See if it supports IPv6
            If Not adapter.Supports(NetworkInterfaceComponent.IPv6) Then
                Wdbg("W", "{0} doesn't support IPv6. Trying to get information about IPv4.", adapter.Description)
                W(DoTranslation("Adapter {0} doesn't support IPv6. Continuing..."), True, ColTypes.Err, adapter.Description)
                NoV6 = True
            End If

            'See if it supports IPv4
            If Not adapter.Supports(NetworkInterfaceComponent.IPv4) Then
                Wdbg("E", "{0} doesn't support IPv4.", adapter.Description)
                W(DoTranslation("Adapter {0} doesn't support IPv4. Probe failed."), True, ColTypes.Err, adapter.Description)
                NoV4 = True
            End If

            'Get adapter IPv(4/6) properties
            If IsInternetAdapter(adapter) And Not NoV4 Then
                Wdbg("I", "Adapter type of {0}: {1}", adapter.Description, adapter.NetworkInterfaceType.ToString)
                Dim adapterProperties As IPInterfaceProperties = adapter.GetIPProperties()
                Dim p As IPv4InterfaceProperties
                Dim s As IPv4InterfaceStatistics = adapter.GetIPv4Statistics
                Dim p6 As IPv6InterfaceProperties
                'TODO: IPV6InterfaceStatistics is not implemented yet
                Try
                    p = adapterProperties.GetIPv4Properties
                    p6 = adapterProperties.GetIPv6Properties
                Catch ex As NetworkInformationException
#Disable Warning BC42104
                    If p6 Is Nothing Then
                        Wdbg("W", "Failed to get IPv6 properties.")
                        W(DoTranslation("Failed to get IPv6 properties for adapter {0}. Continuing..."), True, ColTypes.Err, adapter.Description)
                    End If
                    If p Is Nothing Then
                        Wdbg("E", "Failed to get IPv4 properties.")
                        W(DoTranslation("Failed to get properties for adapter {0}"), True, ColTypes.Err, adapter.Description)
                        Failed = True
                    End If
                    WStkTrc(ex)
#Enable Warning BC42104
                End Try

                'Check if statistics is nothing
                If s Is Nothing Then
                    Wdbg("E", "Failed to get statistics.")
                    W(DoTranslation("Failed to get statistics for adapter {0}"), True, ColTypes.Err, adapter.Description)
                    Failed = True
                End If

                'Print adapter infos if not failed
                If Not Failed Then
                    PrintAdapterIPv4Info(adapter, p, s)
                    'Additionally, print adapter IPv6 infos if available
                    If Not NoV6 Then
                        PrintAdapterIPv6Info(adapter, p6)
                    End If
                End If
            Else
                Wdbg("W", "Adapter {0} doesn't belong in netinfo because the type is {1}", adapter.Description, adapter.NetworkInterfaceType)
            End If
        Next

        'Print general IPv4 and IPv6 information
        W("==========================================", True, ColTypes.Neutral)
        PrintGeneralNetInfo(gs4, gs6)
    End Sub

    ''' <summary>
    ''' Prints IPv4 info for adapter
    ''' </summary>
    ''' <param name="NInterface">A network interface or adapter</param>
    ''' <param name="Properties">Network properties</param>
    ''' <param name="Statistics">Network statistics</param>
    Sub PrintAdapterIPv4Info(ByVal NInterface As NetworkInterface, ByVal Properties As IPv4InterfaceProperties, ByVal Statistics As IPv4InterfaceStatistics)
        W(DoTranslation("IPv4 information:") + vbNewLine +
          DoTranslation("Adapter Number:") + " {0}" + vbNewLine +
          DoTranslation("Adapter Name:") + " {1}" + vbNewLine +
          DoTranslation("Maximum Transmission Unit: {2} Units") + vbNewLine +
          DoTranslation("DHCP Enabled:") + " {3}" + vbNewLine +
          DoTranslation("Non-unicast packets:") + " {4}/{5}" + vbNewLine +
          DoTranslation("Unicast packets:") + " {6}/{7}" + vbNewLine +
          DoTranslation("Error incoming/outgoing packets:") + " {8}/{9}", True, ColTypes.Neutral,
          adapterNumber, NInterface.Description, Properties.Mtu, Properties.IsDhcpEnabled, Statistics.NonUnicastPacketsSent, Statistics.NonUnicastPacketsReceived,
          Statistics.UnicastPacketsSent, Statistics.UnicastPacketsReceived, Statistics.IncomingPacketsWithErrors, Statistics.OutgoingPacketsWithErrors)
    End Sub

    ''' <summary>
    ''' Prints IPv6 info for adapter
    ''' </summary>
    ''' <param name="NInterface">A network interface or adapter</param>
    ''' <param name="Properties">Network properties</param>
    Sub PrintAdapterIPv6Info(ByVal NInterface As NetworkInterface, ByVal Properties As IPv6InterfaceProperties)
        W(DoTranslation("IPv6 information:") + vbNewLine +
          DoTranslation("Adapter Number:") + " {0}" + vbNewLine +
          DoTranslation("Adapter Name:") + " {1}" + vbNewLine +
          DoTranslation("Maximum Transmission Unit: {2} Units"), True, ColTypes.Neutral,
          adapterNumber, NInterface.Description, Properties.Mtu)
    End Sub

    ''' <summary>
    ''' Prints general network info
    ''' </summary>
    ''' <param name="IPv4Stat">IPv4 general statistics</param>
    ''' <param name="IPv6Stat">IPv6 general statistics</param>
    Sub PrintGeneralNetInfo(ByVal IPv4Stat As IPGlobalStatistics, ByVal IPv6Stat As IPGlobalStatistics)
        W(DoTranslation("General IPv6 properties") + vbNewLine +
          DoTranslation("Packets (inbound):") + " {0}/{1}" + vbNewLine +
          DoTranslation("Packets (outbound):") + " {2}/{3}" + vbNewLine +
          DoTranslation("Errors in received packets:") + " {4}/{5}/{6}" + vbNewLine +
          DoTranslation("General IPv4 properties") + vbNewLine +
          DoTranslation("Packets (inbound):") + " {7}/{8}" + vbNewLine +
          DoTranslation("Packets (outbound):") + " {9}/{10}" + vbNewLine +
          DoTranslation("Errors in received packets:") + " {11}/{12}/{13}", True, ColTypes.Neutral,
          IPv6Stat.ReceivedPackets, IPv6Stat.ReceivedPacketsDelivered, IPv6Stat.OutputPacketRequests, IPv6Stat.OutputPacketsDiscarded, IPv6Stat.ReceivedPacketsWithAddressErrors,
          IPv6Stat.ReceivedPacketsWithHeadersErrors, IPv6Stat.ReceivedPacketsWithUnknownProtocol, IPv4Stat.ReceivedPackets, IPv4Stat.ReceivedPacketsDelivered, IPv4Stat.OutputPacketRequests,
          IPv4Stat.OutputPacketsDiscarded, IPv4Stat.ReceivedPacketsWithAddressErrors, IPv4Stat.ReceivedPacketsWithHeadersErrors, IPv4Stat.ReceivedPacketsWithUnknownProtocol)
    End Sub

    Function IsInternetAdapter(ByVal InternetAdapter As NetworkInterface) As Boolean
        Return InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet3Megabit Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetFx Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetT Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.GigabitEthernet Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Wireless80211 Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Tunnel
    End Function

    ''' <summary>
    ''' Pings an address
    ''' </summary>
    ''' <param name="Address">Target address</param>
    ''' <returns>A ping reply status</returns>
    Public Function PingAddress(ByVal Address As String) As PingReply
        Dim Pinger As New Ping
        Dim PingerOpts As New PingOptions With {.DontFragment = True}
        Dim PingBuffer() As Byte = Encoding.ASCII.GetBytes("Kernel Simulator")
        Dim PingTimeout As Integer = 60000 '60 seconds = 1 minute. timeout of Pinger.Send() takes milliseconds.
        Return Pinger.Send(Address, PingTimeout, PingBuffer, PingerOpts)
    End Function

    ''' <summary>
    ''' Changes host name
    ''' </summary>
    ''' <param name="NewHost">New host name</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function ChangeHostname(ByVal NewHost As String) As Boolean
        Try
            HName = NewHost
            Dim ksconf As New IniFile()
            Dim pathConfig As String = paths("Configuration")
            ksconf.Load(pathConfig)
            ksconf.Sections("Login").Keys("Host Name").Value = HName
            ksconf.Save(pathConfig)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Wdbg("E", "Failed to change hostname: {0}", ex.Message)
            Throw New Exceptions.HostnameException(DoTranslation("Failed to change host name: {0}").FormatString(ex.Message), ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Adds an entry to speed dial
    ''' </summary>
    ''' <param name="Entry">A speed dial entry</param>
    ''' <param name="SpeedDialType">Speed dial type</param>
    ''' <param name="ThrowException">Optionally throw exception</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function AddEntryToSpeedDial(ByVal Entry As String, ByVal SpeedDialType As SpeedDialType, Optional ThrowException As Boolean = True) As Boolean
        Dim PathName As String = If(SpeedDialType = SpeedDialType.SFTP, "SFTPSpeedDial", "FTPSpeedDial")
        If Not File.Exists(paths(PathName)) Then MakeFile(paths(PathName))
        Dim SpeedDialJsonContent As String = File.ReadAllText(paths(PathName))
        Dim SpeedDialToken As JArray = JArray.Parse(If(Not String.IsNullOrEmpty(SpeedDialJsonContent), SpeedDialJsonContent, "[]"))
        If Not SpeedDialToken.Contains(Entry) Then
            SpeedDialToken.Add(Entry)
            File.WriteAllText(paths(PathName), JsonConvert.SerializeObject(SpeedDialToken, Formatting.Indented))
            Return True
        Else
            If ThrowException Then Throw New Exceptions.FTPNetworkException(DoTranslation("Entry already exists."))
            Return False
        End If
    End Function

    ''' <summary>
    ''' Lists all speed dial entries
    ''' </summary>
    ''' <param name="SpeedDialType">Speed dial type</param>
    ''' <returns>A list</returns>
    Public Function ListSpeedDialEntries(ByVal SpeedDialType As SpeedDialType) As List(Of JToken)
        Dim PathName As String = If(SpeedDialType = SpeedDialType.SFTP, "SFTPSpeedDial", "FTPSpeedDial")
        If Not File.Exists(paths(PathName)) Then MakeFile(paths(PathName))
        Dim SpeedDialJsonContent As String = File.ReadAllText(paths(PathName))
        Dim SpeedDialToken As JArray = JArray.Parse(If(Not String.IsNullOrEmpty(SpeedDialJsonContent), SpeedDialJsonContent, "[]"))
        Return SpeedDialToken.ToArray.ToList
    End Function

End Module
