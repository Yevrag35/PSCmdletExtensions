using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace MG.Posh.PSProperties
{
    public class PSNoteProperty<T> : PSNotePropertyBase
    {
        [MaybeNull]
        private T _value;

        public sealed override bool IsSettable => true;

        [MaybeNull]
        public virtual T ValueAsT
        {
            get => _value;
            set => _value = value;
        }
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
            T val = _value;
            if (_value is ICloneable clonable)
            {
                val = (T)clonable.Clone();
            }

            return this.Copy(val!);
        }
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
            if (this.TryConvertValue(value, value is null, out T tVal))
            {
                this.ValueAsT = tVal!;
            }
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

