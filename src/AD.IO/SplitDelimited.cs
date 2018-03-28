using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Primitives;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to split delimited strings into enumerable collections.
    /// </summary>
    [PublicAPI]
    public static class SplitDelimitedExtensions
    {
        /// <summary>
        /// Splits the string based on the provided <see cref="Delimiter"/>.
        /// </summary>
        /// <param name="value">
        /// The string to split.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter with which to split the string.
        /// </param>
        /// <returns>
        /// An enumerable of <see cref="StringSegment"/> instances.
        /// </returns>
        [Pure]
        [NotNull]
        public static IEnumerable<string> Split([NotNull] this string value, Delimiter delimiter)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return delimiter.Split(value).Select(x => x.Value);
        }

        /// <summary>
        /// Splits a string on the delimiter character. Preserves delimiters embeded in double quotation marks.
        /// </summary>
        /// <param name="line">The delimited string to be split.</param>
        /// <param name="delimiter">The character that delimits the string.</param>
        /// <param name="removeEnclosures">True if quotes should be removed; otherwise false.</param>
        /// <param name="removeLineEndings">True if line endings should be removed; otherwise false.</param>
        /// <returns>An enumerable collection of the strings between comma characters.</returns>
        [Pure]
        [NotNull]
        [ItemCanBeNull]
        public static IEnumerable<string> SplitDelimitedLine([NotNull] this string line, char delimiter, bool removeEnclosures = true, bool removeLineEndings = true)
        {
            Delimiter delim = new Delimiter(delimiter, '"', '"');

            return delim.Split(line, true).Select(x => removeLineEndings ? x.Value.Replace("\r", null).Replace("\n", null) : x.Value);
        }
    }
}