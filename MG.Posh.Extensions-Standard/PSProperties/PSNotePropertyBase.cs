using MG.Posh.Internal.Reflection;
using System.Management.Automation;

namespace MG.Posh.PSProperties
{
    public abstract class PSNotePropertyBase : PSPropertyInfo
    {
        const string DEFAULT_PSTYPE = "object";
        const string DEFAULT_TYPE = "System.Object";

        public sealed override bool IsGettable => true;
        public sealed override PSMemberTypes MemberType => PSMemberTypes.NoteProperty;
        public override string TypeNameOfValue => this.Value?.GetType().GetTypeName() ?? DEFAULT_TYPE;

        protected virtual string GetPSTypeName()
        {
            return this.Value?.GetType().GetPSTypeName(removeBrackets: true) ?? DEFAULT_PSTYPE;
        }
        protected virtual string GetValueToString()
        {
            return this.Value?.ToString() ?? string.Empty;
        }

        const string STR_FORMAT = "{0} {1}={2}";
        public override string ToString()
        {
            string psTypeName = this.GetPSTypeName();
            string valueAsStr = this.GetValueToString();
            return string.Format(STR_FORMAT, psTypeName, this.Name, valueAsStr);
        }
    }
}

