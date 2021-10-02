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

Public Module Flags

    'Variables
    ''' <summary>
    ''' Trigger double panic
    ''' </summary>
    Public StopPanicAndGoToDoublePanic As Boolean
    ''' <summary>
    ''' Toggle Debugging mode
    ''' </summary>
    Public DebugMode As Boolean
    ''' <summary>
    ''' Flag for log-in
    ''' </summary>
    Public LoginFlag As Boolean
    ''' <summary>
    ''' A signal for command kernel argument
    ''' </summary>
    Public CommandFlag As Boolean
    ''' <summary>
    ''' A flag for checking for an argument on reboot
    ''' </summary>
    Public ArgsInjected As Boolean
    ''' <summary>
    ''' Set Root Password boolean
    ''' </summary>
    Public SetRootPassword As Boolean
    ''' <summary>
    ''' Set Root Password to any password
    ''' </summary>
    Public RootPassword As String = "toor"
    ''' <summary>
    ''' Maintenance Mode
    ''' </summary>
    Public Maintenance As Boolean
    ''' <summary>
    ''' Arguments On Boot
    ''' </summary>
    Public ArgsOnBoot As Boolean
    ''' <summary>
    ''' Clear Screen On Log-in
    ''' </summary>
    Public ClearOnLogin As Boolean
    ''' <summary>
    ''' Show MOTD on log-in
    ''' </summary>
    Public ShowMOTD As Boolean = True
    ''' <summary>
    ''' Show MAL on log-in
    ''' </summary>
    Public ShowMAL As Boolean = True
    ''' <summary>
    ''' Simplified Help Command
    ''' </summary>
    Public SimHelp As Boolean
    ''' <summary>
    ''' Probe slots
    ''' </summary>
    Public SlotProbe As Boolean = True
    ''' <summary>
    ''' Probe the hardware quietly. This overrides the <see cref="VerboseHardwareProbe"/> flag.
    ''' </summary>
    Public QuietHardwareProbe As Boolean
    ''' <summary>
    ''' Show Time/Date on corner
    ''' </summary>
    Public CornerTimeDate As Boolean
    ''' <summary>
    ''' Instance checking
    ''' </summary>
    Public InstanceChecked As Boolean
    ''' <summary>
    ''' A signal when user logs out.
    ''' </summary>
    Public LogoutRequested As Boolean
    ''' <summary>
    ''' Reboot requested
    ''' </summary>
    Public RebootRequested As Boolean
    ''' <summary>
    ''' Log username for FTP
    ''' </summary>
    Public FTPLoggerUsername As Boolean
    ''' <summary>
    ''' Log IP address for FTP
    ''' </summary>
    Public FTPLoggerIP As Boolean
    ''' <summary>
    ''' Only first profile will be returned
    ''' </summary>
    Public FTPFirstProfileOnly As Boolean
    ''' <summary>
    ''' Whether safe mode is enabled
    ''' </summary>
    Public SafeMode As Boolean
    ''' <summary>
    ''' Whether or not to parse whole directory for size
    ''' </summary>
    Public FullParseMode As Boolean
    ''' <summary>
    ''' When the screensaver timer has reached
    ''' </summary>
    Public ScrnTimeReached As Boolean
    ''' <summary>
    ''' Enable marquee on startup
    ''' </summary>
    Public StartScroll As Boolean = True
    ''' <summary>
    ''' Whether or not to render time and dates short or long
    ''' </summary>
    Public LongTimeDate As Boolean = True
    ''' <summary>
    ''' Whether or not to detect if the user is logged in
    ''' </summary>
    Public LoggedIn As Boolean
    ''' <summary>
    ''' Whether or not to show available usernames on login
    ''' </summary>
    Public ShowAvailableUsers As Boolean = True
    ''' <summary>
    ''' Whether or not to show hidden files
    ''' </summary>
    Public HiddenFiles As Boolean
    ''' <summary>
    ''' Whether or not to check for updates on startup
    ''' </summary>
    Public CheckUpdateStart As Boolean = True
    ''' <summary>
    ''' Change culture when changing language
    ''' </summary>
    Public LangChangeCulture As Boolean
    ''' <summary>
    ''' Shows the progress bar while downloading using the {Down|Up}load{File|String}() API.
    ''' </summary>
    Public ShowProgress As Boolean = True
    ''' <summary>
    ''' Records remote debug chat to debug log
    ''' </summary>
    Public RecordChatToDebugLog As Boolean = True
    ''' <summary>
    ''' Wraps the list outputs
    ''' </summary>
    Public WrapListOutputs As Boolean
    ''' <summary>
    ''' Ensures that all hardware will be probed
    ''' </summary>
    Public FullHardwareProbe As Boolean = True
    ''' <summary>
    ''' Makes the hardware prober a bit talkative
    ''' </summary>
    Public VerboseHardwareProbe As Boolean
    ''' <summary>
    ''' Draws the border around the notification
    ''' </summary>
    Public DrawBorderNotification As Boolean
    ''' <summary>
    ''' Whether to show the app information on boot
    ''' </summary>
    Public ShowAppInfoOnBoot As Boolean = True
    ''' <summary>
    ''' Whether to parse the command-line arguments on boot
    ''' </summary>
    Public ParseCommandLineArguments As Boolean = True
    ''' <summary>
    ''' Show how much time a stage took on boot
    ''' </summary>
    Public ShowStageFinishTimes As Boolean
    ''' <summary>
    ''' Whether to start the kernel mods on boot
    ''' </summary>
    Public StartKernelMods As Boolean = True
    ''' <summary>
    ''' Whether to show the current time before login
    ''' </summary>
    Public ShowCurrentTimeBeforeLogin As Boolean = True
    ''' <summary>
    ''' Whether to notify the user about minor boot faults
    ''' </summary>
    Public NotifyFaultsBoot As Boolean = True
    ''' <summary>
    ''' Whether to probe the injected commands at the start of the kernel shell
    ''' </summary>
    Public ProbeInjectedCommands As Boolean = True
    ''' <summary>
    ''' Whether to show more information about adapters
    ''' </summary>
    Public ExtensiveAdapterInformation As Boolean = True
    ''' <summary>
    ''' Whether to print general network information
    ''' </summary>
    Public GeneralNetworkInformation As Boolean = True
    ''' <summary>
    ''' Whether to start the default color wheel in true color mode
    ''' </summary>
    Public ColorWheelTrueColor As Boolean = True

    'Private flags
    ''' <summary>
    ''' Notifies user as soon as the kernel finished booting if there is an error reading configuration.
    ''' </summary>
    Friend NotifyConfigError As Boolean
    ''' <summary>
    ''' Notifies user as soon as the kernel finished booting if there is an error downloading debugging data.
    ''' </summary>
    Friend NotifyDebugDownloadError As Boolean
    ''' <summary>
    ''' Notifies user as soon as the kernel finished booting if network is not available while downloading debugging data.
    ''' </summary>
    Friend NotifyDebugDownloadNetworkUnavailable As Boolean
    ''' <summary>
    ''' When the command cancel is requested
    ''' </summary>
    Friend CancelRequested As Boolean
    ''' <summary>
    ''' Show MOTD every LoginPrompt() call
    ''' </summary>
    Friend ShowMOTDOnceFlag As Boolean = True
    ''' <summary>
    ''' The kernel has errored
    ''' </summary>
    Friend KernelErrored As Boolean
    ''' <summary>
    ''' Notifies user as soon as the kernel finished booting if there was a kernel error in the previous boot
    ''' </summary>
    Friend NotifyKernelError As Boolean

End Module
