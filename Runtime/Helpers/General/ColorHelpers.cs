using UnityEngine;

namespace Daniell.Runtime.Helpers.General
{
    /// <summary>
    /// Collection of color related helpers
    /// </summary>
    public static class ColorHelpers
    {
        /// <summary>
        /// Convert a Color32 to an hex value
        /// </summary>
        /// <param name="color">Color to be converted to hex</param>
        /// <returns>Hex value of the color</returns>
        public static string ToHex(this Color32 color)
        {
            return $"{color.r:X2}{color.g:X2}{color.b:X2}";
        }

        /// <summary>
        /// Convert a Color to an hex value
        /// </summary>
        /// <param name="color">Color to be converted to hex</param>
        /// <returns>Hex value of the color</returns>
        public static string ToHex(this UnityEngine.Color color)
        {
            return ((Color32)color).ToHex();
        }
    }
}