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

namespace KS.Network.Transfer
{
    public class NetworkTransferInfo
    {

        private bool _MessageSuppressed;

        /// <summary>
        /// How many bytes downloaded/uploaded
        /// </summary>
        public long DoneSize { get; private set; }
        /// <summary>
        /// File size
        /// </summary>
        public long FileSize { get; private set; }
        /// <summary>
        /// The transfer type
        /// </summary>
        public NetworkTransferType TransferType { get; private set; }
        /// <summary>
        /// Whether the message is suppressed. Once set, it can't be unset.
        /// </summary>
        /// <returns></returns>
        public bool MessageSuppressed
        {
            get
            {
                return _MessageSuppressed;
            }
            set
            {
                if (!_MessageSuppressed)
                    _MessageSuppressed = value;
            }
        }

        protected internal NetworkTransferInfo(long DoneSize, long FileSize, NetworkTransferType TransferType)
        {
            this.DoneSize = DoneSize;
            this.FileSize = FileSize;
            this.TransferType = TransferType;
        }

    }
}