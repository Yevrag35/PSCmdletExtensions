using MG.Posh.Internal.Reflection;
using MG.Posh.Internal;
using System;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;

namespace MG.Posh.Extensions.Bound
{
    public static partial class BoundParameterExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TCmdlet"></typeparam>
        /// <param name="cmdlet"></param>
        /// <param name="memberExpressions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="cmdlet"/> is null
        ///     -or-
        ///     <paramref name="memberExpressions"/> is null.
        /// </exception>
        public static bool HasAllParameters<TCmdlet>(this TCmdlet cmdlet, params Expression<Func<TCmdlet, object?>>[] memberExpressions) where TCmdlet : PSCmdlet
        {
            Guard.NotNull(memberExpressions, nameof(memberExpressions));

            if (memberExpressions.Length <= 0)
            {
                return false;
            }
            else if (memberExpressions.Length == 1)
            {
                return HasParameter(cmdlet, memberExpressions[0]);
            }

            Guard.NotNull(cmdlet, nameof(cmdlet));

            bool flag = true;
            foreach (var memEx in memberExpressions)
            {
                if (!HasParameterFromExpression(cmdlet, memEx))
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TCmdlet"></typeparam>
        /// <param name="psCmdlet"></param>
        /// <param name="memberExpressions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="psCmdlet"/> is null.</exception>
        public static bool HasAnyParameter<TCmdlet>(this TCmdlet psCmdlet, params Expression<Func<TCmdlet, object?>>[] memberExpressions) where TCmdlet : PSCmdlet
        {
            if (memberExpressions is null || memberExpressions.Length <= 0)
            {
                return HasAnyParameter(cmdlet: psCmdlet);
            }
            else if (memberExpressions.Length == 1)
            {
                return HasParameter(psCmdlet, memberExpressions[0]);
            }

            Guard.NotNull(psCmdlet, nameof(psCmdlet));

            bool flag = false;
            foreach (var memEx in memberExpressions)
            {
                if (HasParameterFromExpression(psCmdlet, memEx))
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        /// <summary>
        ///     Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameters against
        ///     the specified member(s).
        /// </summary>
        /// <typeparam name="TCmdlet">The type of <see cref="PSCmdlet"/> whose parameters can be checked.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that the method is extending.</param>
        /// <param name="memberExpression">
        ///     The <see cref="MemberExpression"/> whose <see cref="MemberInfo.Name"/> will be resolved as
        ///     the parameter name.
        /// </param>
        /// <returns>
        ///     Whether the <see cref="PSCmdlet"/> contains the specified key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="cmdlet"/> is null -or- <paramref name="memberExpression"/> is null.
        /// </exception>
        public static bool HasParameter<TCmdlet>(this TCmdlet cmdlet, Expression<Func<TCmdlet, object?>> memberExpression) where TCmdlet : PSCmdlet
        {
            Guard.NotNull(cmdlet, nameof(cmdlet));
            Guard.NotNull(memberExpression, nameof(memberExpression));

            return HasParameterFromExpression(cmdlet, memberExpression);
        }

        private static bool HasParameterFromExpression<TCmdlet>(TCmdlet cmdlet, Expression<Func<TCmdlet, object?>> memberExpression) where TCmdlet : PSCmdlet
        {
            return memberExpression.TryGetAsMember(out MemberExpression? memberEx)
                   &&
                   HasParameterName(cmdlet, memberEx.Member.Name);
        }
    }
}