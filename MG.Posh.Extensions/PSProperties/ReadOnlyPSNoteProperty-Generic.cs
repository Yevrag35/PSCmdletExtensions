using MG.Posh.Internal.Reflection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MG.Posh.PSProperties
{
    public abstract class ReadOnlyPSNoteProperty<T> : ReadOnlyPSNoteProperty
    {

        [MaybeNull]
        public abstract T ValueAsT { get; }


        protected ReadOnlyPSNoteProperty(string propertyName)
            : base(propertyName)
        {
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
            return this.ValueAsT;
        }
        protected override string GetValueToString()
        {
            return this.ValueAsT?.ToString() ?? string.Empty;
        }
    }
}

