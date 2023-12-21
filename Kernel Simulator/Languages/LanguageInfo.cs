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

using System.Globalization;
using System.Linq;
using KS.Resources;
using Newtonsoft.Json.Linq;

namespace KS.Languages
{
    public class LanguageInfo
    {

        /// <summary>
        /// The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.
        /// </summary>
        public readonly string ThreeLetterLanguageName;
        /// <summary>
        /// The full name of language without the country specifier.
        /// </summary>
        public readonly string FullLanguageName;
        /// <summary>
        /// Whether or not the language is transliterable (Arabic, Korea, ...)
        /// </summary>
        public readonly bool Transliterable;
        /// <summary>
        /// Whether the language is custom
        /// </summary>
        public readonly bool Custom;
        /// <summary>
        /// The localization information containing KS strings
        /// </summary>
        public readonly JObject LanguageResource;
        /// <summary>
        /// List of cultures of language
        /// </summary>
        public readonly List<CultureInfo> Cultures;

        /// <summary>
        /// Initializes the new instance of language information
        /// </summary>
        /// <param name="LangName">The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.</param>
        /// <param name="FullLanguageName">The full name of language without the country specifier.</param>
        /// <param name="Transliterable">Whether or not the language is transliterable (Arabic, Korea, ...)</param>
        public LanguageInfo(string LangName, string FullLanguageName, bool Transliterable)
        {
            // Check to see if the language being installed is found in resources
            if (!string.IsNullOrEmpty(KernelResources.ResourceManager.GetString(LangName.Replace("-", "_"))))
            {
                // Install values to the object instance
                ThreeLetterLanguageName = LangName;
                this.FullLanguageName = FullLanguageName;
                this.Transliterable = Transliterable;

                // Get all cultures associated with the language and install the parsed values. Additionally, it checks if the necessary cultures were added. If not,
                // the current culture is assumed.
                CultureInfo[] Cults = CultureInfo.GetCultures(CultureTypes.AllCultures);
                var Cultures = new List<CultureInfo>();
                foreach (CultureInfo Cult in Cults)
                {
                    if (Cult.EnglishName.ToLower().Contains(FullLanguageName.ToLower()))
                    {
                        Cultures.Add(Cult);
                    }
                }
                if (Cultures.Count == 0)
                    Cultures.Add(CultureInfo.CurrentCulture);
                this.Cultures = Cultures;

                // Get instance of langauge resource and install it
                JObject LanguageResource = (JObject)JObject.Parse(KernelResources.ResourceManager.GetString(LangName.Replace("-", "_"))).SelectToken("Localizations");
                this.LanguageResource = LanguageResource;
                Custom = false;
            }
            else
            {
                throw new Kernel.Exceptions.NoSuchLanguageException(Translate.DoTranslation("Invalid language") + " {0}", LangName);
            }
        }

        /// <summary>
        /// Initializes the new instance of language information
        /// </summary>
        /// <param name="LangName">The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.</param>
        /// <param name="FullLanguageName">The full name of language without the country specifier.</param>
        /// <param name="Transliterable">Whether or not the language is transliterable (Arabic, Korea, ...)</param>
        public LanguageInfo(string LangName, string FullLanguageName, bool Transliterable, JObject LanguageToken)
        {
            // Install values to the object instance
            ThreeLetterLanguageName = LangName;
            this.FullLanguageName = FullLanguageName;
            this.Transliterable = Transliterable;

            // Get all cultures associated with the language and install the parsed values. Additionally, it checks if the necessary cultures were added. If not,
            // the current culture is assumed.
            CultureInfo[] Cults = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var Cultures = new List<CultureInfo>();
            foreach (CultureInfo Cult in Cults)
            {
                if (Cult.EnglishName.ToLower().Contains(FullLanguageName.ToLower()))
                {
                    Cultures.Add(Cult);
                }
            }
            if (Cultures.Count == 0)
                Cultures.Add(CultureInfo.CurrentCulture);
            this.Cultures = Cultures;

            // Install it
            int EnglishLength = JObject.Parse(KernelResources.eng).SelectToken("Localizations").Count();
            Custom = true;
            if (LanguageToken.Count == EnglishLength)
            {
                LanguageResource = LanguageToken;
            }
            else
            {
                throw new Kernel.Exceptions.LanguageParseException(Translate.DoTranslation("Length of the English language doesn't match the length of the language token provided."));
            }
        }

    }
}
