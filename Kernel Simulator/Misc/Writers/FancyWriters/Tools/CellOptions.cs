﻿
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

using ColorSeq;
using KS.ConsoleBase.Colors;

namespace KS.Misc.Writers.FancyWriters.Tools
{
    /// <summary>
    /// Table cell options
    /// </summary>
    public class CellOptions
    {

        /// <summary>
        /// The column, or row value, number
        /// </summary>
        public int ColumnNumber { get; private set; }
        /// <summary>
        /// The row number
        /// </summary>
        public int RowNumber { get; private set; }
        /// <summary>
        /// The column, or row value, index
        /// </summary>
        public int ColumnIndex { get; private set; }
        /// <summary>
        /// The row index
        /// </summary>
        public int RowIndex { get; private set; }
        /// <summary>
        /// Whether to color the cell
        /// </summary>
        public bool ColoredCell { get; set; }
        /// <summary>
        /// The custom cell color
        /// </summary>
        public Color CellColor { get; set; } = ColorTools.NeutralTextColor;
        /// <summary>
        /// The custom background cell color
        /// </summary>
        public Color CellBackgroundColor { get; set; } = ColorTools.BackgroundColor;

        /// <summary>
        /// Makes a new instance of the cell options class
        /// </summary>
        /// <param name="ColumnNumber">The column number</param>
        /// <param name="RowNumber">The row number</param>
        public CellOptions(int ColumnNumber, int RowNumber)
        {
            this.ColumnNumber = ColumnNumber;
            this.RowNumber = RowNumber;
            ColumnIndex = ColumnNumber - 1;
            RowIndex = RowNumber - 1;
        }

    }
}
