using System;
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
        [Obsolete("Use a nullable method instead.")]
        public static int Parse(this IEnumerable<char> enumerable)
        {
            string numerics =
                enumerable.Where(char.IsNumber)
                          .DefaultIfEmpty('0')
                          .Select(x => x.ToString())
                          .Aggregate((current, x) => current + x);


            return int.Parse(numerics);
        }


        /// <summary>
        /// Filters an enumerable of characters for the numeric component, 
        /// concatenates the characters, and then parses the string as an integer.
        /// </summary>
        /// <param name="enumerable">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as an integer.</returns>
        [CanBeNull]
        public static short? ParseShort(this IEnumerable<char> enumerable)
        {
            string numerics =
                enumerable.Where(char.IsNumber)
                          .DefaultIfEmpty('0')
                          .Select(x => x.ToString())
                          .Aggregate((current, x) => current + x);


            return short.TryParse(numerics, out short result) ? (short?) result : null;
        }

        /// <summary>
        /// Filters an enumerable of characters for the numeric component, 
        /// concatenates the characters, and then parses the string as an integer.
        /// </summary>
        /// <param name="enumerable">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as an integer.</returns>
        [CanBeNull]
        public static int? ParseInt(this IEnumerable<char> enumerable)
        {
            string numerics =
                enumerable.Where(char.IsNumber)
                          .DefaultIfEmpty('0')
                          .Select(x => x.ToString())
                          .Aggregate((current, x) => current + x);


            return int.TryParse(numerics, out int result) ? (int?) result : null;
        }

        /// <summary>
        /// Filters an enumerable of characters for the numeric component, 
        /// concatenates the characters, and then parses the string as a long.
        /// </summary>
        /// <param name="enumerable">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as a long.</returns>
        [CanBeNull]
        public static long? ParseLong(this IEnumerable<char> enumerable)
        {
            string numerics =
                enumerable.Where(char.IsNumber)
                          .DefaultIfEmpty('0')
                          .Select(x => x.ToString())
                          .Aggregate((current, x) => current + x);


            return long.TryParse(numerics, out long result) ? (long?) result : null;
        }

        /// <summary>
        /// Filters an enumerable of characters for the numeric component, 
        /// concatenates the characters, and then parses the string as a double.
        /// </summary>
        /// <param name="enumerable">The source enumerable.</param>
        /// <returns>The numeric components of the source <see cref="IEnumerable{Char}"/> as a double.</returns>
        [CanBeNull]
        public static double? ParseDouble(this IEnumerable<char> enumerable)
        {
            string numerics =
                enumerable.Where(x => char.IsNumber(x) || x == '.')
                          .DefaultIfEmpty('0')
                          .Select(x => x.ToString())
                          .Aggregate((current, x) => current + x);


            return double.TryParse(numerics, out double result) ? (double?) result : null;
        }
    }
}