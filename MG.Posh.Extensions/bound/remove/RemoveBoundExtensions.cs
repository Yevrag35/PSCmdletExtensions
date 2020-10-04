using MG.Posh.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;

namespace MG.Posh.Extensions.Bound.Remove
{
    public static class RemoveBoundExtensions
    {
        /// <summary>
        /// Removes the specified parameter from the extending cmdlet's <see cref="InvocationInfo.BoundParameters"/>.
        /// </summary>
        /// <typeparam name="TCmdlet"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="cmdlet"></param>
        /// <param name="parameter">
        ///     The <see cref="MemberExpression"/> of the cmdlet's parameter which will be removed by its <see cref="MemberInfo.Name"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="MemberInfo.Name"/> is successfully found and removed; otherwise,
        ///     <see langword="false"/>.  This method returns <see langword="false"/> if the member's name is not found in 
        ///     <see cref="InvocationInfo.BoundParameters"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <see cref="MemberInfo.Name"/> of <paramref name="parameter"/> is <see langword="null"/>.
        /// </exception>
        public static bool RemoveParameter<TCmdlet, TParameter>(this TCmdlet cmdlet, Expression<Func<TCmdlet, TParameter>> parameter)
            where TCmdlet : PSCmdlet
        {
            bool result = false;
            if (ExpressionFactory.TryAsMemberExpression(parameter, out MemberExpression memEx))
            {
                result = cmdlet.MyInvocation.BoundParameters.Remove(memEx.Member.Name);
            }
            return result;
        }
        /// <summary>
        /// Removes the specified parameter collection from the the extending cmdlet's <see cref="InvocationInfo.BoundParameters"/>.
        /// </summary>
        /// <typeparam name="TCmdlet"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="cmdlet"></param>
        /// <param name="expressions">The <see cref="MemberExpression"/> collection which will be all be removed.</param>
        /// <exception cref="ArgumentNullException">
        ///     A <see cref="MemberInfo.Name"/> of one of the <paramref name="expressions"/> is <see langword="null"/>.
        /// </exception>
        public static void RemoveParameters<TCmdlet, TParameter>(this TCmdlet cmdlet, params Expression<Func<TCmdlet, TParameter>>[] expressions)
            where TCmdlet : PSCmdlet
        {
            if (expressions == null || expressions.Length <= 0)
                return;

            for (int i = 0; i < expressions.Length; i++)
            {
                RemoveParameter(cmdlet, expressions[i]);
            }
        }
    }
}
