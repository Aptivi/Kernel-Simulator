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

using Nitrocid.StandaloneAnalyzer.Analyzers;

namespace Nitrocid.StandaloneAnalyzer
{
    internal static class AnalyzersList
    {
        // For contributors: If you're going to add a new analyzer, you need to copy the implementation from Analyzers to here,
        // and make a dedicated diagnostic class for the standalone analyzer to recognize your new analyzer.
        internal static readonly IAnalyzer[] analyzers =
        [
            new NKS0001(),
            new NKS0002(),
            new NKS0003(),
            new NKS0004(),
            new NKS0005(),
            new NKS0006(),
            new NKS0007(),
            new NKS0008(),
            new NKS0009(),
            new NKS0010(),
            new NKS0011(),
            new NKS0012(),
            new NKS0013(),
            new NKS0014(),
            new NKS0015(),
            new NKS0016(),
            new NKS0017(),
        ];
    }
}
