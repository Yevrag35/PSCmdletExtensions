using MG.Posh.Internal;
using MG.Posh.Internal.Reflection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Xml.Linq;

namespace MG.Posh.PSObjects
{
    public static class PSObjectProjectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyExpressions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public static PSObject ProjectTo<T>(this T obj, params Expression<Func<T, object?>>[] propertyExpressions) where T : class
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(propertyExpressions, nameof(propertyExpressions));

            var pso = new PSObject(propertyExpressions.Length);

            foreach (var exp in propertyExpressions)
            {
                if (ExpressionExtensions.TryGetAsMember(exp, out MemberExpression? memEx))
                {
                    Func<T, object?> func = exp.Compile();
                    pso.Properties.Add(new PSNoteProperty(memEx.Member.Name, func(obj)));
                }
            }

            return pso;
        }

        public static PSObject ProjectTo<T, TProject>(this T obj, Expression<Func<T, TProject>> projectExpression)
            where TProject : notnull
        {
            var compiledExpression = projectExpression.Compile();
            TProject result = compiledExpression(obj);

            var psObject = new PSObject();
            var resultType = result.GetType();

            if (!(projectExpression.Body is NewExpression newExpression))
            {
                throw new ArgumentException($"Expected \"{nameof(projectExpression)}.{nameof(LambdaExpression.Body)}\" of type \"{projectExpression.Body.GetType().GetTypeName()}\" to be of type \"{typeof(NewExpression).GetTypeName()}\".");
            }

            foreach (var member in newExpression.Members)
            {
                var propInfo = resultType.GetProperty(member.Name);
                if (!(propInfo is null))
                {
                    object? value = propInfo.GetValue(result);
                    psObject.Properties.Add(new PSNoteProperty(member.Name, value));
                }
            }

            return psObject;
        }
    }
}

