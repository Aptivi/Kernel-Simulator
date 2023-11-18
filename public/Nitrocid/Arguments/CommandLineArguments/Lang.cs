﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Files.Operations.Querying;
using KS.Files.Paths;
using KS.Kernel.Extensions;
using KS.Languages;

namespace KS.Arguments.CommandLineArguments
{
    class LangArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(ArgumentParameters parameters)
        {
            string langPacksAddonPath = PathsManagement.AddonsPath + "/LanguagePacks";
            if (Checking.FolderExists(langPacksAddonPath))
            {
                AddonTools.ProcessAddon(langPacksAddonPath, AddonType.Important);
                LanguageManager.SetLangDry(parameters.ArgumentsList[0]);
                AddonTools.probedAddons.Clear();
            }
        }

    }
}
