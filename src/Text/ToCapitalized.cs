using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
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
        public static string ToCapitalizeFirst(this string value)
        {
            return value.First().ToString().ToUpper() + string.Join(null, value.Skip(1));
        }
    }
}
