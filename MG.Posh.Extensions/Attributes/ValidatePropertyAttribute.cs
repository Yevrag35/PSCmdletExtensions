using MG.Posh.Internal;
using MG.Posh.Internal.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace MG.Posh.Attributes
{
    public sealed class ValidatePropertyAttribute : ValidateArgumentsAttribute
    {
        readonly HashSet<Type> _cache;
        readonly string[] _names;
        readonly HashSet<string> _working;

        public ValidatePropertyAttribute(params string[] mustHaveNames)
        {
            Guard.NotNull(mustHaveNames, nameof(mustHaveNames));
            if (mustHaveNames.Length <= 0)
            {
                throw new ArgumentException("At least 1 name must be provided to this attribute.");
            }

            _cache = new HashSet<Type>();
            _names = mustHaveNames;
            _working = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
        }

        protected sealed override void Validate(object? arguments, EngineIntrinsics engineIntrinsics)
        {
            switch (arguments)
            {
                case PSObject pso:
                    TryCheckPSO(pso, _names, _working);
                    return;

                case null:
                    return;

                default:
                    TryCheckType(arguments, _names, _working, _cache);
                    return;
            }
        }

        private static void TryCheckPSO(PSObject pso, IEnumerable<string> names, ISet<string> working)
        {
            working.Clear();
            working.UnionWith(names);

            IEnumerable<string> propNames = pso.Properties.Select(x => x.Name);
            working.ExceptWith(propNames);

            if (working.Count > 0)
            {
                throw new ValidationMetadataException($"The value \"{pso}\" of type \"psobject\" does not contain the required properties: \"{string.Join("\", \"", working)}\"");
            }
        }
        private static void TryCheckType(object obj, IEnumerable<string> names, ISet<string> working, ICollection<Type> knownGoodTypes)
        {
            Type type = obj.GetType();

            if (knownGoodTypes.Contains(type))
            {
                return;
            }

            working.Clear();
            working.UnionWith(names);

            IEnumerable<string> memberNames = type
                .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => MemberTypes.Field == x.MemberType || MemberTypes.Property == x.MemberType)
                .Select(x => x.Name);

            working.ExceptWith(memberNames);
            if (working.Count > 0)
            {
                throw new ValidationMetadataException($"The value \"{obj}\" of type \"{type.GetPSTypeName()}\" does not contain the required properties/fields: \"{string.Join("\", \"", working)}\"");
            }

            knownGoodTypes.Add(type);
        }
    }
}

