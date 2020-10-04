using System;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Internal
{
    internal static class StringFormatter
    {
        internal static string FormatString(string formattedText, params object[] arguments)
        {
            if (arguments == null || arguments.Length <= 0)
                return formattedText;

            return string.Format(formattedText, arguments);
        }
    }
}
