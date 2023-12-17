using System;

namespace MG.Posh.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class BuiltInParameterClassAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    internal sealed class BuiltInParameterAttribute : Attribute
    {
    }
}

