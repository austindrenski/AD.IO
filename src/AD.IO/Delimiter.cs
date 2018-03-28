using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Primitives;

namespace AD.IO
{
    /// <inheritdoc cref="IEquatable{T}" />
    /// <summary>
    /// Defines a read-only struct that represents delimiter information.
    /// </summary>
    [PublicAPI]
    public readonly struct Delimiter : IEquatable<Delimiter>
    {
        /// <summary>
        /// A parenthetical delimiter in which strings are split by commas, except where enclosed by quotes.
        /// </summary>
        public static readonly Delimiter Comma = new Delimiter(',', '"', '"');

        /// <summary>
        /// A parenthetical delimiter in which strings are split by commas, except where enclosed by parenthese.
        /// </summary>
        public static readonly Delimiter Parenthetical = new Delimiter(',', '(', ')');

        /// <summary>
        /// A parenthetical delimiter in which strings are split by pipes, except where enclosed by quotes.
        /// </summary>
        public static readonly Delimiter Pipe = new Delimiter('|', '"', '"');

        /// <summary>
        /// A parenthetical delimiter in which strings are split by tabs, except where enclosed by quotes.
        /// </summary>
        public static readonly Delimiter Tab = new Delimiter('\t', '"', '"');

        /// <summary>
        /// The separator character.
        /// </summary>
        public char Separator { get; }

        /// <summary>
        /// The open character.
        /// </summary>
        public char Open { get; }

        /// <summary>
        /// The close character.
        /// </summary>
        public char Close { get; }

        ///  <summary>
        /// Constructs a <see cref="Delimiter"/>.
        ///  </summary>
        /// <param name="separator">
        /// The separator character.
        /// </param>
        /// <param name="open">
        /// The open character.
        /// </param>
        /// <param name="close">
        /// The close character.
        /// </param>
        public Delimiter(char separator, char open, char close)
        {
            Open = open;
            Close = close;
            Separator = separator;
        }

        /// <summary>
        /// Splits the string based on the current <see cref="Delimiter"/>.
        /// </summary>
        /// <param name="value">
        /// The string to split.
        /// </param>
        /// <param name="removeEnclosure">
        /// True if the open and close characters should be removed when present as the leading and trailing characters; otherwise false.
        /// </param>
        /// <returns>
        /// An enumerable of <see cref="StringSegment"/> instances.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        public IEnumerable<StringSegment> Split([NotNull] string value, bool removeEnclosure = false)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            StringSegment remainder = value;
            while (remainder != StringSegment.Empty)
            {
                StringSegment result;
                (result, remainder) = NextSegment(remainder);

                if (removeEnclosure && result[0] == Open && result[result.Length - 1] == Close)
                {
                    yield return result.Subsegment(1, result.Length - 2);
                }
                else
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Splits the next segment based on the current <see cref="Delimiter"/>.
        /// </summary>
        /// <param name="segment">
        /// The string segment to split.
        /// </param>
        /// <returns>
        /// A tuple of the result segment and the remainder.
        /// </returns>
        [Pure]
        public (StringSegment Result, StringSegment Remainder) NextSegment(StringSegment segment)
        {
            for (int i = 0; i < segment.Length; i++)
            {
                if (segment[i] == Open)
                {
                    return GetSubExpression(segment, i);
                }

                if (segment[i] == Separator)
                {
                    return (segment.Subsegment(0, i), i + 1 < segment.Length ? segment.Subsegment(i + 1) : StringSegment.Empty);
                }
            }

            return (segment, StringSegment.Empty);
        }

        /// <summary>
        /// Splits the next segment based on the current <see cref="Delimiter"/>.
        /// </summary>
        /// <param name="segment">
        /// The string segment to split.
        /// </param>
        /// <param name="openIndex">
        /// The index of the open character.
        /// </param>
        /// <returns>
        /// A tuple of the result segment and the remainder.
        /// </returns>
        /// <exception cref="ArgumentException"/>
        [Pure]
        private (StringSegment Result, StringSegment Remainder) GetSubExpression(StringSegment segment, int openIndex)
        {
            for (int i = openIndex + 1; i < segment.Length; i++)
            {
                if (segment[i] == Close)
                {
                    return (segment.Subsegment(0, i + 1), i + 2 < segment.Length ? segment.Subsegment(i + 2) : StringSegment.Empty);
                }
            }

            throw new ArgumentException($"Unbounded subexpression. The string segment '{segment}' does not contain the closing character: '{Close}'.");
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"(separator: '{Separator}', open: '{Open}', close: '{Close}')";
        }

        /// <inheritdoc />
        public bool Equals(Delimiter other)
        {
            return Separator == other.Separator && Open == other.Open && Close == other.Close;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Delimiter delimiter && Equals(delimiter);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 397 * Separator.GetHashCode();
                hashCode ^= 397 * Open.GetHashCode();
                hashCode ^= 397 * Close.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Indicates whether two <see cref="Delimiter"/> instances are equal.
        /// </summary>
        /// <param name="left">
        /// The left item.
        /// </param>
        /// <param name="right">
        /// The right item.
        /// </param>
        /// <returns>
        /// True if equal; otherwise false.
        /// </returns>
        public static bool operator ==(Delimiter left, Delimiter right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether two <see cref="Delimiter"/> instances are not equal.
        /// </summary>
        /// <param name="left">
        /// The left item.
        /// </param>
        /// <param name="right">
        /// The right item.
        /// </param>
        /// <returns>
        /// True if equal; otherwise false.
        /// </returns>
        public static bool operator !=(Delimiter left, Delimiter right)
        {
            return !left.Equals(right);
        }
    }
}