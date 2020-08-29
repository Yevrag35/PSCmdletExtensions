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

        internal static bool TryAsMemberExpression<T, TMember>(Expression<Func<T, TMember>> expression, out MemberExpression member)
        {
            member = null;
            if (expression?.Body is MemberExpression memEx)
            {
                member = memEx;
            }
            else if (expression?.Body is UnaryExpression unEx && unEx.Operand is MemberExpression unExMem)
            {
                member = unExMem;
            }
            return member != null;
        }
    }
}
