using MG.Posh.Extensions.Filters;
using MG.Posh.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;

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

        /// <summary>
        /// Checks if a parameter of a <see cref="PSCmdlet"/> has been bound positionally.
        /// </summary>
        /// <typeparam name="TCmdlet"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> we are extending.</param>
        /// <param name="parameter">The expression of the cmdlet's parameter to check.</param>
        /// <returns>
        ///     <see langword="true"/> if parameter's name is found within the <see cref="InvocationInfo"/>'s 'BoundPositionally'
        ///     list of parameter names.
        ///     <see langword="false"/> if not or if the parameter does not exist.
        /// </returns>
        public static bool ContainsPositionalParameter<TCmdlet, TParameter>(this TCmdlet cmdlet, Expression<Func<TCmdlet, TParameter>> parameter)
            where TCmdlet : PSCmdlet
        {
            bool result = false;
            if (ExpressionFactory.TryAsMemberExpression(parameter, out MemberExpression memEx))
            {
                IReadOnlyList<string> boundPositionally = GetPositionalParameters(cmdlet.MyInvocation);
                if (boundPositionally != null && boundPositionally.Count > 0 && boundPositionally.Contains(memEx.Member.Name))
                    result = true;
            }
            return result;
        }

        /// <summary>
        /// Returns a read-only copy of those parameter names which have been bound positionally.
        /// </summary>
        /// <param name="info">The invocation info to retrieve the bound parameters dictionary from.</param>
        /// <returns>
        ///     A <see cref="IReadOnlyList{T}"/> of strings which represent the parameter names that have been bound positionally.
        /// </returns>
        public static IReadOnlyList<string> GetPositionalParameters(this InvocationInfo info)
        {
            Type boundType = info.BoundParameters.GetType();
            PropertyInfo pi = boundType.GetProperty("BoundPositionally", BindingFlags.Instance | BindingFlags.Public);
            object val = pi?.GetValue(info.BoundParameters);
            return val != null && val is IReadOnlyList<string> list ? list : null;
        }
    }
}
