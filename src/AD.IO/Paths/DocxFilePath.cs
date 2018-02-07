using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using AD.IO.Streams;
using JetBrains.Annotations;

namespace AD.IO.Paths
{
    /// <inheritdoc />
    /// <summary>
    /// Path to a Microsoft Word file on the system. An exception is thrown if the file does not exist.
    /// </summary>
    [PublicAPI]
    public class DocxFilePath : IPath
    {
        [NotNull] private static readonly byte[] ContentTypesXml;
        [NotNull] private static readonly byte[] AppXml;
        [NotNull] private static readonly byte[] CoreXml;
        [NotNull] private static readonly byte[] DocumentXml;
        [NotNull] private static readonly byte[] DocumentXmlRels;
        [NotNull] private static readonly byte[] Footer1Xml;
        [NotNull] private static readonly byte[] Footer2Xml;
        [NotNull] private static readonly byte[] FootnotesXml;
        [NotNull] private static readonly byte[] FootnotesXmlRels;
        [NotNull] private static readonly byte[] Header1Xml;
        [NotNull] private static readonly byte[] Header2Xml;
        [NotNull] private static readonly byte[] RelsXml;
        [NotNull] private static readonly byte[] SettingsXml;
        [NotNull] private static readonly byte[] StylesXml;
        [NotNull] private static readonly byte[] Theme1Xml;

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

        static DocxFilePath()
        {
            Assembly assembly = typeof(DocxFilePath).GetTypeInfo().Assembly;

            ContentTypesXml = assembly.GetManifestResourceStream("AD.IO.Templates.[Content_Types].xml").CopyPure().Result.GetBuffer();
            AppXml = assembly.GetManifestResourceStream("AD.IO.Templates.app.xml").CopyPure().Result.GetBuffer();
            CoreXml = assembly.GetManifestResourceStream("AD.IO.Templates.core.xml").CopyPure().Result.GetBuffer();
            DocumentXml = assembly.GetManifestResourceStream("AD.IO.Templates.document.xml").CopyPure().Result.GetBuffer();
            DocumentXmlRels = assembly.GetManifestResourceStream("AD.IO.Templates.document.xml.rels.xml").CopyPure().Result.GetBuffer();
            Footer1Xml = assembly.GetManifestResourceStream("AD.IO.Templates.footer1.xml").CopyPure().Result.GetBuffer();
            Footer2Xml = assembly.GetManifestResourceStream("AD.IO.Templates.footer2.xml").CopyPure().Result.GetBuffer();
            FootnotesXml = assembly.GetManifestResourceStream("AD.IO.Templates.footnotes.xml").CopyPure().Result.GetBuffer();
            FootnotesXmlRels = assembly.GetManifestResourceStream("AD.IO.Templates.footnotes.xml.rels.xml").CopyPure().Result.GetBuffer();
            Header1Xml = assembly.GetManifestResourceStream("AD.IO.Templates.header1.xml").CopyPure().Result.GetBuffer();
            Header2Xml = assembly.GetManifestResourceStream("AD.IO.Templates.header2.xml").CopyPure().Result.GetBuffer();
            RelsXml = assembly.GetManifestResourceStream("AD.IO.Templates.rels.xml").CopyPure().Result.GetBuffer();
            SettingsXml = assembly.GetManifestResourceStream("AD.IO.Templates.settings.xml").CopyPure().Result.GetBuffer();
            StylesXml = assembly.GetManifestResourceStream("AD.IO.Templates.styles.xml").CopyPure().Result.GetBuffer();
            Theme1Xml = assembly.GetManifestResourceStream("AD.IO.Templates.theme1.xml").CopyPure().Result.GetBuffer();
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
            Extension = Path.GetExtension(filePath);
            Name = Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// Creates a file along the path if one does not exist.
        /// </summary>
        [NotNull]
        public static DocxFilePath Create([NotNull] string filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return File.Exists(filePath) ? new DocxFilePath(filePath) : CreateNew(filePath);
        }

        /// <summary>
        /// Creates a file along the path, overwriting any existing file.
        /// </summary>
        [NotNull]
        public static DocxFilePath Create([NotNull] string filePath, bool overwrite)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!overwrite)
            {
                throw new Exception("This method requires approval to overwrite any existing file at the toPath.");
            }

            return CreateNew(filePath);
        }

        [NotNull]
        IPath IPath.Create([NotNull] string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return Create(path);
        }

        /// <summary>
        /// Creates a docx file from the given docx file.
        /// </summary>
        /// <exception cref="System.ArgumentException"/>
        [NotNull]
        private static DocxFilePath CreateNew([NotNull] string toPath)
        {
            if (toPath is null)
            {
                throw new ArgumentNullException(nameof(toPath));
            }

            using (ZipArchive archive = new ZipArchive(new FileStream(toPath, FileMode.OpenOrCreate), ZipArchiveMode.Update))
            {
                using (Stream entry = archive.CreateEntry("[Content_Types].xml").Open())
                {
                    entry.Write(ContentTypesXml, 0, ContentTypesXml.Length);
                }

                using (Stream entry = archive.CreateEntry("_rels/.rels").Open())
                {
                    entry.Write(RelsXml, 0, RelsXml.Length);
                }

                using (Stream entry = archive.CreateEntry("docProps/app.xml").Open())
                {
                    entry.Write(AppXml, 0, AppXml.Length);
                }

                using (Stream entry = archive.CreateEntry("docProps/core.xml").Open())
                {
                    entry.Write(CoreXml, 0, CoreXml.Length);
                }

                using (Stream entry = archive.CreateEntry("word/document.xml").Open())
                {
                    entry.Write(DocumentXml, 0, DocumentXml.Length);
                }

                using (Stream entry = archive.CreateEntry("word/_rels/document.xml.rels").Open())
                {
                    entry.Write(DocumentXmlRels, 0, DocumentXmlRels.Length);
                }

                using (Stream entry = archive.CreateEntry("word/theme/theme1.xml").Open())
                {
                    entry.Write(Theme1Xml, 0, Theme1Xml.Length);
                }

                using (Stream entry = archive.CreateEntry("word/settings.xml").Open())
                {
                    entry.Write(SettingsXml, 0, SettingsXml.Length);
                }

                using (Stream entry = archive.CreateEntry("word/styles.xml").Open())
                {
                    entry.Write(StylesXml, 0, StylesXml.Length);
                }

                using (Stream entry = archive.CreateEntry("word/footnotes.xml").Open())
                {
                    entry.Write(FootnotesXml, 0, FootnotesXml.Length);
                }

                using (Stream entry = archive.CreateEntry("word/_rels/footnotes.xml.rels").Open())
                {
                    entry.Write(FootnotesXmlRels, 0, FootnotesXmlRels.Length);
                }
            }

            return new DocxFilePath(toPath);
        }

        /// <summary>
        /// Returns the file path.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public override string ToString()
        {
            return _path;
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return _path.AsEnumerable().GetEnumerator();
        }

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