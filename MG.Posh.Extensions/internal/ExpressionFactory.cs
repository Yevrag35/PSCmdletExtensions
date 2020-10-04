using System;
using System.Linq.Expressions;

namespace MG.Posh.Extensions.Internal
{ 
    internal static class ExpressionFactory
    {
        internal static MemberExpression AsMemberExpression<T, TMember>(Expression<Func<T, TMember>> expression)
        {
            MemberExpression member = null;
            if (expression?.Body is MemberExpression memEx)
            {
                member = memEx;
            }
            else if (expression?.Body is UnaryExpression unEx && unEx.Operand is MemberExpression unExMem)
            {
                member = unExMem;
            }
            return member;
        }

        internal static bool TryAsMemberExpression<T, TMember>(Expression<Func<T, TMember>> expression, out MemberExpression member)
        {
            member = AsMemberExpression(expression);
            return member != null;
        }
    }
}
