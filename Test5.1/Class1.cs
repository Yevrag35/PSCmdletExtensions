using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using MG.Posh.Extensions.Bound;

namespace Test5._1
{
    [Cmdlet(VerbsDiagnostic.Test, "This")]
    public class Class1 : PSCmdlet
    {
        [Parameter(Position = 0)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            bool result = this.IsParameterBoundPositionally(nameof(this.Name));
            this.WriteObject(result);
        }
    }
}
