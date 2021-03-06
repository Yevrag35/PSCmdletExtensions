﻿using MG.Posh.Extensions.Internal;
using System;
using System.Management.Automation;

namespace MG.Posh.Extensions.Writes
{
    /// <summary>
    /// A static class that extends <see cref="Cmdlet"/>'s base "write" output stream methods to accept
    /// formatted-strings with arguments and write standard output.
    /// </summary>
    public static class WriteExtensions
    {
        /// <summary>
        /// Terminate the command and report the error.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="exception">This is an exception that describes the error.</param>
        /// <param name="errorId">The string used to construct the FullyQualifiedErrorId.</param>
        /// <param name="category">The category that best describes the error.</param>
        /// <param name="targetObject">This is the object against which the <see cref="Cmdlet"/> or provider was operating on when the error occurred.</param>
        /// <exception cref="PipelineStoppedException"/>
        public static void ThrowTerminatingError<T>(this T cmdlet, Exception exception, string errorId, ErrorCategory category, object targetObject = null)
            where T : Cmdlet
        {
            cmdlet.ThrowTerminatingError(new ErrorRecord(
                exception, errorId, category, targetObject
            ));
        }

        /// <summary>
        /// Formats a given string and arguments and sends it to <see cref="Cmdlet.WriteDebug(string)"/> to display debug information.
        /// </summary>
        /// <remarks>
        /// Use this to display debug information
        /// on the inner-workings of your <see cref="Cmdlet"/>.  By default, debug output will not be displayed, although this can be configured
        /// with the DebugPreference shell variable or the -Debug command-line parameter.
        /// </remarks>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="formattedText">A composite format string for the debug output.</param>
        /// <param name="arguments">The object(s) to format.</param>
        /// <exception cref="FormatException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteDebugFormat<T>(this T cmdlet, string formattedText, params object[] arguments) where T : Cmdlet
        {
            if (string.IsNullOrWhiteSpace(formattedText))
                return;

            cmdlet.WriteDebug(StringFormatter.FormatString(formattedText, arguments));
        }

        /// <summary>
        /// Writes the specifed message as an <see cref="ArgumentException"/> with the target object to the error pipe.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="message">The message of the <see cref="ArgumentException"/>.</param>
        /// <param name="category">The category that best describes the error.</param>
        /// <param name="targetObject">This is the object against which the <see cref="Cmdlet"/> or provider was operating on when the error occurred.
        /// This is optional.</param>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteArgumentError<T>(this T cmdlet, string message, ErrorCategory category, object targetObject = null) 
            where T : Cmdlet
        {
            var errRec = new ErrorRecord(new ArgumentException(message), typeof(ArgumentException).FullName, category, targetObject);
            cmdlet.WriteError(errRec);
        }

        /// <summary>
        /// Writes the specified <see cref="Exception"/> with the target object to the error pipe.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="exception">This is an exception that describes the error.</param>
        /// <param name="errorId">The string used to construct the FullyQualifiedErrorId.</param>
        /// <param name="category">The category that best describes the error.</param>
        /// <param name="targetObject">This is the object against which the <see cref="Cmdlet"/> or provider was operating on when the error occurred.
        /// This is optional.</param>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteError<T>(this T cmdlet, Exception exception, string errorId, ErrorCategory category, object targetObject = null)
            where T : Cmdlet
        {
            var errRec = new ErrorRecord(exception, errorId, category, targetObject);
            cmdlet.WriteError(errRec);
        }

        /// <summary>
        /// Writes the specified message into the given <see cref="Exception"/> type provided all to the error pipe.
        /// </summary>
        /// <remarks><typeparamref name="T1"/> should have a public constructor that accepts a string (message) parameter.</remarks>
        /// <typeparam name="T2">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <typeparam name="T1">The type of exception to create in the <see cref="ErrorRecord"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="message">The message of the <see cref="Exception"/>.</param>
        /// <param name="category">The category that best describes the error.</param>
        /// <param name="targetObject">This is the object against which the <see cref="Cmdlet"/> or provider was operating on when the error occurred.</param>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteError<T1, T2>(this T2 cmdlet, string message, ErrorCategory category, object targetObject = null) 
            where T1 : Exception where T2 : Cmdlet
        {
            Exception ex = (Exception)Activator.CreateInstance(typeof(T1), message);
            var errRec = new ErrorRecord(ex, ex.GetType().FullName, category, targetObject);
            cmdlet.WriteError(errRec);
        }

