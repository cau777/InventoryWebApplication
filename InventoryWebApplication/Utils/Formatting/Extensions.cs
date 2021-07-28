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

        public static string Capitalize(this string o)
        {
            StringBuilder builder = new StringBuilder(o);
            builder[0] = char.ToUpper(builder[0]);

            return builder.ToString();
        }


        public static string JoinArray<T>(this string o, [NotNull] T[] words)
        {
            StringBuilder joined = new StringBuilder();

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
    }
}