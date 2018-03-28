using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using AD.IO.Streams;
using JetBrains.Annotations;

namespace AD.IO.Paths
{
    // TODO: update documentation on DocxFilePath
    /// <inheritdoc />
    /// <summary>
    /// Path to a Microsoft Word file on the system. An exception is thrown if the file does not exist.
    /// </summary>
    [PublicAPI]
    public class DocxFilePath : IPath
    {
        [NotNull] private static readonly XElement ContentTypesXml;
        [NotNull] private static readonly XElement AppXml;
        [NotNull] private static readonly XElement CoreXml;
        [NotNull] private static readonly XElement DocumentXml;
        [NotNull] private static readonly XElement DocumentXmlRels;
        [NotNull] private static readonly XElement Footer1Xml;
        [NotNull] private static readonly XElement Footer2Xml;
        [NotNull] private static readonly XElement FootnotesXml;
        [NotNull] private static readonly XElement FootnotesXmlRels;
        [NotNull] private static readonly XElement Header1Xml;
        [NotNull] private static readonly XElement Header2Xml;
        [NotNull] private static readonly XElement RelsXml;
        [NotNull] private static readonly XElement SettingsXml;
        [NotNull] private static readonly XElement StylesXml;
        [NotNull] private static readonly XElement Theme1Xml;

        /// <summary>
        /// The full file path.
        /// </summary>
        [NotNull] private readonly string _path;

        /// <summary>
        /// The <see cref="MemoryStream"/> of the file during initialization.
        /// </summary>
        [NotNull] private readonly MemoryStream _memoryStream;

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
        /// The <see cref="MemoryStream"/> of the file during initialization.
        /// </summary>
        [NotNull]
        public MemoryStream MemoryStream => _memoryStream.CopyPure().Result;

        /// <summary>
        ///
        /// </summary>
        static DocxFilePath()
        {
            Assembly assembly = typeof(DocxFilePath).GetTypeInfo().Assembly;

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.[Content_Types].xml"), Encoding.UTF8))
            {
                ContentTypesXml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.app.xml"), Encoding.UTF8))
            {
                AppXml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.core.xml"), Encoding.UTF8))
            {
                CoreXml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.document.xml"), Encoding.UTF8))
            {
                DocumentXml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.document.xml.rels.xml"), Encoding.UTF8))
            {
                DocumentXmlRels = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.footer1.xml"), Encoding.UTF8))
            {
                Footer1Xml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.footer2.xml"), Encoding.UTF8))
            {
                Footer2Xml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.footnotes.xml"), Encoding.UTF8))
            {
                FootnotesXml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.footnotes.xml.rels.xml"), Encoding.UTF8))
            {
                FootnotesXmlRels = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.header1.xml"), Encoding.UTF8))
            {
                Header1Xml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.header2.xml"), Encoding.UTF8))
            {
                Header2Xml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.rels.xml"), Encoding.UTF8))
            {
                RelsXml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.settings.xml"), Encoding.UTF8))
            {
                SettingsXml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.styles.xml"), Encoding.UTF8))
            {
                StylesXml = XElement.Parse(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("AD.IO.Templates.theme1.xml"), Encoding.UTF8))
            {
                Theme1Xml = XElement.Parse(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Creates a new <see cref="DocxFilePath"/> to hold the path to a file.
        /// </summary>
        /// <param name="filePath">A string file path.</param>
        /// <exception cref="FileNotFoundException"/>
        public DocxFilePath([NotNull] string filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            if (Path.GetExtension(filePath) != ".docx")
            {
                throw new ArgumentException("Path is not a docx file.");
            }

            if (filePath.Contains('~'))
            {
                throw new ArgumentException("File path contains a tilda character. It may be invalid.");
            }

            _path = filePath;
            _memoryStream = GetMemoryStream(filePath);
            Extension = Path.GetExtension(filePath);
            Name = Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// Creates a docx <see cref="MemoryStream"/>.
        /// </summary>
        /// <exception cref="System.ArgumentException"/>
        [Pure]
        [NotNull]
        public static MemoryStream Create()
        {
            MemoryStream result = new MemoryStream();

            using (ZipArchive archive = new ZipArchive(result, ZipArchiveMode.Create, true))
            {
                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("[Content_Types].xml").Open()))
                {
                    entry.Write(ContentTypesXml);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("_rels/.rels").Open()))
                {
                    entry.Write(RelsXml);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("docProps/app.xml").Open()))
                {
                    entry.Write(AppXml);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("docProps/core.xml").Open()))
                {
                    entry.Write(CoreXml);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("word/document.xml").Open()))
                {
                    entry.Write(DocumentXml);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("word/_rels/document.xml.rels").Open()))
                {
                    entry.Write(DocumentXmlRels);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("word/theme/theme1.xml").Open()))
                {
                    entry.Write(Theme1Xml);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("word/settings.xml").Open()))
                {
                    entry.Write(SettingsXml);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("word/styles.xml").Open()))
                {
                    entry.Write(StylesXml);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("word/footnotes.xml").Open()))
                {
                    entry.Write(FootnotesXml);
                }

                using (StreamWriter entry = new StreamWriter(archive.CreateEntry("word/_rels/footnotes.xml.rels").Open()))
                {
                    entry.Write(FootnotesXmlRels);
                }
            }

            result.Seek(0, SeekOrigin.Begin);

            return result;
        }

        /// <summary>
        /// Creates a file along the path, overwriting if specified.
        /// </summary>
        [NotNull]
        public static DocxFilePath Create([NotNull] string filePath, bool overwrite)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!overwrite && File.Exists(filePath))
            {
                return new DocxFilePath(filePath);
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                byte[] buffer = Create().GetBuffer();
                fileStream.Write(buffer, 0, buffer.Length);
            }

            return new DocxFilePath(filePath);
        }

        /// <inheritdoc />
        [NotNull]
        IPath IPath.Create([NotNull] string path)
        {
            return Create(path, false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        /// <exception cref="FileNotFoundException" />
        [Pure]
        [NotNull]
        private static MemoryStream GetMemoryStream([NotNull] string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(nameof(path));
            }

            MemoryStream memoryStream = new MemoryStream();

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileStream.CopyTo(memoryStream);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        /// <inheritdoc cref="IPath.ToString"/>
        /// <summary>
        /// Returns the file path.
        /// </summary>
        [Pure]
        [NotNull]
        public override string ToString()
        {
            return _path;
        }

        /// <inheritdoc />
        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return _path.AsEnumerable().GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _path.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Implicitly casts a FilePath as its internal file path string.
        /// </summary>
        [NotNull]
        public static implicit operator string([NotNull] DocxFilePath filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

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
            {
                throw new ArgumentNullException(nameof(filePath));
            }

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
            {
                throw new ArgumentNullException(nameof(filePath));
            }

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
            {
                throw new ArgumentNullException(nameof(filePath));
            }

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
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return new UrlPath(filePath);
        }
    }
}