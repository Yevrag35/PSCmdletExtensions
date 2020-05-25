using MG.Posh.Extensions.Internal;
using System;
using System.Management.Automation;

namespace MG.Posh.Extensions.Cmdlets
{
    /// <summary>
    /// A class that extends <see cref="PSCmdlet"/>'s "ShouldProcess" and "ShouldContinue" methods to accept
    /// formatted-strings with arguments.
    /// </summary>
    public class ShouldFormatPSCmdlet : PSCmdlet
    {
        /// <summary>
        /// Confirm the operation with the user.  Cmdlets which make changes (e.g. delete files, stop services, etc.) should call ShouldProcessFormat
        /// to give the user the opportunity to confirm that the operation should actually be performed.  The provided action will be formatted with the
        /// given arguments.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> the method is extending.</param>
        /// <param name="action">The action that should be performed.</param>
        /// <param name="target">A composite string format of the target resource being acted upon.  This will potentially be displayed to the user.</param>
        /// <param name="targetArguments">The object(s) to format.</param>
        /// <exception cref="FormatException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        /// <returns>
        /// If ShouldProcessFormat returns true, the operation should be performed.  If ShouldProcessFormat returns false, the operation should not
        /// be performed, and the <see cref="PSCmdlet"/> should move on to the next target resource.
        /// </returns>
        public bool ShouldProcessFormat(string action, string target, params object[] targetArguments)
        {
            string formattedTarget = StringFormatter.FormatString(target, targetArguments);
            if (string.IsNullOrWhiteSpace(formattedTarget))
            {
                formattedTarget = string.Empty;
            }
            return base.ShouldProcess(formattedTarget, action);
        }
    }
}
