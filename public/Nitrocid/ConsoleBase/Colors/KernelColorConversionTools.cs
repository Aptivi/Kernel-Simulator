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

using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using System;
using Terminaux.Colors;

namespace KS.ConsoleBase.Colors
{
    /// <summary>
    /// Kernel color conversion tools
    /// </summary>
    public static class KernelColorConversionTools
    {
        #region From hex to...
        /// <summary>
        /// Converts from the hexadecimal representation of a color to the RGB sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHexToRGB(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex[1..], 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                return $"{R};{G};{B}";
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the CMYK sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHexToCmyk(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex[1..], 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                var cmyk = new Color(R, G, B).CMYK;
                var cmy = cmyk.CMY;
                string cmykSeq = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMYK: {0})", cmykSeq);
                return cmykSeq;
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the HSL sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromHexToHsl(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex[1..], 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                var hsl = new Color(R, G, B).HSL;
                string hslSeq = $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSL: {0})", hslSeq);
                return hslSeq;
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }
        #endregion

        #region From RGB to...
        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(int R, int G, int B)
        {
            if (R < 0 | R > 255)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid red color specifier."));
            if (G < 0 | G > 255)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid green color specifier."));
            if (B < 0 | B > 255)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid blue color specifier."));
            string hex = $"#{R:X2}{G:X2}{B:X2}";
            DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
            return hex;
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(string RGBSequence)
        {
            if (RGBSequence.Contains(';'))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    string hex = $"#{R:X2}{G:X2}{B:X2}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                    return hex;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromRGBToCmyk(int R, int G, int B) =>
            ConvertFromRGBToCmyk($"{R};{G};{B}");

        /// <summary>
        /// Converts from the RGB sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromRGBToCmyk(string RGBSequence)
        {
            if (RGBSequence.Contains(';'))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                    var cmyk = new Color(R, G, B).CMYK;
                    var cmy = cmyk.CMY;
                    string cmykSeq = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMYK: {0})", cmykSeq);
                    return cmykSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromRGBToHsl(int R, int G, int B) =>
            ConvertFromRGBToHsl($"{R};{G};{B}");

        /// <summary>
        /// Converts from the RGB sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromRGBToHsl(string RGBSequence)
        {
            if (RGBSequence.Contains(';'))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                    var hsl = new Color(R, G, B).HSL;
                    string hslSeq = $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSL: {0})", hslSeq);
                    return hslSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
            }
        }
        #endregion

        #region From CMYK to...
        /// <summary>
        /// Converts from the CMYK sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromCmykToHex(int C, int M, int Y, int K)
        {
            if (C < 0 | C > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid cyan color specifier."));
            if (M < 0 | M > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid magenta color specifier."));
            if (Y < 0 | Y > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid yellow color specifier."));
            if (K < 0 | K > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid black key specifier."));
            var rgb = new Color($"cmyk:{C};{M};{Y};{K}");
            string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
            DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
            return hex;
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromCmykToHex(string CMYKSequence)
        {
            if (CMYKSequence.Contains(';') && CMYKSequence.StartsWith("cmyk:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYKSequence[5..].Split(';');
                if (ColorSpecifierArray.Length == 4)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    int K = Convert.ToInt32(ColorSpecifierArray[3]);
                    var rgb = new Color($"cmyk:{C};{M};{Y};{K}");
                    string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                    return hex;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmykToRgb(int C, int M, int Y, int K) =>
            ConvertFromCmykToRgb($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmykToRgb(string CMYKSequence)
        {
            if (CMYKSequence.Contains(';') && CMYKSequence.StartsWith("cmyk:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYKSequence[5..].Split(';');
                if (ColorSpecifierArray.Length == 4)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    int K = Convert.ToInt32(ColorSpecifierArray[3]);
                    var rgb = new Color($"cmyk:{C};{M};{Y};{K}");
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RGB: {0})", rgb.PlainSequenceTrueColor);
                    return rgb.PlainSequenceTrueColor;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromCmykToHsl(int C, int M, int Y, int K) =>
            ConvertFromCmykToHsl($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromCmykToHsl(string CMYKSequence)
        {
            if (CMYKSequence.Contains(';') && CMYKSequence.StartsWith("cmyk:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYKSequence[5..].Split(';');
                if (ColorSpecifierArray.Length == 4)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    int K = Convert.ToInt32(ColorSpecifierArray[3]);
                    var rgb = new Color($"cmyk:{C};{M};{Y};{K}");
                    var hsl = rgb.HSL;
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSL: hsl:{0};{1};{2})", hsl.HueWhole, hsl.SaturationWhole, hsl.LightnessWhole);
                    return $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }
        #endregion

        #region From HSL to...
        /// <summary>
        /// Converts from the HSL sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromHslToHex(int H, int S, int L)
        {
            if (H < 0 | H > 360)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid color hue specifier. Make sure that you specify the hue as degrees, not as radians."));
            if (S < 0 | S > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid saturation color specifier."));
            if (L < 0 | L > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid lightness color specifier."));
            var rgb = new Color($"hsl:{H};{S};{L}");
            string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
            DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
            return hex;
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromHslToHex(string HSLSequence)
        {
            if (HSLSequence.Contains(';') && HSLSequence.StartsWith("hsl:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSLSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int L = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsl:{H};{S};{L}");
                    string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                    return hex;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHslToCmyk(int H, int S, int L) =>
            ConvertFromHslToCmyk($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHslToCmyk(string HSLSequence)
        {
            if (HSLSequence.Contains(';') && HSLSequence.StartsWith("hsl:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSLSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int L = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsl:{H};{S};{L}");
                    var cmyk = rgb.CMYK;
                    var cmy = cmyk.CMY;
                    string cmykSeq = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMYK: {0})", cmykSeq);
                    return cmykSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHslToRgb(int H, int S, int L) =>
            ConvertFromHslToRgb($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHslToRgb(string HSLSequence)
        {
            if (HSLSequence.Contains(';') && HSLSequence.StartsWith("hsl:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSLSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int L = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsl:{H};{S};{L}");
                    var hsl = rgb;
                    string hslSeq = $"{hsl.R};{hsl.G};{hsl.B}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RGB: {0})", hslSeq);
                    return hslSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }
        #endregion
    }
}
