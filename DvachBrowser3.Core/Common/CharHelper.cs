using System;
using System.Linq;
using System.Text;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник для символов.
    /// </summary>
    public static class CharHelper
    {
        /// <summary>
        /// Перевести массив символов в строку (для UTF-32).
        /// </summary>
        /// <param name="chars">Массив символов.</param>
        /// <returns>Строка.</returns>
        public static string CharArrToString(params int[] chars)
        {
            return chars.Aggregate(new StringBuilder(), (sb, i) => sb.Append(ConvertFromUtf32(i))).ToString();
        }

        /// <summary>
        /// Конвертировать из UTF32.
        /// </summary>
        /// <param name="utf32">UTF32.</param>
        /// <returns>Строка.</returns>
        public static string ConvertFromUtf32(int utf32)
        {
            if (((utf32 < 0) || (utf32 > 0x10ffff)) || ((utf32 >= 0xd800) && (utf32 <= 0xdfff)))
            {
                throw new ArgumentOutOfRangeException("utf32");
            }
            if (utf32 < 0x10000)
            {
                return new string((char)utf32, 1);
            }
            utf32 -= 0x10000;
            return new string(new char[] { (char)((utf32 / 0x400) + 0xd800), (char)((utf32 % 0x400) + 0xdc00) });
        }         
    }
}