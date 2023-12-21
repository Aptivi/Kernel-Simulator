﻿using System;
using System.Collections.Generic;

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

using System.IO;
using KS.Misc.Editors.HexEdit.Commands;
using KS.Misc.Threading;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Misc.Editors.HexEdit
{
	public static class HexEditShellCommon
	{

		// Variables
		public static readonly Dictionary<string, CommandInfo> HexEdit_Commands = new() { { "addbyte", new CommandInfo("addbyte", ShellType.HexShell, "Adds a new byte at the end of the file", new CommandArgumentInfo(["<byte>"], true, 1), new HexEdit_AddByteCommand()) }, { "addbytes", new CommandInfo("addbytes", ShellType.HexShell, "Adds the new bytes at the end of the file", new CommandArgumentInfo([], false, 0), new HexEdit_AddBytesCommand()) }, { "clear", new CommandInfo("clear", ShellType.HexShell, "Clears the binary file", new CommandArgumentInfo([], false, 0), new HexEdit_ClearCommand()) }, { "delbyte", new CommandInfo("delbyte", ShellType.HexShell, "Deletes a byte using the byte number", new CommandArgumentInfo(["<bytenumber>"], true, 1), new HexEdit_DelByteCommand()) }, { "delbytes", new CommandInfo("delbytes", ShellType.HexShell, "Deletes the range of bytes", new CommandArgumentInfo(["<startbyte> [endbyte]"], true, 1), new HexEdit_DelBytesCommand()) }, { "exitnosave", new CommandInfo("exitnosave", ShellType.HexShell, "Exits the hex editor", new CommandArgumentInfo([], false, 0), new HexEdit_ExitNoSaveCommand()) }, { "help", new CommandInfo("help", ShellType.HexShell, "Lists available commands", new CommandArgumentInfo(["[command]"], false, 0), new HexEdit_HelpCommand()) }, { "print", new CommandInfo("print", ShellType.HexShell, "Prints the contents of the file with byte numbers to the console", new CommandArgumentInfo(["[startbyte] [endbyte]"], false, 0), new HexEdit_PrintCommand()) }, { "querybyte", new CommandInfo("querybyte", ShellType.HexShell, "Queries a byte in a specified range of bytes or all bytes", new CommandArgumentInfo(["<byte> [startbyte] [endbyte]"], true, 1), new HexEdit_QueryByteCommand()) }, { "replace", new CommandInfo("replace", ShellType.HexShell, "Replaces a byte with another one", new CommandArgumentInfo(["<byte> <replacedbyte>"], true, 2), new HexEdit_ReplaceCommand()) }, { "save", new CommandInfo("save", ShellType.HexShell, "Saves the file", new CommandArgumentInfo([], false, 0), new HexEdit_SaveCommand()) } };
		public static FileStream HexEdit_FileStream;
		public static List<byte> HexEdit_FileBytes;
		public static KernelThread HexEdit_AutoSave = new("Hex Edit Autosave Thread", false, HexEditTools.HexEdit_HandleAutoSaveBinaryFile);
		public static bool HexEdit_AutoSaveFlag = true;
		public static int HexEdit_AutoSaveInterval = 60;
		internal static byte[] HexEdit_FileBytesOrig;
		internal static readonly Dictionary<string, CommandInfo> HexEdit_ModCommands = [];

	}
}