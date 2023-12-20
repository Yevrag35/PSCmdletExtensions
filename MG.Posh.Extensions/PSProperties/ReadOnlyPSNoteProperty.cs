using MG.Posh.Internal;
using System.Management.Automation;
using MG.Posh.Exceptions;
using System;

namespace MG.Posh.PSProperties
{
    /// <summary>
    /// A read-only, <see langword="abstract"/> base class of <see cref="PSNoteProperty"/>.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="PSNotePropertyBase"/>
    /// </remarks>
    public abstract class ReadOnlyPSNoteProperty : PSNotePropertyBase
    {
        /// <summary>
        /// Gets <see langword="false"/> since the value of an <see cref="ReadOnlyPSNoteProperty"/>
        /// can never be set after constructed.
        /// </summary>
        public sealed override bool IsSettable => false;

        /// <summary>
        ///     Gets the value of the <see cref="ReadOnlyPSNoteProperty"/>.
        /// </summary>
        /// <remarks>
        ///     Even though this property has a setter, a <see cref="SetValueException"/> will be thrown if 
        ///     trying to overwrite the initial value set from the constructor.
        /// </remarks>
        /// <exception cref="SetValueException"/>
        public sealed override object? Value
        {
            get => this.GetValue();
            set
            {
                var ex = new ReadOnlyPropertyException(this.Name);
                throw new SetValueException(ex.Message, ex);
            }
        }

        protected ReadOnlyPSNoteProperty(string propertyName)
        {
            this.SetMemberName(propertyName);
        }

        protected abstract ReadOnlyPSNoteProperty Copy(object? clonedValue);
        public sealed override PSMemberInfo Copy()
        {
            object? value = this.Value;
            if (value is ICloneable clonable)
            {
                value = clonable.Clone();
            }

            return this.Copy(value);
        }

        protected abstract object? GetValue();
        protected override string GetValueToString()
        {
            return this.GetValue()?.ToString() ?? string.Empty;
        }
    }
}

