using System.Management.Automation;
using System;
using MG.Posh.Internal.Reflection;
using MG.Posh.Internal;

namespace MG.Posh.Exceptions
{
    /// <summary>
    /// Custom extension methods for <see cref="Exception"/> classes to be projected into <see cref="ErrorRecord"/>
    /// instances.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Projects the <see cref="Exception"/> to a PowerShell <see cref="ErrorRecord"/> instance.
        /// </summary>
        /// <param name="exception">The exception which describes the error.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <returns>
        ///     A new <see cref="ErrorRecord"/> instance constructed from
        ///     <paramref name="exception"/> with the default <see cref="ErrorCategory"/>.
        /// </returns>
        public static ErrorRecord ToRecord(this Exception exception)
        {
            return ToRecord(exception, ErrorCategory.NotSpecified, null);
        }

        /// <summary>
        /// Projects the <see cref="Exception"/> to a PowerShell <see cref="ErrorRecord"/> with the specified
        /// <see cref="ErrorCategory"/>.
        /// </summary>
        /// <param name="exception">The exception which describes the error.</param>
        /// <param name="category">
        ///     The <see cref="ErrorCategory"/> which best describes the error.
        /// </param>
        /// <returns>
        ///     A new <see cref="ErrorRecord"/> instance constructed from
        ///     <paramref name="exception"/> with the specified <see cref="ErrorCategory"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        public static ErrorRecord ToRecord(this Exception exception, ErrorCategory category)
        {
            return ToRecord(exception, category, null);
        }

        /// <summary>
        /// Projects the <see cref="Exception"/> to a PowerShell <see cref="ErrorRecord"/> with the specified
        /// <see cref="ErrorCategory"/> and the target object the cmdlet or provider was operating when the error
        /// occurred.
        /// </summary>
        /// <param name="exception">The exception which describes the error.</param>
        /// <param name="category">
        ///     The <see cref="ErrorCategory"/> which best describes the error.
        /// </param>
        /// <inheritdoc cref="ErrorRecord.TargetObject">
        ///     <paramref name="targetObj"/>
        /// </inheritdoc>
        /// <returns>
        ///     A new <see cref="ErrorRecord"/> instance constructed from
        ///     <paramref name="exception"/> with the specified <see cref="ErrorCategory"/>
        ///     and target object.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        public static ErrorRecord ToRecord(this Exception exception, ErrorCategory category, object? targetObj)
        {
            Guard.NotNull(exception, nameof(exception));

            Type type = exception.GetType();
            return new ErrorRecord(exception, TypeExtensions.GetTypeName(type), category, targetObj);
        }
    }
}