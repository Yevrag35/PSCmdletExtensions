using MG.Posh.Internal.Reflection;
using System;
using System.Management.Automation;

namespace MG.Posh.PSProperties
{
    /// <summary>
    ///     An <see langword="abstract"/> base class that serves as a property that is a simple name-value
    ///     pair.
    /// </summary>
    /// <remarks>
    ///     Does not inherit from <see cref="PSNoteProperty"/> but does from <see cref="PSPropertyInfo"/>.
    ///     This allows for specific members to be overridden.
    /// </remarks>
    public abstract class PSNotePropertyBase : PSPropertyInfo
    {
        const string DEFAULT_PSTYPE = "object";
        const string DEFAULT_TYPE = "System.Object";

        /// <inheritdoc cref="PSNoteProperty.IsGettable"/>
        public sealed override bool IsGettable => true;
        /// <inheritdoc cref="PSNoteProperty.MemberType"/>
        public sealed override PSMemberTypes MemberType => PSMemberTypes.NoteProperty;
        /// <inheritdoc cref="PSNoteProperty.TypeNameOfValue"/>
        public override string TypeNameOfValue => this.Value?.GetType().GetTypeName() ?? DEFAULT_TYPE;

        /// <summary>
        /// Constructs the PowerShell-friendly name of the type of <see cref="PSMemberInfo.Value"/>
        /// </summary>
        /// <returns>
        ///     Returns the constructed type name or "object" if <see cref="PSMemberInfo.Value"/>
        ///     is <see langword="null"/>.
        /// </returns>
        protected virtual string GetPSTypeName()
        {
            return this.Value?.GetType().GetPSTypeName(removeBrackets: true) ?? DEFAULT_PSTYPE;
        }
        /// <summary>
        /// Returns a string representation of <see cref="PSMemberInfo.Value"/>.
        /// </summary>
        /// <remarks>
        ///     Default implementation simply returns <see cref="object.ToString()"/> on 
        ///     <see cref="PSMemberInfo.Value"/>.
        /// </remarks>
        /// <returns>
        ///     A string that represents <see cref="PSMemberInfo.Value"/> or if <see langword="null"/>,
        ///     an empty string.
        /// </returns>
        protected virtual string GetValueToString()
        {
            return this.Value?.ToString() ?? string.Empty;
        }

#if NET5_0_OR_GREATER
        const int SPACE_AND_EQUALS_LENGTH = 2;
        /// <inheritdoc cref="PSNoteProperty.ToString()"/>
        public override string ToString()
        {
            string psTypeName = this.GetPSTypeName();
            string valueAsStr = this.GetValueToString();

            int length = psTypeName.Length + this.Name.Length + valueAsStr.Length + SPACE_AND_EQUALS_LENGTH;

            return string.Create(length, (psTypeName, name: this.Name, valueAsStr), (chars, state) =>
            {
                state.psTypeName.AsSpan().CopyTo(chars);
                int position = state.psTypeName.Length;

                chars[position++] = ' ';

                state.name.AsSpan().CopyTo(chars.Slice(position));
                position += state.name.Length;

                chars[position++] = '=';

                state.valueAsStr.AsSpan().CopyTo(chars.Slice(position));
            });
        }
#else
        const string STR_FORMAT = "{0} {1}={2}";
        /// <inheritdoc cref="PSNoteProperty.ToString()"/>
        public override string ToString()
        {
            string psTypeName = this.GetPSTypeName();
            string valueAsStr = this.GetValueToString();
            return string.Format(STR_FORMAT, psTypeName, this.Name, valueAsStr);
        }
#endif
    }
}
