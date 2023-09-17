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

namespace KS.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell type enumeration
    /// </summary>
    public enum ShellType
    {
        /// <summary>
        /// Normal UESH shell
        /// </summary>
        Shell,
        /// <summary>
        /// FTP shell
        /// </summary>
        FTPShell,
        /// <summary>
        /// Mail shell
        /// </summary>
        MailShell,
        /// <summary>
        /// SFTP shell
        /// </summary>
        SFTPShell,
        /// <summary>
        /// Text shell
        /// </summary>
        TextShell,
        /// <summary>
        /// RSS Shell
        /// </summary>
        RSSShell,
        /// <summary>
        /// JSON Shell
        /// </summary>
        JsonShell,
        /// <summary>
        /// HTTP shell
        /// </summary>
        HTTPShell,
        /// <summary>
        /// Hex shell
        /// </summary>
        HexShell,
        /// <summary>
        /// Kernel administration shell
        /// </summary>
        AdminShell,
        /// <summary>
        /// SQL shell
        /// </summary>
        SqlShell,
        /// <summary>
        /// Kernel debug shell
        /// </summary>
        DebugShell
    }
}
