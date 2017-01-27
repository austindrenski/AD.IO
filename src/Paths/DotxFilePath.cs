using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Path to a Microsoft Word template on the system. An exception is thrown if the file does not exist.
    /// </summary>
    [PublicAPI]
    public struct DotxFilePath : IPath
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
        /// Creates a new <see cref="DotxFilePath"/> to hold the path to a file.
        /// </summary>
        /// <param name="filePath">A string file path.</param>
        /// <exception cref="FileNotFoundException"/>
        public DotxFilePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            if (Path.GetExtension(filePath) != ".dotx")
            {
                throw new ArgumentException("Path is not a dotx file.");
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
        public static DotxFilePath Create(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            return new DotxFilePath(filePath);
        }

        IPath IPath.Create(string path)
        {
            return Create(path);
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
        public static implicit operator string(DotxFilePath filePath)
        {
            return filePath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a DotxFilePath. An exception is thrown if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        public static implicit operator DotxFilePath(string filePath)
        {
            return new DotxFilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a string as a DotxFilePath. An exception is thrown if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        public static implicit operator FilePath(DotxFilePath filePath)
        {
            return new FilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a DotxFilePath as a ZipFilePath. An exception is thrown if the file is not a zip file path, or if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="System.ArgumentException"/>
        public static explicit operator ZipFilePath(DotxFilePath filePath)
        {
            return new ZipFilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a DotxFilePath as a UrlPath. An exception is thrown if the file path is not a well-formed URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="System.ArgumentException"/>
        public static explicit operator UrlPath(DotxFilePath filePath)
        {
            return new UrlPath(filePath);
        }
    }
}
