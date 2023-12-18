using MG.Posh.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace TestingModule_NETFramework
{
    [Cmdlet(VerbsDiagnostic.Test, "Property")]
    public sealed class TestPropertyCmdlet : PSCmdlet
    {
        [Parameter]
        [ValidateProperty("Name")]
        public object NameObject { get; set; }

        [Parameter]
        [ValidateProperty("Id")]
        public object IdObject { get; set; }

        protected override void ProcessRecord()
        {
            if (!(this.NameObject is null))
            {
                this.WriteObject(this.NameObject, true);
            }
            
            if (!(this.IdObject is null))
            {
                this.WriteObject(this.IdObject, true);
            }
        }
    }
}
