using System.Linq;
using JetBrains.Annotations;

namespace AD.IO.Standard
{
    /// <summary>
    /// Extension method providing capitalization utilities.
    /// </summary>
    [PublicAPI]
    public static class ToCapitalizedExtensions
    {
        /// <summary>
        /// Capitalizes the first character of the string.
        /// </summary>
        /// <param name="value">The string to be capitalized.</param>
        /// <returns>The input string with its first character converted to its uppercase equivalent.</returns>
        [CanBeNull]
        [Pure]
        public static string ToCapitalizeFirst(this string value)
        {
            char[] result = value.ToCharArray();
            result[0] = value.ToUpper()[0];
            return string.Join(null, result);
        }

        /// <summary>
        /// Capitalizes the first letter character following each period of the string.
        /// </summary>
        /// <param name="value">The string to be capitalized.</param>
        /// <returns>The input string with each first character following a period converted to its uppercase equivalent.</returns>
        [CanBeNull]
        [Pure]
        public static string ToCapitalizeAllFirst(this string value)
        {
            char[] result = value.ToCharArray();
            bool start = true;
            for (int i = 0; i < result.Length; i++)
            {
                switch (result[i])
                {
                    default:
                    {
                        if (!start)
                        {
                            continue;
                        }
                        if (!char.IsLetter(result[i]))
                        {
                            continue;
                        }
                        result[i] = value.ToUpper()[i];
                        start = false;
                        continue;
                    }
                    case '.':
                    {
                        start = true;
                        continue;
                    }
                }
            }
            return string.Join(null, result);
        }
    }
}
