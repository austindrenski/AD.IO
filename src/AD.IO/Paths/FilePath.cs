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
    /// Path to a file on the system. An exception is thrown if the file does not exist.
    /// </summary>
    [PublicAPI]
    public class FilePath : IPath
    {
        /// <summary>
        /// The full file path.
        /// </summary>
        readonly string _path;

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
        /// Creates a new FilePath object to hold the path to a file.
        /// </summary>
        /// <param name="filePath">A string file path.</param>
        /// <exception cref="FileNotFoundException"/>
        public FilePath([NotNull] string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            _path = filePath;
            Extension = Path.GetExtension(filePath);
            Name = Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// Creates a file along the path if one does not exist.
        /// </summary>
        [NotNull]
        public static FilePath Create([NotNull] string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (File.Exists(filePath))
                return new FilePath(filePath);

            using (File.Create(filePath)) {}

            return new FilePath(filePath);
        }

        /// <inheritdoc />
        /// <summary>
        /// Explicit IPath implementation.
        /// </summary>
        [NotNull]
        IPath IPath.Create([NotNull] string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            return Create(path);
        }

        /// <summary>
        /// Returns the file path.
        /// </summary>
        public override string ToString() => _path;

        IEnumerator<char> IEnumerable<char>.GetEnumerator() => _path.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _path.AsEnumerable().GetEnumerator();

        /// <summary>
        /// Implicitly casts a FilePath as its internal file path string.
        /// </summary>
        [CanBeNull]
        public static implicit operator string([CanBeNull] FilePath filePath) => filePath?._path;

        /// <summary>
        /// Implicitly casts a string as a FilePath. An exception is thrown if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        [NotNull]
        public static implicit operator FilePath([NotNull] string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            return new FilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a FilePath as a DocxFilePath. An exception is thrown if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        [NotNull]
        public static implicit operator DocxFilePath([NotNull] FilePath filePath) => new DocxFilePath(filePath);

        /// <summary>
        /// Implicitly casts a FilePath as a ZipFilePath. An exception is thrown if the file is not a zip file path, or if the file is not found.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="System.ArgumentException"/>
        [NotNull]
        public static explicit operator ZipFilePath([NotNull] FilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            return new ZipFilePath(filePath);
        }

        /// <summary>
        /// Implicitly casts a FilePath as a UrlPath. An exception is thrown if the file path is not a well-formed URI.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="System.ArgumentException"/>
        [NotNull]
        public static explicit operator UrlPath([NotNull] FilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            return new UrlPath(new Uri(filePath).AbsoluteUri);
        }
    }
}