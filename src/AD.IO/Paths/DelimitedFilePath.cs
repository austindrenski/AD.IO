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
    /// Path to a delimited file on the system. An exception is thrown if the file does not exist, or if it is not a delimited file.
    /// </summary>
    [PublicAPI]
    public class DelimitedFilePath : IPath
    {
        /// <summary>
        /// The full file path.
        /// </summary>
        [NotNull]
        private readonly string _path;

        /// <inheritdoc />
        /// <summary>
        /// The file path extension.
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
        /// The items from the first row of the file.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> Headers { get; }

        /// <summary>
        /// The the first row of the file.
        /// </summary>
        [CanBeNull]
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
        public DelimitedFilePath([NotNull] string delimitedFilePath, char delimiter = '|')
        {
            if (delimitedFilePath is null)
            {
                throw new ArgumentNullException(nameof(delimitedFilePath));
            }
            if (!File.Exists(delimitedFilePath))
            {
                throw new FileNotFoundException();
            }

            _path = delimitedFilePath;

            Delimiter = delimiter;

            Name = Path.GetFileNameWithoutExtension(delimitedFilePath);

            Extension = Path.GetExtension(delimitedFilePath);

            using (FileStream fileStream = new FileStream(delimitedFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    HeaderRow = reader.ReadLine();
                }
            }

            Headers = HeaderRow?.SplitDelimitedLine(delimiter) ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// Creates a delimited file along the path if one does not exist.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        [NotNull]
        public static DelimitedFilePath Create([NotNull] string delimitedFilePath, char delimiter)
        {
            if (delimitedFilePath is null)
            {
                throw new ArgumentNullException(nameof(delimitedFilePath));
            }
            if (!File.Exists(delimitedFilePath))
            {
                File.Create(delimitedFilePath).Dispose();
            }

            return new DelimitedFilePath(delimitedFilePath, delimiter);
        }

        /// <summary>
        /// Creates a delimited file along the path if one does not exist.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        [NotNull]
        public static DelimitedFilePath Create([NotNull] string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return Create(path, '|');
        }

        /// <inheritdoc />
        /// <summary>
        /// Explicit IPath implementation.
        /// </summary>
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
        /// Returns the delimited file path.
        /// </summary>
        [NotNull]
        // ReSharper disable once InheritdocConsiderUsage
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Implicitly casts a DelimitedFilePath as its internal delimited file path string.
        /// </summary>
        [NotNull]
        public static implicit operator string([NotNull] DelimitedFilePath delimitedFilePath)
        {
            if (delimitedFilePath is null)
            {
                throw new ArgumentNullException(nameof(delimitedFilePath));
            }

            return delimitedFilePath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a DelimitedFilePath. An exception is thrown if the delimited file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        [NotNull]
        public static implicit operator DelimitedFilePath([NotNull] string delimitedFilePath)
        {
            if (delimitedFilePath is null)
            {
                throw new ArgumentNullException(nameof(delimitedFilePath));
            }

            return new DelimitedFilePath(delimitedFilePath);
        }

        /// <summary>
        /// Implicitly casts a DelimitedFilePath as a FilePath.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        [NotNull]
        public static explicit operator FilePath([NotNull] DelimitedFilePath delimitedFilePath)
        {
            if (delimitedFilePath is null)
            {
                throw new ArgumentNullException(nameof(delimitedFilePath));
            }

            return new FilePath(delimitedFilePath);
        }

        /// <summary>
        /// Implicitly casts a FilePath as a UrlPath. An exception is thrown if the delimited file path is not a well-formed URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="ArgumentException"/>
        [NotNull]
        public static explicit operator UrlPath([NotNull] DelimitedFilePath delimitedFilePath)
        {
            if (delimitedFilePath is null)
            {
                throw new ArgumentNullException(nameof(delimitedFilePath));
            }

            Uri uri = new Uri(delimitedFilePath);
            return new UrlPath(uri.AbsoluteUri);
        }

        [NotNull]
        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return _path.AsEnumerable().GetEnumerator();
        }

        [NotNull]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _path.AsEnumerable().GetEnumerator();
        }
    }
}