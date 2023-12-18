using MG.Posh.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace TestingModule_NETFramework
{
    [Cmdlet(VerbsDiagnostic.Test, "Type")]
    public sealed class TestTypeCmdlet : PSCmdlet
    {
        [Parameter]
        [ValidateTypes(false, true, typeof(string), typeof(Hashtable))]
        public object Input1 { get; set; }

        [Parameter]
        [ValidateTypes(false, false, typeof(IEnumerable))]
        public object Input2 { get; set; }

        protected override void ProcessRecord()
        {
            if (!(this.Input1 is null))
            {
                this.WriteObject(this.Input1, true);
            }

            if (!(this.Input2 is null))
            {
                this.WriteObject(this.Input2, true);
            }
        }
    }
}

