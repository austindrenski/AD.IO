using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    /// Extension methods to read XML.
    /// </summary>
    [PublicAPI]
    public static class ReadXmlExtensions
    {
        /// <summary>
        /// The empty <see cref="XElement"/> array.
        /// </summary>
        private static readonly XElement[] Empty = new XElement[0];

        /// <summary>
        /// Parses a delimited file into an <see cref="IEnumerable{XElement}"/>. File must include a header row.
        /// </summary>
        /// <param name="filePath">
        /// The path of the file to be parsed.
        /// </param>
        /// <returns>
        /// A <see cref="IEnumerable{XElement}"/> representing the delimited data.
        /// </returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<XElement> ReadXml([NotNull] this DelimitedFilePath filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            string firstRow =
                File.ReadLines(filePath)
                    .FirstOrDefault();

            if (firstRow is null)
            {
                return Empty;
            }

            XName record = "record";

            XName[] headers =
                firstRow.Replace(" ", null)
                        .Replace("(", null)
                        .Replace(")", null)
                        .Replace("$", null)
                        .Replace(".", null)
                        .SplitDelimitedLine(filePath.Delimiter)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => (XName) x)
                        .ToArray();

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
        /// Opens a <see cref="ZipArchive"/> to an OpenXML document as an <see cref="XElement"/>.
        /// </summary>
        /// <param name="archive">
        /// The archive to be opened.
        /// </param>
        /// <param name="entryPath">
        /// The entry path within the archive to read as XML.
        /// </param>
        /// <param name="fallback">
        /// An optional element to return if the entry is not found. If null, an exception is thrown.
        /// </param>
        /// <returns>
        /// An <see cref="XElement"/> representing the root of the XML document.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        public static XElement ReadXml([NotNull] this ZipArchive archive, [NotNull] string entryPath = "word/document.xml", [CanBeNull] XElement fallback = default)
        {
            if (archive is null)
            {
                throw new ArgumentNullException(nameof(archive));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            ZipArchiveEntry entry = archive.GetEntry(entryPath);

            if (entry is null)
            {
                return fallback ?? throw new ArgumentException($"{entryPath} not found.");
            }

            using (Stream entryStream = entry.Open())
            {
                return XElement.Load(entryStream);
            }
        }

        /// <summary>
        /// Opens a <see cref="Stream"/> to an OpenXML file as an <see cref="XElement"/>.
        /// </summary>
        /// <param name="stream">
        /// The stream of the file to be opened.
        /// </param>
        /// <param name="entryPath">
        /// The entry path within the zip archive to read as XML.
        /// </param>
        /// <param name="fallback">
        /// An optional element to return if the entry is not found. If null, an exception is thrown.
        /// </param>
        /// <returns>
        /// An <see cref="XElement"/> representing the document root.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        public static XElement ReadXml([NotNull] this Stream stream, [NotNull] string entryPath = "word/document.xml", [CanBeNull] XElement fallback = default)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                return archive.ReadXml(entryPath, fallback);
            }
        }

        /// <summary>
        /// Opens a <see cref="Stream"/> to a Microsoft Word document (.docx) as an <see cref="XElement"/>.
        /// </summary>
        /// <param name="stream">
        ///     The stream of the .docx file to be opened.
        /// </param>
        /// <param name="entryPath">
        ///     The entry path within the zip archive to read as XML.
        /// </param>
        /// <param name="fileName">
        ///     The file name to store as an attribute on the root node.
        /// </param>
        /// <returns>
        /// An <see cref="XElement"/> representing the document root of the Microsoft Word document.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        public static XElement ReadXml([NotNull] this Stream stream, [NotNull] string entryPath, [NotNull] string fileName)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            XElement element = ReadXml(stream, entryPath);

            element.SetAttributeValue("fileName", fileName);

            return element;
        }

        /// <summary>
        /// Opens a Microsoft Word document (.docx) as an <see cref="XElement"/>.
        /// </summary>
        /// <param name="filePath">
        /// The file path of the .docx file to be opened. The file name is stored as an attribure of the root element.
        /// </param>
        /// <param name="entryPath"
        /// >The entry path within the zip archive to read as XML.
        /// </param>
        /// <returns>
        /// An <see cref="XElement"/> representing the document root of the Microsoft Word document.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        public static XElement ReadXml([NotNull] this DocxFilePath filePath, [NotNull] string entryPath = "word/document.xml")
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return fileStream.ReadXml(entryPath, filePath.Name);
            }
        }

        /// <summary>
        /// Opens Microsoft Word documents (.docx) as an <see cref="IEnumerable{XElement}"/>.
        /// </summary>
        /// <param name="streams">
        /// An enumerable collection of .docx files. The file names are stored as attribures of the root elements.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{XElement}"/> wherein each <see cref="XElement"/> is the document root of one Microsoft Word document.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<XElement> ReadXml([NotNull] [ItemNotNull] this IEnumerable<Stream> streams)
        {
            if (streams is null)
            {
                throw new ArgumentNullException(nameof(streams));
            }

            return streams.Select(x => x.ReadXml());
        }

        /// <summary>
        /// Opens Microsoft Word documents (.docx) as an <see cref="IEnumerable{XElement}"/>.
        /// </summary>
        /// <param name="filePaths">
        /// An enumerable collection of .docx files. The file names are stored as attribures of the root elements.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{XElement}"/> wherein each <see cref="XElement"/> is the document root of one Microsoft Word document.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<XElement> ReadXml([NotNull] [ItemNotNull] this IEnumerable<DocxFilePath> filePaths)
        {
            if (filePaths is null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            return filePaths.Select(x => x.ReadXml());
        }

        /// <summary>
        /// Opens Microsoft Word documents (.docx) as a <see cref="ParallelQuery{XElement}"/>.
        /// </summary>
        /// <param name="streams">
        /// An enumerable collection of .docx files. The file names are stored as attribures of the root elements.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{XElement}"/> wherein each <see cref="XElement"/> is the document root of a Microsoft Word document.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        [ItemNotNull]
        public static ParallelQuery<XElement> ReadXml([NotNull] [ItemNotNull] this ParallelQuery<Stream> streams)
        {
            if (streams is null)
            {
                throw new ArgumentNullException(nameof(streams));
            }

            return streams.Select(x => x.ReadXml());
        }

        /// <summary>
        /// Opens Microsoft Word documents (.docx) as a <see cref="ParallelQuery{XElement}"/>.
        /// </summary>
        /// <param name="filePaths">
        /// An enumerable collection of .docx files. The file names are stored as attribures of the root elements.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{XElement}"/> wherein each <see cref="XElement"/> is the document root of a Microsoft Word document.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        [ItemNotNull]
        public static ParallelQuery<XElement> ReadXml([NotNull] [ItemNotNull] this ParallelQuery<DocxFilePath> filePaths)
        {
            if (filePaths is null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            return filePaths.Select(x => x.ReadXml());
        }
    }
}