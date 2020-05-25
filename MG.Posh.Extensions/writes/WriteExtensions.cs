using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace MG.Posh.Extensions.Writes
{
    /// <summary>
    /// A static class that extends <see cref="PSCmdlet"/>'s base "write" output stream methods to accept
    /// formatted-strings with arguments.
    /// </summary>
    public static class WriteExtensions
    {
        /// <summary>
        /// Formats a given string and arguments and sends it to <see cref="Cmdlet.WriteDebug(string)"/>.  Use this to display debug information
        /// on the inner-workings of your <see cref="Cmdlet"/>.  By default, debug output will not be displayed, although this can be configured
        /// with the DebugPreference shell variable or the -Debug command-line parameter.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="formattedText">A composite format string for the debug output.</param>
        /// <param name="arguments">The object(s) to format.</param>
        /// <exception cref="FormatException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteDebugFormat<T>(T cmdlet, string formattedText, params object[] arguments) where T : Cmdlet
        {
            if (string.IsNullOrWhiteSpace(formattedText))
                return;

            cmdlet.WriteDebug(FormatString(formattedText, arguments));
        }

        /// <summary>
        /// Formats a given string and arguments and sends it to <see cref="Cmdlet.WriteVerbose(string)"/>.  Use this to display more detailed information
        /// about the activity of your <see cref="Cmdlet"/>.  By default, verbose output will not be displayed, although this can be configured with the
        /// VerbosePreference shell variable or the -Verbose and -Debug command-line parameters.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="formattedText">A composite format string for the verbose output.</param>
        /// <param name="arguments">The object(s) to format.</param>
        /// <exception cref="FormatException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteVerboseFormat<T>(T cmdlet, string formattedText, params object[] arguments) where T : Cmdlet
        {
            if (string.IsNullOrWhiteSpace(formattedText))
                return;

            cmdlet.WriteVerbose(FormatString(formattedText, arguments));
        }

        /// <summary>
        /// Formats a given string and arguments and sends it to <see cref="Cmdlet.WriteWarning(string)"/>.  Use this to display more detailed 
        /// information about the activity of your <see cref="Cmdlet"/>.  By default, warning output will not be displayed, although this can 
        /// be configured with the WarningPreference shell variable or the -Verbose and -Debug command-line parameters.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="formattedText">A composite format string for the warning output.</param>
        /// <param name="arguments">The object(s) to format.</param>
        /// <exception cref="FormatException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteWarningFormat<T>(T cmdlet, string formattedText, params object[] arguments) where T : Cmdlet
        {
            if (string.IsNullOrWhiteSpace(formattedText))
                return;

            cmdlet.WriteWarning(FormatString(formattedText, arguments));
        }

        private static string FormatString(string formattedText, params object[] arguments)
        {
            if (arguments == null || arguments.Length <= 0)
                return formattedText;

            else
                return string.Format(formattedText, arguments);
        }
    }
}
