using MG.Posh.Internal.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System;
using MG.Posh.Internal;

namespace MG.Posh.Internal.Reflection
{
    /// <summary>
    /// Custom extension methods for <see cref="Expression"/> and derived classes.
    /// </summary>
    internal static class ExpressionExtensions
    {
        /// <summary>
        /// Attempts to retrieve the body of a <see cref="LambdaExpression"/> as a member 
        /// declaration.
        /// </summary>
        /// <param name="expression">
        ///     The expression whose body could possibly be a member expression.
        /// </param>
        /// <param name="memberExpression">
        ///     When this method returns <see langword="true"/>, the 
        ///     <see cref="MemberExpression"/> read from the body of 
        ///     <paramref name="expression"/>; otherwise, a <see langword="null"/> reference.
        ///     <para>
        ///         This parameter is passed uninitialized.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <see cref="LambdaExpression.Body"/> is 
        ///     <see cref="MemberExpression"/> or is a <see cref="UnaryExpression"/> whose 
        ///     <see cref="UnaryExpression.Operand"/> is <see cref="MemberExpression"/>; otherwise,
        ///     <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        internal static bool TryGetAsMember(this LambdaExpression expression, [NotNullWhen(true)] out MemberExpression? memberExpression)
        {
            Guard.NotNull(expression, nameof(expression));

            if (expression.Body is MemberExpression memEx)
            {
                memberExpression = memEx;
                return true;
            }
            else if (expression.Body is UnaryExpression unEx && unEx.Operand is MemberExpression unExMem)
            {
                memberExpression = unExMem;
                return true;
            }

            memberExpression = null;
            return false;
        }

        /// <summary>
        /// Attempts to retrieve a <see cref="LambdaExpression"/> instance's body as a
        /// <see cref="MemberExpression"/> of a defined <see cref="PropertyInfo"/> or 
        /// <see cref="FieldInfo"/> that is settable.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <param name="setter">
        ///     When this method returns, the <see cref="FieldOrPropertyInfo"/>
        ///     of the expression's <see cref="MemberExpression.Member"/>; otherwise, the
        ///     default instance of <see cref="FieldOrPropertyInfo"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="expression"/>'s body is a valid 
        ///     <see cref="MemberExpression"/> and that <see cref="MemberExpression.Member"/> is a
        ///     settable <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>; otherwise,
        ///     <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        internal static bool TryGetAsSetter(this LambdaExpression expression, [NotNullWhen(true)] out IMemberSetter? setter)
        {
            Guard.NotNull(expression, nameof(expression));

            OneOf<FieldInfo, PropertyInfo> tempOne;
            if (expression.Body is MemberExpression memEx)
            {
                tempOne = GetAsEitherInfo(memEx);
            }
            else if (expression.Body is UnaryExpression unEx && unEx.Operand is MemberExpression unExMem)
            {
                tempOne = GetAsEitherInfo(unExMem);
            }
            else
            {
                tempOne = OneOf<FieldInfo, PropertyInfo>.Null;
            }

            FieldOrPropertyInfo info = default;
            if (tempOne.TryGetT1(out PropertyInfo? pi, out FieldInfo? fi))
            {
                info = new FieldOrPropertyInfo(pi);
            }
            else if (tempOne.IsT1)
            {
                info = new FieldOrPropertyInfo(fi!);
            }

            setter = info;
            return !info.IsEmpty;
        }

        private static OneOf<FieldInfo, PropertyInfo> GetAsEitherInfo(MemberExpression memberExpression)
        {
            switch (memberExpression.Member.MemberType)
            {
                case MemberTypes.Field:
                    return (FieldInfo)memberExpression.Member;

                case MemberTypes.Property:
                    return (PropertyInfo)memberExpression.Member;

                case MemberTypes.Constructor:
                case MemberTypes.Event:
                case MemberTypes.Method:
                case MemberTypes.TypeInfo:
                case MemberTypes.Custom:
                case MemberTypes.NestedType:
                case MemberTypes.All:
                default:
                    return OneOf<FieldInfo, PropertyInfo>.Null;
            }
        }
    }
}

