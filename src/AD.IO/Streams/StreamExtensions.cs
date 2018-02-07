using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AD.IO.Streams
{
    /// <summary>
    /// Extensions to support stream operations.
    /// </summary>
    [PublicAPI]
    public static class StreamExtensions
    {
        /// <summary>
        /// Asynchronously copies the stream to a <see cref="MemoryStream" /> maintaining the current position if specified.
        /// This method throws an exception when: (1) reading is not supported, or (2) seeking is not supported and <paramref name="maintainPosition"/> is set to true.
        /// </summary>
        /// <param name="stream">
        /// The input stream.
        /// </param>
        /// <param name="maintainPosition">
        /// True to maintain relative positions within the streams; otherwise false.
        /// </param>
        /// <returns>
        /// A <see cref="MemoryStream" /> copy of the input stream.
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        [Pure]
        [NotNull]
        public static async Task<MemoryStream> CopyPure([NotNull] this Stream stream, bool maintainPosition = false)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new InvalidOperationException("The input stream does not support reading.");
            }

            if (maintainPosition && !stream.CanSeek)
            {
                throw new InvalidOperationException("The input stream does not support seeking.");
            }

            long position = maintainPosition ? stream.Position : default;

            stream.Seek(0, SeekOrigin.Begin);

            MemoryStream result = new MemoryStream();
            await stream.CopyToAsync(result);

            stream.Seek(position, SeekOrigin.Begin);
            result.Seek(position, SeekOrigin.Begin);

            return result;
        }
    }
}