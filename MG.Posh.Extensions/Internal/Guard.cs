using System;
using System.Diagnostics.CodeAnalysis;

namespace MG.Posh.Internal
{
    internal static class Guard
    {
        /// <exception cref="ArgumentNullException"/>
        internal static void NotNull<T>([NotNull] T? value, string parameterName) where T : class
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName ?? string.Empty);
            }
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        internal static void NotNullOrEmpty([NotNull] string? value, string parameterName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName ?? string.Empty);
            }
            else if (string.Empty == value)
            {
                throw new ArgumentException($"'{parameterName}' cannot be an empty string.", parameterName ?? string.Empty);
            }
        }
    }
}

