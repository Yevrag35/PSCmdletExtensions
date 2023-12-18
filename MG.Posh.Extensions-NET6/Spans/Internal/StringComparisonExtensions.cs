using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Posh.Extensions.Spans.Internal
{
    internal static class StringComparisonExtensions
    {
        internal static bool IsIgnoreCase(this StringComparison comparison)
        {
            switch (comparison)
            {
                case StringComparison.CurrentCulture:
                    goto default;

                case StringComparison.CurrentCultureIgnoreCase:
                    goto case StringComparison.OrdinalIgnoreCase;

                case StringComparison.InvariantCulture:
                    goto default;

                case StringComparison.InvariantCultureIgnoreCase:
                    goto case StringComparison.OrdinalIgnoreCase;

                case StringComparison.Ordinal:
                    goto default;

                case StringComparison.OrdinalIgnoreCase:
                    return true;

                default:
                    return false;
            }
        }
    }
}

