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
    /// Path to a zip file on the system. An exception is thrown if the file does not exist, or if it is not a zip file.
    /// </summary>
    [PublicAPI]
    public class ZipFilePath : IPath
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
        /// Creates a new ZipFilePath object to hold the path to a zip file.
        /// </summary>
        /// <param name="zipFilePath">A string zip file path.</param>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public ZipFilePath(string zipFilePath)
        {
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException();
            }
            if (Path.GetExtension(zipFilePath) != ".zip")
            {
                throw new ArgumentException("Path is not a zip file.");
            }
            _path = zipFilePath;
            Extension = Path.GetExtension(zipFilePath);
            Name = Path.GetFileNameWithoutExtension(zipFilePath);
        }

        /// <summary>
        /// Creates a zip file along the path if one does not exist.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static ZipFilePath Create(string zipFilePath)
        {
            if (!File.Exists(zipFilePath))
            {
                File.Create(zipFilePath).Dispose();
            }
            if (Path.GetExtension(zipFilePath) != ".zip")
            {
                throw new ArgumentException("Path is not a zip file.");
            }
            return new ZipFilePath(zipFilePath);
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
        /// Returns the zip file path.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Implicitly casts a ZipFilePath as its internal zip file path string.
        /// </summary>
        public static implicit operator string(ZipFilePath zipFilePath)
        {
            return zipFilePath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a ZipFilePath. An exception is thrown if the zip file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static implicit operator ZipFilePath(string zipFilePath)
        {
            return new ZipFilePath(zipFilePath);
        }

        /// <summary>
        /// Implicitly casts a ZipFilePath as a FilePath.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static explicit operator FilePath(ZipFilePath zipFilePath)
        {
            return new FilePath(zipFilePath);
        }

        /// <summary>
        /// Implicitly casts a FilePath as a UrlPath. An exception is thrown if the zip file path is not a well-formed URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static explicit operator UrlPath(ZipFilePath zipFilePath)
        {
            return new UrlPath(new Uri(zipFilePath).AbsoluteUri);
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