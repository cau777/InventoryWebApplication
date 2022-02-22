using System;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace InventoryWebApplication.Utils.Formatting
{
    public static class Extensions
    {
        public static string ToTitle(this string o)
        {
            StringBuilder builder = new(o);
            builder[0] = char.ToUpper(builder[0]);

            for (int i = 1; i < builder.Length; i++)
                if (builder[i - 1] == ' ')
                    builder[i] = char.ToUpper(builder[i]);

            return builder.ToString();
        }

        /// <summary>
        ///     Makes the first letter of the string capital
        /// </summary>
        /// <param name="o">The string to be capitalized</param>
        /// <returns>A string with the first letter capital</returns>
        public static string Capitalize(this string o)
        {
            return o.Length switch
            {
                0 => "",
                1 => char.ToUpper(o[0]).ToString(),
                _ => char.ToUpper(o[0]) + o[1..]
            };
        }

        public static string JoinArray<T>(this string o, [NotNull] T[] words)
        {
            StringBuilder joined = new();

            for (int x = 0; x < words.Length - 1; x++)
            {
                joined.Append(words[x]);
                joined.Append(o);
            }

            return joined + words.Last().ToString();
        }

        public static string RemovePrefix(this string o, string prefix)
        {
            return o.StartsWith(prefix) ? o.Substring(prefix.Length) : o;
        }

        public static string RemoveSuffix(this string o, string suffix)
        {
            return o.EndsWith(suffix) ? o.Substring(0, o.Length - suffix.Length) : o;
        }

        /// <summary>
        ///     Cuts a string grater than a certain length and appends "..."
        /// </summary>
        /// <param name="str">The string to be cut</param>
        /// <param name="max">The maximum length of the string</param>
        /// <returns>A string not longer than max</returns>
        public static string CutAfter(this string str, int max)
        {
            if (str.Length < max)
                return str;

            return str[..(max - 3)] + "...";
        }

        public static string FormatShortDate(this DateTime date) =>
            date.ToString(DateTimeFormatInfo.InvariantInfo.ShortDatePattern);
    }
}