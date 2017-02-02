using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using AD.IO.Properties;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Path to a Microsoft Word file on the system. An exception is thrown if the file does not exist.
    /// </summary>
    [PublicAPI]
    public class DocxFilePath : IPath
    {
        /// <summary>
        /// The full file path.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// The file extension.
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// The file name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new <see cref="DocxFilePath"/> to hold the path to a file.
        /// </summary>
        /// <param name="filePath">A string file path.</param>
        /// <exception cref="FileNotFoundException"/>
        public DocxFilePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
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
        public static DocxFilePath Create(string filePath)
        {
            return File.Exists(filePath) ? new DocxFilePath(filePath) : CreateNew(filePath);
        }

        /// <summary>
        /// Creates a file along the path, overwriting any existing file.
        /// </summary>
        public static DocxFilePath Create(string filePath, bool overwrite)
        {
            if (!overwrite)
            {
                throw new Exception("This method requires approval to overwrite any existing file at the toPath.");
            }
            return CreateNew(filePath);
        }

        IPath IPath.Create(string path)
        {
            return Create(path);
        }

        /// <summary>
        /// Creates a docx file from the given docx file.
        /// </summary>
        /// <exception cref="System.ArgumentException"/>
        private static DocxFilePath CreateNew(string toPath)
        {
            string directory = Path.GetTempFileName().Replace(".tmp", null);
            Directory.CreateDirectory(directory);
            if (File.Exists(toPath))
            {
                File.Delete(toPath);
            }
            ZipFile.CreateFromDirectory(directory, toPath);
            Directory.Delete(directory);

            using (ZipArchive archive = ZipFile.Open(toPath, ZipArchiveMode.Update))
            {
                archive.CreateEntry("[Content_Types].xml");
                using (StreamWriter writer = new StreamWriter(archive.GetEntry("[Content_Types].xml").Open()))
                {
                    writer.Write(XElement.Parse(Resources._Content_Types_));
                }

                archive.CreateEntry("_rels/.rels");
                using (StreamWriter writer = new StreamWriter(archive.GetEntry("_rels/.rels").Open()))
                {
                    writer.Write(XElement.Parse(Resources.rels));
                }

                archive.CreateEntry("docProps/app.xml");
                using (StreamWriter writer = new StreamWriter(archive.GetEntry("docProps/app.xml").Open()))
                {
                    writer.Write(XElement.Parse(Resources.app));
                }

                archive.CreateEntry("docProps/core.xml");
                using (StreamWriter writer = new StreamWriter(archive.GetEntry("docProps/core.xml").Open()))
                {
                    writer.Write(XElement.Parse(Resources.core));
                }

                archive.CreateEntry("word/_rels/document.xml.rels");
                using (StreamWriter writer = new StreamWriter(archive.GetEntry("word/_rels/document.xml.rels").Open()))
                {
                    writer.Write(XElement.Parse(Resources.document_xml_rels));
                }

                archive.CreateEntry("word/theme/theme1.xml");
                using (StreamWriter writer = new StreamWriter(archive.GetEntry("word/theme/theme1.xml").Open()))
                {
                    writer.Write(XElement.Parse(Resources.theme1));
                }

                archive.CreateEntry("word/document.xml");
                using (StreamWriter writer = new StreamWriter(archive.GetEntry("word/document.xml").Open()))
                {
                    writer.Write(XElement.Parse(Resources.document));
                }

                archive.CreateEntry("word/settings.xml");
                using (StreamWriter writer = new StreamWriter(archive.GetEntry("word/settings.xml").Open()))
                {
                    writer.Write(XElement.Parse(Resources.settings));
                }

                archive.CreateEntry("word/styles.xml");
                using (StreamWriter writer = new StreamWriter(archive.GetEntry("word/styles.xml").Open()))
                {
                    writer.Write(XElement.Parse(Resources.styles));
                }
            }
            return new DocxFilePath(toPath);
        }

        /// <summary>
        /// Returns the file path.
        /// </summary>
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
        public static implicit operator string(DocxFilePath filePath)
        {
            return filePath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a DocxFilePath. An exception is thrown if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        public static implicit operator DocxFilePath(string filePath)
        {
            return new DocxFilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a string as a DocxFilePath. An exception is thrown if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        public static implicit operator FilePath(DocxFilePath filePath)
        {
            return new FilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a DocxFilePath as a ZipFilePath. An exception is thrown if the file is not a zip file path, or if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="System.ArgumentException"/>
        public static explicit operator ZipFilePath(DocxFilePath filePath)
        {
            return new ZipFilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a DocxFilePath as a UrlPath. An exception is thrown if the file path is not a well-formed URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="System.ArgumentException"/>
        public static explicit operator UrlPath(DocxFilePath filePath)
        {
            return new UrlPath(filePath);
        }
    }
}