using MG.Posh.Internal.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace MG.Posh.Extensions.Bound
{
    /// <summary>
    /// An extension class allowing to easily check if PowerShell <see cref="PSCmdlet"/> parameters are bound.
    /// </summary>
    public static partial class BoundParameterExtensions
    {
        static readonly Lazy<HashSet<string>> _builtInNames = new Lazy<HashSet<string>>(GetBuiltInParameterNames);

        private static Type GetBuiltInParameterClass()
        {
            Type attType = typeof(BuiltInParameterClassAttribute);
            foreach (Type type in attType.Assembly.GetExportedTypes())
            {
                if (type.IsAbstract && type.IsSealed && type.IsDefined(attType, false))
                {
                    return type;
                }
            }

            throw new ArgumentException("Unable to find the BuiltInParameter class.");
        }
        private static HashSet<string> GetBuiltInParameterNames()
        {
            Type builtInClass = GetBuiltInParameterClass();
            Type pNameAtt = typeof(BuiltInParameterAttribute);
            Type strType = typeof(string);

            var set = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (FieldInfo fi in builtInClass.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (fi.IsDefined(pNameAtt) && strType.Equals(fi.FieldType) && fi.GetValue(null) is string fiValue)
                {
                    set.Add(fiValue);
                }
            }

            set.TrimExcess();
            return set;
        }
    }
}