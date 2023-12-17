using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Runtime.CompilerServices;

namespace MG.Posh.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ArgumentToTypeTransformAttribute : BaseObjectTransformationAttribute
    {
        /// <summary>
        /// The .NET type to make the argument value when the argument cannot be transformed. This defaults
        /// to <see langword="null"/>.
        /// </summary>
        public Type? DefaultTypeWhenInvalid { get; }

        public ArgumentToTypeTransformAttribute()
            : this(null)
        {
        }

        public ArgumentToTypeTransformAttribute(Type? defaultTypeWhenInvalid)
        {
            this.DefaultTypeWhenInvalid = defaultTypeWhenInvalid;
        }

        /// <summary>
        /// Performs any custom transformation logic on the <see cref="Type"/> that was parsed from the
        /// argument.
        /// </summary>
        /// <remarks>
        ///     This method is only run when a valid <see cref="Type"/> is resolved, therefore 
        ///     <paramref name="resolvedType"/> will never be <see langword="null"/>.
        ///     <para>
        ///         The base implementation simply returns <paramref name="resolvedType"/> as-is.    
        ///     </para>
        /// </remarks>
        /// <param name="resolvedType"></param>
        /// <param name="engineIntrinsics"></param>
        /// <returns>
        ///     The custom transformed <see cref="Type"/>.
        /// </returns>
        /// <exception cref="ParseException">
        ///     Thrown when <paramref name="resolvedType"/> is an invalid <see cref="Type"/> to be returned.
        /// </exception>
        protected virtual Type? PerformCustomTransform(Type resolvedType, EngineIntrinsics engineIntrinsics)
        {
            return resolvedType;
        }

        protected sealed override object? TransformBaseObject(object? baseObject, bool objectIsNull, EngineIntrinsics engineIntrinsics)
        {
            if (objectIsNull)
            {
                return this.DefaultTypeWhenInvalid;
            }

            try
            {
                return GetTypeObjectFromArgument(baseObject, this.DefaultTypeWhenInvalid, engineIntrinsics);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (ArgumentTransformationMetadataException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ArgumentTransformationMetadataException("Unable to transform the value to a .NET type.", e);
            }
        }

        #region BACKEND METHODS

        [return: NotNullIfNotNull(nameof(defaultType))]
        private static Type? GetTypeObjectFromArgument(object? target, Type? defaultType, EngineIntrinsics engineIntrinsics)
        {
            switch (target)
            {
                case Type type:
                    return type;

                case ScriptBlock block:
                    return ResolveTypeFromAst(block.Ast, defaultType, engineIntrinsics.SessionState.Module);

                case string typeName:
                    return ResolveTypeFromName(typeName, defaultType, engineIntrinsics.SessionState.Module);

                default:
                    return defaultType;
            }
        }

        const string NOT_TYPE_EX_FORMAT = "{0} is not a type expression.";
        const string NOT_VALID_TYPE_FORMAT = "{0} is not a valid .NET or custom-defined type.";
        const string PSREADLINE = "PSReadLine";
        private static bool IsModulePSReadLine([NotNullWhen(true)] PSModuleInfo? runningModule)
        {
            return PSREADLINE.Equals(runningModule?.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsTypeAst(Ast ast)
        {
            return ast is TypeExpressionAst;
        }

        /// <exception cref="ArgumentException"/>
        [return: NotNullIfNotNull(nameof(defaultType))]
        private static Type? ResolveTypeFromAst(Ast ast, Type? defaultType, PSModuleInfo? runningModule)
        {
            TypeExpressionAst? first = ast.Find(IsTypeAst, false) as TypeExpressionAst;
            if (first is null)
            {
                return defaultType;
            }

            Type? type = first.TypeName.GetReflectionType();
            if (!(type is null))
            {
                return type;
            }
            else if (IsModulePSReadLine(runningModule))
            {
                return defaultType;
            }
            else
            {
                ParseException ex = GetNotTypeExpressionException(ast);
                return ThrowNotValidType(ast.Extent.Text, ex);
            }
        }

        /// <exception cref="ArgumentException"/>
        [return: NotNullIfNotNull(nameof(defaultType))]
        private static Type? ResolveTypeFromName(string typeName, Type? defaultType, PSModuleInfo? runningModule)
        {
            Ast ast = Parser.ParseInput(typeName, out _, out ParseError[] errors);

            if (!(errors is null) && errors.Length > 0)
            {
                if (IsModulePSReadLine(runningModule))
                {
                    return defaultType;
                }

                ParseException ex = GetNotTypeExpressionException(ast);
                return ThrowNotValidType(typeName, ex);
            }

            return ResolveTypeFromAst(ast, defaultType, runningModule);
        }

        private static ParseException GetNotTypeExpressionException(Ast ast)
        {
            string msg = string.Format(NOT_TYPE_EX_FORMAT, ast.Extent.Text);
            return new ParseException(msg);
        }
        /// <summary>
        /// Throws a <see cref="ParseException"/> with a defined inner <see cref="Exception"/> stating 
        /// that <paramref name="typeName"/> is not a valid type.
        /// </summary>
        /// <param name="typeName">The name of the type name resolved from the argument.</param>
        /// <param name="innerException">The inner exception causing this exception.</param>
        /// <returns>
        ///     Does not return and always throws a <see cref="ParseException"/>.
        /// </returns>
        /// <exception cref="ParseException"/>
        [DoesNotReturn]
        protected static Type ThrowNotValidType(string typeName, Exception? innerException)
        {
            string msg = string.Format(NOT_VALID_TYPE_FORMAT, typeName);
            throw new ParseException(msg, innerException);
        }

        #endregion
    }
}

