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
        {
            var psObject = new PSObject();
            dynamic result = null!;
            Type? resultType = null;
            bool resultConstructed = false;
            Type objType = typeof(T);

            if (projectExpression.Body is NewExpression newExpression)
            {
                foreach (var member in newExpression.Members)
                {
                    object? value;
                    var directPropInfo = objType.GetProperty(member.Name);

                    if (!(directPropInfo is null))
                    {
                        // Direct property access
                        value = directPropInfo.GetValue(obj);
                    }
                    else
                    {
                        // Construct the anonymous object if not already done
                        if (!resultConstructed)
                        {
                            var compiledExpression = projectExpression.Compile();
                            result = compiledExpression(obj)!;
                            resultType = result.GetType();
                            resultConstructed = true;
                        }

                        // Use constructed anonymous object for complex transformations
                        var resultPropInfo = resultType?.GetProperty(member.Name);
                        value = resultPropInfo?.GetValue(result);
                    }

                    psObject.Properties.Add(new PSNoteProperty(member.Name, value));
                }
            }

            return psObject;
        }

        //public static PSObject ProjectTo<T, TProject>(this T obj, Expression<Func<T, TProject>> projectExpression)
        //{
        //    var pso = new PSObject();

        //    if (!(projectExpression.Body is NewExpression newExpression))
        //    {
        //        throw new ArgumentException($"Expected an expression body of type \"{typeof(NewExpression).GetTypeName()}\" but got \"{projectExpression.Body.GetType().GetTypeName()}\"");
        //    }

        //    var propertyAccessors = ProcessExpression<T>(newExpression);

        //    foreach (var (propertyName, accessor) in propertyAccessors)
        //    {
        //        object? value = accessor(obj);
        //        pso.Properties.Add(new PSNoteProperty(propertyName, value));
        //    }

        //    return pso;
        //}

        //private static IEnumerable<(string PropertyName, Func<T, object> Accessor)> ProcessExpression<T>(NewExpression newExpression)
        //{
        //    if (newExpression.Members.Count <= 0)
        //    {
        //        yield break;
        //    }

        //    var paramExpr = Expression.Parameter(typeof(T));

        //    foreach (var member in newExpression.Members)
        //    {
        //        var property = Expression.Property(paramExpr, member.Name);

        //        UnaryExpression convertedExp = Expression.Convert(property, typeof(object));
        //        var lambda = Expression.Lambda<Func<T, object>>(convertedExp, paramExpr);

        //        var compiledLambda = lambda.Compile();

        //        yield return (member.Name, compiledLambda);
        //    }
        //}
    }
}

