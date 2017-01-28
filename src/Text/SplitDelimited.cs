using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to split delimited strings into enumerable collections.
    /// </summary>
    [PublicAPI]
    public static class SplitDelimitedExtensions
    {
        /// <summary>
        /// Splits a string on the delimiter character. Preserves delimiters embeded in double quotation marks.
        /// </summary>
        /// <param name="line">The delimited string to be split.</param>
        /// <param name="delimiter">The character that delimits the string.</param>
        /// <returns>An enumerable collection of the strings between comma characters.</returns>
        [CanBeNull]
        [Pure]
        public static IEnumerable<string> SplitDelimitedLine(this string line, char delimiter)
        {
            bool insideQuote = false;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    default:
                    {
                        stringBuilder.Append(line[i]);
                        continue;
                    }
                    case '"':
                    {
                        insideQuote = !insideQuote;
                        continue;
                    }
                    case '\r':
                    {
                        continue;
                    }
                    case '\n':
                    {
                        continue;
                    }
                    case ',':
                    {
                        if (insideQuote)
                        {
                            goto default;
                        }
                        if (!delimiter.Equals(','))
                        {
                            goto default;
                        }
                        yield return stringBuilder.ToString();
                        stringBuilder.Clear();
                        continue;
                    }
                    case '|':
                    {
                        if (insideQuote)
                        {
                            goto default;
                        }
                        if (!delimiter.Equals('|'))
                        {
                            goto default;
                        }
                        yield return stringBuilder.ToString();
                        stringBuilder.Clear();
                        continue;
                    }
                }
            }
            yield return stringBuilder.ToString();
        }

        /// <summary>
        /// Splits the strings of an the enumerable on the delimiter characters. Preserves delimiters embeded in double quotation marks.
        /// </summary>
        /// <param name="lines">The enumerable collection of delimited strings to be split.</param>
        /// <param name="delimiter">The character delimiting the strings.</param>
        /// <returns>An enumerable of enumerable collections of the split strings.</returns>
        [CanBeNull]
        [Pure]
        public static IEnumerable<IEnumerable<string>> SplitDelimitedLine(this IEnumerable<string> lines, char delimiter)
        {
            return lines.Select(x => x.SplitDelimitedLine(delimiter));
        }

        /// <summary>
        /// Splits the strings of an the enumerable on the delimiter characters. Preserves delimiters embeded in double quotation marks.
        /// </summary>
        /// <param name="lines">The enumerable collection of delimited strings to be split.</param>
        /// <param name="delimiter">The character delimiting the strings.</param>
        /// <returns>An enumerable of enumerable collections of the split strings.</returns>
        [CanBeNull]
        [Pure]
        public static ParallelQuery<ParallelQuery<string>> SplitDelimitedLine(this ParallelQuery<string> lines, char delimiter)
        {
            return lines.Select(x => x.SplitDelimitedLine(delimiter)?.AsParallel());
        }
    }
}