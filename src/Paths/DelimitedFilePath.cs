using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Path to a delimited file on the system. An exception is thrown if the file does not exist, or if it is not a delimited file.
    /// </summary>
    [PublicAPI]
    public struct DelimitedFilePath : IPath
    {
        /// <summary>
        /// The full file path.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// The file path extension.
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// The file name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The items from the first row of the file.
        /// </summary>
        public IEnumerable<string> Headers { get; }

        /// <summary>
        /// The the first row of the file.
        /// </summary>
        public string HeaderRow { get; }

        /// <summary>
        /// The character that delimits data in the file.
        /// </summary>
        public char Delimiter { get; }

        /// <summary>
        /// Creates a new DelimitedFilePath object to hold the path to a delimited file.
        /// </summary>
        /// <param name="delimitedFilePath">A string delimited file path.</param>
        /// <param name="delimiter">The character that delimits the file.</param>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public DelimitedFilePath(string delimitedFilePath, char delimiter)
        {
            if (!File.Exists(delimitedFilePath))
            {
                throw new FileNotFoundException();
            }
            if (Path.GetExtension(delimitedFilePath) != ".csv")
            {
                throw new ArgumentException("Path is not a delimited file.");
            }
            _path = delimitedFilePath;
            Extension = Path.GetExtension(delimitedFilePath);
            Name = Path.GetFileNameWithoutExtension(delimitedFilePath);
            HeaderRow = File.ReadLines(delimitedFilePath).FirstOrDefault();
            Headers = HeaderRow?.SplitDelimitedLine(',');
            Delimiter = delimiter;
        }

        /// <summary>
        /// Creates a delimited file along the path if one does not exist.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static DelimitedFilePath Create(string delimitedFilePath, char delimiter)
        {
            if (Path.GetExtension(delimitedFilePath) != ".csv")
            {
                throw new ArgumentException("Path is not a delimited file.");
            }
            if (!File.Exists(delimitedFilePath))
            {
                File.Create(delimitedFilePath).Dispose();
            }
            return new DelimitedFilePath(delimitedFilePath, delimiter);
        }

        /// <summary>
        /// Explicit IPath implementation.
        /// </summary>
        IPath IPath.Create(string path)
        {
            return Create(path, ',');
        }

        /// <summary>
        /// Returns the delimited file path.
        /// </summary>
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Implicitly casts a DelimitedFilePath as its internal delimited file path string.
        /// </summary>
        public static implicit operator string(DelimitedFilePath delimitedFilePath)
        {
            return delimitedFilePath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a DelimitedFilePath. An exception is thrown if the delimited file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static implicit operator DelimitedFilePath(string delimitedFilePath)
        {
            return new DelimitedFilePath(delimitedFilePath, ',');
        }

        /// <summary>
        /// Implicitly casts a DelimitedFilePath as a FilePath.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static explicit operator FilePath(DelimitedFilePath delimitedFilePath)
        {
            return new FilePath(delimitedFilePath);
        }

        /// <summary>
        /// Implicitly casts a FilePath as a UrlPath. An exception is thrown if the delimited file path is not a well-formed URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        public static explicit operator UrlPath(DelimitedFilePath delimitedFilePath)
        {
            return new UrlPath(delimitedFilePath);
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