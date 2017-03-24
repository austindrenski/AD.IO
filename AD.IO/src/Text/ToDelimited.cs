using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;
using System.Reflection;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to create delimited strings from various collections.
    /// </summary>
    [PublicAPI]
    public static class ToDelimitedExtensions
    {
        /// <summary>
        /// Provides safe handling for string components.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="delimiter">The string delimiter.</param>
        /// <returns>The original string, an empty string, or the original string wrapped in double quotes.</returns>
        [Pure]
        [NotNull]
        private static string MakeSafeString([CanBeNull] string value, [CanBeNull] string delimiter)
        {
            if (value == null)
            {
                return "";
            }
            if (delimiter == null)
            {
                return value;
            }
            if (!value.Contains(delimiter))
            {
                return value;
            }
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                return value;
            }
            return $"\"{value}\"";
        }

        /// <summary>
        /// Appends the strings of the enumerable collection by a delimiter. 
        /// Strings containing the delimiter are enclosed in double quotation marks.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this IEnumerable<string> enumerable, [CanBeNull] string delimiter = "|")
        {
            IEnumerable<string> safeEnumerable = enumerable.Select(x => MakeSafeString(x, delimiter));
            return string.Join(delimiter, safeEnumerable);
        }

        /// <summary>
        /// Appends the strings of the enumerable collection by a delimiter. 
        /// Strings containing the delimiter are enclosed in double quotation marks.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this ParallelQuery<string> enumerable, [CanBeNull] string delimiter = "|")
        {
            ParallelQuery<string> safeEnumerable = enumerable.Select(x => MakeSafeString(x, delimiter));
            return string.Join(delimiter, safeEnumerable);
        }

        /// <summary>
        /// Returns the delimited values of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/> from which values are retrieved.</param>
        /// <param name="delimiter">The character to delimit the values of the child elements.</param>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull] this XElement element, [CanBeNull] string delimiter = "|")
        {
            return element.HasElements 
                ? element.Elements().Select(x => x.Value).ToDelimited(delimiter) 
                : element.Value;
        }

        /// <summary>
        /// Returns the delimiter delimited values of the children each <see cref="XElement"/> delimited by new lines.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter and new lines.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this IEnumerable<XElement> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Returns the delimiter delimited values of the children each <see cref="XElement"/> delimited by new lines.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter and new lines.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this ParallelQuery<XElement> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this IEnumerable<T> enumerable, [CanBeNull] string delimiter = "|")
        {
            if (typeof(T).IsPrimitive)
            {
                return enumerable.Select(x => $"{x}").ToDelimited();
            }

            enumerable = enumerable as T[] ?? enumerable.ToArray();

            PropertyInfo[] properties =
                typeof(T) == typeof(object)
                    ? enumerable.FirstOrDefault()?
                                .GetType()
                                .GetProperties()
                      ?? new PropertyInfo[0]
                    : typeof(T).GetProperties();

            return enumerable.Select(
                                 x =>
                                     properties.Select(
                                                   y =>
                                                       $"{y.GetValue(x)}")
                                               .ToDelimited())
                             .ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this ParallelQuery<T> enumerable, [CanBeNull] string delimiter = "|")
        {
            if (typeof(T).IsPrimitive)
            {
                return enumerable.Select(x => $"{x}").ToDelimited();
            }

            PropertyInfo[] properties =
                typeof(T) == typeof(object)
                    ? enumerable.FirstOrDefault()?
                                .GetType()
                                .GetProperties()
                      ?? new PropertyInfo[0]
                    : typeof(T).GetProperties();

            return enumerable.Select(
                                 x =>
                                     properties.Select(
                                                   y =>
                                                       $"{y.GetValue(x)}")
                                               .ToDelimited())
                             .ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this IEnumerable<IEnumerable<string>> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this IEnumerable<ParallelQuery<string>> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }
        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this ParallelQuery<IEnumerable<string>> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }
        
        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this ParallelQuery<ParallelQuery<string>> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this IEnumerable<IEnumerable<T>> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this IEnumerable<ParallelQuery<T>> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this ParallelQuery<IEnumerable<T>> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this ParallelQuery<ParallelQuery<T>> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>(this ImmutableArray<ImmutableArray<T>> enumerable, [CanBeNull] string delimiter = "|")
        {
            return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the document by a delimiter.
        /// </summary>
        /// <param name="document">An XDocument to be transformed into a delimited string.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull] this XDocument document, [CanBeNull] string delimiter = "|")
        {
            if (document.Root == null || !document.Root.HasElements)
            {
                return null;
            }

            ImmutableArray<XElement> elements = document.Root.Elements().ToImmutableArray();

            string headers =
                elements.First()
                        .Elements()
                        .Select(
                            x =>
                                x.Name
                                 .LocalName)
                        .ToDelimited(delimiter);

            string body = elements.Select(x => x.Elements().Select(y => y.ToDelimited(delimiter))).ToDelimited(delimiter);

            return headers + Environment.NewLine + body;
        }
    }
}