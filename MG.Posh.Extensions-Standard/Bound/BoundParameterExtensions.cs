using MG.Posh.Internal;
using MG.Posh.Internal.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        static readonly Lazy<HashSet<string>> _builtInNames;
        static readonly PropertyInfo? _positionalProperty;
        static readonly Version _max = new Version(7, 4, 0, int.MaxValue);

        static BoundParameterExtensions()
        {
            _builtInNames = new Lazy<HashSet<string>>(GetBuiltInParameterNames);

            PropertyInfo boundProp = typeof(InvocationInfo)
                .GetProperty("BoundParameters", BindingFlags.Public | BindingFlags.Instance);

            var ass = typeof(PSCmdlet).Assembly;
            var version = ass.GetCustomAttributes<AssemblyFileVersionAttribute>().FirstOrDefault();

            if (!Version.TryParse(version?.Version, out Version? vers) || _max < vers)
            {
                Debug.Fail("_max needs to be updated for this version.");
            }

            var query = ass.GetTypes().Where(x => "PSBoundParametersDictionary" == x.Name);

            var first = query.FirstOrDefault();
            _positionalProperty = first?.GetProperty("BoundPositionally");
        }


        public static bool IsParameterBoundPositionally(this PSCmdlet cmdlet, string parameterName)
        {
            Guard.NotNull(cmdlet, nameof(cmdlet));
            Guard.NotNullOrEmpty(parameterName, nameof(parameterName));

            if (_positionalProperty is null)
            {
                Debug.Fail("Unable to find Positional List in BoundParameters");
                return false;
            }

            return _positionalProperty.GetValue(cmdlet.MyInvocation.BoundParameters) is ICollection<string> boundPositionally
                   &&
                   boundPositionally.Contains(parameterName);
        }

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