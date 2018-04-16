using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using AD.IO.Paths;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to write an <see cref="IEnumerable{XElement}"/> to a delimited file.
    /// </summary>
    [PublicAPI]
    public static class WriteDelimitedExtensions
    {
        /// <summary>
        /// Writes the <see cref="IEnumerable{T}"/> as a delimited file.
        /// </summary>
        /// <param name="elements">The source enumerable.</param>
        /// <param name="filePath">The file to which the content is written.</param>
        /// <param name="delimiter">The character used to delimit values.</param>
        /// <param name="overwrite">True to overwrite an existing file.</param>
        public static void WriteDelimited<T>([NotNull] this IEnumerable<T> elements, [NotNull] DelimitedFilePath filePath, char delimiter = '|', bool overwrite = true)
        {
            if (elements is null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!overwrite)
            {
                return;
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (T record in elements)
                    {
                        writer.WriteLine(record.ToDelimitedString(delimiter));
                    }
                }
            }
        }

        /// <summary>
        /// Writes the <see cref="IEnumerable{T}"/> as a delimited file.
        /// </summary>
        /// <param name="elements">The source enumerable.</param>
        /// <param name="filePath">The file to which the content is written.</param>
        /// <param name="headers">True if headers should be included; otherwise false.</param>
        /// <param name="delimiter">The character used to delimit values.</param>
        /// <param name="overwrite">True to overwrite an existing file.</param>
        public static void WriteDelimited<T>([NotNull] this IEnumerable<IEnumerable<T>> elements, [NotNull] DelimitedFilePath filePath, bool headers = false, char delimiter = '|', bool overwrite = true)
        {
            if (elements is null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!overwrite)
            {
                return;
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(elements.ToDelimited(headers, delimiter));
                }
            }
        }

        /// <summary>
        /// Writes the <see cref="IEnumerable{XElement}"/> as a delimited file.
        /// </summary>
        /// <param name="elements">The source enumerable.</param>
        /// <param name="filePath">The file to which the content is written.</param>
        /// <param name="headers">True if headers should be included; otherwise false.</param>
        /// <param name="delimiter">The character used to delimit values.</param>
        /// <param name="overwrite">True to overwrite an existing file.</param>
        public static void WriteDelimited([NotNull] this IEnumerable<XElement> elements, [NotNull] DelimitedFilePath filePath, bool headers = false, char delimiter = '|', bool overwrite = true)
        {
            if (elements is null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!overwrite)
            {
                return;
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(elements.ToDelimited(headers, delimiter));
                }
            }
        }

        /// <summary>
        /// Writes the <see cref="IEnumerable{XElement}"/> as a delimited file.
        /// </summary>
        /// <param name="elements">The source enumerable.</param>
        /// <param name="filePath">The file to which the content is written.</param>
        /// <param name="completedMessage">The message written to the console upon completion.</param>
        /// <param name="delimiter">The character used to delimit values.</param>
        /// <param name="overwrite">True to overwrite an existing file.</param>
        public static void WriteDelimited([NotNull] this IEnumerable<XElement> elements, [NotNull] DelimitedFilePath filePath, [NotNull] string completedMessage, char delimiter = '|', bool overwrite = true)
        {
            if (elements is null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (completedMessage is null)
            {
                throw new ArgumentNullException(nameof(completedMessage));
            }

            elements.WriteDelimited(filePath, delimiter, overwrite);
            Console.WriteLine(completedMessage);
        }
    }
}