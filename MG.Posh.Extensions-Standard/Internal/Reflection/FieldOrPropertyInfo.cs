using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MG.Posh.Internal.Reflection
{
    internal interface IMemberSetter
    {
        void SetValue(object instance, object value);
    }

    internal readonly struct FieldOrPropertyInfo : IMemberSetter
    {
        readonly bool _isNotEmpty;
        readonly bool _isField;
        readonly FieldInfo? _fi;
        readonly PropertyInfo? _pi;

        public bool IsEmpty => !_isNotEmpty;
        public bool IsField => _isNotEmpty && _isField;
        public bool IsProperty => _isNotEmpty && !_isField;

        public FieldOrPropertyInfo(FieldInfo fieldInfo)
        {
            Guard.NotNull(fieldInfo, nameof(fieldInfo));

            _isNotEmpty = true;
            _fi = fieldInfo;
            _isField = true;
            _pi = null;
        }
        public FieldOrPropertyInfo(PropertyInfo propertyInfo)
        {
            Guard.NotNull(propertyInfo, nameof(propertyInfo));

            _isNotEmpty = true;
            _pi = propertyInfo;
            _isField = false;
            _fi = null;
        }

        [return: MaybeNull]
        public object? GetValue(object instance)
        {
            if (this.IsProperty)
            {
                return _pi!.GetValue(instance);
            }
            else if (this.IsField)
            {
                return _fi!.GetValue(instance);
            }

            return null;
        }
        public void SetValue(object? instance, object? value)
        {
            if (this.IsProperty)
            {
                _pi!.SetValue(instance, value);
            }
            else if (this.IsField)
            {
                _fi!.SetValue(instance, value);
            }
        }
    }
}

