using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Primitives;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to create delimited strings from various collections.
    /// </summary>
    [PublicAPI]
    public static class ToDelimitedExtensions
    {
        private const char Quote = '"';

        /// <summary>
        /// Provides safe handling for string components.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="delimiter">The string delimiter.</param>
        /// <returns>The original string, an empty string, or the original string wrapped in double quotes.</returns>
        [Pure]
        [NotNull]
        private static string MakeSafeString([CanBeNull] string value, char delimiter)
        {
            if (value is null || value.Length == 0)
            {
                return string.Empty;
            }

            if (value.IndexOfAny(new char[] { Quote, '\r', '\n', delimiter }) == -1)
            {
                return value;
            }

            int resultLength = value.Length + 2;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == Quote)
                {
                    resultLength++;
                }
            }

            InplaceStringBuilder sb = new InplaceStringBuilder(resultLength);

            sb.Append(Quote);

            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                sb.Append(c);

                if (c == Quote)
                {
                    sb.Append(c);
                }
            }

            sb.Append(Quote);

            return sb.ToString();
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimitedString<T>([NotNull] this T source, char delimiter = '|')
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(delimiter));
            }

            if (typeof(T).IsPrimitive)
            {
                return source.ToString();
            }

            switch (source)
            {
                case string s:
                {
                    return MakeSafeString(s, delimiter);
                }
                case StringSegment segment:
                {
                    return MakeSafeString(segment.Value, delimiter);
                }
                case StringValues values:
                {
                    return values.Select(x => x).ToDelimitedString(delimiter);
                }
            }

            if (typeof(T).Name.StartsWith(nameof(ValueTuple)))
            {
                return
                    typeof(T).GetFields()
                             .Select(y => y.GetValue(source))
                             .Select(x => x?.ToString() ?? string.Empty)
                             .ToDelimitedString(delimiter);
            }

            return
                typeof(T).GetProperties()
                         .Select(x => x.GetValue(source))
                         .Select(x => x?.ToString() ?? string.Empty)
                         .ToDelimitedString(delimiter);
        }

        /// <summary>
        /// Returns the delimited values of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="source">The <see cref="XElement"/> from which values are retrieved.</param>
        /// <param name="delimiter">The character to delimit the values of the child elements.</param>
        [Pure]
        [CanBeNull]
        public static string ToDelimitedString([NotNull] this XElement source, char delimiter = '|')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.HasElements
                ? source.Elements().Select(x => x.Value).ToDelimitedString(delimiter)
                : source.Value.ToDelimitedString();
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimitedString<T>([NotNull] [ItemNotNull] this IEnumerable<T> source, char delimiter = '|')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return string.Join(delimiter.ToString(), source.Select(x => x.ToDelimitedString(delimiter)));
        }

        /// <summary>
        /// Appends the strings of the enumerable collection by a delimiter.
        /// Strings containing the delimiter are enclosed in double quotation marks.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimitedString([NotNull] [ItemCanBeNull] this IEnumerable<string> source, char delimiter = '|')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return string.Join(delimiter.ToString(), source.Select(x => MakeSafeString(x, delimiter)));
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull] [ItemNotNull] this IEnumerable<T> source, char delimiter = '|')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return string.Join(Environment.NewLine, source.Select(x => x.ToDelimitedString(delimiter)));
        }

        /// <summary>
        /// Appends the strings of the enumerable collection by a delimiter.
        /// Strings containing the delimiter are enclosed in double quotation marks.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull] [ItemCanBeNull] this IEnumerable<string> source, char delimiter = '|')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return string.Join(Environment.NewLine, source.Select(x => MakeSafeString(x, delimiter)));
        }

        /// <summary>
        /// Returns the delimiter delimited values of the children each <see cref="XElement"/> delimited by new lines.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter and new lines.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull] [ItemNotNull] this IEnumerable<XElement> source, char delimiter = '|')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return string.Join(Environment.NewLine, source.Select(x => x.ToDelimitedString(delimiter)));
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull] [ItemNotNull] this IEnumerable<IEnumerable<T>> source, char delimiter = '|')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return string.Join(Environment.NewLine, source.Select(x => x.ToDelimitedString(delimiter)));
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull] [ItemNotNull] this IEnumerable<IEnumerable<string>> source, char delimiter = '|')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return string.Join(Environment.NewLine, source.Select(x => x.ToDelimitedString(delimiter)));
        }

        /// <summary>
        /// Appends the elements of the document by a delimiter.
        /// </summary>
        /// <param name="source">An XDocument to be transformed into a delimited string.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull] this XDocument source, char delimiter = '|')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Root == null || !source.Root.HasElements)
            {
                return null;
            }

            XElement[] elements =
                source.Root
                      .Elements()
                      .ToArray();

            string headers =
                elements.First()
                        .Elements()
                        .Select(x => x.Name.LocalName)
                        .ToDelimitedString(delimiter);

            string body =
                string.Join(Environment.NewLine, elements.Select(x => x.ToDelimitedString(delimiter)));

            return string.Join(Environment.NewLine, headers, body);
        }
    }
}