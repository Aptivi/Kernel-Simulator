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

using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Files.Paths;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Textify.Versioning;

namespace KS.Modifications.Dependencies
{
    internal static class ModDependencySatisfier
    {
        internal static ModDependency[] GetDependencies(ModInfo mod)
        {
            if (mod is null)
                return [];

            // Get the mod directory
            string modDirectory = PathsManagement.GetKernelPath(KernelPathType.Mods);

            // Get the mod dependency metadata
            string metadataPath = $"{modDirectory}{Path.GetFileNameWithoutExtension(mod.ModFilePath)}-moddeps.json";
            DebugWriter.WriteDebug(DebugLevel.I, "Metadata path: {0}", metadataPath);
            if (!Checking.FileExists(metadataPath))
                return [];

            // Parse it and return all dependencies
            ModDependency[] deps = JsonConvert.DeserializeObject<ModDependency[]>(Reading.ReadContentsText(metadataPath));
            DebugWriter.WriteDebug(DebugLevel.I, "Initial dep count: {0}", deps.Length);
            List<ModDependency> finalDeps = [];
            foreach (ModDependency dep in deps)
            {
                // Guesstimate the path to the mod dependency file
                dep.modPath = $"{modDirectory}{dep.ModName}.dll";

                // Parse the name
                DebugWriter.WriteDebug(DebugLevel.I, "Dependent mod name: {0}", dep.ModName);
                if (string.IsNullOrEmpty(dep.ModName))
                    continue;

                // Parse the version
                DebugWriter.WriteDebug(DebugLevel.I, "Dependent mod version: {0}", dep.ModVersion);
                if (string.IsNullOrEmpty(dep.ModVersion))
                {
                    // The mod that depends on this mod accepts all versions.
                    finalDeps.Add(dep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Added");
                }
                else
                {
                    try
                    {
                        // Do the job!
                        var version = SemVer.Parse(dep.ModVersion);
                        DebugWriter.WriteDebug(DebugLevel.I, "Parsed version as {0}", version.ToString());
                        finalDeps.Add(dep);
                        DebugWriter.WriteDebug(DebugLevel.I, "Added");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Failed to add dependency: {0}", ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }
            }

            // Return the final list
            return [.. finalDeps];
        }

        internal static void SatisfyDependencies(ModInfo mod)
        {
            // Get the dependencies
            var deps = GetDependencies(mod);
            DebugWriter.WriteDebug(DebugLevel.I, "Got {0} dependencies to satisfy.", deps.Length);

            // Load the dependencies
            List<string> failedDeps = [];
            foreach (var dep in deps)
            {
                // To avoid circular dependency
                if (ModParser.queued.Contains(dep.modPath))
                    continue;

                // Try to parse the mod dependency
                try
                {
                    // Check to see if it's already loaded
                    if (ModManager.Mods.Values.Select((mi) => mi.ModFilePath).Any())
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Mod {0} v{1}: {2} already loaded!", dep.ModName, dep.ModVersion, dep.modPath);
                        continue;
                    }

                    // Parse it
                    DebugWriter.WriteDebug(DebugLevel.I, "Parsing mod {0} v{1}: {2}", dep.ModName, dep.ModVersion, dep.modPath);
                    ModParser.ParseMod(dep.ModName + ".dll");

                    // Now, check it
                    if (!ModManager.Mods.Values.Select((mi) => mi.ModFilePath).Any())
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Looks like that mod {0} v{1}: {2} didn't load successfully.", dep.ModName, dep.ModVersion, dep.modPath);
                        failedDeps.Add(dep.ModName);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed. Mod {0} v{1}: {2} didn't load successfully: {3}", dep.ModName, dep.ModVersion, dep.modPath, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    failedDeps.Add(dep.ModName);
                }
            }

            // Check for failed deps
            if (failedDeps.Count > 0)
                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("Dependency unsatisfied. Failed dependencies are listed below:") + $"\n  - {string.Join("\n  - ", failedDeps)}");
        }
    }
}
