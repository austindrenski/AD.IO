using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using JetBrains.Annotations;

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
            if (value is null)
            {
                return "";
            }
            if (delimiter is null)
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
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this IEnumerable<string> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            IEnumerable<string> safeEnumerable = source.Select(x => MakeSafeString(x, delimiter));
            return string.Join(delimiter, safeEnumerable);
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
        public static string ToDelimited([NotNull][ItemCanBeNull] this ParallelQuery<string> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            ParallelQuery<string> safeEnumerable = source.Select(x => MakeSafeString(x, delimiter));
            return string.Join(delimiter, safeEnumerable);
        }

        /// <summary>
        /// Returns the delimited values of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="source">The <see cref="XElement"/> from which values are retrieved.</param>
        /// <param name="delimiter">The character to delimit the values of the child elements.</param>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull] this XElement source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.HasElements
                ? source.Elements().Select(x => x.Value).ToDelimited(delimiter)
                : source.Value;
        }

        /// <summary>
        /// Returns the delimiter delimited values of the children each <see cref="XElement"/> delimited by new lines.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter and new lines.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this IEnumerable<XElement> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Returns the delimiter delimited values of the children each <see cref="XElement"/> delimited by new lines.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter and new lines.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this ParallelQuery<XElement> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimitedString<T>([NotNull] this T source, [CanBeNull] string delimiter = "|")
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (typeof(T).GetTypeInfo().IsPrimitive)
            {
                return source.ToString();
            }

            PropertyInfo[] properties = typeof(T).GetTypeInfo().GetProperties();

            return string.Join(delimiter, properties.Select(x => x.GetValue(source)?.ToString()));
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this IEnumerable<T> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (typeof(T).GetTypeInfo().IsPrimitive)
            {
                return source.Select(x => $"{x}").ToDelimited(delimiter);
            }

            source = source as T[] ?? source.ToArray();

            object example = source.FirstOrDefault();

            if (example?.GetType().FullName?.Contains("ValueTuple") ?? false)
            {
                return
                    source.Select(x => example.GetType().GetTypeInfo().GetFields().Select(y => y.GetValue(x)?.ToString()))
                          .Select(x => x.ToDelimited(delimiter))
                          .ToDelimited(Environment.NewLine);
            }
            PropertyInfo[] properties =
                typeof(T) == typeof(object)
                    ? source.FirstOrDefault()?
                            .GetType()
                            .GetTypeInfo()
                            .GetProperties()
                      ?? new PropertyInfo[0]
                    : typeof(T).GetTypeInfo().GetProperties();

            return
                source.Select(x => properties.Select(y => y.GetValue(x)?.ToString()))
                      .Select(x => x.ToDelimited(delimiter))
                      .ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this ParallelQuery<T> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (typeof(T).GetTypeInfo().IsPrimitive)
            {
                return source.Select(x => x?.ToString()).ToDelimited(delimiter);
            }

            object example = source.FirstOrDefault();

            if (example?.GetType().FullName?.Contains("ValueTuple") ?? false)
            {
                return
                    source.Select(x => example.GetType().GetTypeInfo().GetFields().Select(y => y.GetValue(x)?.ToString()))
                          .Select(x => x.ToDelimited(delimiter))
                          .ToDelimited(Environment.NewLine);
            }
            PropertyInfo[] properties =
                typeof(T) == typeof(object)
                    ? source.FirstOrDefault()?
                            .GetType()
                            .GetTypeInfo()
                            .GetProperties()
                      ?? new PropertyInfo[0]
                    : typeof(T).GetTypeInfo().GetProperties();

            return
                source.Select(x => properties.Select(y => y.GetValue(x)?.ToString()))
                      .Select(x => x.ToDelimited(delimiter))
                      .ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this IEnumerable<IEnumerable<string>> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this IEnumerable<ParallelQuery<string>> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }
        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this ParallelQuery<IEnumerable<string>> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull][ItemCanBeNull] this ParallelQuery<ParallelQuery<string>> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this IEnumerable<IEnumerable<T>> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this IEnumerable<ParallelQuery<T>> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this ParallelQuery<IEnumerable<T>> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this ParallelQuery<ParallelQuery<T>> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="source">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>([NotNull][ItemCanBeNull] this IImmutableList<IImmutableList<T>> source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x?.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the document by a delimiter.
        /// </summary>
        /// <param name="source">An XDocument to be transformed into a delimited string.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited([NotNull] this XDocument source, [CanBeNull] string delimiter = "|")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Root == null || !source.Root.HasElements)
            {
                return null;
            }

            ImmutableArray<XElement> elements = source.Root.Elements().ToImmutableArray();

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