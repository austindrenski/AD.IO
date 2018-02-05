using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Parses an <see cref="IEnumerable{Char}"/> as a number.
    /// </summary>
    [PublicAPI]
    public static class ParseExtensions
    {
        /// <summary>
        /// Filters an enumerable of characters for the numeric component, concatenates the characters, and then parses the string as an integer.
        /// </summary>
        /// <param name="souce">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as an integer.</returns>
        [Pure]
        [CanBeNull]
        public static short? ParseShort([NotNull] this IEnumerable<char> souce)
        {
            if (souce is null)
            {
                throw new ArgumentNullException(nameof(souce));
            }

            string numerics =
                souce.Where(char.IsNumber)
                     .Aggregate(default(string), (current, x) => current + x);

            return short.TryParse(numerics, out short result) ? (short?) result : null;
        }

        /// <summary>
        /// Filters an enumerable of characters for the numeric component, concatenates the characters, and then parses the string as an integer.
        /// </summary>
        /// <param name="souce">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as an integer.</returns>
        [Pure]
        [CanBeNull]
        public static int? ParseInt([NotNull] this IEnumerable<char> souce)
        {
            if (souce is null)
            {
                throw new ArgumentNullException(nameof(souce));
            }

            string numerics =
                souce.Where(char.IsNumber)
                     .Aggregate(default(string), (current, x) => current + x);
            
            return int.TryParse(numerics, out int result) ? (int?) result : null;
        }

        /// <summary>
        /// Filters an enumerable of characters for the numeric component, concatenates the characters, and then parses the string as a long.
        /// </summary>
        /// <param name="souce">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as a long.</returns>
        [Pure]
        [CanBeNull]
        public static long? ParseLong([NotNull] this IEnumerable<char> souce)
        {
            if (souce is null)
            {
                throw new ArgumentNullException(nameof(souce));
            }

            string numerics =
                souce.Where(char.IsNumber)
                     .Aggregate(default(string), (current, x) => current + x);

            return long.TryParse(numerics, out long result) ? (long?) result : null;
        }

        /// <summary>
        /// Filters an enumerable of characters for the numeric component, concatenates the characters, and then parses the string as a double.
        /// </summary>
        /// <param name="souce">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as a double.</returns>
        [Pure]
        [CanBeNull]
        public static double? ParseDouble(this IEnumerable<char> souce)
        {
            if (souce is null)
            {
                throw new ArgumentNullException(nameof(souce));
            }

            string numerics =
                souce.Where(x => char.IsNumber(x) || x == '.')
                     .Aggregate(default(string), (current, x) => current + x);

            return double.TryParse(numerics, out double result) ? (double?) result : null;
        }
    }
}