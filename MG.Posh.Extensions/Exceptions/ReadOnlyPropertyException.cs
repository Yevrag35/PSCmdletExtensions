using MG.Posh.Internal;
using System;
using System.Runtime.Serialization;

namespace MG.Posh.Exceptions
{
    [Serializable]
    public sealed class ReadOnlyPropertyException : Exception, ISerializable
    {
        const string MSG = "Unable to set the property value because '{0}' is marked as read-only.";

        public string? PropertyName { get; }

        public ReadOnlyPropertyException(string propertyName)
            : base(string.Format(MSG, propertyName))
        {
            this.PropertyName = propertyName;
        }

        private ReadOnlyPropertyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Guard.NotNull(info, nameof(info));
            this.PropertyName = info.GetString(nameof(this.PropertyName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));

            info.AddValue(nameof(this.PropertyName), this.PropertyName, typeof(string));

            base.GetObjectData(info, context);
        }
    }
}

