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

using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters.Tools;

namespace KS.Misc.Writers.MiscWriters
{
    /// <summary>
    /// Welcome message writer
    /// </summary>
    public static class WelcomeMessage
    {

        private static string customBanner = "";

        /// <summary>
        /// The customized message banner to write. If none is specified, or if it only consists of whitespace, it uses the default message.
        /// </summary>
        public static string CustomBanner { get => GetCustomBanner(); set => customBanner = value; }

        /// <summary>
        /// Gets the custom banner actual text with placeholders parsed
        /// </summary>
        public static string GetCustomBanner()
        {
            // The default message to write
            string MessageWrite = "      >> " + Translate.DoTranslation("Welcome to the kernel! - Version {0}") + " <<      ";

            // Check to see if user specified custom message
            if (!string.IsNullOrWhiteSpace(customBanner))
                MessageWrite = PlaceParse.ProbePlaces(customBanner);

            // Just return the result
            return MessageWrite;
        }

        /// <summary>
        /// Writes the welcoming message to the console (welcome to kernel)
        /// </summary>
        public static void WriteMessage()
        {
            if (!Flags.EnableSplash)
            {
                ConsoleBase.ConsoleWrapper.CursorVisible = false;

                // The default message to write
                string MessageWrite = GetCustomBanner();

                // Finally, write the message
                if (Flags.StartScroll)
                {
                    TextWriterSlowColor.WriteSlowly(MessageWrite, true, 10d, ColorTools.ColTypes.Banner, KernelTools.KernelVersion.ToString());
                }
                else
                {
                    TextWriterColor.Write(MessageWrite, true, ColorTools.ColTypes.Banner, KernelTools.KernelVersion.ToString());
                }

                if (Flags.NewWelcomeStyle)
                {
                    string FigletRenderedBanner = FigletTools.RenderFiglet($"{KernelTools.KernelVersion}", KernelTools.BannerFigletFont);
                    TextWriterColor.Write(CharManager.NewLine + CharManager.NewLine + FigletRenderedBanner);
                }
                else
                {
                    // Show license
                    WriteLicense(true);
                }
                ConsoleBase.ConsoleWrapper.CursorVisible = true;
            }
        }

        /// <summary>
        /// Writes the license
        /// </summary>
        public static void WriteLicense(bool TwoNewlines)
        {
            TextWriterColor.Write(CharManager.NewLine + "    Kernel Simulator  Copyright (C) 2018-2022  Aptivi" + 
                                  CharManager.NewLine + "    This program comes with ABSOLUTELY NO WARRANTY, not even " + 
                                  CharManager.NewLine + "    MERCHANTABILITY or FITNESS for particular purposes." + 
                                  CharManager.NewLine + "    This is free software, and you are welcome to redistribute it" + 
                                  CharManager.NewLine + "    under certain conditions; See COPYING file in source code." + CharManager.NewLine, true, ColorTools.ColTypes.License);
            TextWriterColor.Write("* " + Translate.DoTranslation("For more information about the terms and conditions of using this software, visit") + " http://www.gnu.org/licenses/", true, ColorTools.ColTypes.License);
            if (TwoNewlines)
                TextWriterColor.Write();
        }

    }
}
