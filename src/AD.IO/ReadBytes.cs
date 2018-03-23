using System;
using System.IO;
using System.IO.Compression;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <summary>
    /// Extension methods to read data as byte arrays.
    /// </summary>
    [PublicAPI]
    public static class ReadBytesExtensions
    {
        /// <summary>
        /// The empty array.
        /// </summary>
        private static readonly byte[] Empty = new byte[0];

        /// <summary>
        /// Reads a byte array from an entry path in the archive.
        /// </summary>
        /// <param name="archive">
        /// The archive from which to read the <paramref name="entryPath"/>.
        /// </param>
        /// <param name="entryPath">
        /// The path to read from within the archive.
        /// </param>
        /// <returns>
        /// The path entry as a byte array.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        [Pure]
        [NotNull]
        public static byte[] ReadByteArray([NotNull] this ZipArchive archive, [NotNull] string entryPath)
        {
            if (archive is null)
            {
                throw new ArgumentNullException(nameof(archive));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            ZipArchiveEntry entry = archive.GetEntry(entryPath);

            if (entry is null)
            {
                return Empty;
            }

            using (Stream stream = entry.Open())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Reads a byte array from the entry path from a stream representing a zipped archive.
        /// </summary>
        /// <param name="stream">
        /// The stream to read from as a zipped archive.
        /// </param>
        /// <param name="entryPath">
        /// The path to read from within the zipped archive.
        /// </param>
        /// <param name="restorePosition">
        /// True if (when applicable) the stream should reset the initial position after reading.
        /// </param>
        /// <returns>
        /// The path entry as an array of bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        [Pure]
        [NotNull]
        public static byte[] ReadByteArray([NotNull] this Stream stream, [NotNull] string entryPath, bool restorePosition = true)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (entryPath is null)
            {
                throw new ArgumentNullException(nameof(entryPath));
            }

            long position = stream.Position;

            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                if (restorePosition && stream.CanSeek)
                {
                    stream.Seek(position, SeekOrigin.Begin);
                }

                return archive.ReadByteArray(entryPath);
            }
        }
    }
}