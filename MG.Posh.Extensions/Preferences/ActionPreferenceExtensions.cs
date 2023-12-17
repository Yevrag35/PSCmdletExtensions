using MG.Posh.Extensions.Variables;
using MG.Posh.Internal;
using System;
using System.Management.Automation;

namespace MG.Posh.Extensions.Preferences
{
    /// <summary>
    /// An extension class providing expanded capabilities for determining specified or default
    /// <see cref="ActionPreference"/> parameter or variable values.
    /// </summary>
    public static class ActionPreferenceExtensions
    {
        /// <summary>
        /// Retrieves the current Debug preference either set from the "-Debug" <see cref="SwitchParameter"/>
        /// or read from the global "$DebugPreference" variable from the running cmdlet's <see cref="SessionState"/>.
        /// </summary>
        /// <param name="cmdlet">The executing cmdlet to read the preference from.</param>
        /// <returns>
        ///     If the "-Debug" switch is specified, then <see cref="ActionPreference.Continue"/> is returned; otherwise,
        ///     the current value of the global "$DebugPreference" will be returned, with 
        ///     <see cref="ActionPreference.SilentlyContinue"/> being the default value.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        public static ActionPreference GetDebugPreference(this PSCmdlet cmdlet)
        {
            Guard.NotNull(cmdlet, nameof(cmdlet));

            return GetCurrentActionPreferenceFromSwitch(cmdlet, PSDefaultParameterNames.Debug, PSInternalConstants.DEBUG_PREFERENCE);
        }

        /// <summary>
        /// Retrieves the current ErrorAction preference either set from the "-ErrorAction" parameter
        /// or read from the global "$ErrorActionPreference" variable from a running cmdlet's
        /// <see cref="SessionState"/>.
        /// </summary>
        /// <param name="cmdlet">The executing cmdlet to read the preference from.</param>
        /// <returns>
        ///     If the "-ErrorAction" parameter is specified, then the value is read from the parameter's
        ///     value, otherwise, the current value of the global "$ErrorActionPreference" will be returned, with 
        ///     <see cref="ActionPreference.Continue"/> being the default value.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        public static ActionPreference GetErrorActionPreference(this PSCmdlet cmdlet)
        {
            Guard.NotNull(cmdlet, nameof(cmdlet));

            return GetCurrentActionPreferenceFromParam(cmdlet, PSDefaultParameterNames.ErrorAction, PSInternalConstants.ERROR_ACTION_PREFERENCE);
        }

        /// <summary>
        /// Retrieves the current Debug preference either set from the "-Debug" <see cref="SwitchParameter"/>
        /// or read from the global "$DebugPreference" variable from the running cmdlet's <see cref="SessionState"/>.
        /// </summary>
        /// <param name="cmdlet">The executing cmdlet to read the preference from.</param>
        /// <returns>
        ///     If the "-Debug" switch is specified, then <see cref="ActionPreference.Continue"/> is returned; otherwise,
        ///     the current value of the global "$DebugPreference" will be returned, with 
        ///     <see cref="ActionPreference.SilentlyContinue"/> being the default value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmdlet"/> is null.</exception>
        public static ActionPreference GetVerbosePreference(this PSCmdlet cmdlet)
        {
            Guard.NotNull(cmdlet, nameof(cmdlet));

            return GetCurrentActionPreferenceFromSwitch(cmdlet, PSDefaultParameterNames.Verbose, PSInternalConstants.VERBOSE_PREFERENCE);
        }

        private static ActionPreference GetCurrentActionPreferenceFromParam(PSCmdlet cmdlet, string parameterName, string variableName)
        {
            if (cmdlet.MyInvocation.BoundParameters.TryGetValue(parameterName, out object? boundVal)
                &&
                boundVal is ActionPreference actionPref)
            {
                return actionPref;
            }
            else if (cmdlet.SessionState.PSVariable.TryGetValue(variableName, out actionPref))
            {
                return actionPref;
            }

            return default;
        }
        private static ActionPreference GetCurrentActionPreferenceFromSwitch(PSCmdlet cmdlet, string parameterName, string variableName)
        {
            if (TryGetPreferenceFromSwitch(cmdlet, parameterName))
            {
                return ActionPreference.Continue;
            }
            else if (cmdlet.SessionState.PSVariable.TryGetValue(variableName, out ActionPreference actionPref))
            {
                return actionPref;
            }

            return default; // silently continue
        }

        private static bool TryGetPreferenceFromSwitch(PSCmdlet cmdlet, string variableName)
        {
            return cmdlet.MyInvocation.BoundParameters.TryGetValue(variableName, out object? boundVal)
                            &&
                            boundVal is SwitchParameter swParam
                            &&
                            swParam.ToBool();
        }
    }
}
