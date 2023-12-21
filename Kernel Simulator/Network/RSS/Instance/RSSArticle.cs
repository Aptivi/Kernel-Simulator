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

using System.Xml;

namespace KS.Network.RSS.Instance
{
    public class RSSArticle
    {

        /// <summary>
        /// RSS Article Title
        /// </summary>
        public readonly string ArticleTitle;
        /// <summary>
        /// RSS Article Link
        /// </summary>
        public readonly string ArticleLink;
        /// <summary>
        /// RSS Article Descirption
        /// </summary>
        public readonly string ArticleDescription;
        /// <summary>
        /// RSS Article Parameters
        /// </summary>
        public readonly Dictionary<string, XmlNode> ArticleVariables;

        /// <summary>
        /// Makes a new instance of RSS article
        /// </summary>
        /// <param name="ArticleTitle"></param>
        /// <param name="ArticleLink"></param>
        /// <param name="ArticleDescription"></param>
        /// <param name="ArticleVariables"></param>
        public RSSArticle(string ArticleTitle, string ArticleLink, string ArticleDescription, Dictionary<string, XmlNode> ArticleVariables)
        {
            this.ArticleTitle = ArticleTitle.Trim();
            this.ArticleLink = ArticleLink.Trim();
            this.ArticleDescription = ArticleDescription.Trim();
            this.ArticleVariables = ArticleVariables;
        }

    }
}