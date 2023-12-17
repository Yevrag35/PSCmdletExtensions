using System;
using System.Diagnostics;

namespace MG.Posh.Extensions.Spans.Internal
{
    /// <summary>
    /// Custom extension methods for <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/> instances
    /// of type <see cref="char"/>.
    /// </summary>
    internal static class CharSpanExtensions
    {
        /// <summary>
        /// Copies the characters of this <see cref="string"/> instance into a destination
        /// <see cref="Span{T}"/> and advances the given ref <see cref="int"/> the 
        /// <see cref="string.Length"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="span">The span to copy items into.</param>
        /// <param name="position">
        ///     The ref <see cref="int"/> to add the number of the characters to if copying was
        ///     successful.
        /// </param>
        [DebuggerStepThrough]
        internal static void CopyToSlice(this string? value, Span<char> span, ref int position)
        {
            CopyToSlice(spanValue: value, span, ref position);
        }
        /// <summary>
        /// Copies the contents of this <see cref="ReadOnlySpan{T}"/> into a destination 
        /// <see cref="Span{T}"/> and advances the given ref <see cref="int"/> the number 
        /// of characters that were copied.
        /// </summary>
        /// <param name="spanValue">The source read-only span whose characters are copied.</param>
        /// <param name="span">The span to copy items into.</param>
        /// <param name="position">
        ///     The ref <see cref="int"/> to add the number of the characters to if copying was
        ///     successful.
        /// </param>
        internal static void CopyToSlice(this ReadOnlySpan<char> spanValue, Span<char> span, ref int position)
        {
            if (!spanValue.IsEmpty && spanValue.TryCopyTo(span.Slice(position)))
            {
                position += spanValue.Length;
            }
        }
        /// <summary>
        /// Copies the contents of this <see cref="Span{T}"/> into a destination 
        /// <see cref="Span{T}"/> and advances the given ref <see cref="int"/> the number 
        /// of characters that were copied.
        /// </summary>
        /// <param name="writtableSpan">The source span whose characters are copied.</param>
        /// <param name="span">The span to copy items into.</param>
        /// <param name="position">
        ///     The ref <see cref="int"/> to add the number of the characters to if copying was
        ///     successful.
        /// </param>
        [DebuggerStepThrough]
        internal static void CopyToSlice(this Span<char> writtableSpan, Span<char> span, ref int position)
        {
            CopyToSlice(spanValue: writtableSpan, span, ref position);
        }

        /// <summary>
        /// Attemps to copy the contents of this <see cref="ReadOnlySpan{T}"/> into a 
        /// <see cref="Span{T}"/> and advancing the given ref <see cref="int"/> the number 
        /// of characters that were copied, returning a value indicating whether or not the operation
        /// succeeded.
        /// </summary>
        /// <param name="spanValue">The source read-only span whose characters are copied.</param>
        /// <param name="span">The target of the copy operation.</param>
        /// <param name="position">
        ///     The ref <see cref="int"/> to add the number of the characters to if copying was
        ///     successful.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the copying operation was successful; otherwise
        ///     <see langword="false"/>.
        /// </returns>
        internal static bool TryCopyToSlice(this ReadOnlySpan<char> spanValue, Span<char> span, ref int position)
        {
            bool result = false;

            if (!spanValue.IsEmpty && spanValue.TryCopyTo(span.Slice(position)))
            {
                position += spanValue.Length;
                result = true;
            }

            return result;
        }
        /// <summary>
        /// Attemps to copy the contents of this <see cref="Span{T}"/> into a destination
        /// <see cref="Span{T}"/> and advancing the given ref <see cref="int"/> the number 
        /// of characters that were copied, returning a value indicating whether or not the operation
        /// succeeded.
        /// </summary>
        /// <param name="writtableSpan">The source span whose characters are copied.</param>
        /// <param name="span">The target of the copy operation.</param>
        /// <param name="position">
        ///     The ref <see cref="int"/> to add the number of the characters to if copying was
        ///     successful.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the copying operation was successful; otherwise
        ///     <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        internal static bool TryCopyToSlice(this Span<char> writtableSpan, Span<char> span, ref int position)
        {
            return TryCopyToSlice(spanValue: writtableSpan, span, ref position);
        }
        /// <summary>
        /// Determines whether the beginning of the <paramref name="span"/> matches the specified <paramref name="value"/> when compared ignoring case.
        /// </summary>
        /// <param name="span">The source span.</param>
        /// <param name="value">The character to compare to the beginning of the source span.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="value"/> matches the beginning of 
        ///     <paramref name="span"/>; otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        internal static bool StartsWith(this ReadOnlySpan<char> span, in char value)
        {
            return StartsWith(span, in value, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// Determines whether the beginning of the <paramref name="span"/> matches the specified <paramref name="value"/> when compared using the specified 
        /// <paramref name="comparisonType"/> option.
        /// </summary>
        /// <param name="span">The source span.</param>
        /// <param name="value">The character to compare to the beginning of the source span.</param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that determines how the 
        ///     <paramref name="span"/> and <paramref name="value"/> are compared.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="value"/> matches the beginning of 
        ///     <paramref name="span"/>; otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        internal static bool StartsWith(this ReadOnlySpan<char> span, in char value, StringComparison comparisonType)
        {
#if NET8_0_OR_GREATER
            return span.StartsWith(
                new ReadOnlySpan<char>(in value),
                comparisonType);
#else
            if (span.IsEmpty)
            {
                return false;
            }

            char c = span[0];
            return comparisonType switch
            {
                StringComparison.CurrentCulture or StringComparison.InvariantCulture or StringComparison.Ordinal => c.Equals(value),
                StringComparison.CurrentCultureIgnoreCase or StringComparison.OrdinalIgnoreCase => char.ToLower(c).Equals(char.ToLower(value)),
                StringComparison.InvariantCultureIgnoreCase => char.ToUpperInvariant(c).Equals(char.ToUpperInvariant(value)),
                _ => false,
            };
            ;
#endif
        }
    }
}