using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace MG.Posh.PSProperties
{
    /// <summary>
    /// A generic version of <see cref="PSNoteProperty"/> allowing for a non-casted version of the 
    /// property's value and serves as a property that is a simple name-value pair.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="PSNotePropertyBase"/>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class PSNoteProperty<T> : PSNotePropertyBase
    {
        [MaybeNull]
        private T _value;

        /// <inheritdoc cref="PSNoteProperty.IsSettable"/>
        /// <remarks>
        ///     This will always be <see langword="true"/>.
        /// </remarks>
        public sealed override bool IsSettable => true;

        /// <summary>
        /// Gets or Sets the value of the property as <typeparamref name="T"/>.
        /// </summary>
        [MaybeNull]
        public virtual T ValueAsT
        {
            get => _value;
            set => _value = value;
        }
        /// <inheritdoc cref="PSNoteProperty.Value"/>
        /// <exception cref="PSInvalidCastException"/>
        public sealed override object? Value
        {
            get => this.ValueAsT;
            set => this.SetBackingValue(value);
        }

        public PSNoteProperty(string propertyName)
        {
            this.SetMemberName(propertyName);
        }
        public PSNoteProperty(string propertyName, T value)
            : this(propertyName)
        {
            _value = value;
        }

        public sealed override PSMemberInfo Copy()
        {
            T val = _value!;
            if (_value is ICloneable clonable)
            {
                val = (T)clonable.Clone();
            }

            return this.Copy(val!);
        }
        /// <summary>
        /// Copies this property into a new instance with a "cloned" copy of its value.
        /// </summary>
        /// <param name="clonedValue">The cloned value to set </param>
        /// <returns></returns>
        protected virtual PSNoteProperty<T> Copy(T clonedValue)
        {
            return new PSNoteProperty<T>(this.Name, clonedValue);
        }
        protected override string GetValueToString()
        {
            return this.ValueAsT?.ToString() ?? string.Empty;
        }
        private void SetBackingValue(object? value)
        {
            if (value is T tVal)
            {
                this.ValueAsT = tVal;
                return;
            }
            else if (this.TryConvertValue(value, value is null, out T tConv))
            {
                this.ValueAsT = tConv!;
                return;
            }
            else
            {
                this.ValueAsT = LanguagePrimitives.ConvertTo<T>(value);
            }
        }
        protected virtual bool TryConvertValue(object? value, bool valueIsNull, [MaybeNull] out T valueToSet)
        {
            valueToSet = default;
            return false;
        }
    }
}