        /// <summary>
        /// Writes the specified message into the given <see cref="Exception"/> type provided all to the error pipe.
        /// </summary>
        /// <remarks><typeparamref name="T1"/> should have a public constructor that accepts a string (message) parameter.</remarks>
        /// <typeparam name="T2">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <typeparam name="T1">The type of exception to create in the <see cref="ErrorRecord"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="message">The message of the <see cref="Exception"/>.</param>
        /// <param name="category">The category that best describes the error.</param>
        /// <param name="targetObject">This is the object against which the <see cref="Cmdlet"/> or provider was operating on when the error occurred.</param>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteError<T>(this T cmdlet, string message, Type exceptionType, ErrorCategory category, object targetObject = null) where T : Cmdlet
        {
            if (!typeof(Exception).IsAssignableFrom(exceptionType))
                throw new InvalidCastException(string.Format("The type \"System.Exception\" is not assignable from the type \"{0}\"", exceptionType.FullName));

            Exception ex = (Exception)Activator.CreateInstance(exceptionType, message);
            var errRec = new ErrorRecord(ex, ex.GetType().FullName, category, targetObject);
            cmdlet.WriteError(errRec);
        }

        /// <summary>
        /// Formats a given string and arguments and sends it to <see cref="Cmdlet.WriteVerbose(string)"/> to display verbose information.
        /// </summary>
        /// <remarks>
        /// Use this to display more detailed information
        /// about the activity of your <see cref="Cmdlet"/>.  By default, verbose output will not be displayed, although this can be configured with the
        /// VerbosePreference shell variable or the -Verbose and -Debug command-line parameters.
        /// </remarks>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="formattedText">A composite format string for the verbose output.</param>
        /// <param name="arguments">The object(s) to format.</param>
        /// <exception cref="FormatException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteVerboseFormat<T>(this T cmdlet, string formattedText, params object[] arguments) where T : Cmdlet
        {
            if (string.IsNullOrWhiteSpace(formattedText))
                return;

            cmdlet.WriteVerbose(StringFormatter.FormatString(formattedText, arguments));
        }

        /// <summary>
        /// Formats a given string and arguments and sends it to <see cref="Cmdlet.WriteWarning(string)"/> to display warning information.
        /// </summary>
        /// <remarks>
        /// Use this to display more detailed 
        /// information about the activity of your <see cref="Cmdlet"/>.  By default, warning output will not be displayed, although this can 
        /// be configured with the WarningPreference shell variable or the -Verbose and -Debug command-line parameters.
        /// </remarks>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="formattedText">A composite format string for the warning output.</param>
        /// <param name="arguments">The object(s) to format.</param>
        /// <exception cref="FormatException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteWarningFormat<T>(this T cmdlet, string formattedText, params object[] arguments) where T : Cmdlet
        {
            if (string.IsNullOrWhiteSpace(formattedText))
                return;

            cmdlet.WriteWarning(StringFormatter.FormatString(formattedText, arguments));
        }

        /// <summary>
        /// Sends an <see cref="object"/> to the PowerShell output stream and optionally specifies whether or not to enumerate
        /// it if it's a collection.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="Cmdlet"/>.</typeparam>
        /// <param name="cmdlet">The <see cref="Cmdlet"/> the method is extending.</param>
        /// <param name="object">The object to send to the pipeline</param>
        /// <param name="enumerateCollection">Indicates whether the object will be enumerated as its sent.</param>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="PipelineStoppedException"/>
        public static void WriteToPipeline<T>(this T cmdlet, object @object, bool enumerateCollection = true) where T : Cmdlet
        {
            if (@object != null)
            {
                cmdlet.WriteObject(@object, enumerateCollection);
            }
        }
    }
}
