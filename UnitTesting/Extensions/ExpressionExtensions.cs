using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.Extensions
{
    internal static class ExpressionExtensions
    {
        internal static bool TryGetAsProperty(this LambdaExpression expression, [NotNullWhen(true)] out PropertyInfo? property)
        {
            MemberInfo? info;
            if (expression.Body is MemberExpression memEx)
            {
                info = memEx.Member;
            }
            else if (expression.Body is UnaryExpression unEx && unEx.Operand is MemberExpression unMemEx)
            {
                info = unMemEx.Member;
            }
            else
            {
                info = null;
            }

            property = info as PropertyInfo;

            return property is not null;
        }
    }
}
