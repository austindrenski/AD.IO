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
        public static void WriteDelimited<T>(this IEnumerable<T> elements, DelimitedFilePath filePath, string delimiter = "|", bool overwrite = true)
        {
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
        /// <param name="delimiter">The character used to delimit values.</param>
        /// <param name="overwrite">True to overwrite an existing file.</param>
        public static void WriteDelimited<T>(this IEnumerable<IEnumerable<T>> elements, DelimitedFilePath filePath, string delimiter = "|", bool overwrite = true)
        {
            if (!overwrite)
            {
                return;
            }
            using (FileStream stream = new FileStream(filePath, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(elements.ToDelimited(delimiter));
                }
            }
        }

        /// <summary>
        /// Writes the <see cref="IEnumerable{XElement}"/> as a delimited file.
        /// </summary>
        /// <param name="elements">The source enumerable.</param>
        /// <param name="filePath">The file to which the content is written.</param>
        /// <param name="delimiter">The character used to delimit values.</param>
        /// <param name="overwrite">True to overwrite an existing file.</param>
        public static void WriteDelimited(this IEnumerable<XElement> elements, DelimitedFilePath filePath, string delimiter = "|", bool overwrite = true)
        {
            if (!overwrite)
            {
                return;
            }
            using (FileStream stream = new FileStream(filePath, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(elements.ToDelimited(delimiter));
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
        public static void WriteDelimited(this IEnumerable<XElement> elements, DelimitedFilePath filePath, string completedMessage, string delimiter = "|", bool overwrite = true)
        {
            elements.WriteDelimited(filePath, delimiter, overwrite);
            Console.WriteLine(completedMessage);
        }
    }
}