using MG.Posh.Internal.Reflection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MG.Posh.PSProperties
{
    public abstract class ReadOnlyPSNoteProperty<T> : ReadOnlyPSNoteProperty
    {
        private T _value;

        [MaybeNull]
        public T ValueAsT => _value;

        protected ReadOnlyPSNoteProperty(string propertyName)
            : base(propertyName)
        {
            _value = default!;
        }
        protected ReadOnlyPSNoteProperty(string propertyName, T value)
            : base(propertyName)
        {
            _value = value;
            //this.SetValue(value, value is null);
        }

        protected sealed override ReadOnlyPSNoteProperty Copy(object? clonedValue)
        {
            if (!(clonedValue is T clonedTVal))
            {
                throw new InvalidOperationException($"The cloned value is not of the generic type '{this.TypeNameOfValue}'.");
            }

            return this.Copy(clonedTVal, clonedTVal is null);
        }
        protected abstract ReadOnlyPSNoteProperty<T> Copy(T clonedValue, bool valueIsNull);
        
        protected override string GetPSTypeName()
        {
            return typeof(T).GetPSTypeName(removeBrackets: true);
        }
        protected sealed override object? GetValue()
        {
            return _value;
        }
        protected override string GetValueToString()
        {
            return _value?.ToString() ?? string.Empty;
        }

        protected sealed override void SetValue(object? value)
        {
            if (this.TryConvertValue(value, value is null, out T valueToSet))
            {
                this.SetValue(valueToSet!, valueToSet is null);
            }
        }
        protected virtual void SetValue([MaybeNull] T value, bool valueIsNull)
        {
            _value = value;
        }
        protected virtual bool TryConvertValue(object? value, bool valueIsNull, [MaybeNull] out T valueToSet)
        {
            if (value is T tVal)
            {
                valueToSet = tVal;
                return true;
            }
            else
            {
                valueToSet = default;
                return false;
            }
        }
    }
}

