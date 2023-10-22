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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using KS.Files.Operations;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.Shells.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Files.Editors.JsonShell
{
    /// <summary>
    /// JSON shell tools
    /// </summary>
    public static class JsonTools
    {

        /// <summary>
        /// Opens the JSON file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="FilesystemTools.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool OpenJsonFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", File);
                JsonShellCommon.FileStream = new FileStream(File, FileMode.Open);
                var JsonFileReader = new StreamReader(JsonShellCommon.FileStream);
                string JsonFileContents = Reading.ReadToEndAndSeek(ref JsonFileReader);
                JsonShellCommon.FileToken = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                JsonShellCommon.FileTokenOrig = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", File, JsonShellCommon.FileStream.Length, JsonShellCommon.FileStream.Position);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Open file {0} failed: {1}", File, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Closes text file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool CloseTextFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                JsonShellCommon.FileStream.Close();
                JsonShellCommon.FileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                JsonShellCommon.FileToken = JToken.Parse("{}");
                JsonShellCommon.FileTokenOrig = JToken.Parse("{}");
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Closing file failed: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Saves JSON file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SaveFile(bool ClearJson) => SaveFile(ClearJson, JsonShellCommon.Formatting);

        /// <summary>
        /// Saves JSON file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SaveFile(bool ClearJson, Formatting Formatting)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                JsonShellCommon.FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                var FileLinesByte = Encoding.Default.GetBytes(JsonConvert.SerializeObject(JsonShellCommon.FileToken, Formatting));
                DebugWriter.WriteDebug(DebugLevel.I, "Converted lines to bytes. Length: {0}", FileLinesByte.Length);
                JsonShellCommon.FileStream.Write(FileLinesByte, 0, FileLinesByte.Length);
                JsonShellCommon.FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                if (ClearJson)
                {
                    JsonShellCommon.FileToken = JToken.Parse("{}");
                }
                JsonShellCommon.FileTokenOrig = JToken.Parse("{}");
                JsonShellCommon.FileTokenOrig = JsonShellCommon.FileToken;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Saving file failed: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Handles autosave
        /// </summary>
        public static void HandleAutoSaveJsonFile()
        {
            if (JsonShellCommon.AutoSaveFlag)
            {
                try
                {
                    Thread.Sleep(JsonShellCommon.AutoSaveInterval * 1000);
                    if (JsonShellCommon.FileStream is not null)
                    {
                        SaveFile(false);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

        /// <summary>
        /// Was JSON edited?
        /// </summary>
        public static bool WasJsonEdited() => !JToken.DeepEquals(JsonShellCommon.FileToken, JsonShellCommon.FileTokenOrig);

        /// <summary>
        /// Gets a property in the JSON file
        /// </summary>
        /// <param name="Property">The property. You can use JSONPath.</param>
        public static JToken GetProperty(string Property)
        {
            if (JsonShellCommon.FileStream is not null)
            {
                var TargetToken = JsonShellCommon.FileToken.SelectToken(Property);
                if (TargetToken is not null)
                {
                    return TargetToken;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The property inside the JSON file isn't found."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Gets a property in the JSON file. It returns null if not found.
        /// </summary>
        /// <param name="ParentProperty">Where is the target property found?</param>
        /// <param name="Property">The property. You can use JSONPath.</param>
        public static JToken GetPropertySafe(string ParentProperty, string Property)
        {
            if (JsonShellCommon.FileStream is not null)
            {
                var TargetToken = GetProperty(ParentProperty);
                TargetToken = TargetToken.SelectToken(Property);
                if (TargetToken is not null)
                {
                    return TargetToken;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Adds a new object to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where is the target array found?</param>
        /// <param name="Key">Name of property containing the array</param>
        /// <param name="Value">The new value</param>
        public static void AddNewObject(string ParentProperty, string Key, JToken Value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetProperty(ParentProperty);
            JToken PropertyToken = TargetToken[Key];

            // Check to see if we're dealing with the array
            if (PropertyToken.Type == JTokenType.Array)
                ((JArray)PropertyToken).Add(Value);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Adds a new object to the current JSON file by index
        /// </summary>
        /// <param name="ParentProperty">Where is the target array found?</param>
        /// <param name="Index">Index number of an array to add the value to</param>
        /// <param name="Value">The new value</param>
        public static void AddNewObjectIndexed(string ParentProperty, int Index, JToken Value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetProperty(ParentProperty);
            JToken PropertyToken = TargetToken.ElementAt(Index);

            // Check to see if we're dealing with the array
            if (PropertyToken.Type == JTokenType.Array)
                ((JArray)PropertyToken).Add(Value);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Adds a new property to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where to place the new property?</param>
        /// <param name="Key">New property</param>
        /// <param name="Value">The value for the new property</param>
        public static void AddNewProperty(string ParentProperty, string Key, JToken Value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetProperty(ParentProperty);
            JObject TokenObject = (JObject)TargetToken;
            TokenObject.Add(Key, Value);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Adds a new array to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where to place the new array?</param>
        /// <param name="Key">New array</param>
        /// <param name="Values">The values for the new array</param>
        public static void AddNewArray(string ParentProperty, string Key, JArray Values)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetProperty(ParentProperty);
            JObject TokenObject = (JObject)TargetToken;
            TokenObject.Add(Key, Values);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Removes a property from the current JSON file
        /// </summary>
        /// <param name="Property">The property. You can use JSONPath.</param>
        public static void RemoveProperty(string Property)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetProperty(Property);
            TargetToken.Parent.Remove();
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Removes an object from the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where is the target object found?</param>
        /// <param name="ObjectName">The object name. You can use JSONPath.</param>
        public static void RemoveObject(string ParentProperty, string ObjectName)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetProperty(ParentProperty);
            JToken PropertyToken = TargetToken[ObjectName];
            PropertyToken.Remove();
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Removes an object from the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where is the target object found?</param>
        /// <param name="Index">Index number of an array to add the value to</param>
        public static void RemoveObjectIndexed(string ParentProperty, int Index)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetProperty(ParentProperty);
            JToken PropertyToken = TargetToken.ElementAt(Index);
            PropertyToken.Remove();
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Serializes the property to the string
        /// </summary>
        /// <param name="Property">The property. You can use JSONPath.</param>
        public static string SerializeToString(string Property)
        {
            var TargetToken = GetProperty(Property);
            return JsonConvert.SerializeObject(TargetToken, Formatting.Indented);
        }

        /// <summary>
        /// Beautifies the JSON text contained in the file.
        /// </summary>
        /// <param name="JsonFile">Path to JSON file. It's automatically neutralized using <see cref="FilesystemTools.NeutralizePath(string, bool)"/>.</param>
        /// <returns>Beautified JSON</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string BeautifyJson(string JsonFile)
        {
            // Neutralize the file path
            DebugWriter.WriteDebug(DebugLevel.I, "Neutralizing json file {0}...", JsonFile);
            JsonFile = FilesystemTools.NeutralizePath(JsonFile, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got json file {0}...", JsonFile);

            // Try to beautify JSON
            string JsonFileContents = Reading.ReadContentsText(JsonFile);
            return BeautifyJsonText(JsonFileContents);
        }

        /// <summary>
        /// Beautifies the JSON text.
        /// </summary>
        /// <param name="JsonText">Contents of a minified JSON.</param>
        /// <returns>Beautified JSON</returns>
        public static string BeautifyJsonText(string JsonText)
        {
            // Make an instance of JToken with this text
            var JsonToken = JToken.Parse(JsonText);
            DebugWriter.WriteDebug(DebugLevel.I, "Created a token with text length of {0}", JsonText.Length);

            // Beautify JSON
            string BeautifiedJson = JsonConvert.SerializeObject(JsonToken, Formatting.Indented);
            DebugWriter.WriteDebug(DebugLevel.I, "Beautified the JSON text. Length: {0}", BeautifiedJson.Length);
            return BeautifiedJson;
        }

        /// <summary>
        /// Minifies the JSON text contained in the file.
        /// </summary>
        /// <param name="JsonFile">Path to JSON file. It's automatically neutralized using <see cref="FilesystemTools.NeutralizePath(string, bool)"/>.</param>
        /// <returns>Minified JSON</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string MinifyJson(string JsonFile)
        {
            // Neutralize the file path
            DebugWriter.WriteDebug(DebugLevel.I, "Neutralizing json file {0}...", JsonFile);
            JsonFile = FilesystemTools.NeutralizePath(JsonFile, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got json file {0}...", JsonFile);

            // Try to minify JSON
            string JsonFileContents = Reading.ReadContentsText(JsonFile);
            return MinifyJsonText(JsonFileContents);
        }

        /// <summary>
        /// Minifies the JSON text.
        /// </summary>
        /// <param name="JsonText">Contents of a beautified JSON.</param>
        /// <returns>Minified JSON</returns>
        public static string MinifyJsonText(string JsonText)
        {
            // Make an instance of JToken with this text
            var JsonToken = JToken.Parse(JsonText);
            DebugWriter.WriteDebug(DebugLevel.I, "Created a token with text length of {0}", JsonText.Length);

            // Minify JSON
            string MinifiedJson = JsonConvert.SerializeObject(JsonToken);
            DebugWriter.WriteDebug(DebugLevel.I, "Minified the JSON text. Length: {0}", MinifiedJson.Length);
            return MinifiedJson;
        }

    }
}
