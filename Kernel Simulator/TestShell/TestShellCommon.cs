﻿using System;
using System.Collections.Generic;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

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

using KS.TestShell.Commands;

namespace KS.TestShell
{
	static class TestShellCommon
	{

		internal static readonly Dictionary<string, CommandInfo> Test_ModCommands = [];
		public static readonly Dictionary<string, CommandInfo> Test_Commands = new()
		{
			{ "print", new CommandInfo("print", ShellType.TestShell, "Prints a string to console using color type and line print", new CommandArgumentInfo(["<Color> <Line> <Message>"], true, 3), new Test_PrintCommand()) },
			{ "printf", new CommandInfo("printf", ShellType.TestShell, "Prints a string to console using color type and line print with format support", new CommandArgumentInfo(["<Color> <Line> <Variable1;Variable2;Variable3;...> <Message>"], true, 4), new Test_PrintFCommand()) },
			{ "printd", new CommandInfo("printd", ShellType.TestShell, "Prints a string to debugger", new CommandArgumentInfo(["<Message>"], true, 1), new Test_PrintDCommand()) },
			{ "printdf", new CommandInfo("printdf", ShellType.TestShell, "Prints a string to debugger with format support", new CommandArgumentInfo(["<Variable1;Variable2;Variable3;...> <Message>"], true, 2), new Test_PrintDFCommand()) },
			{ "printsep", new CommandInfo("printsep", ShellType.TestShell, "Prints a separator", new CommandArgumentInfo(["<Message>"], true, 1), new Test_PrintSepCommand()) },
			{ "printsepf", new CommandInfo("printsepf", ShellType.TestShell, "Prints a separator with format support", new CommandArgumentInfo(["<Variable1;Variable2;Variable3;...> <Message>"], true, 2), new Test_PrintSepCommand()) },
			{ "printsepcolor", new CommandInfo("printsepcolor", ShellType.TestShell, "Prints a separator with color support", new CommandArgumentInfo(["<Color> <Message>"], true, 2), new Test_PrintSepColorCommand()) },
			{ "printsepcolorf", new CommandInfo("printsepcolorf", ShellType.TestShell, "Prints a separator with color and format support", new CommandArgumentInfo(["<Color> <Variable1;Variable2;Variable3;...> <Message>"], true, 3), new Test_PrintSepColorFCommand()) },
			{ "probehw", new CommandInfo("probehw", ShellType.TestShell, "Tests probing the hardware", new CommandArgumentInfo([], false, 0), new Test_ProbeHwCommand()) },
			{ "panic", new CommandInfo("panic", ShellType.TestShell, "Tests the kernel error facility", new CommandArgumentInfo(["<ErrorType> <Reboot> <RebootTime> <Description>"], true, 4), new Test_PanicCommand()) },
			{ "panicf", new CommandInfo("panicf", ShellType.TestShell, "Tests the kernel error facility with format support", new CommandArgumentInfo(["<ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>"], true, 5), new Test_PanicFCommand()) },
			{ "translate", new CommandInfo("translate", ShellType.TestShell, "Tests translating a string that exists in resources to specific language", new CommandArgumentInfo(["<Lang> <Message>"], true, 2), new Test_TranslateCommand()) },
			{ "places", new CommandInfo("places", ShellType.TestShell, "Prints a string to console and parses the placeholders found", new CommandArgumentInfo(["<Message>"], true, 1), new Test_PlacesCommand()) },
			{ "testcrc32", new CommandInfo("testcrc32", ShellType.TestShell, "Encrypts a string using CRC32", new CommandArgumentInfo(["<string>"], true, 1), new Test_TestCRC32Command()) },
			{ "testsha512", new CommandInfo("testsha512", ShellType.TestShell, "Encrypts a string using SHA512", new CommandArgumentInfo(["<string>"], true, 1), new Test_TestSHA512Command()) },
			{ "testsha384", new CommandInfo("testsha384", ShellType.TestShell, "Encrypts a string using SHA384", new CommandArgumentInfo(["<string>"], true, 1), new Test_TestSHA384Command()) },
			{ "testsha256", new CommandInfo("testsha256", ShellType.TestShell, "Encrypts a string using SHA256", new CommandArgumentInfo(["<string>"], true, 1), new Test_TestSHA256Command()) },
			{ "testsha1", new CommandInfo("testsha1", ShellType.TestShell, "Encrypts a string using SHA1", new CommandArgumentInfo(["<string>"], true, 1), new Test_TestSHA1Command()) },
			{ "testmd5", new CommandInfo("testmd5", ShellType.TestShell, "Encrypts a string using MD5", new CommandArgumentInfo(["<string>"], true, 1), new Test_TestMD5Command()) },
			{ "testregexp", new CommandInfo("testregexp", ShellType.TestShell, "Tests the regular expression facility", new CommandArgumentInfo(["<pattern> <string>"], true, 2), new Test_TestRegExpCommand()) },
			{ "loadmods", new CommandInfo("loadmods", ShellType.TestShell, "Starts all mods", new CommandArgumentInfo([], false, 0), new Test_LoadModsCommand()) },
			{ "stopmods", new CommandInfo("stopmods", ShellType.TestShell, "Stops all mods", new CommandArgumentInfo([], false, 0), new Test_StopModsCommand()) },
			{ "reloadmods", new CommandInfo("reloadmods", ShellType.TestShell, "Reloads all mods", new CommandArgumentInfo([], false, 0), new Test_ReloadModsCommand()) },
			{ "blacklistmod", new CommandInfo("blacklistmod", ShellType.TestShell, "Adds a mod to the blacklist", new CommandArgumentInfo(["<mod>"], true, 1), new Test_BlacklistModCommand()) },
			{ "unblacklistmod", new CommandInfo("unblacklistmod", ShellType.TestShell, "Removes a mod from the blacklist", new CommandArgumentInfo(["<mod>"], true, 1), new Test_UnblacklistModCommand()) },
			{ "debug", new CommandInfo("debug", ShellType.TestShell, "Enables or disables debug", new CommandArgumentInfo(["<Enable:True/False>"], true, 1), new Test_DebugCommand()) },
			{ "rdebug", new CommandInfo("rdebug", ShellType.TestShell, "Enables or disables remote debug", new CommandArgumentInfo(["<Enable:True/False>"], true, 1), new Test_RDebugCommand()) },
			{ "colortest", new CommandInfo("colortest", ShellType.TestShell, "Tests the VT sequence for 255 colors", new CommandArgumentInfo(["<1-255>"], true, 1), new Test_ColorTestCommand()) },
			{ "colortruetest", new CommandInfo("colortruetest", ShellType.TestShell, "Tests the VT sequence for true color", new CommandArgumentInfo(["<R;G;B>"], true, 1), new Test_ColorTrueTestCommand()) },
			{ "colorwheel", new CommandInfo("colorwheel", ShellType.TestShell, "Tests the color wheel", new CommandArgumentInfo([], false, 0), new Test_ColorWheelCommand()) },
			{ "sendnot", new CommandInfo("sendnot", ShellType.TestShell, "Sends a notification to test the receiver", new CommandArgumentInfo(["<Priority> <title> <desc>"], true, 3), new Test_SendNotCommand()) },
			{ "sendnotprog", new CommandInfo("sendnotprog", ShellType.TestShell, "Sends a progress notification to test the receiver", new CommandArgumentInfo(["<Priority> <title> <desc> <failat>"], true, 4), new Test_SendNotProgCommand()) },
			{ "dcalend", new CommandInfo("dcalend", ShellType.TestShell, "Tests printing date using different calendars", new CommandArgumentInfo(["<calendar>"], true, 1), new Test_DCalendCommand()) },
			{ "listcodepages", new CommandInfo("listcodepages", ShellType.TestShell, "Lists all supported codepages", new CommandArgumentInfo([], false, 0), new Test_ListCodePagesCommand()) },
			{ "lscompilervars", new CommandInfo("lscompilervars", ShellType.TestShell, "What compiler variables are enabled in the application?", new CommandArgumentInfo([], false, 0), new Test_LsCompilerVarsCommand()) },
			{ "testdictwriterstr", new CommandInfo("testdictwriterstr", ShellType.TestShell, "Tests the dictionary writer with the string and string array", new CommandArgumentInfo([], false, 0), new Test_TestDictWriterStrCommand()) },
			{ "testdictwriterint", new CommandInfo("testdictwriterint", ShellType.TestShell, "Tests the dictionary writer with the integer and integer array", new CommandArgumentInfo([], false, 0), new Test_TestDictWriterIntCommand()) },
			{ "testdictwriterchar", new CommandInfo("testdictwriterchar", ShellType.TestShell, "Tests the dictionary writer with the char and char array", new CommandArgumentInfo([], false, 0), new Test_TestDictWriterCharCommand()) },
			{ "testlistwriterstr", new CommandInfo("testlistwriterstr", ShellType.TestShell, "Tests the list writer with the string and string array", new CommandArgumentInfo([], false, 0), new Test_TestListWriterStrCommand()) },
			{ "testlistwriterint", new CommandInfo("testlistwriterint", ShellType.TestShell, "Tests the list writer with the integer and integer array", new CommandArgumentInfo([], false, 0), new Test_TestListWriterIntCommand()) },
			{ "testlistwriterchar", new CommandInfo("testlistwriterchar", ShellType.TestShell, "Tests the list writer with the char and char array", new CommandArgumentInfo([], false, 0), new Test_TestListWriterCharCommand()) },
			{ "lscultures", new CommandInfo("lscultures", ShellType.TestShell, "Lists supported cultures", new CommandArgumentInfo(["[search]"], false, 0), new Test_LsCulturesCommand()) },
			{ "getcustomsaversetting", new CommandInfo("getcustomsaversetting", ShellType.TestShell, "Gets custom saver settings", new CommandArgumentInfo(["<saver> <setting>"], true, 2), new Test_GetCustomSaverSettingCommand()) },
			{ "setcustomsaversetting", new CommandInfo("setcustomsaversetting", ShellType.TestShell, "Sets custom saver settings", new CommandArgumentInfo(["<saver> <setting> <value>"], true, 3), new Test_SetCustomSaverSettingCommand()) },
			{ "showtime", new CommandInfo("showtime", ShellType.TestShell, "Shows local kernel time", new CommandArgumentInfo([], false, 0), new Test_ShowTimeCommand()) },
			{ "showdate", new CommandInfo("showdate", ShellType.TestShell, "Shows local kernel date", new CommandArgumentInfo([], false, 0), new Test_ShowDateCommand()) },
			{ "showtd", new CommandInfo("showtd", ShellType.TestShell, "Shows local kernel date and time", new CommandArgumentInfo([], false, 0), new Test_ShowTDCommand()) },
			{ "showtimeutc", new CommandInfo("showtimeutc", ShellType.TestShell, "Shows UTC kernel time", new CommandArgumentInfo([], false, 0), new Test_ShowTimeUtcCommand()) },
			{ "showdateutc", new CommandInfo("showdateutc", ShellType.TestShell, "Shows UTC kernel date", new CommandArgumentInfo([], false, 0), new Test_ShowDateUtcCommand()) },
			{ "showtdutc", new CommandInfo("showtdutc", ShellType.TestShell, "Shows UTC kernel date and time", new CommandArgumentInfo([], false, 0), new Test_ShowTDUtcCommand()) },
			{ "testtable", new CommandInfo("testtable", ShellType.TestShell, "Tests the table functionality", new CommandArgumentInfo(["[margin]"], false, 0), new Test_TestTableCommand()) },
			{ "checkstring", new CommandInfo("checkstring", ShellType.TestShell, "Checks to see if the translatable string exists in the KS resources", new CommandArgumentInfo(["<string>"], true, 1), new Test_CheckStringCommand()) },
			{ "checksettingsentryvars", new CommandInfo("checksettingsentryvars", ShellType.TestShell, "Checks all the KS settings to see if the variables are written correctly", new CommandArgumentInfo([], false, 0), new Test_CheckSettingsEntryVarsCommand()) },
			{ "checklocallines", new CommandInfo("checklocallines", ShellType.TestShell, "Checks all the localization text line numbers to see if they're all equal", new CommandArgumentInfo([], false, 0), new Test_CheckLocalLinesCommand()) },
			{ "checkstrings", new CommandInfo("checkstrings", ShellType.TestShell, "Checks to see if the translatable strings exist in the KS resources", new CommandArgumentInfo(["<stringlistfile>"], true, 1), new Test_CheckStringsCommand()) },
			{ "sleeptook", new CommandInfo("sleeptook", ShellType.TestShell, "How many milliseconds did it really take to sleep?", new CommandArgumentInfo(["[-t] <sleepms>"], true, 1), new Test_SleepTookCommand(), false, false, false, false, false) },
			{ "getlinestyle", new CommandInfo("getlinestyle", ShellType.TestShell, "Gets the line ending style from text file", new CommandArgumentInfo(["<textfile>"], true, 1), new Test_GetLineStyleCommand()) },
			{ "printfiglet", new CommandInfo("printfiglet", ShellType.TestShell, "Prints a string to console using color type and line print with Figlet support", new CommandArgumentInfo(["<Color> <FigletFont> <Message>"], true, 3), new Test_PrintFigletCommand()) },
			{ "printfigletf", new CommandInfo("printfigletf", ShellType.TestShell, "Prints a string to console using color type and line print with format and Figlet support", new CommandArgumentInfo(["<Color> <FigletFont> <Variable1;Variable2;Variable3;...> <Message>"], true, 4), new Test_PrintFigletFCommand()) },
			{ "powerlinetest", new CommandInfo("powerlinetest", ShellType.TestShell, "Tests your console for PowerLine support", new CommandArgumentInfo([], false, 0), new Test_PowerLineTestCommand()) },
			{ "testexecuteasm", new CommandInfo("testexecuteasm", ShellType.TestShell, "Tests assembly entry point execution", new CommandArgumentInfo(["<pathtoasm>"], true, 1), new Test_TestExecuteAsmCommand()) },
			{ "help", new CommandInfo("help", ShellType.TestShell, "Shows help screen", new CommandArgumentInfo(["[command]"], false, 0), new Test_HelpCommand()) },
			{ "start", new CommandInfo("start", ShellType.TestShell, "Exits the test shell and starts the kernel", new CommandArgumentInfo([], false, 0), new Test_StartCommand()) },
			{ "shutdown", new CommandInfo("shutdown", ShellType.TestShell, "Exits the test shell and shuts down the kernel", new CommandArgumentInfo([], false, 0), new Test_ShutdownCommand()) }
		};
		public static bool Test_ShutdownFlag;

	}
}
