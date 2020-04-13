using MG.Posh.Extensions.Bound.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Bound
{
    public static partial  class BoundParameterExtensions
    {
        /// <summary>
        ///     Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameters against
        ///     the specified member(s).
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> whose BoundParameters are checked.</param>
        /// <param name="parameter">The <see cref="MemberExpression"/> containing the parameter name to check are bound.</param>
        /// <returns>Whether the <see cref="PSCmdlet"/> contains the specified key.</returns>
        public static bool ContainsParameter<T1, T2>(this T1 cmdlet, Expression<Func<T1, T2>> parameter) where T1 : PSCmdlet
        {
            return InternalBoundChecker.ContainsAllParameters(cmdlet, InternalBoundChecker.GetMemberNames(parameter));
            //bool result = false;
            //if (TryAsMemberExpression(parameter, out MemberExpression memEx))
            //{
            //    result = cmdlet.MyInvocation.BoundParameters.ContainsKey(memEx.Member.Name);
            //}
            //return result;
        }

        /// <summary>
        ///     Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameters against
        ///     all the specified member, returning <see langword="true"/> if all names are present.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> whose BoundParameters are checked.</param>
        /// <param name="parameters">The instances of <see cref="MemberExpression"/> containing the parameters' names to check are bound.</param>
        /// <returns>Whether the <see cref="PSCmdlet"/> contains all of the specified keys.</returns>
        public static bool ContainsAllParameters<T>(this T cmdlet, params Expression<Func<T, object>>[] parameters) where T : PSCmdlet
        {
            return InternalBoundChecker.ContainsAllParameters(cmdlet, InternalBoundChecker.GetMemberNames(parameters));
            //if (parameters == null)
            //    throw new ArgumentNullException("parameters");

            //else if (parameters.Length <= 0)
            //    return false;

            //return parameters.All(p => ContainsParameter(cmdlet, p));
        }

        /// <summary>
        ///     Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameters against
        ///     all the specified members, returning <see langword="true"/> if any of the names are present.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> whose BoundParameters are checked.</param>
        /// <param name="parameters">The instances of <see cref="MemberExpression"/> containing the parameters' names to check are bound.</param>
        /// <returns>
        ///     Returns a <see cref="bool"/> value indicating if
        ///     the <see cref="PSCmdlet"/> contains any of the specified keys.
        ///     If no parameter expressions are specified, then it will return whether or
        ///     not any parameters have been bound at all.
        /// </returns>
        public static bool ContainsAnyParameters<T>(this T cmdlet, params Expression<Func<T, object>>[] parameters) where T : PSCmdlet
        {
            if (parameters == null || parameters.Length > 0)
                return cmdlet.MyInvocation.BoundParameters.Count > 0;

            return parameters.Any(p => ContainsParameter(cmdlet, p));
        }

        private static bool TryAsMemberExpression<T>(Expression<Func<T, object>> expression, out MemberExpression member)
            where T : PSCmdlet
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
