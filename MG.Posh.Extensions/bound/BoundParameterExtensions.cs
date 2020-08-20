using MG.Posh.Extensions.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Bound
{
    /// <summary>
    /// An extension class providing methods for <see cref="PSCmdlet"/> instances to easily check their BoundParameter dictionary.
    /// </summary>
    public static partial class BoundParameterExtensions
    {
        /// <summary>
        ///     Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameters against
        ///     the specified parameter name(s), returning <see langword="true"/> if all are bound.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that the method is extending.</param>
        /// <param name="parameterNames">The parameter names to check are bound.</param>
        /// <returns>
        ///     Returns a <see cref="bool"/> value indicating if
        ///     the <see cref="PSCmdlet"/> contains all of the specified keys.
        /// </returns>
        public static bool ContainsAllParameters<T>(this T cmdlet, params string[] parameterNames) where T : PSCmdlet
        {
            if (parameterNames == null || parameterNames.Length <= 0)
                return false;

            for (int i = 0; i < parameterNames.Length; i++)
            {
                string name = parameterNames[i];
                if (!cmdlet.MyInvocation.BoundParameters.ContainsKey(name))
                    return false;
            }
            return true;
        }
        /// <summary>
        ///     Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameters against
        ///     the specified collection of parameter names, returning <see langword="true"/> if all are bound.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that the method is extending.</param>
        /// <param name="parameterNames">The collection of parameter names to check are bound.</param>
        /// <returns>
        ///     Returns a <see cref="bool"/> value indicating if
        ///     the <see cref="PSCmdlet"/> contains all of the specified keys.
        /// </returns>
        public static bool ContainsAllParameterNames<T>(this T cmdlet, IEnumerable<string> parameterNames) where T : PSCmdlet
        {
            if (parameterNames == null)
                return false;

            return parameterNames.All(name => cmdlet.MyInvocation.BoundParameters.ContainsKey(name));
        }

        /// <summary>
        ///     Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameters against
        ///     the specified parameter name(s), returning <see langword="true"/> if any are bound.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that the method is extending.</param>
        /// <param name="parameterNames">The parameter names to check are bound.  This argument can be <see cref="null"/>.</param>
        /// <returns>
        ///     Returns a <see cref="bool"/> value indicating if
        ///     the <see cref="PSCmdlet"/> contains any of the specified keys.
        ///     If no parameterNames are specified, then it will return whether or
        ///     not any parameters have been bound at all.
        /// </returns>
        public static bool ContainsAnyParameterNames<T>(this T cmdlet, params string[] parameterNames) where T : PSCmdlet
        {
            if (parameterNames == null || parameterNames.Length <= 0)
                return cmdlet.MyInvocation.BoundParameters.Count > 0;

            for (int i = 0; i < parameterNames.Length; i++)
            {
                string name = parameterNames[i];
                if (cmdlet.MyInvocation.BoundParameters.ContainsKey(name))
                    return true;
            }
            return false;
        }
        /// <summary>
        ///     Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameters against
        ///     the specified collection of parameter names, returning <see langword="true"/> if any are bound.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that the method is extending.</param>
        /// <param name="parameterNames">The parameter names to check are bound.</param>
        /// <returns>
        ///     Returns a <see cref="bool"/> value indicating if
        ///     the <see cref="PSCmdlet"/> contains any of the specified keys.
        /// </returns>
        public static bool ContainsAnyParameterNames<T>(this T cmdlet, IEnumerable<string> parameterNames) where T : PSCmdlet
        {
            if (parameterNames == null)
                return false;

            return parameterNames.Any(name => cmdlet.MyInvocation.BoundParameters.ContainsKey(name));
        }
    }
}
