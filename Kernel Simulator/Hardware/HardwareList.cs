﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections;
using System.Linq;

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

using System.Reflection;
using FluentFTP.Helpers;
using InxiFrontend;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Platform;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;

namespace KS.Hardware
{
    public static class HardwareList
    {

        /// <summary>
        /// Lists simple information about hardware
        /// </summary>
        public static void ListHardware()
        {
            if (HardwareProbe.HardwareInfo is not null)
            {
                // We are checking to see if any of the probers reported a failure starting with CPU
                if (HardwareProbe.HardwareInfo.Hardware.CPU is null | HardwareProbe.HardwareInfo.Hardware.CPU is not null & HardwareProbe.HardwareInfo.Hardware.CPU.Count == 0)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "CPU failed to probe.");
                    TextWriterColor.Write(Translate.DoTranslation("CPU: One or more of the CPU cores failed to be probed. Showing information anyway..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
                }

                // then RAM
                if (HardwareProbe.HardwareInfo.Hardware.RAM is null)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "RAM failed to probe.");
                    TextWriterColor.Write(Translate.DoTranslation("RAM: One or more of the RAM chips failed to be probed. Showing information anyway..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
                }

                // then GPU
                if (HardwareProbe.HardwareInfo.Hardware.GPU is null)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "GPU failed to probe.");
                    TextWriterColor.Write(Translate.DoTranslation("GPU: One or more of the graphics cards failed to be probed. Showing information anyway..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
                }

                // and finally HDD
                if (HardwareProbe.HardwareInfo.Hardware.HDD is null | HardwareProbe.HardwareInfo.Hardware.HDD is not null & HardwareProbe.HardwareInfo.Hardware.HDD.Count == 0)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "HDD failed to probe.");
                    TextWriterColor.Write(Translate.DoTranslation("HDD: One or more of the hard drives failed to be probed. Showing information anyway..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Warning));
                }

                // Print information about the probed hardware
                // CPU Info
                foreach (string ProcessorInfo in HardwareProbe.HardwareInfo.Hardware.CPU.Keys)
                {
                    var TargetProcessor = HardwareProbe.HardwareInfo.Hardware.CPU[ProcessorInfo];
                    TextWriterColor.Write("CPU: " + Translate.DoTranslation("Processor name:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                    TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), ProcessorInfo);
                    TextWriterColor.Write("CPU: " + Translate.DoTranslation("Processor clock speed:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                    TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), TargetProcessor.Speed);
                    TextWriterColor.Write("CPU: " + Translate.DoTranslation("Processor bits:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                    TextWriterColor.Write(" {0}-bit", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), TargetProcessor.Bits);
                    TextWriterColor.Write("CPU: " + Translate.DoTranslation("Processor SSE2 support:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                    TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), TargetProcessor.Flags.Contains("sse2") | TargetProcessor.Flags.Contains("SSE2"));
                }
                TextWriterColor.Write("CPU: " + Translate.DoTranslation("Total number of processors:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), Environment.ProcessorCount);

                // Print RAM info
                TextWriterColor.Write("RAM: " + Translate.DoTranslation("Total memory:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), PlatformDetector.IsOnWindows() ? ((long)Math.Round(Convert.ToDouble(HardwareProbe.HardwareInfo.Hardware.RAM.TotalMemory) * 1024d)).FileSizeToString() : HardwareProbe.HardwareInfo.Hardware.RAM.TotalMemory);

                // GPU info
                foreach (string GPUInfo in HardwareProbe.HardwareInfo.Hardware.GPU.Keys)
                {
                    var TargetGraphics = HardwareProbe.HardwareInfo.Hardware.GPU[GPUInfo];
                    TextWriterColor.Write("GPU: " + Translate.DoTranslation("Graphics card:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                    TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), TargetGraphics.Name);
                }

                // Drive Info
                foreach (string DriveInfo in HardwareProbe.HardwareInfo.Hardware.HDD.Keys)
                {
                    var TargetDrive = HardwareProbe.HardwareInfo.Hardware.HDD[DriveInfo];
                    if (TargetDrive.Vendor == "(Standard disk drives)")
                    {
                        TextWriterColor.Write("HDD: " + Translate.DoTranslation("Disk model:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                        TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), TargetDrive.Model);
                        TextWriterColor.Write("HDD: " + Translate.DoTranslation("Disk size:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                        TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), PlatformDetector.IsOnWindows() ? Convert.ToInt64(TargetDrive.Size).FileSizeToString() : TargetDrive.Size);
                    }
                    else
                    {
                        TextWriterColor.Write("HDD: " + Translate.DoTranslation("Disk model:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                        TextWriterColor.Write(" {0} {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), TargetDrive.Vendor, TargetDrive.Model);
                        TextWriterColor.Write("HDD: " + Translate.DoTranslation("Disk size:"), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                        TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), PlatformDetector.IsOnWindows() ? Convert.ToInt64(TargetDrive.Size).FileSizeToString() : TargetDrive.Size);
                    }
                    foreach (string PartInfo in TargetDrive.Partitions.Keys)
                    {
                        var TargetPart = TargetDrive.Partitions[PartInfo];
                        TextWriterColor.Write("HDD ({0}): " + Translate.DoTranslation("Partition size:"), false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), TargetPart.ID);
                        TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), PlatformDetector.IsOnWindows() ? Convert.ToInt64(TargetPart.Size).FileSizeToString() : TargetPart.Size);
                        TextWriterColor.Write("HDD ({0}): " + Translate.DoTranslation("Partition filesystem:"), false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), TargetPart.ID);
                        TextWriterColor.Write(" {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue), TargetPart.FileSystem);
                    }
                }
            }
        }

        /// <summary>
        /// Lists information about hardware
        /// </summary>
        /// <param name="HardwareType">Hadrware type defined by Inxi.NET. If "all", prints all information.</param>
        public static void ListHardware(string HardwareType)
        {
            var HardwareField = FieldManager.GetField(HardwareType, FieldManager.GetField(nameof(HardwareProbe.HardwareInfo.Hardware), typeof(Inxi)).FieldType);
            DebugWriter.Wdbg(DebugLevel.I, "Got hardware field {0}.", HardwareField is not null ? HardwareField.Name : "unknown");
            if (HardwareField is not null)
            {
                ListHardwareProperties(HardwareField);
            }
            else if (HardwareType.ToLower() == "all")
            {
                FieldInfo[] HardwareFields = FieldManager.GetField(nameof(HardwareProbe.HardwareInfo.Hardware), typeof(Inxi)).FieldType.GetFields();
                foreach (FieldInfo HardwareFieldType in HardwareFields)
                    ListHardwareProperties(HardwareFieldType);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Either the hardware type {0} is not probed, or is not valid."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), HardwareType);
            }
        }

        private static void ListHardwareProperties(FieldInfo Field)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Got hardware field {0}.", Field.Name);
            SeparatorWriterColor.WriteSeparator(Field.Name, true);
            var FieldValue = Field.GetValue(HardwareProbe.HardwareInfo.Hardware);
            if (FieldValue is not null)
            {
                IDictionary FieldValueDict = FieldValue as IDictionary;
                if (FieldValueDict is not null)
                {
                    foreach (string HardwareKey in FieldValueDict.Keys)
                    {
                        TextWriterColor.Write("- {0}: ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), HardwareKey);
                        foreach (PropertyInfo HardwareValuePropertyInfo in FieldValueDict[HardwareKey].GetType().GetProperties())
                        {
                            TextWriterColor.Write("  - {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), HardwareValuePropertyInfo.Name);
                            if (Field.Name == "HDD" & HardwareValuePropertyInfo.Name == "Partitions")
                            {
                                TextWriterColor.WritePlain("", true);
                                IDictionary Partitions = HardwareValuePropertyInfo.GetValue(FieldValueDict[HardwareKey]) as IDictionary;
                                if (Partitions is not null)
                                {
                                    foreach (string PartitionKey in Partitions.Keys)
                                    {
                                        TextWriterColor.Write("    - {0}: ", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), PartitionKey);
                                        foreach (PropertyInfo PartitionValuePropertyInfo in Partitions[PartitionKey].GetType().GetProperties())
                                        {
                                            TextWriterColor.Write("      - {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), PartitionValuePropertyInfo.Name);
                                            TextWriterColor.Write(Convert.ToString(PartitionValuePropertyInfo.GetValue(Partitions[PartitionKey])), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                                        }
                                    }
                                }
                                else
                                {
                                    TextWriterColor.Write(Translate.DoTranslation("Partitions not parsed to list."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                }
                            }
                            else if (Field.Name == "CPU" & HardwareValuePropertyInfo.Name == "Flags")
                            {
                                TextWriterColor.Write(string.Join(", ", HardwareValuePropertyInfo.GetValue(FieldValueDict[HardwareKey]) as string[]), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                            }
                            else
                            {
                                TextWriterColor.Write(Convert.ToString(HardwareValuePropertyInfo.GetValue(FieldValueDict[HardwareKey])), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                            }
                        }
                    }
                }
                else
                {
                    foreach (FieldInfo HardwareFieldInfo in Field.FieldType.GetFields())
                    {
                        TextWriterColor.Write("- {0}: ", false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry), HardwareFieldInfo.Name);
                        TextWriterColor.Write(Convert.ToString(HardwareFieldInfo.GetValue(FieldValue)), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                    }
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("The hardware type {0} is not probed yet. If you're sure that it's probed, restart the kernel with debugging enabled."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), Field.Name);
            }
        }

    }
}
