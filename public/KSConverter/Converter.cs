﻿
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentFTP;
using KS.ConsoleBase;
using KS.Files.Querying;
using KS.Kernel.Configuration;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Network;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Shells;
using static KS.ConsoleBase.Colors.ColorTools;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Users.Groups;
using KS.Users;

namespace KSConverter
{
    static class Converter
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        public static void Main()
        {
            // Check console
            ConsoleChecker.CheckConsole();

            try
            {
                // Initialize all needed variables
                var ListOfOldPaths = ConverterTools.GetOldPaths("");
                var ListOfBackups = ConverterTools.GetOldPaths("KSBackup");

                // Load user token
                UserManagement.LoadUserToken();

                // Make backup directory
                SeparatorWriterColor.WriteSeparator("[1/7] Making backup directory", true, ColTypes.Stage);
                Debug.WriteLine($"Backup directory: {ConverterTools.GetHomeDirectory() + "/KSBackup"}");
                Debug.WriteLine($"FolderExists = {Checking.FolderExists(ConverterTools.GetHomeDirectory() + "/KSBackup")}");
                if (!Checking.FolderExists(ConverterTools.GetHomeDirectory() + "/KSBackup"))
                {
                    // Just make it!
                    TextWriterColor.Write("  - Backup directory not found. Creating directory...", true, ColTypes.Progress);
                    Debug.WriteLine("Creating directory...");
                    Directory.CreateDirectory(ConverterTools.GetHomeDirectory() + "/KSBackup");
                }
                else
                {
                    // Directory found. Skip the creation.
                    Debug.WriteLine("Already found.");
                    TextWriterColor.Write("  - Warning: backup directory is already found.", true, ColTypes.Warning);
                }
                Console.WriteLine();

                // Make backup of old configuration files in case something goes wrong during conversion.
                SeparatorWriterColor.WriteSeparator("[2/7] Making backup of old configuration files", true, ColTypes.Stage);
                foreach (string ConfigEntry in ListOfOldPaths.Keys)
                {
                    Debug.WriteLine($"Old path config entry: {ConfigEntry}");
                    Debug.WriteLine($"Old path exists: {Checking.FileExists(ListOfOldPaths[ConfigEntry])}");
                    Debug.WriteLine($"Backup exists: {Checking.FileExists(ListOfBackups[ConfigEntry])}");
                    if (Checking.FileExists(ListOfOldPaths[ConfigEntry]) & !Checking.FileExists(ListOfBackups[ConfigEntry]))
                    {
                        // Move the old config file to backup
                        Debug.WriteLine($"Moving {ConfigEntry} from {ListOfOldPaths[ConfigEntry]} to {ListOfBackups[ConfigEntry]}...");
                        TextWriterColor.Write("  - {0}: {1} -> {2}", true, ColTypes.NeutralText, ConfigEntry, ListOfOldPaths[ConfigEntry], ListOfBackups[ConfigEntry]);
                        File.Move(ListOfOldPaths[ConfigEntry], ListOfBackups[ConfigEntry]);
                    }
                    else if (Checking.FileExists(ListOfBackups[ConfigEntry]))
                    {
                        Debug.WriteLine("We already have backup!");
                        TextWriterColor.Write("  - Warning: {0} already exists", true, ColTypes.Warning, ListOfBackups[ConfigEntry]);
                    }
                    else
                    {
                        // File not found. Skip it.
                        Debug.WriteLine("We don't have config.");
                        TextWriterColor.Write("  - Warning: {0} not found in home directory.", true, ColTypes.Warning, ListOfOldPaths[ConfigEntry]);
                    }
                }
                Console.WriteLine();

                // Import all blocked devices to DebugDeviceNames.json
                SeparatorWriterColor.WriteSeparator("[3/7] Importing all blocked devices to DebugDeviceNames.json", true, ColTypes.Stage);
                Debug.WriteLine($"Blocked device backup exists = {Checking.FileExists(ListOfBackups["BlockedDevices"])}");
                if (Checking.FileExists(ListOfBackups["BlockedDevices"]))
                {
                    // Read blocked devices from old file
                    TextWriterColor.Write("  - Reading blocked devices from blocked_devices.csv...", true, ColTypes.Progress);
                    Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups["BlockedDevices"]}...");
                    var BlockedDevices = File.ReadAllLines(ListOfBackups["BlockedDevices"]).ToList();
                    Debug.WriteLine($"We have {BlockedDevices.Count} devices.");
                    TextWriterColor.Write("  - {0} devices found.", true, ColTypes.NeutralText, BlockedDevices.Count);

                    // Add blocked devices to new format
                    Debug.WriteLine($"Iterating {BlockedDevices.Count} blocked devices...");
                    foreach (string BlockedDevice in BlockedDevices)
                    {
                        Debug.WriteLine($"Adding blocked device {BlockedDevice} to the new config format...");
                        TextWriterColor.Write("  - Adding {0} to DebugDeviceNames.json...", true, ColTypes.Progress, BlockedDevice);
                        RemoteDebugTools.AddDeviceToJson(BlockedDevice, false);
                        RemoteDebugTools.SetDeviceProperty(BlockedDevice, RemoteDebugTools.DeviceProperty.Blocked, true);
                    }
                }
                else
                {
                    // File not found. Skip stage.
                    Debug.WriteLine("We don't have file.");
                    TextWriterColor.Write("  - Warning: blocked_devices.csv not found in home directory.", true, ColTypes.Warning);
                }
                Console.WriteLine();

                // Import all FTP speed dial settings to JSON
                SeparatorWriterColor.WriteSeparator("[4/7] Importing all FTP speed dial addresses to FTP_SpeedDial.json", true, ColTypes.Stage);
                Debug.WriteLine($"Speed dial addresses exists = {Checking.FileExists(ListOfBackups["FTPSpeedDial"])}");
                if (Checking.FileExists(ListOfBackups["FTPSpeedDial"]))
                {
                    // Read FTP speed dial addresses from old file
                    TextWriterColor.Write("  - Reading FTP speed dial addresses from ftp_speeddial.csv...", true, ColTypes.Progress);
                    Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups["FTPSpeedDial"]}...");
                    var SpeedDialLines = File.ReadAllLines(ListOfBackups["FTPSpeedDial"]);
                    Debug.WriteLine($"We have {SpeedDialLines.Length} addresses.");
                    TextWriterColor.Write("  - {0} addresses found.", true, ColTypes.NeutralText, SpeedDialLines.Length);

                    // Add addresses to new format
                    foreach (string SpeedDialLine in SpeedDialLines)
                    {
                        // Populate variables
                        var ChosenLineSeparation = SpeedDialLine.Split(',');
                        Debug.WriteLine($"Separation count = {ChosenLineSeparation.Length}");
                        string Address = ChosenLineSeparation[0];
                        Debug.WriteLine($"Address = {Address}");
                        string Port = ChosenLineSeparation[1];
                        Debug.WriteLine($"Port = {Port}");
                        string Username = ChosenLineSeparation[2];
                        Debug.WriteLine($"Username = {Username}");
                        FtpEncryptionMode Encryption = (FtpEncryptionMode)Enum.Parse(typeof(FtpEncryptionMode), ChosenLineSeparation[3]);
                        Debug.WriteLine($"Encryption = {Encryption}");

                        // Add the entry!
                        TextWriterColor.Write("  - Adding {0} to FTP_SpeedDial.json...", true, ColTypes.Progress, Address);
                        NetworkTools.AddEntryToSpeedDial(Address, Convert.ToInt32(Port), Username, NetworkTools.SpeedDialType.FTP, Encryption);
                    }
                }
                else
                {
                    // File not found. Skip stage.
                    Debug.WriteLine("We don't have file.");
                    TextWriterColor.Write("  - Warning: ftp_speeddial.csv not found in home directory.", true, ColTypes.Warning);
                }
                Console.WriteLine();

