﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Kernel.Threading;
using KS.Languages;
using KS.Security.Permissions;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Users;
using KS.Users.Login;
using System;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Runs a command as the superuser
    /// </summary>
    /// <remarks>
    /// You can run a command as "root," the superuser, using just your password.
    /// </remarks>
    class SudoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool sudoDone = false;
            PermissionsTools.Demand(PermissionTypes.UseSudo);

            // First, check to see if we're already root
            var root = UserManagement.GetUser("root");
            if (UserManagement.CurrentUserInfo == root)
            {
                TextWriterColor.Write(Translate.DoTranslation("You are already a superuser!"));
                return 10000 + (int)KernelExceptionType.ShellOperation;
            }    

            // Now, prompt for the current username's password
            string currentUsername = UserManagement.CurrentUser.Username;
            bool failed = false;
            try
            {
                if (Login.ShowPasswordPrompt(currentUsername))
                {
                    sudoDone = true;
                    DebugWriter.WriteDebug(DebugLevel.I, "Switching to root user...");
                    UserManagement.CurrentUserInfo = root;
                    UserManagement.LockUser(currentUsername);
                    UserManagement.LockUser("root");
                    var AltThreads = ShellManager.ShellStack[^1].AltCommandThreads;
                    if (AltThreads.Count == 0 || AltThreads[^1].IsAlive)
                    {
                        var CommandThread = new KernelThread($"Sudo Shell Command Thread", false, (cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutorParameters)cmdThreadParams));
                        ShellManager.ShellStack[^1].AltCommandThreads.Add(CommandThread);
                    }
                    ShellManager.GetLine(parameters.ArgumentsText);
                }
                else
                    return 10000 + (int)KernelExceptionType.ShellOperation;
            }
            catch (Exception ex)
            {
                failed = true;
                DebugWriter.WriteDebug(DebugLevel.I, "Executing command {0} as superuser failed: {1}", parameters.ArgumentsText, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Failed to execute the command as superuser.") + $" {ex.Message}");
            }
            finally
            {
                if (sudoDone)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Sudo is done. Switching to user {0}...", currentUsername);
                    UserManagement.CurrentUserInfo = UserManagement.GetUser(currentUsername);
                    UserManagement.UnlockUser(currentUsername);
                    UserManagement.UnlockUser("root");
                }
            }
            return failed ? 10000 + (int)KernelExceptionType.ShellOperation : 0;
        }

    }
}
