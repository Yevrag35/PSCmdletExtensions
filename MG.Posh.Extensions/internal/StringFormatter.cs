using System;

namespace MG.Posh.Extensions.Internal
{
    internal static class StringFormatter
    {
        internal static string FormatString(string formattedText, params object[] arguments)
        {
            if (arguments == null || arguments.Length <= 0)
                return formattedText;

            else
                return string.Format(formattedText, arguments);
        }
    }
}
