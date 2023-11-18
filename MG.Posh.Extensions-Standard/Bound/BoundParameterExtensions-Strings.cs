using MG.Posh.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;

namespace MG.Posh.Extensions.Bound
{
    public static partial class BoundParameterExtensions
    {
        /// <summary>
        /// Determines if the currently executing <see cref="PSCmdlet"/> instance has the specified bound parameter.
        /// </summary>
        /// <typeparam name="TCmdlet">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that the method is extending.</param>
        /// <param name="parameterName">The name of the parameter to check.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="cmdlet"/>'s 
        ///     <see cref="InvocationInfo.BoundParameters"/> contains a key equal to
        ///     <paramref name="parameterName"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="parameterName"/> is an empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="parameterName"/> is null
        ///     -and or-
        ///     <paramref name="cmdlet"/> is null.
        /// </exception>
        [DebuggerStepThrough]
        public static bool HasParameter(this PSCmdlet cmdlet, string parameterName)
        {
            Guard.NotNull(cmdlet, nameof(cmdlet));
            Guard.NotNullOrEmpty(parameterName, nameof(parameterName));

            return HasParameterName(cmdlet, parameterName);
        }

        /// <summary>
        /// Determines if the executing cmdlet has all of the specified parameter names bound.
        /// </summary>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that the method is extending.</param>
        /// <param name="parameterNames">A collection of parameter names that the cmdlet must have bound.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="cmdlet"/>'s 
        ///     <see cref="InvocationInfo.BoundParameters"/> contains all keys specified in the 
        ///     <paramref name="parameterNames"/> collection; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="cmdlet"/> is null
        ///     - or -
        ///     <paramref name="parameterNames"/> is null
        ///     - or -
        ///     One of the string values in <paramref name="parameterNames"/> is null.
        /// </exception>
        [DebuggerStepThrough]
        public static bool HasAllParameters(this PSCmdlet cmdlet, IEnumerable<string> parameterNames)
        {
            Guard.NotNull(cmdlet, nameof(cmdlet));
            Guard.NotNull(parameterNames, nameof(parameterNames));

            return HasAllParameterNames(cmdlet, parameterNames);
        }

        /// <summary>
        /// Determines if the executing cmdlet has any bound parameters specified, regardless of what it is.
        /// </summary>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that the method is extending.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="cmdlet"/>'s <see cref="InvocationInfo.BoundParameters"/>
        ///     contains at least 1 element; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmdlet"/> is null.</exception>
        [DebuggerStepThrough]
        public static bool HasAnyParameter(this PSCmdlet cmdlet)
        {
            return HasAnyParameter(cmdlet, excludeDefaultParameters: false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdlet"></param>
        /// <param name="excludeDefaultParameters"></param>
        /// <returns></returns>
        public static bool HasAnyParameter(this PSCmdlet cmdlet, bool excludeDefaultParameters)
        {
            Guard.NotNull(cmdlet, nameof(cmdlet));

            var parameters = cmdlet.MyInvocation.BoundParameters;

            bool result = parameters.Count > 0;

            return excludeDefaultParameters
                ? result && !_builtInNames.Value.IsSupersetOf(parameters.Keys)
                : result;
        }
        /// <summary>
        /// Determines if the executing cmdlet has bound any of the specified parameter names.
        /// </summary>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that the method is extending.</param>
        /// <param name="parameterNames">A collection of parameter names that the cmdlet may have bound.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="cmdlet"/>'s <see cref="InvocationInfo.BoundParameters"/>
        ///     contains at least 1 key from <paramref name="parameterNames"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="cmdlet"/> is null
        ///     - or -
        ///     <paramref name="parameterNames"/> is null
        ///     - or -
        ///     One of the string values in <paramref name="parameterNames"/> is null.
        /// </exception>
        public static bool HasAnyParameter(this PSCmdlet cmdlet, IEnumerable<string>? parameterNames)
        {
            if (parameterNames is null || (parameterNames.TryGetNonEnumeratedCount(out int count) && count <= 0))
            {
                return HasAnyParameter(cmdlet);
            }

            Guard.NotNull(cmdlet, nameof(cmdlet));

            return HasAnyParameterName(cmdlet, parameterNames);
        }

        private static bool HasAllParameterNames(PSCmdlet cmdlet, IEnumerable<string> parameterNames)
        {
            bool flag = true;
            foreach (string pName in parameterNames)
            {
                if (!cmdlet.MyInvocation.BoundParameters.ContainsKey(pName))
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }
        private static bool HasAnyParameterName(PSCmdlet cmdlet, IEnumerable<string> parameterNames)
        {
            bool flag = false;
            foreach (string pName in parameterNames)
            {
                if (cmdlet.MyInvocation.BoundParameters.ContainsKey(pName))
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }
        [DebuggerStepThrough]
        private static bool HasParameterName(PSCmdlet cmdlet, string parameterName)
        {
            return cmdlet.MyInvocation.BoundParameters.ContainsKey(parameterName);
        }
    }
}