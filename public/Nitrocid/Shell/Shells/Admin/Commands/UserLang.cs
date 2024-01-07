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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Users;

namespace Nitrocid.Shell.Shells.Admin.Commands
{
    class UserLangCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string userName = parameters.ArgumentsList[0];
            string lang = parameters.ArgumentsList[1];
            int userIndex = UserManagement.GetUserIndex(userName);
            if (lang == "clear")
            {
                // If we're doing this on ourselves, change the kernel language to the system language
                lang = LanguageManager.currentLanguage.ThreeLetterLanguageName;
                if (UserManagement.CurrentUser.Username == userName)
                {
                    LanguageManager.currentUserLanguage = LanguageManager.currentLanguage;
                    UserManagement.CurrentUser.PreferredLanguage = lang;
                }

                // Now, change the language in the user config
                UserManagement.Users[userIndex].PreferredLanguage = null;
                TextWriterColor.Write(Translate.DoTranslation("Preferred user language set to {0}. You may want to log in again."), lang);
            }
            else if (LanguageManager.Languages.TryGetValue(lang, out LanguageInfo langInfo))
            {
                // Do it locally
                LanguageManager.currentUserLanguage = langInfo;

                // Now, change the language in the user config
                UserManagement.Users[userIndex].PreferredLanguage = lang;
                TextWriterColor.Write(Translate.DoTranslation("Preferred user language set to {0}. You may want to log in again."), lang);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Invalid language") + " {0}", lang);
                return 10000 + (int)KernelExceptionType.NoSuchLanguage;
            }
            return 0;
        }
    }
}
