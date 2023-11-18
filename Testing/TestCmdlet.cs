using MG.Posh.Extensions.Bound;
using System.Management.Automation;

namespace Testing
{
    [Cmdlet(VerbsDiagnostic.Test, "This")]
    public sealed class TestCmdlet : PSCmdlet
    {
        [Parameter]
        public string Name { get; set; } = null!;

        protected override void ProcessRecord()
        {
            //bool result = this.HasAnyParameter(excludeDefaultParameters: true);
            //this.WriteObject(result);
        }
    }
}

