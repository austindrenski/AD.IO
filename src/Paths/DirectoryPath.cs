using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Path to a directory on the system. An exception is thrown if the directory does not exist.
    /// </summary>
    [PublicAPI]
    public struct DirectoryPath : IPath
    {
        /// <summary>
        /// The full directory path.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// The file path extension.
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// The directory name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new DirectoryPath object to hold the path to a directory.
        /// </summary>
        /// <param name="directoryPath">A string directory path.</param>
        /// <exception cref="DirectoryNotFoundException"/>
        public DirectoryPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException();
            }
            _path = directoryPath;
            Extension = null;
            Name = Path.GetDirectoryName(directoryPath);
        }

        /// <summary>
        /// Creates a directory along the path if one does not exist.
        /// </summary>
        public static DirectoryPath Create(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
            return new DirectoryPath(directoryPath);
        }

        /// <summary>
        /// Explicit IPath implementation.
        /// </summary>
        IPath IPath.Create(string path)
        {
            return Create(path);
        }
        
        /// <summary>
        /// Returns the directory path.
        /// </summary>
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Implicitly casts a DirectoryPath as its internal directory path string.
        /// </summary>
        public static implicit operator string(DirectoryPath directoryPath)
        {
            return directoryPath._path;
        }

        /// <summary>
        /// Implicitly casts a string as a DirectoryPath. An exception is thrown if the directory is not found.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException"/>
        public static implicit operator DirectoryPath(string directoryPath)
        {
            return new DirectoryPath(directoryPath);
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