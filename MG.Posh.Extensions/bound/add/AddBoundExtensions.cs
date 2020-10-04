using MG.Posh.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Bound.Add
{
    public static class AddBoundExtensions
    {
        /// <summary>
        /// Adds the specified parameter of the extending <see cref="PSCmdlet"/> and it's current value to the cmdlet's BoundParameters.
        /// </summary>
        /// <typeparam name="TCmdlet"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="cmdlet"></param>
        /// <param name="parameter">
        ///     The <see cref="MemberExpression"/> of the cmdlet's parameter to add with its current value.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     An parameter with the same name has already been added to the BoundParameters dictionary.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     The member name of <paramref name="parameter"/> was not found.
        /// </exception>
        public static void AddParameter<TCmdlet, TParameter>(
            this TCmdlet cmdlet, 
            Expression<Func<TCmdlet, TParameter>> parameter) where TCmdlet : PSCmdlet
        {
            if (ExpressionFactory.TryAsMemberExpression(parameter, out MemberExpression memEx))
            {
                Func<TCmdlet, TParameter> func = parameter.Compile();
                cmdlet.MyInvocation.BoundParameters.Add(memEx.Member.Name, func(cmdlet));
            }
        }
    }
}
