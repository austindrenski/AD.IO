using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Returns the delimited values of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/> from which values are retrieved.</param>
        /// <param name="delimiter">The character to delimit the values of the child elements.</param>
        public static string ToDelimited(this XElement element, string delimiter = "|")
        {
            return element.HasElements ? element.Elements().Select(x => x.Value).ToDelimited(delimiter) : element.Value;
        }

        /// <summary>
        /// Returns the separator delimited values of the children each <see cref="XElement"/> delimited by new lines.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="separator">The separator used to delimit the collection.</param>
        /// <returns>A string delimited by the separator and new lines.</returns>
        public static string ToDelimited(this IEnumerable<XElement> enumerable, string separator = "|")
        {
            return enumerable.Select(x => x.ToDelimited(separator)).ToDelimited(Environment.NewLine);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a separator.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="separator">The separator used to delimit the collection.</param>
        /// <returns>A string delimited by the separator.</returns>
        public static string ToDelimited<T>(this IEnumerable<T> enumerable, string separator = "|") where T : struct
        {
            return string.Join(separator, enumerable);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a separator.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="separator">The separator used to delimit the collection.</param>
        /// <returns>A string delimited by the separator.</returns>
        public static string ToDelimited(this IEnumerable<string> enumerable, string separator = "|")
        {
            IEnumerable<string> safeEnumerable = enumerable.Select(x => x.Contains(separator) && !x.StartsWith("\"") && !x.EndsWith("\"") ? $"\"{x}\"" : x);
            return string.Join(separator, safeEnumerable);
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a separator.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="separator">The separator used to delimit the collection.</param>
        /// <returns>A string delimited by the separator.</returns>
        public static string ToDelimited(this IEnumerable<IEnumerable<string>> enumerable, string separator = "|")
        {
            return string.Join(Environment.NewLine, enumerable.Select(x => x.ToDelimited(separator)));
        }

        /// <summary>
        /// Appends the elements of the enumerable collection by a separator.
        /// </summary>
        /// <param name="enumerable">A collection to be delimited.</param>
        /// <param name="separator">The separator used to delimit the collection.</param>
        /// <returns>A string delimited by the separator.</returns>
        public static string ToDelimited<T>(this IEnumerable<IEnumerable<T>> enumerable, string separator = "|") where T : struct
        {
            return string.Join(Environment.NewLine, enumerable.Select(x => string.Join(separator, x)));
        }

        /// <summary>
        /// Appends the elements of the document by a separator.
        /// </summary>
        /// <param name="document">An XDocument to be transformed into a delimited string.</param>
        /// <param name="separator">The separator used to delimit the collection.</param>
        /// <returns>A string delimited by the separator.</returns>
        public static string ToDelimited(this XDocument document, string separator = "|")
        {
            return document.Root?
                           .Elements()
                           .FirstOrDefault()?
                           .Elements()
                           .Select(x => x.Name.LocalName)
                           .ToDelimited(separator)
                 + Environment.NewLine
                 + document.Root?
                           .Elements()
                           .Select(x => x.Elements().Select(y => y.ToDelimited(separator)))
                           .ToDelimited(separator);
        }
    }
}