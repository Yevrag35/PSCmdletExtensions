using MG.Posh.Extensions.Spans.Internal;
using Microsoft.PowerShell.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Management.Automation;

namespace MG.Posh.Extensions.Spans
{
    /// <summary>
    /// A read-only <see cref="string"/> that can be used for checking equality and pattern matching based on traditional wildcard characters.
    /// </summary>
    /// <remarks>A lightweight alternative to <see cref="WildcardPattern"/>.</remarks>
    public readonly struct WildcardString : IComparable<string>, IComparable<WildcardString>, IEquatable<WildcardString>, IEquatable<string>, IEnumerable<char>, ISpanFormattable
#if NET8_0_OR_GREATER
    , ISpanParsable<WildcardString>
#endif
    {
        const char QUESTION = '?';
        const char STAR = '*';

        readonly bool _containsWildcards;
        readonly int _length;
        readonly bool _isNotEmpty;
        readonly string? _pattern;

        /// <summary>
        /// Gets the <see cref="char"/> object at the specified position in the current 
        /// <see cref="WildcardString"/> instance.
        /// </summary>
        /// <param name="index">The position in the current string.</param>
        /// <returns>The char object at the specified index.</returns>
        public char this[int index] => _pattern?[index] ?? default;

        /// <summary>
        /// Indicates whether this instance contains any wildcard 
        /// characters ('?' or '*') in the string.
        /// </summary>
        /// <remarks>
        ///     If <see langword="false"/>, during the <see cref="IsMatch(ReadOnlySpan{char})"/> and
        ///     <see cref="IsMatch(string?)"/> method executions, only strict <see cref="char"/> equality 
        ///     will be checked.
        /// </remarks>
        /// <returns>
        ///     <see langword="true"/> if the string contains at least 1 wildcard character; otherwise,
        ///     <see langword="false"/>.
        /// </returns>
        [MemberNotNullWhen(true, nameof(_pattern))]
        public bool ContainsWildcards => _containsWildcards;

        /// <summary>
        /// Indicates whether the <see cref="WildcardString"/> instance is equal to 
        /// <see cref="string.Empty"/>.
        /// </summary>
        [MemberNotNullWhen(false, nameof(_pattern))]
        public bool IsEmpty => !_isNotEmpty;
        /// <summary>
        /// Gets the number of characters in the current <see cref="WildcardString"/> instance.
        /// </summary>
        public int Length => _length;

        private WildcardString(ReadOnlySpan<char> span)
        {
            bool notEmpty = span.IsEmpty;
            _isNotEmpty = notEmpty;
            _length = span.Length;
            _containsWildcards = notEmpty && ContainsWildcardCharacters(span);
            _pattern = notEmpty ? new string(span) : string.Empty;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WildcardString"/> struct using the 
        /// specified <see cref="string"/>.
        /// </summary>
        /// <param name="pattern">The string pattern to use.</param>
        public WildcardString(string? pattern)
        {
            bool notEmpty = !string.IsNullOrEmpty(pattern);
            _isNotEmpty = notEmpty;
            pattern ??= string.Empty;
            _length = pattern.Length;
            bool containsWc = notEmpty && ContainsWildcardCharacters(pattern);
            _containsWildcards = containsWc;
            _pattern = pattern;
        }

        private static bool AreCharactersEqual(in char x, in char y, in bool ignoreCase)
        {
            return !ignoreCase
                ? x == y
                : char.ToUpperInvariant(x) == char.ToUpperInvariant(y);
        }
        /// <summary>
        /// Creates a new read-only span over the <see cref="WildcardString"/> object.
        /// </summary>
        /// <returns>The read-only span representation of the <see cref="WildcardString"/>.</returns>
        public ReadOnlySpan<char> AsSpan()
        {
            return _pattern.AsSpan();
        }
        /// <summary>
        /// Creates a new read-only span over a portion of the <see cref="WildcardString"/> object from a specified
        /// position to the end of the string.
        /// </summary>
        /// <param name="start">The zero-based index at which to begin this slice.</param>
        /// <returns>The read-only span representation of the <see cref="WildcardString"/>.</returns>
        public ReadOnlySpan<char> AsSpan(int start)
        {
            return _pattern.AsSpan(start);
        }
        /// <summary>
        /// Creates a new read-only span over a portion of the <see cref="WildcardString"/> object from a specified
        /// position for a specified number of characters.
        /// </summary>
        /// <param name="start">The zero-based index at which to begin this slice.</param>
        /// <param name="length">The desired length for the slice.</param>
        /// <returns>The read-only span representation of the <see cref="WildcardString"/>.</returns>
        public ReadOnlySpan<char> AsSpan(int start, int length)
        {
            return _pattern.AsSpan(start, length);
        }
        /// <inheritdoc cref="string.CompareTo(string?)"/>
        public int CompareTo(string? strB)
        {
            return StringComparer.CurrentCulture.Compare(_pattern ?? string.Empty, strB);
        }
        public int CompareTo(WildcardString other)
        {
            return this.CompareTo(other._pattern ?? string.Empty);
        }

        private static bool ContainsWildcardCharacters(ReadOnlySpan<char> pattern)
        {
            return pattern.IndexOfAny(stackalloc char[] { STAR, QUESTION }) >= 0;
        }

        public bool Equals(WildcardString other)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals(_pattern, other._pattern);
        }
        /// <inheritdoc cref="string.Equals(string?)"/>
        public bool Equals(string? value)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals(_pattern, other);
        }
        /// <inheritdoc cref="string.Equals(string?, StringComparison)"/>
        public bool Equals(string? value, StringComparison comparisonType)
        {
            return StringComparer.FromComparison(comparisonType).Equals(_pattern ?? string.Empty, other);
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is WildcardString other)
            {
                return this.Equals(other);
            }
            else if (obj is string s)
            {
                return this.Equals(new(s));
            }

