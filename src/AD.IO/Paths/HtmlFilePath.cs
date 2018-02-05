using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <inheritdoc />
    /// <summary>
    /// Path to an HTML file on the system. An exception is thrown if the file does not exist, or if it is not an HTML file.
    /// </summary>
    [PublicAPI]
    public class HtmlFilePath : IPath
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
        /// Creates a new <see cref="HtmlFilePath"/> object to hold the path to an HTML file.
        /// </summary>
        /// <param name="htmlFilePath">A string HTML file path.</param>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public HtmlFilePath(string htmlFilePath)
        {
            if (!File.Exists(htmlFilePath))
            {
                throw new FileNotFoundException();
            }
            if (Path.GetExtension(htmlFilePath) != ".html")
            {
                throw new ArgumentException("Path is not an HTML file.");
            }
            _path = htmlFilePath;
            Extension = Path.GetExtension(htmlFilePath);
            Name = Path.GetFileNameWithoutExtension(htmlFilePath);
        }

        /// <summary>
        /// Creates an HTML file along the path if one does not exist.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static HtmlFilePath Create(string htmlFilePath)
        {
            if (!File.Exists(htmlFilePath))
            {
                File.Create(htmlFilePath).Dispose();
            }
            return new HtmlFilePath(htmlFilePath);
        }

        /// <inheritdoc />
        /// <summary>
        /// Explicit <see cref="IPath"/> implementation.
        /// </summary>
        IPath IPath.Create(string path)
        {
            return Create(path);
        }

        /// <summary>
        /// Returns the internal HTML file path string.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Implicitly casts a <see cref="HtmlFilePath"/> as its internal HTML file path string.
        /// </summary>
        public static implicit operator string(HtmlFilePath htmlFilePath)
        {
            return htmlFilePath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a <see cref="HtmlFilePath"/>. An exception is thrown if the HTML file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static implicit operator HtmlFilePath(string htmlFilePath)
        {
            return new HtmlFilePath(htmlFilePath);
        }

        /// <summary>
        /// Implicitly casts a <see cref="HtmlFilePath"/> as a <see cref="FileNotFoundException"/>.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="FilePath"/>
        public static explicit operator FilePath(HtmlFilePath htmlFilePath)
        {
            return new FilePath(htmlFilePath);
        }

        /// <summary>
        /// Implicitly casts an <see cref="HtmlFilePath"/> as a <see cref="UrlPath"/>. An exception is thrown if the HTML file path is not a well-formed URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static explicit operator UrlPath(HtmlFilePath htmlFilePath)
        {
            return new UrlPath(htmlFilePath);
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