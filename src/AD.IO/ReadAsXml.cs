using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AD.IO.Paths;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to import files as XML nodes.
    /// </summary>
    [PublicAPI]
    public static class ReadAsXmlExtensions
    {
        /// <summary>
        /// Parses a delimited file into an <see cref="IEnumerable{XElement}"/>. File must include a header row.
        /// </summary>
        /// <param name="filePath">The path of the file to be parsed.</param>
        /// <returns>A <see cref="IEnumerable{XElement}"/> representing the delimited data.</returns>
        /// <exception cref="AggregateException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="PathTooLongException"/>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        [Pure]
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<XElement> ReadAsXml([NotNull] this DelimitedFilePath filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            string firstRow =
                File.ReadLines(filePath)
                    .FirstOrDefault();

            if (firstRow == null)
            {
                return Enumerable.Empty<XElement>();
            }

            XName record = "record";

            ImmutableArray<XName> headers =
                firstRow.Replace(" ", null)
                        .Replace("(", null)
                        .Replace(")", null)
                        .Replace("$", null)
                        .Replace(".", null)
                        .SplitDelimitedLine(filePath.Delimiter)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => (XName) x)
                        .ToImmutableArray();

            if (!headers.Any())
            {
                throw new ArgumentException("No valid data found on the first line of the input file.");
            }

            ConcurrentBag<XElement> concurrentBag = new ConcurrentBag<XElement>();

            Parallel.ForEach(
                File.ReadLines(filePath, Encoding.UTF8).Skip(1),
                line =>
                {
                    concurrentBag.Add(
                        new XElement(
                            record,
                            line.SplitDelimitedLine(filePath.Delimiter)
                                .Select((y, i) => new XElement(headers[i], y))));
                });
            return concurrentBag;
        }

        /// <summary>
        /// Opens a Microsoft Word document (.docx) as an <see cref="XElement"/>.
        /// </summary>
        /// <param name="filePath">The file path of the .docx file to be opened. The file name is stored as an attribure of the root element.</param>
        /// <param name="entryPath">The entry path within the zip archive to read as XML.</param>
        /// <returns>An <see cref="XElement"/> representing the document root of the Microsoft Word document.</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="PathTooLongException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        [Pure]
        [CanBeNull]
        public static XElement ReadAsXml([NotNull] this DocxFilePath filePath, [NotNull] string entryPath = "word/document.xml")
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            XElement element;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (ZipArchive file = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry entry = file.GetEntry(entryPath);
                    if (entry is null)
                    {
                        return null;
                    }
                    using (Stream stream = entry.Open())
                    {
                        element = XElement.Load(stream);
                    }
                }
            }
            element.SetAttributeValue("fileName", filePath.Name);
            return element;
        }

        /// <summary>
        /// Opens Microsoft Word documents (.docx) as an <see cref="IEnumerable{XElement}"/>.
        /// </summary>
        /// <param name="filePaths">An enumerable collection of .docx files. The file names are stored as attribures of the root elements.</param>
        /// <returns>An <see cref="IEnumerable{XElement}"/> wherein each <see cref="XElement"/> is the document root of one Microsoft Word document.</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="PathTooLongException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        [Pure]
        [NotNull]
        [ItemCanBeNull]
        public static IEnumerable<XElement> ReadAsXml([NotNull][ItemNotNull] this IEnumerable<DocxFilePath> filePaths)
        {
            if (filePaths is null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            return filePaths.Select(x => x.ReadAsXml());
        }

        /// <summary>
        /// Opens Microsoft Word documents (.docx) as a <see cref="ParallelQuery{XElement}"/>.
        /// </summary>
        /// <param name="filePaths">An enumerable collection of .docx files. The file names are stored as attribures of the root elements.</param>
        /// <returns>An <see cref="IEnumerable{XElement}"/> wherein each <see cref="XElement"/> is the document root of a Microsoft Word document.</returns>
        /// <exception cref="AggregateException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="PathTooLongException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        [Pure]
        [NotNull]
        [ItemCanBeNull]
        public static ParallelQuery<XElement> ReadAsXml([NotNull][ItemNotNull] this ParallelQuery<DocxFilePath> filePaths)
        {
            if (filePaths is null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            return filePaths.Select(x => x.ReadAsXml());
        }
    }
}