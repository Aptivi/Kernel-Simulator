﻿using System;
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

using System.IO;
using System.Reflection;
using KS.Files;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Reflection
{
    public static class AssemblyLookup
    {

        private readonly static List<string> AssemblyLookupPaths = new List<string>();

        /// <summary>
        /// Adds the path pointing to the dependencies to the assembly search path
        /// </summary>
        /// <param name="Path">Path to the dependencies</param>
        public static void AddPathToAssemblySearchPath(string Path)
        {
            Path = Filesystem.NeutralizePath(Path);

            // Add the path to the search path
            if (!AssemblyLookupPaths.Contains(Path))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Adding path {0} to lookup paths...", Path);
                AssemblyLookupPaths.Add(Path);
            }
        }

        /// <summary>
        /// Loads assembly from the search paths
        /// </summary>
        /// <returns>If successful, returns the assembly instance. Otherwise, null.</returns>
        internal static Assembly LoadFromAssemblySearchPaths(object sender, ResolveEventArgs args)
        {
            Assembly FinalAssembly = null;
            string DepAssemblyName = new AssemblyName(args.Name).Name;
            DebugWriter.Wdbg(DebugLevel.I, "Requested to load {0}.", args.Name);

            // Try to load assembly from lookup path
            foreach (string LookupPath in AssemblyLookupPaths)
            {
                string DepAssemblyFilePath = Path.Combine(LookupPath, DepAssemblyName + ".dll");
                try
                {
                    // Try loading
                    DebugWriter.Wdbg(DebugLevel.I, "Loading from {0}...", DepAssemblyFilePath);
                    FinalAssembly = Assembly.LoadFrom(DepAssemblyFilePath);
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to load {0} from {1}: {2}", args.Name, DepAssemblyFilePath, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    DebugWriter.Wdbg(DebugLevel.E, "Trying another path...");
                }
            }

            // Get the final assembly
            return FinalAssembly;
        }

    }
}