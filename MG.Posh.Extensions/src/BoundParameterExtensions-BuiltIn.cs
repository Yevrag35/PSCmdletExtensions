using MG.Posh.Extensions.Bound.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Bound.BuiltIn
{
    public enum PSParameter
    {
        Verbose,
        Debug,
        ErrorAction,
        WarningAction,
        InformationAction,
        ErrorVariable,
        WarningVariable,
        InformationVariable,
        OutVariable,
        OutBuffer,
        PipelineVariable
    }

    /// <summary>
    /// An extension class providing methods for <see cref="PSCmdlet"/> to easily check if specific built-in advanced parameters
    /// have been bound.
    /// </summary>
    public static class BoundPSParameterExtensions
    {
        /// <summary>
        /// Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameter dictionary
        /// checking if the specified built-in advanced parameter has been bound.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> whose BoundParameters are checked.</param>
        /// <param name="parameter">The built-in advanced parameter to check if bound.</param>
        public static bool ContainsPSParameter<T>(this T cmdlet, PSParameter parameter) where T : PSCmdlet
        {
            return InternalBoundChecker.ContainsParameter(cmdlet, parameter.ToString());
        }
        /// <summary>
        /// Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameter dictionary
        /// checking if all of the parameters in the <see cref="IEnumerable{T}"/> are bound.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> whose BoundParameters are checked.</param>
        /// <param name="parameters">The collection of built-in advanced parameters to check.</param>
        /// <returns>
        ///     Returns True or False depending if all parameters in the specified collection are bound.
        ///     If the parameter <see cref="IEnumerable{T}"/> is null, the method will return False.
        /// </returns>
        public static bool ContainsAllPSParameters<T>(this T cmdlet, IEnumerable<PSParameter> parameters) where T : PSCmdlet
        {
            return InternalBoundChecker.ContainsAllParameters(cmdlet, parameters?.Select(x => x.ToString()));
        }
        public static bool ContainsAllPSParameters<T>(this T cmdlet, params PSParameter[] parameters) where T : PSCmdlet
        {
            return InternalBoundChecker.ContainsAllParameters(cmdlet, parameters?.Select(x => x.ToString()));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> whose BoundParameters are checked.</param>
        /// <param name="parameters"></param>
        public static bool ContainsAnyPSParameter<T>(this T cmdlet, params PSParameter[] parameters) where T : PSCmdlet
        {
            return InternalBoundChecker.ContainsAnyParameters(cmdlet, parameters?.Select(x => x.ToString()));
        }
    }
}
