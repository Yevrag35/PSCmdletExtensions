using System;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace MG.Posh.PSObjects
{
    public static class BaseObject
    {
        /// <summary>
        /// Returns the base object from the specified <see cref="object"/> parameter.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>
        ///     If <paramref name="obj"/> is a PSObject instance, then <see cref="PSObject.BaseObject"/> 
        ///     is returned, unless the PSObject is equal to <see cref="AutomationNull.Value"/> in which
        ///     case <see langword="null"/> is returned; otherwise <paramref name="obj"/> is returned as-is.
        /// </returns>
        public static object? FromObject(object? obj)
        {
            return FromObject(obj, getImmediateBaseObject: false);
        }
        /// <summary>
        /// Returns the base object from the specified <see cref="object"/> parameter.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="getImmediateBaseObject">
        ///     Indicates whether to retrieve the <see cref="PSObject.ImmediateBaseObject"/> instead of 
        ///     <see cref="PSObject.BaseObject"/>.
        /// </param>
        /// <returns>
        ///     If <paramref name="obj"/> is a PSObject instance, then either
        ///     <see cref="PSObject.BaseObject"/> or <see cref="PSObject.ImmediateBaseObject"/>
        ///     (as determined by <paramref name="getImmediateBaseObject"/>)
        ///     is returned, unless the PSObject is equal to <see cref="AutomationNull.Value"/> in which
        ///     case <see langword="null"/> is returned; otherwise <paramref name="obj"/> is returned as-is.
        /// </returns>
        public static object? FromObject(object? obj, bool getImmediateBaseObject)
        {
            if (obj is null || !(obj is PSObject psObj))
            {
                return obj;
            }

            if (AutomationNull.Value.Equals(psObj))
            {
                return null;
            }

            return !getImmediateBaseObject ? psObj.BaseObject : psObj.ImmediateBaseObject;
        }
    }
}

