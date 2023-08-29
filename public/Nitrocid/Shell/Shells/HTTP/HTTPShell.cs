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

using System;
using System.Net.Http;
using System.Threading;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Network.Base.Connections;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.HTTP
{
    /// <summary>
    /// The HTTP shell
    /// </summary>
    public class HTTPShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "HTTPShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            NetworkConnection httpConnection = (NetworkConnection)ShellArgs[0];
            HTTPShellCommon.clientConnection = httpConnection;
            HTTPShellCommon.HTTPSite = httpConnection.ConnectionUri.OriginalString;

            while (!Bail)
            {
                try
                {
                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.HTTPShell, Translate.DoTranslation("There was an error in the HTTP shell:") + " {0}", ex, ex.Message);
                }

                // Exiting, so reset the site
                if (Bail)
                {
                    if (!detaching)
                    {
                        ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance)?.Dispose();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(HTTPShellCommon.ClientHTTP);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        HTTPShellCommon.clientConnection = null;
                    }
                    detaching = false;
                    HTTPShellCommon.HTTPSite = "";
                }
            }
        }

    }
}
