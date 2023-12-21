﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using KS.Files;

// Kernel Simulator  Copyright (C) 2018-2019  Aptivi
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

using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Writers.DebugWriters;

namespace KS.ManPages
{
	static class PageParser
	{

		/// <summary>
        /// Initializes a manual page
        /// </summary>
        /// <param name="ManualFile">A manual file path (neutralized)</param>
		public static void InitMan(string ManualFile)
		{
			ManualFile = Filesystem.NeutralizePath(ManualFile);
			if (Checking.FileExists(ManualFile))
			{
				// File found, but we need to verify that we're actually dealing with the manual page
				if (Path.GetExtension(ManualFile) == ".man")
				{
					// We found the manual, but we need to check its contents.
					DebugWriter.Wdbg(DebugLevel.I, "Found manual page {0}.", ManualFile);
					DebugWriter.Wdbg(DebugLevel.I, "Parsing manpage...");
					var ManualInstance = new Manual(ManualFile);
					PageManager.AddManualPage(ManualInstance.Title, ManualInstance);
				}
			}
		}

		/// <summary>
        /// Checks to see if the manual page is valid
        /// </summary>
        /// <param name="ManualFile">A manual file path (neutralized)</param>
        /// <param name="ManTitle">Manual title to install to the new manual class instance</param>
        /// <param name="ManRevision">Manual revision to install to the new manual class instance</param>
        /// <param name="Body">Body to install to the new manual class instance</param>
        /// <param name="Todos">Todo list to install to the new manual class instance</param>
		internal static bool CheckManual(string ManualFile, ref string ManTitle, ref string ManRevision, ref StringBuilder Body, ref List<string> Todos)
		{
			bool Success = true;
			try
			{
				bool InternalParseDone = false;
				ManualFile = Filesystem.NeutralizePath(ManualFile);
				DebugWriter.Wdbg(DebugLevel.I, "Current manual file: {0}", ManualFile);

				// First, get all lines in the file
				string[] ManLines = File.ReadAllLines(ManualFile);
				var BodyParsing = default(bool);
				foreach (string ManLine in ManLines)
				{
					// Check for the rest if the manpage has MAN START section
					if (InternalParseDone)
					{
						// Check for the TODOs
						string TodoConstant = "TODO";
						if (ManLine.StartsWith("~~-") & ManLine.Contains(TodoConstant))
						{
							DebugWriter.Wdbg(DebugLevel.I, "TODO found on this line: {0}", ManLine);
							Todos.Add(ManLine);
						}

						// Check the manual metadata
						string RevisionConstant = "-REVISION:";
						string TitleConstant = "-TITLE:";
						string BodyStartConstant = "-BODY START-";
						string BodyEndConstant = "-BODY END-";

						// Check the body or manual metadata
						if (!ManLine.StartsWith("~~-"))
						{
							if (BodyParsing)
							{
								// If we're not at the end of the body
								if ((ManLine ?? "") != (BodyEndConstant ?? ""))
								{
									if (!string.IsNullOrWhiteSpace(ManLine))
										DebugWriter.Wdbg(DebugLevel.I, "Appending {0} to builder", ManLine);
									Body.AppendLine(PlaceParse.ProbePlaces(ManLine));
								}
								else
								{
									// Stop parsing the body
									BodyParsing = false;
								}
							}
							// Check for constants
							else if (ManLine.StartsWith(RevisionConstant))
							{
								// Found the revision constant
								DebugWriter.Wdbg(DebugLevel.I, "Revision found on this line: {0}", ManLine);
								string Rev = ManLine.Substring(RevisionConstant.Length);
								if (string.IsNullOrWhiteSpace(Rev))
								{
									DebugWriter.Wdbg(DebugLevel.W, "Revision not defined. Assuming v1...");
									Rev = "1";
								}
								ManRevision = Rev;
							}
							else if (ManLine.StartsWith(TitleConstant))
							{
								// Found the title constant
								DebugWriter.Wdbg(DebugLevel.I, "Title found on this line: {0}", ManLine);
								string Title = ManLine.Substring(TitleConstant.Length);
								if (string.IsNullOrWhiteSpace(Title))
								{
									DebugWriter.Wdbg(DebugLevel.W, "Title not defined.");
									Title = $"Untitled ({PageManager.Pages.Count})";
								}
								ManTitle = Title;
							}
							else if (ManLine == "-BODY START-")
							{
								BodyParsing = true;
							}
						}
					}

					// Check to see if the manual starts with (*MAN START*) header
					if (ManLine == "(*MAN START*)")
					{
						DebugWriter.Wdbg(DebugLevel.I, "Successfully found (*MAN START*) in manpage {0}.", ManualFile);
						InternalParseDone = true;
					}
				}

				// Check for body
				if (InternalParseDone)
				{
					DebugWriter.Wdbg(DebugLevel.I, "Valid manual page! ({0})", ManualFile);
					if (string.IsNullOrWhiteSpace(Body.ToString()))
					{
						DebugWriter.Wdbg(DebugLevel.W, "Body for \"{0}\" does not contain anything.", ManualFile);
						Body.AppendLine(Translate.DoTranslation("Consider filling this manual page."));
					}
				}
				else
				{
					throw new Kernel.Exceptions.InvalidManpageException(Translate.DoTranslation("The manual page {0} is invalid."), ManualFile);
				}
			}
			catch (Exception ex)
			{
				Success = false;
				DebugWriter.Wdbg(DebugLevel.E, "The manual page {0} is invalid. {1}", ManTitle, ex.Message);
				DebugWriter.WStkTrc(ex);
			}
			return Success;
		}

	}
}