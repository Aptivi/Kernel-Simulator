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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using System;

namespace Nitrocid.Extras.Calculators.Commands
{
    /// <summary>
    /// This command will show information about the imaginary number formula specified by a specified real and imaginary number.
    /// </summary>
    /// <remarks>
    /// This command can be used to get information about the imaginary number formula specified by a specified real and imaginary number. It shows the formula in
    /// the following format:<code>Z = Re + Im i</code>
    /// </remarks>
    class ImaginaryCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check both the real and the imaginary numbers for verification
            if (!double.TryParse(parameters.ArgumentsList[0], out double Real))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The real number is not valid."), true, KernelColorType.Error);
                return 2;
            }
            if (!double.TryParse(parameters.ArgumentsList[1], out double Imaginary))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The imaginary number is not valid."), true, KernelColorType.Error);
                return 2;
            }

            // Print the Z formula, first of all.
            TextWriterColor.Write(Translate.DoTranslation("Formula:") + " Z = {0} + {1}i", Real, Imaginary);

            // Process the radius
            double Radius = Math.Sqrt(Math.Pow(Real, 2) + Math.Pow(Imaginary, 2));
            TextWriterColor.Write(Translate.DoTranslation("Radius:") + " {0}", Radius);

            // Determine the cosine (Real to Radius) and the sine (Imaginary to Radius)
            double RealRadius = Math.Cos(Real / Radius);
            double ImaginaryRadius = Math.Sin(Imaginary / Radius);
            TextWriterColor.Write(Translate.DoTranslation("Real to Radius in Radians:") + " {0} rad", RealRadius);
            TextWriterColor.Write(Translate.DoTranslation("Imaginary to Radius in Radians:") + " {0} rad", ImaginaryRadius);

            // Try to find out the angle from the given cos/sin values
            double AngleCos = Math.Acos(RealRadius);
            double AngleSin = Math.Asin(ImaginaryRadius);
            TextWriterColor.Write(Translate.DoTranslation("Angle in Radians:") + " {0} rad, {1} rad", AngleCos, AngleSin);

            // Now, write the result in both the exponentional format (Z = r * (e)^{angle}i)
            //                           and the triangular format    (Z = r (cos {angle} + i sin {angle})
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Exponential:") + " Z = {0} * (e)^{1}i", true, KernelColorType.Success, Radius, AngleSin);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Triangular:") + " Z = {0} * (cos ({1}) + i sin ({2}))", true, KernelColorType.Success, Radius, AngleCos, AngleSin);
            return 0;
        }
    }
}
