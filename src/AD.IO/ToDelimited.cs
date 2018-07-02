using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        #region Generic

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [NotNull]
        public static string ToDelimitedString<T>([CanBeNull] this T source, char delimiter = '|')
        {
            if (source == null)
                return string.Empty;

            Type type = source.GetType();

            if (type.IsPrimitive)
                return source.ToString();

            switch (source)
            {
                case string s:
                    return MakeSafeString(s, delimiter);

                case StringSegment segment:
                    return MakeSafeString(segment.Value, delimiter);

                case StringValues values:
                    return values.Select(x => x).ToDelimitedString(delimiter);
            }

            if (type.Name.StartsWith(nameof(ValueTuple)))
            {
                return
                    type.GetRuntimeFields()
                        .Select(y => y.GetValue(source))
                        .Select(x => x?.ToString() ?? string.Empty)
                        .ToDelimitedString(delimiter);
            }

            return
                type.GetRuntimeProperties()
                    .Select(x => x.GetValue(source))
                    .Select(x => x?.ToString() ?? string.Empty)
                    .ToDelimitedString(delimiter);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [NotNull]
        public static string ToDelimitedString<T>([NotNull] [ItemCanBeNull] this IEnumerable<T> source, char delimiter = '|')
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return string.Join(delimiter.ToString(), source.Select(x => x.ToDelimitedString(delimiter)));
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="headers">True if headers should be included; otherwise false.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [NotNull]
        public static string ToDelimited<T>([NotNull] [ItemCanBeNull] this IEnumerable<T> source, bool headers = true, char delimiter = '|')
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (!headers)
                return string.Join(Environment.NewLine, source.Select(x => x.ToDelimitedString(delimiter)));

            source = source as T[] ?? source.ToArray();

            return
                string.Join(
                    Environment.NewLine,
                    GetDelimitedHeaders(source, delimiter),
                    string.Join(
                        Environment.NewLine,
                        source.Select(x => x.ToDelimitedString(delimiter))));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Pure]
        [NotNull]
        static string GetDelimitedHeaders<T>([NotNull] IEnumerable<T> source, char delimiter = '|')
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            Type type = source.AsQueryable().ElementType;

            if (type.Name.StartsWith(nameof(ValueTuple)))
            {
                return
                    type.GetRuntimeFields()
                        .Select(x => MakeSafeString(x.Name, delimiter))
                        .ToDelimitedString(delimiter);
            }

            return
                type.GetRuntimeProperties()
                    .Select(x => MakeSafeString(x.Name, delimiter))
                    .ToDelimitedString(delimiter);
        }

        #endregion

        #region XML

        /// <summary>
        /// Returns the delimited values of the <see cref="XNode"/>.
        /// </summary>
        /// <param name="node">The <see cref="XNode"/> from which values are retrieved.</param>
        /// <param name="delimiter">The character to delimit the values of the child elements.</param>
        /// <returns>
        /// A delimited string representing the node.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        [Pure]
        [NotNull]
        public static string ToDelimited([NotNull] this XNode node, char delimiter = '|')
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));

            switch (node)
            {
                case XDocument d:
                    return GetDelimitedString(d, true, delimiter);

                case XElement e:
                    return GetDelimitedString(e, delimiter);

                case XText t:
                    return MakeSafeString(t.Value, delimiter);

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Returns the delimiter delimited values of the children each <see cref="XElement"/> delimited by new lines.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="headers">True if headers should be included; otherwise false.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter and new lines.</returns>
        [Pure]
        [NotNull]
        public static string ToDelimited([NotNull] [ItemNotNull] this IEnumerable<XElement> source, bool headers = true, char delimiter = '|')
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (!headers)
                return string.Join(Environment.NewLine, source.Where(x => x != null).Select(x => GetDelimitedString(x, delimiter)));

            source = source as XElement[] ?? source.ToArray();

            return
                string.Join(
                    Environment.NewLine,
                    GetDelimitedHeaders(source, delimiter),
                    string.Join(
                        Environment.NewLine,
                        source.Where(x => x != null)
                              .Select(x => GetDelimitedString(x, delimiter))));
        }

        /// <summary>
        /// Appends the elements of the document by a delimiter.
        /// </summary>
        /// <param name="source">An XDocument to be transformed into a delimited string.</param>
        /// <param name="headers">True if headers should be included; otherwise false.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [NotNull]
        static string GetDelimitedString([NotNull] XDocument source, bool headers = true, char delimiter = '|')
        {
            if (source.Root is null || !source.Root.HasElements)
                return string.Empty;

            if (!headers)
                return string.Join(Environment.NewLine, source.Root.Elements().Where(x => x != null).Select(x => GetDelimitedString(x, delimiter)));

            XElement[] elements =
                source.Root
                      .Elements()
                      .ToArray();

            return
                string.Join(
                    Environment.NewLine,
                    GetDelimitedHeaders(elements, delimiter),
                    string.Join(
                        Environment.NewLine,
                        elements.Where(x => x != null)
                                .Select(x => GetDelimitedString(x, delimiter))));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        [Pure]
        [NotNull]
        static string GetDelimitedHeaders([NotNull] IEnumerable<XElement> source, char delimiter = '|')
            => source.First()
                     .Elements()
                     .Select(x => x.Name.LocalName)
                     .Select(x => MakeSafeString(x, delimiter))
                     .ToDelimitedString(delimiter);

        /// <summary>
        /// Returns the delimited values of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="source">The <see cref="XElement"/> from which values are retrieved.</param>
        /// <param name="delimiter">The character to delimit the values of the child elements.</param>
        [Pure]
        [NotNull]
        static string GetDelimitedString([NotNull] XElement source, char delimiter = '|')
            => source.HasElements
                   ? source.Elements().Select(x => x.Value).ToDelimitedString(delimiter)
                   : source.Value.ToDelimitedString();

        #endregion

        #region Utilities

        /// <summary>
        /// Provides safe handling for string components.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="delimiter">The string delimiter.</param>
        /// <returns>
        /// The original string, an empty string, or the original string wrapped in double quotes.
        /// </returns>
        [Pure]
        [NotNull]
        static string MakeSafeString([NotNull] string value, char delimiter)
        {
            const char quote = '"';

            ReadOnlySpan<char> span = value.AsSpan();

            if (span.IsEmpty || span.IndexOfAny(new char[] { quote, delimiter, '\r', '\n' }) == -1)
                return value;

            int length = span.Length + 2;

            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] == quote)
                    length++;
            }

            Span<char> result = stackalloc char[length];

            // Wrap the result in quotes.
            result[0] = quote;
            result[result.Length - 1] = quote;

            // Fill the inner result space, escaping quotations.
            int index = 0;
            for (int i = 0; i < span.Length; i++)
            {
                char c = span[i];

                result[++index] = c;

                if (c == quote)
                    result[++index] = quote;
            }

            return result.ToString();
        }

        #endregion
    }
}