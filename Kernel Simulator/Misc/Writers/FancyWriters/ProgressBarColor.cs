﻿using System;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Colors;

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

using TermProgress = Terminaux.Writer.FancyWriters.ProgressBarColor;

namespace KS.Misc.Writers.FancyWriters
{
	public static class ProgressBarColor
	{

		/// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
		public static void WriteProgressPlain(double Progress, int Left, int Top)
		{
			try
			{
				TermProgress.WriteProgressPlain(Progress, Left, Top);
			}
			catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
			{
				DebugWriter.WStkTrc(ex);
				KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
			}
		}

		/// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
		public static void WriteProgress(double Progress, int Left, int Top)
		{
			try
			{
				WriteProgress(Progress, Left, Top, KernelColorTools.ColTypes.Progress, KernelColorTools.ColTypes.Gray);
			}
			catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
			{
				DebugWriter.WStkTrc(ex);
				KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
			}
		}

		/// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
		public static void WriteProgress(double Progress, int Left, int Top, KernelColorTools.ColTypes ProgressColor)
		{
			try
			{
				WriteProgress(Progress, Left, Top, ProgressColor, KernelColorTools.ColTypes.Gray);
			}
			catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
			{
				DebugWriter.WStkTrc(ex);
				KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
			}
		}

		/// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
		public static void WriteProgress(double Progress, int Left, int Top, KernelColorTools.ColTypes ProgressColor, KernelColorTools.ColTypes FrameColor)
		{
			try
			{
				TermProgress.WriteProgress(Progress, Left, Top, KernelColorTools.GetConsoleColor(ProgressColor), KernelColorTools.GetConsoleColor(FrameColor));
			}
			catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
			{
				DebugWriter.WStkTrc(ex);
				KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
			}
		}

		/// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
		public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor)
		{
			try
			{
				WriteProgress(Progress, Left, Top, ProgressColor, KernelColorTools.GetGray());
			}
			catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
			{
				DebugWriter.WStkTrc(ex);
				KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
			}
		}

		/// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
		public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor)
		{
			try
			{
				TermProgress.WriteProgress(Progress, Left, Top, ProgressColor, FrameColor);
			}
			catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
			{
				DebugWriter.WStkTrc(ex);
				KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
			}
		}

	}
}