using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace AD.IO
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a stream with an observed length but no backing store.
    /// </summary>
    [PublicAPI]
    public class ObservableNullStream : Stream
    {
        /// <summary>
        /// The observed length in bytes of the stream.
        /// </summary>
        private long _observedLength;

        /// <inheritdoc />
        public override bool CanRead => Null.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => Null.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => Null.CanWrite;

        /// <inheritdoc />
        /// <summary>
        /// The observed length in bytes of the stream.
        /// </summary>
        public override long Length => _observedLength;

        /// <inheritdoc />
        public override long Position
        {
            get => Null.Position;
            set => Null.Position = value;
        }

        /// <inheritdoc />
        public override void Flush()
        {
            Null.Flush();
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            return Null.Read(buffer, offset, count);
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            return Null.Seek(offset, origin);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets the length of the observed stream.
        /// </summary>
        public override void SetLength(long value)
        {
            Null.SetLength(value);
            _observedLength = value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds the count to the length of the observed stream.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            Null.Write(buffer, offset, count);
            _observedLength += count;
        }
    }
}