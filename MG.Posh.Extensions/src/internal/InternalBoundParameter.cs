using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Bound.Internal
{
    internal static class InternalBoundChecker
    {
        internal static bool ContainsParameter(PSCmdlet cmdlet, string parameterName)
        {
            return (cmdlet.MyInvocation?.BoundParameters?.ContainsKey(parameterName)).GetValueOrDefault();
        }

        internal static bool ContainsAllParameters(PSCmdlet cmdlet, IEnumerable<string> names)
        {
            return (names?.All(n => ContainsParameter(cmdlet, n))).GetValueOrDefault();
        }

        internal static bool ContainsAnyParameters(PSCmdlet cmdlet, IEnumerable<string> names)
        {
            bool? check = names?.Any(n => ContainsParameter(cmdlet, n));

            return check ?? cmdlet.MyInvocation.BoundParameters.Count > 0;
        }

        internal static IEnumerable<string> GetMemberNames<T1, T2>(params Expression<Func<T1, T2>>[] expressions)
        {
            if (expressions != null && expressions.Length > 0)
            {
                foreach (Expression<Func<T1, T2>> exp in expressions)
                {
                    if (exp?.Body is MemberExpression memEx)
                    {
                        yield return memEx.Member.Name;
                    }
                    else if (exp?.Body is UnaryExpression unEx && unEx.Operand is MemberExpression unExMem)
                    {
                        yield return unExMem.Member.Name;
                    }
                }
            }
        }
    }
}
