using MG.Posh.Internal;
using MG.Posh.Internal.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;

namespace MG.Posh.Attributes
{
    /// <summary>
    /// Validates the parameter's value <see cref="Type"/> matches at least one defined.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ValidateTypesAttribute : ValidateArgumentsAttribute
    {
        readonly bool _allowsNull;

        public bool IncludeFamily { get; }

#if NET6_0_OR_GREATER
        protected IReadOnlySet<Type> AllowedTypes { get; }
        protected ValidateTypesAttribute(bool allowNullElements, IReadOnlySet<Type> allowedTypes)
#else
        protected ISet<Type> AllowedTypes { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowNullElements"></param>
        /// <param name="allowedTypes"></param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="allowedTypes"/> does not contain at least 1 element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="allowedTypes"/> is null.
        /// </exception>
        protected ValidateTypesAttribute(bool allowNullElements, bool excludeSubclasses, ISet<Type> allowedTypes)
#endif
        {
            Guard.NotNull(allowedTypes, nameof(allowedTypes));
            if (allowedTypes.Count <= 0)
            {
                throw new ArgumentException("At least one type must be allowed when specifying this attribute.");
            }

            _allowsNull = allowNullElements;
            this.AllowedTypes = allowedTypes;
            this.IncludeFamily = !excludeSubclasses;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowNullElements"></param>
        /// <param name="allowedTypes"></param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="allowedTypes"/> does not contain at least 1 element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="allowedTypes"/> is null.
        /// </exception>
        public ValidateTypesAttribute(bool allowNullElements, bool excludeSubclasses, params Type[] allowedTypes)
        {
            Guard.NotNull(allowedTypes, nameof(allowedTypes));
            if (allowedTypes.Length <= 0)
            {
                throw new ArgumentException("At least one type must be allowed when specifying this attribute.");
            }

            _allowsNull = allowNullElements;
            this.AllowedTypes = new HashSet<Type>(allowedTypes);
            this.IncludeFamily = !excludeSubclasses;
        }

        protected virtual bool ShouldThrowOnNull(bool allowsNull, EngineIntrinsics engineIntrinsics)
        {
            return !allowsNull;
        }

        protected sealed override void Validate(object? arguments, EngineIntrinsics engineIntrinsics)
        {
            if (arguments is null)
            {
                return;
            }
            else if (!(arguments is IEnumerable col))
            {
                this.ValidateElement(arguments, engineIntrinsics);
            }
            else
            {
                foreach (object? element in col)
                {
                    this.ValidateElement(element, engineIntrinsics);
                }
            }
        }
        private void ValidateElement(object? element, EngineIntrinsics engineIntrinsics)
        {
            if (element is null)
            {
                if (this.ShouldThrowOnNull(_allowsNull, engineIntrinsics))
                {
                    ThrowElementIsNull();
                }

                return;
            }

            this.ValidateType(element.GetType(), element, engineIntrinsics);
        }

        protected virtual void ValidateType(Type elementType, object element, EngineIntrinsics engineIntrinsics)
        {
            if (!this.AllowedTypes.Contains(elementType))
            {
                if (!this.IncludeFamily || !this.AllowedTypes.Any(x => elementType.IsAssignableFrom(x)))
                {
                    this.ThrowInvalidType(element.ToString(), elementType, this.AllowedTypes);
                }
            }
        }

        const string NULL_ELEMENT = "A passed element to the argument was null.";
        /// <summary>
        /// Throws a <see cref="ValidationMetadataException"/> stating that the element was 
        /// <see langword="null"/>.
        /// </summary>
        /// <remarks>
        ///     This is usually only thrown when <see cref="AllowNullElements"/> is <see langword="false"/>.
        /// </remarks>
        /// <exception cref="ValidationMetadataException"></exception>
        [DoesNotReturn]
        protected static void ThrowElementIsNull()
        {
            throw new ValidationMetadataException(NULL_ELEMENT);
        }

        const string INVALID_FORMAT = "The element \"{0}\" of type \"{1}\" is not of an valid type for the parameter. Valid types are: \"{2}\"";
        /// <summary>
        /// Throws an <see cref="ValidationMetadataException"/> stating <paramref name="element"/> is not
        /// of an accepted type determined by the provided <paramref name="allowedTypes"/>.
        /// </summary>
        /// <param name="element">The offending element that did not pass validation.</param>
        /// <param name="allowedTypes">
        ///     The given allowed types that <paramref name="element"/> was not of part of. These are
        ///     included in the subsequent exception message. If this value is <see langword="null"/>,
        ///     then <see cref="AllowedTypes"/> is used.
        /// </param>
        /// <exception cref="ValidationMetadataException"></exception>
        [DoesNotReturn]
        protected void ThrowInvalidType(string? elementAsString, Type elementType, IEnumerable<Type>? allowedTypes)
        {
            string typeStr = elementType.GetPSTypeName(removeBrackets: true);
            allowedTypes ??= this.AllowedTypes;

            string typesAsString = string.Join(
                separator: "\", \"", 
                values: allowedTypes
                    .Select(x => 
                        x.GetPSTypeName(removeBrackets: true))
                    .OrderBy(x => x));

            string msg = string.Format(
                format: INVALID_FORMAT,
                arg0: elementAsString,
                arg1: typeStr,
                arg2: typesAsString);

            throw new ValidationMetadataException(msg);
        }
    }
}

