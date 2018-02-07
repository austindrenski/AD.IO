using System.Collections.Generic;
using JetBrains.Annotations;

namespace AD.IO.Paths
{
    /// <inheritdoc />
    /// <summary>
    /// Defines common functionality for system paths.
    /// </summary>
    [PublicAPI]
    public interface IPath : IEnumerable<char>
    {
        /// <summary>
        /// The file extension or null.
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// The file name or null.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Creates a new path.
        /// </summary>
        IPath Create(string path);

        /// <summary>
        /// Returns the internal string path.
        /// </summary>
        string ToString();
    }
}