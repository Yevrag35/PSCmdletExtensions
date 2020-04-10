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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdlet"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static bool ContainsPSParameter<T>(this T cmdlet, PSParameter parameter) where T : PSCmdlet
        {
            return cmdlet.MyInvocation.BoundParameters.ContainsKey(parameter.ToString());
        }
        public static bool ContainsAllPSParameter<T>(this T cmdlet, IEnumerable<PSParameter> parameters) where T : PSCmdlet
        {
            if (parameters == null)
                return false;

            return parameters.All(x => cmdlet.MyInvocation.BoundParameters.ContainsKey(x.ToString()));
        }
        public static bool ContainsAnyPSParameter<T>(this T cmdlet, params PSParameter[] parameters) where T : PSCmdlet
        {
            if (parameters == null || parameters.Length <= 0)
                return cmdlet.MyInvocation.BoundParameters.Count > 0;

            return parameters.Any(x => cmdlet.MyInvocation.BoundParameters.ContainsKey(x.ToString()));
        }
    }
}