                // Import all users to JSON
                SeparatorWriterColor.WriteSeparator("[5/7] Importing all users to Users.json", true, ColTypes.Stage);
                Debug.WriteLine($"Users file exists = {Checking.FileExists(ListOfBackups["Users"])}");
                if (Checking.FileExists(ListOfBackups["Users"]))
                {
                    // Read all users from old file
                    TextWriterColor.Write("  - Reading users from users.csv...", true, ColTypes.Progress);
                    Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups["Users"]}...");
                    var UsersLines = File.ReadAllLines(ListOfBackups["Users"]);
                    Debug.WriteLine($"We have {UsersLines.Length} addresses.");
                    TextWriterColor.Write("  - {0} users found.", true, ColTypes.NeutralText, UsersLines.Length);

                    // Add users to new format
                    foreach (string UsersLine in UsersLines)
                    {
                        // Populate variables
                        Debug.WriteLine($"Parsing line {UsersLine}...");
                        var ChosenLineSeparation = UsersLine.Split(',');
                        Debug.WriteLine($"Separation count = {ChosenLineSeparation.Length}");
                        string Username = ChosenLineSeparation[0];
                        Debug.WriteLine($"Username = {Username}");
                        string Password = ChosenLineSeparation[1];
                        Debug.WriteLine($"Password = {Password}");
                        string Administrator = ChosenLineSeparation.Length >= 3 ? ChosenLineSeparation[2] : "False";
                        Debug.WriteLine($"Administrator = {Administrator}");
                        string Disabled = ChosenLineSeparation.Length >= 4 ? ChosenLineSeparation[3] : "False";
                        Debug.WriteLine($"Disabled = {Disabled}");
                        string Anonymous = ChosenLineSeparation.Length >= 5 ? ChosenLineSeparation[4] : "False";
                        Debug.WriteLine($"Anonymous = {Anonymous}");

                        // Add the entry!
                        TextWriterColor.Write("  - Adding {0} to Users.json...", true, ColTypes.Progress, Username);
                        UserManagement.InitializeUser(Username, Password, false);
                        if (Administrator == "True")
                        {
                            Debug.WriteLine($"Adding the Administrator permission to {Username}...");
                            GroupManagement.AddGroup(GroupManagement.GroupType.Administrator, Username);
                        }
                        if (Disabled == "True")
                        {
                            Debug.WriteLine($"Adding the Disabled permission to {Username}...");
                            GroupManagement.AddGroup(GroupManagement.GroupType.Disabled, Username);
                        }
                        if (Anonymous == "True")
                        {
                            Debug.WriteLine($"Adding the Anonymous permission to {Username}...");
                            GroupManagement.AddGroup(GroupManagement.GroupType.Anonymous, Username);
                        }
                    }
                }
                else
                {
                    // File not found. Skip stage.
                    Debug.WriteLine("We don't have file.");
                    TextWriterColor.Write("  - Warning: users.csv not found in home directory.", true, ColTypes.Warning);
                }
                Console.WriteLine();

                // Import all aliases to JSON
                SeparatorWriterColor.WriteSeparator("[6/7] Importing all aliases to Aliases.json", true, ColTypes.Stage);
                Debug.WriteLine($"Aliases file exists = {Checking.FileExists(ListOfBackups["Aliases"])}");
                if (Checking.FileExists(ListOfBackups["Aliases"]))
                {
                    // Read all aliases from old file
                    TextWriterColor.Write("  - Reading users from aliases.csv...", true, ColTypes.Progress);
                    Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups["Aliases"]}...");
                    var AliasesLines = File.ReadAllLines(ListOfBackups["Aliases"]);
                    Debug.WriteLine($"We have {AliasesLines.Length} aliases.");
                    TextWriterColor.Write("  - {0} aliases found.", true, ColTypes.NeutralText, AliasesLines.Length);

                    // Add aliases to new format
                    foreach (string AliasLine in AliasesLines)
                    {
                        // POpulate variables
                        var AliasLineSplit = AliasLine.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        Debug.WriteLine($"Separation count = {AliasLineSplit.Length}");
                        string AliasCommand = AliasLineSplit[1];
                        Debug.WriteLine($"AliasCommand = {AliasCommand}");
                        string ActualCommand = AliasLineSplit[2];
                        Debug.WriteLine($"ActualCommand = {ActualCommand}");
                        string AliasType = AliasLineSplit[0];
                        Debug.WriteLine($"AliasType = {AliasType}");

                        // Add the entry!
                        TextWriterColor.Write("  - Adding {0} to Aliases.json...", true, ColTypes.Progress, AliasCommand);
                        switch (AliasType ?? "")
                        {
                            case "Shell":
                                {
                                    Dictionary<string, string> aliases = AliasManager.GetAliasesListFromType(ShellType.Shell);
                                    if (!aliases.ContainsKey(AliasCommand))
                                        AliasManager.AddAlias(AliasCommand, ActualCommand, ShellType.Shell);
                                    break;
                                }
                            case "Remote":
                                {
                                    Dictionary<string, string> aliases = AliasManager.GetAliasesListFromType(ShellType.RemoteDebugShell);
                                    if (!aliases.ContainsKey(AliasCommand))
                                        AliasManager.AddAlias(AliasCommand, ActualCommand, ShellType.RemoteDebugShell);
                                    break;
                                }
                            case "FTPShell":
                                {
                                    Dictionary<string, string> aliases = AliasManager.GetAliasesListFromType(ShellType.FTPShell);
                                    if (!aliases.ContainsKey(AliasCommand))
                                        AliasManager.AddAlias(AliasCommand, ActualCommand, ShellType.FTPShell);
                                    break;
                                }
                            case "SFTPShell":
                                {
                                    Dictionary<string, string> aliases = AliasManager.GetAliasesListFromType(ShellType.SFTPShell);
                                    if (!aliases.ContainsKey(AliasCommand))
                                        AliasManager.AddAlias(AliasCommand, ActualCommand, ShellType.SFTPShell);
                                    break;
                                }
                            case "Mail":
                                {
                                    Dictionary<string, string> aliases = AliasManager.GetAliasesListFromType(ShellType.MailShell);
                                    if (!aliases.ContainsKey(AliasCommand))
                                        AliasManager.AddAlias(AliasCommand, ActualCommand, ShellType.MailShell);
                                    break;
                                }
                            default:
                                {
                                    TextWriterColor.Write("  - Invalid type {0}", true, ColTypes.Error, AliasType);
                                    break;
                                }
                        }
                    }

                    // Save the changes
                    Debug.WriteLine("Saving...");
                    TextWriterColor.Write("  - Saving aliases to Aliases.json...", true, ColTypes.Progress);
                    AliasManager.SaveAliases();
                }
                else
                {
                    // File not found. Skip stage.
                    Debug.WriteLine("We don't have file.");
                    TextWriterColor.Write("  - Warning: aliases.csv not found in home directory.", true, ColTypes.Warning);
                }
                Console.WriteLine();

                // Import all config to JSON
                SeparatorWriterColor.WriteSeparator("[7/7] Importing all kernel config to KernelConfig.json", true, ColTypes.Stage);
                Debug.WriteLine($"Config file exists = {Checking.FileExists(ListOfBackups["Configuration"])}");
                if (Checking.FileExists(ListOfBackups["Configuration"]))
                {
#if !NETCOREAPP
                    // Read all config from old file
                    TextWriterColor.Write("  - Reading config from kernelConfig.ini...", true, ColTypes.Progress);
                    Debug.WriteLine("Reading configuration...");
                    if (!PreFivePointFive.ReadPreFivePointFiveConfig(ListOfBackups["Configuration"]))
                    {
                        if (!FivePointFive.ReadFivePointFiveConfig(ListOfBackups["Configuration"]))
                        {
                            Debug.WriteLine("Incompatible format. Both ReadPreFivePointFiveConfig and ReadFivePointFiveConfig returned False. Regenerating...");
                            TextWriterColor.Write("  - Warning: kernelConfig.ini has incompatible format. Generating new config anyways...", true, ColTypes.Warning);
                        }
                    }

                    // Save the changes
                    Debug.WriteLine("Saving...");
                    TextWriterColor.Write("  - Saving configuration to KernelConfig.json...", true, ColTypes.Progress);
                    Config.CreateConfig();
#else
                    // We need to use .NET Framework version of KSConverter to be able to fully use MadMilkman.Ini
                    // as we used it back when Kernel Simulator and MadMilkman.Ini were so tied to .NET Framework.
                    Debug.WriteLine("Config conversion needs to be done in .NET Framework.");
                    TextWriterColor.Write("  - Warning: To convert kernel configuration from kernelConfig.ini to KernelConfig.json, you need to run the .NET Framework version of KSConverter.", true, ColTypes.Warning);
#endif
                }
                else
                {
                    // File not found. Skip stage.
                    Debug.WriteLine("We don't have file.");
                    TextWriterColor.Write("  - Warning: kernelConfig.ini not found in home directory.", true, ColTypes.Warning);
                }
                Console.WriteLine();

                // Print this message:
                TextWriterColor.Write("- Successfully converted all settings to new format! Enjoy!", true, ColTypes.Success);
                TextWriterColor.Write("- Press any key to exit.", true, ColTypes.Success);
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write("- Error converting settings: {0}", true, ColTypes.Error, ex.Message);
                TextWriterColor.Write("- Press any key to exit. Stack trace below:", true, ColTypes.Error);
                TextWriterColor.Write(ex.StackTrace, true, ColTypes.NeutralText);
                Console.ReadKey(true);
            }
        }

    }
}
