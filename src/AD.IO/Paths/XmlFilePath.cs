using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO.Paths
{
    /// <inheritdoc />
    /// <summary>
    /// Path to an XML file on the system. An exception is thrown if the file does not exist, or if it is not an XML file.
    /// </summary>
    [PublicAPI]
    public class XmlFilePath : IPath
    {
        /// <summary>
        /// The full file path.
        /// </summary>
        private readonly string _path;

        /// <inheritdoc />
        /// <summary>
        /// The file path extension.
        /// </summary>
        public string Extension { get; }

        /// <inheritdoc />
        /// <summary>
        /// The file name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new XmlFilePath object to hold the path to an XML file.
        /// </summary>
        /// <param name="xmlFilePath">A string XML file path.</param>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public XmlFilePath(string xmlFilePath)
        {
            if (!File.Exists(xmlFilePath))
            {
                throw new FileNotFoundException();
            }
            if (Path.GetExtension(xmlFilePath) != ".xml")
            {
                throw new ArgumentException("Path is not an XML file.");
            }
            _path = xmlFilePath;
            Extension = Path.GetExtension(xmlFilePath);
            Name = Path.GetFileNameWithoutExtension(xmlFilePath);
        }

        /// <summary>
        /// Creates an XML file along the path if one does not exist.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static XmlFilePath Create(string xmlFilePath)
        {
            if (!File.Exists(xmlFilePath))
            {
                File.Create(xmlFilePath).Dispose();
            }
            return new XmlFilePath(xmlFilePath);
        }

        /// <inheritdoc />
        /// <summary>
        /// Explicit IPath implementation.
        /// </summary>
        IPath IPath.Create(string path)
        {
            return Create(path);
        }

        /// <summary>
        /// Returns the XML file path.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Implicitly casts a XmlFilePath as its internal XML file path string.
        /// </summary>
        public static implicit operator string(XmlFilePath xmlFilePath)
        {
            return xmlFilePath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a XmlFilePath. An exception is thrown if the XML file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static implicit operator XmlFilePath(string xmlFilePath)
        {
            return new XmlFilePath(xmlFilePath);
        }

        /// <summary>
        /// Implicitly casts a XmlFilePath as a FilePath.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static explicit operator FilePath(XmlFilePath xmlFilePath)
        {
            return new FilePath(xmlFilePath);
        }

        /// <summary>
        /// Implicitly casts a FilePath as a UrlPath. An exception is thrown if the XML file path is not a well-formed URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static explicit operator UrlPath(XmlFilePath xmlFilePath)
        {
            return new UrlPath(xmlFilePath);
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return _path.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _path.AsEnumerable().GetEnumerator();
        }
    }
}