using MG.Posh.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace MG.Posh.Extensions.Variables
{
    /// <summary>
    /// 
    /// </summary>
    public static class PSIntrinsicsExtensions
    {
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="DriveNotFoundException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="ProviderInvocationException"/>
        /// <exception cref="ProviderNotFoundException"/>
        [return: NotNullIfNotNull(nameof(defaultValue))]
        public static T GetValueAs<T>(this PSVariableIntrinsics intrinsics, string variableName, T defaultValue)
        {
            if (!TryGetValue(intrinsics, variableName, out T result))
            {
                result = defaultValue;
            }

            return result ?? defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="intrinsics"></param>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="DriveNotFoundException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="ProviderInvocationException"/>
        /// <exception cref="ProviderNotFoundException"/>
        /// <returns></returns>
        public static bool TryGetValue<T>(this PSVariableIntrinsics intrinsics, string variableName, [MaybeNull] out T value)
        {
            Guard.NotNull(intrinsics, nameof(intrinsics));
            Guard.NotNullOrEmpty(variableName, nameof(variableName));

            object? val = intrinsics.GetValue(variableName);

            if (val is T tVal)
            {
                value = tVal;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }
}
