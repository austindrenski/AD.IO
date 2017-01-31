using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Parses <see cref="IEnumerator{Char}"/> as numbers.
    /// </summary>
    [PublicAPI]
    public static class ParseExtensions
    {
        /// <summary>
        /// Filters an enumerable of characters for the numeric component, 
        /// concatenates the characters, and then parses the string as an integer.
        /// </summary>
        /// <param name="enumerable">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as an integer.</returns>
        public static int Parse(this IEnumerable<char> enumerable)
        {
            return int.Parse(
                enumerable.Where(char.IsNumber)
                          .Aggregate("", (current, x) => current + x));
        }

        /// <summary>
        /// Filters an enumerable of characters for the numeric component, 
        /// concatenates the characters, and then parses the string as a double.
        /// </summary>
        /// <param name="enumerable">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as a double.</returns>
        public static double ParseDouble(this IEnumerable<char> enumerable)
        {
            return double.Parse(
                enumerable.Where(x => char.IsNumber(x) || x == '.')
                          .Aggregate("", (current, x) => current + x));
        }

    }
}
