﻿
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using KS.Shell.Prompts.Presets.FTP;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.FTP.Commands;
using System;

namespace KS.Shell.Shells.FTP
{
    /// <summary>
    /// Common FTP shell class
    /// </summary>
    internal class FTPShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// FTP commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "cat",
                new CommandInfo("cat", ShellType, /* Localizable */ "Reads the content of a remote file to the console",
                    new[] {
                        new CommandArgumentInfo(new[] { "file" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_CatCommand(), CommandFlags.Wrappable)
            },
            
            { "cdl",
                new CommandInfo("cdl", ShellType, /* Localizable */ "Changes local directory to download to or upload from",
                    new[] {
                        new CommandArgumentInfo(new[] { "directory" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_CdlCommand())
            },
            
            { "cdr",
                new CommandInfo("cdr", ShellType, /* Localizable */ "Changes remote directory to download from or upload to",
                    new[] {
                        new CommandArgumentInfo(new[] { "directory" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_CdrCommand())
            },
            
            { "cp",
                new CommandInfo("cp", ShellType, /* Localizable */ "Copies file or directory to another file or directory.",
                    new[] {
                        new CommandArgumentInfo(new[] { "sourcefileordir", "targetfileordir" }, Array.Empty<SwitchInfo>(), true, 2)
                    }, new FTP_CpCommand())
            },
            
            { "del",
                new CommandInfo("del", ShellType, /* Localizable */ "Deletes remote file from server",
                    new[] {
                        new CommandArgumentInfo(new[] { "file" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_DelCommand())
            },
            
            { "detach",
                new CommandInfo("detach", ShellType, /* Localizable */ "Exits the shell without disconnecting",
                    new[] {
                        new CommandArgumentInfo()
                    }, new FTP_DetachCommand())
            },
            
            { "execute",
                new CommandInfo("execute", ShellType, /* Localizable */ "Executes an FTP server command",
                    new[] {
                        new CommandArgumentInfo(new[] { "command" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_ExecuteCommand())
            },
            
            { "get",
                new CommandInfo("get", ShellType, /* Localizable */ "Downloads remote file to local directory using binary or text",
                    new[] {
                        new CommandArgumentInfo(new[] { "file", "output" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_GetCommand())
            },
            
            { "getfolder",
                new CommandInfo("getfolder", ShellType, /* Localizable */ "Downloads remote folder to local directory using binary or text",
                    new[] {
                        new CommandArgumentInfo(new[] { "folder", "outputfolder" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_GetFolderCommand())
            },
            
            { "info",
                new CommandInfo("info", ShellType, /* Localizable */ "FTP server information",
                    new[] {
                        new CommandArgumentInfo()
                    }, new FTP_InfoCommand())
            },
            
            { "lsl",
                new CommandInfo("lsl", ShellType, /* Localizable */ "Lists local directory",
                    new[] {
                        new CommandArgumentInfo(new[] { "dir" }, new[] {
                            new SwitchInfo("showdetails", /* Localizable */ "Shows the file details in the list"),
                            new SwitchInfo("suppressmessages", /* Localizable */ "Suppresses the annoying \"permission denied\" messages")
                        })
                    }, new FTP_LslCommand())
            },
            
            { "lsr",
                new CommandInfo("lsr", ShellType, /* Localizable */ "Lists remote directory",
                    new[] {
                        new CommandArgumentInfo(new[] { "dir" }, new[] {
                            new SwitchInfo("showdetails", /* Localizable */ "Shows the file details in the list")
                        })
                    }, new FTP_LsrCommand())
            },
            
            { "mv",
                new CommandInfo("mv", ShellType, /* Localizable */ "Moves file or directory to another file or directory. You can also use that to rename files.",
                    new[] {
                        new CommandArgumentInfo(new[] { "sourcefileordir", "targetfileordir" }, Array.Empty<SwitchInfo>(), true, 2)
                    }, new FTP_MvCommand())
            },
            
            { "put",
                new CommandInfo("put", ShellType, /* Localizable */ "Uploads local file to remote directory using binary or text",
                    new[] {
                        new CommandArgumentInfo(new[] { "file", "output" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_PutCommand())
            },
            
            { "putfolder",
                new CommandInfo("putfolder", ShellType, /* Localizable */ "Uploads local folder to remote directory using binary or text",
                    new[] {
                        new CommandArgumentInfo(new[] { "<folder", "outputfolder" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_PutFolderCommand())
            },
            
            { "pwdl",
                new CommandInfo("pwdl", ShellType, /* Localizable */ "Gets current local directory",
                    new[] {
                        new CommandArgumentInfo()
                    }, new FTP_PwdlCommand())
            },
            
            { "pwdr",
                new CommandInfo("pwdr", ShellType, /* Localizable */ "Gets current remote directory",
                    new[] {
                        new CommandArgumentInfo()
                    }, new FTP_PwdrCommand())
            },
            
            { "perm",
                new CommandInfo("perm", ShellType, /* Localizable */ "Sets file permissions. This is supported only on FTP servers that run Unix.",
                    new[] {
                        new CommandArgumentInfo(new[] { "file", "permnumber" }, Array.Empty<SwitchInfo>(), true, 2)
                    }, new FTP_PermCommand())
            },
            
            { "sumfile",
                new CommandInfo("sumfile", ShellType, /* Localizable */ "Calculates file sums.",
                    new[] {
                        new CommandArgumentInfo(new[] { "file", "MD5/SHA1/SHA256/SHA512/CRC" }, Array.Empty<SwitchInfo>(), true, 2)
                    }, new FTP_SumFileCommand())
            },
            
            { "sumfiles",
                new CommandInfo("sumfiles", ShellType, /* Localizable */ "Calculates sums of files in specified directory.",
                    new[] {
                        new CommandArgumentInfo(new[] { "file", "MD5/SHA1/SHA256/SHA512/CRC" }, Array.Empty<SwitchInfo>(), true, 2)
                    }, new FTP_SumFilesCommand())
            },
            
            { "type",
                new CommandInfo("type", ShellType, /* Localizable */ "Sets the type for this session",
                    new[] {
                        new CommandArgumentInfo(new[] { "a/b" }, Array.Empty<SwitchInfo>(), true, 1)
                    }, new FTP_TypeCommand())
            }
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new FTPDefaultPreset() },
            { "PowerLine1", new FtpPowerLine1Preset() },
            { "PowerLine2", new FtpPowerLine2Preset() },
            { "PowerLine3", new FtpPowerLine3Preset() },
            { "PowerLineBG1", new FtpPowerLineBG1Preset() },
            { "PowerLineBG2", new FtpPowerLineBG2Preset() },
            { "PowerLineBG3", new FtpPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new FTPShell();

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["FTPShell"];

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => Network.Base.Connections.NetworkConnectionType.FTP.ToString();

    }
}
