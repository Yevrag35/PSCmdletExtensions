using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Bound.BuiltIn
{
    public enum BuiltInParameter
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
    public static class BoundBuiltinParameterExtensions
    {
        /// <summary>
        ///     Performs a "ContainsKey" lookup on the current <see cref="PSCmdlet.MyInvocation"/> BoundParameters against
        ///     the specified built-in parameter, returning <see langword="true"/> if bound.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="PSCmdlet"/> whose bound parameters are checked.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> that is extended.</param>
        /// <param name="parameter">The built-in parameter to check.</param>
        /// <returns></returns>
        public static bool ContainsBuiltinParameter<T>(this T cmdlet, BuiltInParameter parameter) where T : PSCmdlet
        {
            return cmdlet.MyInvocation.BoundParameters.ContainsKey(parameter.ToString());
        }
        public static bool ContainsAllBuiltinParameter<T>(this T cmdlet, IEnumerable<BuiltInParameter> parameters) where T : PSCmdlet
        {
            if (parameters == null)
                return false;

            return parameters.All(x => cmdlet.MyInvocation.BoundParameters.ContainsKey(x.ToString()));
        }
        public static bool ContainsAnyBuiltinParameter<T>(this T cmdlet, params BuiltInParameter[] parameters) where T : PSCmdlet
        {
            if (parameters == null || parameters.Length <= 0)
                return cmdlet.MyInvocation.BoundParameters.Count > 0;

            return parameters.Any(x => cmdlet.MyInvocation.BoundParameters.ContainsKey(x.ToString()));
        }
    }
}