            return false;
        }
        /// <inheritdoc cref="string.GetEnumerator"/>
        public IEnumerator<char> GetEnumerator()
        {
            return _pattern?.GetEnumerator() ?? Enumerable.Empty<char>().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <inheritdoc cref="string.GetHashCode()"/>
        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }

            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(_pattern);
        }

        /// <summary>
        /// Determines if the input <see cref="string"/> object matches the current 
        /// <see cref="WildcardString"/> object based on traditional wildcard pattern rules.
        /// </summary>
        /// <remarks>
        ///     Traditional pattern matching includes:
        ///     <para>
        ///         <code>* - 0 or more of any character.<br/>
        ///     ? - exactly 1 of any character.</code>
        ///     </para>
        /// </remarks>
        /// <param name="input">The string to pattern match.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="input"/> matches the <see cref="WildcardString"/> pattern;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsMatch(string? input, StringComparison comparisonType)
        {
            return this.IsMatch(input.AsSpan(), comparisonType);
        }
        /// <summary>
        /// Determines if the specified <see cref="char"/> span matches the current 
        /// <see cref="WildcardString"/> object based on traditional wildcard pattern rules.
        /// </summary>
        /// <remarks>
        ///     Traditional pattern matching includes:
        ///     <para>
        ///         <code>* - 0 or more of any character.<br/>
        ///     ? - exactly 1 of any character.</code>
        ///     </para>
        /// </remarks>
        /// <param name="input">The string to pattern match.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="input"/> matches the <see cref="WildcardString"/> pattern;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsMatch(ReadOnlySpan<char> input, StringComparison comparisonType)
        {
            ReadOnlySpan<char> span = _pattern;

            if (!_containsWildcards)
            {
                return span.Equals(input, comparisonType);
            }

            return IsMatch(span, input, comparisonType.IsIgnoreCase());
        }

        private static bool IsMatch(ReadOnlySpan<char> pattern, ReadOnlySpan<char> input, bool ignoreCase)
        {
            int starIndex = -1;
            int iIndex = -1;

            int i = 0;
            int j = 0;

            while (i < input.Length)
            {
                if (j < pattern.Length && (pattern[j] == QUESTION || AreCharactersEqual(pattern[j], input[i], in ignoreCase)))
                {
                    ++i;
                    ++j;
                }
                else if (j < pattern.Length && pattern[j] == STAR)
                {
                    starIndex = j;
                    iIndex = i;
                    ++j;
                }
                else if (starIndex == -1)
                {
                    return false;
                }
                else
                {
                    j = starIndex + 1;
                    i = iIndex + 1;
                    iIndex++;
                }
            }

            while (j < pattern.Length && pattern[j] == STAR)
            {
                ++j;
            }

            return j == pattern.Length;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="WildcardString"/> contains only whitespace characters.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="WildcardString"/> contains only whitespace 
        ///     chaaracters; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsWhitespace()
        {
            return _pattern.AsSpan().IsWhiteSpace();
        }

        public WildcardString ToLower()
        {
            ReadOnlySpan<char> pat = _pattern;

            if (pat.IsWhiteSpace())
            {
                return this;
            }

            Span<char> scratch = stackalloc char[pat.Length];
            _ = pat.ToLower(scratch, CultureInfo.CurrentCulture);

            if (pat.Equals(scratch, StringComparison.Ordinal))
            {
                return this;
            }
            else
            {
                return new WildcardString(span: scratch);
            }
        }
        public WildcardString ToUpper()
        {
            ReadOnlySpan<char> pat = _pattern;

            if (pat.IsWhiteSpace())
            {
                return this;
            }

            Span<char> scratch = stackalloc char[pat.Length];
            _ = pat.ToUpper(scratch, CultureInfo.CurrentCulture);

            if (pat.Equals(scratch, StringComparison.Ordinal))
            {
                return this;
            }
            else
            {
                return new WildcardString(span: scratch);
            }
        }

        #region FORMATTABLE
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            charsWritten = 0;
            return this.AsSpan().TryCopyToSlice(destination, ref charsWritten);
        }
        /// <summary>
        /// Returns the underlying <see cref="string"/> instance of this <see cref="WildcardString"/> object; no
        /// actual conversion is performed.
        /// </summary>
        /// <returns>The underlying <see cref="string"/>.</returns>
        public override string ToString()
        {
            return !this.IsEmpty ? _pattern : string.Empty;
        }
        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
        {
            return this.ToString();
        }

        #endregion

        #region PARSABLE
        public static WildcardString Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            return !s.IsEmpty
                ? new(new string(s))
                : Empty;
        }
        public static WildcardString Parse(string? s, IFormatProvider? provider)
        {
            return s;
        }
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out WildcardString result)
        {
            result = Parse(s, provider);
            return true;
        }
        public static bool TryParse(string? s, IFormatProvider? provider, out WildcardString result)
        {
            result = Parse(s, provider);
            return true;
        }

        #endregion

        /// <summary>
        /// A <see langword="static"/>, read-only instance of <see cref="WildcardString"/> representing an 
        /// empty pattern.
        /// </summary>
        public static readonly WildcardString Empty = new(string.Empty);
        public static implicit operator WildcardString(string? pattern)
        {
            return !string.IsNullOrEmpty(pattern) ? new WildcardString(pattern) : Empty;
        }
        public static explicit operator string(WildcardString pattern)
        {
            return pattern.ToString();
        }
        public static implicit operator ReadOnlySpan<char>(WildcardString pattern)
        {
            return pattern.AsSpan();
        }

        public static bool operator ==(WildcardString x, WildcardString y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(WildcardString x, WildcardString y)
        {
            return !(x == y);
        }
        public static bool operator ==(WildcardString x, string? y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(WildcardString x, string? y)
        {
            return !(x == y);
        }
        public static bool operator ==(string? x, WildcardString y)
        {
            return y.Equals(x);
        }
        public static bool operator !=(string? x, WildcardString y)
        {
            return !(x == y);
        }
    }
}
