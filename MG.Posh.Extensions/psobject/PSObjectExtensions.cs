using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;

namespace MG.Posh.Extensions
{
    /// <summary>
    /// An extension class providing <see cref="Cmdlet"/> methods of creating <see cref="PSObject"/>
    /// structures from segments of other .NET classes and/or structs.
    /// </summary>
    public static class PSObjectExtensions
    {
        /// <summary>
        /// Adds a <see cref="PSNoteProperty"/> to an existing <see cref="PSObject"/> with the names
        /// and values of members of the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the specified class or struct that the expressions represent..</typeparam>
        /// <param name="pso">The <see cref="PSObject"/> the method is extended.</param>
        /// <param name="obj">The object whose member expressions will be resolved and added to the <see cref="PSObject"/>.</param>
        /// <param name="memberExpressions">The expression representations where the names and values will be populated from.</param>
        public static void AddFromObject<T1, T2>(this PSObject pso, T1 obj, 
            params Expression<Func<T1, T2>>[] memberExpressions)
        {
            if (memberExpressions == null || memberExpressions.Length <= 0)
                return;

            foreach (Expression<Func<T1, T2>> ex in memberExpressions)
            {
                pso.Properties.Add<T1, T2>(obj, ex);
            }
        }

        internal static void Add<T1, T2>(this PSMemberInfoCollection<PSPropertyInfo> props,
            T1 obj, Expression<Func<T1, T2>> memberExpression)
        {
            MemberInfo mi = null;
            if (memberExpression.Body is MemberExpression memEx)
            {
                mi = memEx.Member;
            }
            else if (memberExpression.Body is UnaryExpression unEx && unEx.Operand is MemberExpression unExMem)
            {
                mi = unExMem.Member;
            }

            if (mi != null)
            {
                Func<T1, T2> func = memberExpression.Compile();
                props.Add(new PSNoteProperty(mi.Name, func(obj)));
            }
        }

        private static void Add(this PSMemberInfoCollection<PSPropertyInfo> props, string name, object value)
        {
            props.Add(new PSNoteProperty(name, value));
        }

        /// <summary>
        /// Converts a <see cref="Hashtable"/> into a <see cref="PSObject"/>.  All key/value pairs
        /// will be copied to a new <see cref="PSObject"/> as <see cref="PSNoteProperty"/> members.
        /// </summary>
        /// <remarks>If the provided <see cref="Hashtable"/> is empty, then <see langword="null"/>
        /// will be returned instead of an empty <see cref="PSObject"/>.</remarks>
        /// <param name="ht">The hasthable to transform.</param>
        public static PSObject ToPSObject(this Hashtable ht)
        {
            if (ht == null || ht.Count <= 0)
                return null;

            var pso = new PSObject();
            foreach (DictionaryEntry kvp in ht)
            {
                pso.Properties.Add(Convert.ToString(kvp.Key), kvp.Value);
            }
            return pso;
        }
    }
}
