using MG.Posh.PSObjects;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace MG.Posh.Attributes
{
    /// <summary>
    /// An <see cref="ArgumentTransformationAttribute"/> implementation that allows for transformation on 
    /// the base object of the incoming parameter value.
    /// </summary>
    /// <remarks>
    ///     The base implementation simply unwraps an object if it is a <see cref="PSObject"/> instance.
    ///     In derived classes, this behavior can be expanded on.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class BaseObjectTransformationAttribute : ArgumentTransformationAttribute
    {
        /// <summary>
        /// Indicates whether to transform the argument's <see cref="PSObject.ImmediateBaseObject"/>
        /// value instead of <see cref="PSObject.BaseObject"/>.
        /// </summary>
        public bool UsePSObjectImmediateBase { get; }

        /// <summary>
        /// The default constructor for <see cref="BaseObjectTransformationAttribute"/> that will retrieve
        /// the <see cref="PSObject.BaseObject"/> of any PSObject argument that is
        /// passed to the parameter.
        /// </summary>
        public BaseObjectTransformationAttribute()
            : this(false)
        {
        }
        /// <summary>
        /// Initializes a new instance of for <see cref="BaseObjectTransformationAttribute"/> that will
        /// retrieve the base object of any PSObject argument that is passed to the parameter. The 
        /// base object to transform being either <see cref="PSObject.BaseObject"/> or 
        /// <see cref="PSObject.ImmediateBaseObject"/> as determined by 
        /// <paramref name="useImmediateBaseObject"/>.
        /// </summary>
        /// <param name="useImmediateBaseObject">
        ///     Determines if the base object of the argument passed to be transformed is 
        ///     <see cref="PSObject.ImmediateBaseObject"/> if <see langword="true"/>; otherwise,
        ///     <see cref="PSObject.BaseObject"/> when <see langword="false"/>.
        /// </param>
        public BaseObjectTransformationAttribute(bool useImmediateBaseObject)
        {
            this.UsePSObjectImmediateBase = useImmediateBaseObject;
        }

        /// <summary>
        /// Method that transforms the <paramref name="inputData"/> parameter after retrieving its 
        /// wrapped base object argument into some other <see cref="object"/> or <see langword="null"/> 
        /// that will be used for the parameter's value.
        /// </summary>
        /// <param name="engineIntrinsics">
        ///     The engine APIs for the context under which the transformation is being made.
        /// </param>
        /// <param name="inputData">
        ///     Parameter argument to mutate.
        /// </param>
        /// <returns>
        ///     The transformed value(s) of <paramref name="inputData"/> or <see langword="null"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Should be thrown for invalid arguments.
        /// </exception>
        /// <exception cref="ArgumentTransformationMetadataException">
        ///     Should be thrown for any problems during transformation.
        /// </exception>
        public sealed override object? Transform(EngineIntrinsics engineIntrinsics, object? inputData)
        {
            object? immediate = BaseObject.FromObject(inputData, this.UsePSObjectImmediateBase);

            return this.TransformBaseObject(immediate, immediate is null, engineIntrinsics);
        }

        /// <summary>
        /// When overriden in derived classes, takes the base object output from the input data provided 
        /// to <see cref="Transform(EngineIntrinsics, object?)"/> to transform the object that will be used
        /// for the parameter's value.
        /// </summary>
        /// <remarks>
        ///     The default implementation will just return <paramref name="baseObject"/> as is.
        /// </remarks>
        /// <param name="baseObject">
        ///     The base object of the argument to mutate.
        /// </param>
        /// <param name="objectIsNull">
        ///     Indicates whether <paramref name="baseObject"/> is <see langword="null"/>.
        /// </param>
        /// <param name="engineIntrinsics">
        ///     The engine APIs for the context under which the transformation is being made.
        /// </param>
        /// <returns>The transformed value(s) of <paramref name="baseObject"/>.</returns>
        /// <exception cref="ArgumentException">Should be thrown for invalid arguments.</exception>
        /// <exception cref="ArgumentTransformationMetadataException">
        ///     Should be thrown for any problems during transformation.
        /// </exception>
        protected virtual object? TransformBaseObject(object? baseObject, bool objectIsNull, EngineIntrinsics engineIntrinsics)
        {
            return baseObject;
        }
    }
}

