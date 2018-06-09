using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.IO.Paths
{
    // TODO: update documentation on DocxFilePath
    /// <inheritdoc cref="IPath"/>
    /// <summary>
    /// Path to a Microsoft Word file on the system. An exception is thrown if the file does not exist.
    /// </summary>
    [PublicAPI]
    public class DocxFilePath : IPath
    {
        private static readonly XNamespace T = "http://schemas.openxmlformats.org/package/2006/content-types";
        private static readonly XNamespace R = "http://schemas.openxmlformats.org/package/2006/relationships";
        private static readonly XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        /// <summary>
        /// The full file path.
        /// </summary>
        [NotNull] private readonly string _path;

        /// <inheritdoc />
        /// <summary>
        /// The file extension.
        /// </summary>
        [NotNull]
        public string Extension { get; }

        /// <inheritdoc />
        /// <summary>
        /// The file name.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// The bytes of the file during initialization.
        /// </summary>
        public ReadOnlyMemory<byte> Bytes { get; }

        /// <summary>
        /// Creates a new <see cref="DocxFilePath"/> to hold the path to a file.
        /// </summary>
        /// <param name="filePath">A string file path.</param>
        /// <exception cref="FileNotFoundException"/>
        public DocxFilePath([NotNull] string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            if (Path.GetExtension(filePath) != ".docx")
                throw new ArgumentException("Path is not a docx file.");

            if (filePath.Contains('~'))
                throw new ArgumentException("File path contains a tilda character. It may be invalid.");

            _path = filePath;
            Bytes = GetBytes(filePath);
            Extension = Path.GetExtension(filePath);
            Name = Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// Creates a file along the path, overwriting if specified.
        /// </summary>
        [NotNull]
        public static DocxFilePath Create([NotNull] string filePath, bool overwrite)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (!overwrite && File.Exists(filePath))
                return new DocxFilePath(filePath);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                byte[] buffer = Create().ToArray();
                fileStream.Write(buffer, default, buffer.Length);
            }

            return new DocxFilePath(filePath);
        }

        /// <inheritdoc />
        [NotNull]
        IPath IPath.Create([NotNull] string path) => Create(path, false);

        /// <inheritdoc cref="IPath"/>
        /// <summary>
        /// Returns the file path.
        /// </summary>
        [Pure]
        public override string ToString() => _path;

        /// <inheritdoc />
        IEnumerator<char> IEnumerable<char>.GetEnumerator() => _path.AsEnumerable().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => _path.AsEnumerable().GetEnumerator();

        /// <summary>
        /// Implicitly casts a FilePath as its internal file path string.
        /// </summary>
        [NotNull]
        public static implicit operator string([NotNull] DocxFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            return filePath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a DocxFilePath. An exception is thrown if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        [NotNull]
        public static implicit operator DocxFilePath([NotNull] string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            return new DocxFilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a string as a DocxFilePath. An exception is thrown if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        [NotNull]
        public static implicit operator FilePath([NotNull] DocxFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            return new FilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a DocxFilePath as a ZipFilePath. An exception is thrown if the file is not a zip file path, or if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="System.ArgumentException"/>
        [NotNull]
        public static explicit operator ZipFilePath([NotNull] DocxFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            return new ZipFilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a DocxFilePath as a UrlPath. An exception is thrown if the file path is not a well-formed URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="System.ArgumentException"/>
        [NotNull]
        public static explicit operator UrlPath([NotNull] DocxFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            return new UrlPath(filePath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        /// <exception cref="FileNotFoundException" />
        [Pure]
        private static ReadOnlyMemory<byte> GetBytes([NotNull] string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException(nameof(path));

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                MemoryStream memoryStream = new MemoryStream();
                fileStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Creates the byte representation of a minimal Open XML document.
        /// </summary>
        /// <returns>
        /// The byte representation of a minimal Open XML document.
        /// </returns>
        [Pure]
        public static ReadOnlyMemory<byte> Create()
        {
            XDocument relationships =
                new XDocument(
                    new XDeclaration("1.0", "UTF-8", "yes"),
                    new XElement(R + "Relationships",
                        new XElement(R + "Relationship",
                            new XAttribute("Id", "rId1"),
                            new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"),
                            new XAttribute("Target", "word/document.xml"))));

            XDocument document =
                new XDocument(
                    new XDeclaration("1.0", "UTF-8", "yes"),
                    new XElement(W + "document",
                        new XAttribute(XNamespace.Xmlns + "w", W),
                        new XElement(W + "body")));

            MemoryStream memory = new MemoryStream();

            using (Package package = Package.Open(memory, FileMode.Create))
            {
                PackagePart relationshipsPart =
                    package.CreatePart(
                        new Uri("/_rels/.rels", UriKind.Relative),
                        "application/vnd.openxmlformats-package.relationships+xml");

                PackagePart documentPart =
                    package.CreatePart(
                        new Uri("/word/document.xml", UriKind.Relative),
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");

                XmlWriterSettings settings =
                    new XmlWriterSettings
                    {
                        Async = false,
                        DoNotEscapeUriAttributes = false,
                        CheckCharacters = true,
                        CloseOutput = false,
                        ConformanceLevel = ConformanceLevel.Document,
                        Encoding = Encoding.UTF8,
                        Indent = false,
                        IndentChars = "  ",
                        NamespaceHandling = NamespaceHandling.OmitDuplicates,
                        NewLineChars = Environment.NewLine,
                        NewLineHandling = NewLineHandling.None,
                        NewLineOnAttributes = false,
                        OmitXmlDeclaration = false,
                        WriteEndDocumentOnClose = true
                    };

                using (StreamWriter writer = new StreamWriter(relationshipsPart.GetStream()))
                {
                    using (XmlWriter xml = XmlWriter.Create(writer, settings))
                    {
                        relationships.WriteTo(xml);
                    }
                }

                using (StreamWriter writer = new StreamWriter(documentPart.GetStream()))
                {
                    using (XmlWriter xml = XmlWriter.Create(writer, settings))
                    {
                        document.WriteTo(xml);
                    }
                }
            }

            return memory.ToArray();
        }
    }
}