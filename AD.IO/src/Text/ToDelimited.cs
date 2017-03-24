using System;
using System.Collections.Generic;
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
        /// Appends the strings of the enumerable collection by a delimiter. 
        /// Strings containing the delimiter are enclosed in double quotation marks.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited(this IEnumerable<string> enumerable, string delimiter = "|")
        {
            IEnumerable<string> safeEnumerable =
                enumerable.Select(x =>
                {
                    if (x == null)
                    {
                        return "";
                    }
                    if (!x.Contains(delimiter))
                    {
                        return x;
                    }
                    if (x.StartsWith("\"") && x.EndsWith("\""))
                    {
                        return x;
                    }
                    return $"\"{x}\"";
                });
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
        public static string ToDelimited(this ParallelQuery<string> enumerable, string delimiter = "|")
        {
            ParallelQuery<string> safeEnumerable =
                enumerable.Select(x =>
                {
                    if (x == null)
                    {
                        return "";
                    }
                    if (!x.Contains(delimiter))
                    {
                        return x;
                    }
                    if (x.StartsWith("\"") && x.EndsWith("\""))
                    {
                        return x;
                    }
                    return $"\"{x}\"";
                });
            return string.Join(delimiter, safeEnumerable);
        }

        /// <summary>
        /// Returns the delimited values of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/> from which values are retrieved.</param>
        /// <param name="delimiter">The character to delimit the values of the child elements.</param>
        [Pure]
        [CanBeNull]
        public static string ToDelimited(this XElement element, string delimiter = "|")
        {
            return element.HasElements ? element.Elements().Select(x => x.Value).ToDelimited(delimiter) : element.Value;
        }

        /// <summary>
        /// Returns the delimiter delimited values of the children each <see cref="XElement"/> delimited by new lines.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter and new lines.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited(this IEnumerable<XElement> enumerable, string delimiter = "|")
        {
            return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Returns the delimiter delimited values of the children each <see cref="XElement"/> delimited by new lines.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter and new lines.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited(this ParallelQuery<XElement> enumerable, string delimiter = "|")
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
        public static string ToDelimited<T>(this IEnumerable<T> enumerable, string delimiter = "|")
        {
            if (typeof(T).IsPrimitive)
            {
                return enumerable.Select(x => $"{x}").ToDelimited();
            }
            return enumerable.Select(x => $"{x}").ToDelimited();
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited<T>(this ParallelQuery<T> enumerable, string delimiter = "|")
        {
            if (typeof(T).IsPrimitive)
            {
                return enumerable.Select(x => $"{x}").ToDelimited();
            }
            return enumerable.Select(x => $"{x}").ToDelimited();
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a delimiter.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        [Pure]
        [CanBeNull]
        public static string ToDelimited(this IEnumerable<IEnumerable<string>> enumerable, string delimiter = "|")
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
        public static string ToDelimited(this ParallelQuery<IEnumerable<string>> enumerable, string delimiter = "|")
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
        public static string ToDelimited<T>(this IEnumerable<IEnumerable<T>> enumerable, string delimiter = "|")
        {
            if (typeof(T).IsPrimitive)
            {
                return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
            }
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
        public static string ToDelimited<T>(this ParallelQuery<IEnumerable<T>> enumerable, string delimiter = "|")
        {
            if (typeof(T).IsPrimitive)
            {
                return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
            }
            return enumerable.Select(x => x.ToDelimited(delimiter)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the document by a delimiter.
        /// </summary>
        /// <param name="document">An XDocument to be transformed into a delimited string.</param>
        /// <param name="delimiter">The delimiter used to delimit the collection.</param>
        /// <returns>A string delimited by the delimiter.</returns>
        /// <exception cref="ArgumentException"/>
        [Pure]
        [CanBeNull]
        public static string ToDelimited(this XDocument document, string delimiter = "|")
        {
            if (document.Root == null)
            {
                throw new ArgumentException("Document root is null.");
            }
            if (!document.Root.HasElements)
            {
                throw new ArgumentException("Document root does not contain any elements.");
            }
            return document.Root
                           .Elements()
                           .First()
                           .Elements()
                           .Select(x => x.Name.LocalName)
                           .ToDelimited(delimiter)
                 + Environment.NewLine
                 + document.Root
                           .Elements()
                           .Select(x => x.Elements().Select(y => y.ToDelimited(delimiter)))
                           .ToDelimited(delimiter);
        }
    }
}