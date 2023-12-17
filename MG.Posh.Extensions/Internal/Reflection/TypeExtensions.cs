using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace MG.Posh.Internal.Reflection
{
    internal static class TypeExtensions
    {
        [return: NotNullIfNotNull(nameof(type))]
        public static string? GetTypeName(this Type? type)
        {
            return type?.FullName ?? type?.Name;
        }

        static readonly char[] _brackets = new[] { '[', ']' };
        [return: NotNullIfNotNull(nameof(type))]
        public static string? GetPSTypeName(this Type? type)
        {
            return GetPSTypeName(type, removeBrackets: false);
        }

        [return: NotNullIfNotNull(nameof(type))]
        public static string? GetPSTypeName(this Type? type, bool removeBrackets)
        {
            string? name = null;
            if (type is null)
            {
                return name;
            }
            else
            {
                name = LanguagePrimitives.ConvertTypeNameToPSTypeName(type.FullName);
                if (removeBrackets)
                {
                    name = name.Trim(_brackets);
                }
            }

            return name ?? type.FullName ?? type.Name;
        }
    }
}

