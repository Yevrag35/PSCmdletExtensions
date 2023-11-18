using MG.Posh.Extensions.Bound;
using System.Management.Automation;

namespace Testing
{
    [Cmdlet(VerbsDiagnostic.Test, "This")]
    public sealed class TestCmdlet : PSCmdlet
    {
        [Parameter(Position = 0)]
        public string Name { get; set; } = null!;

        protected override void ProcessRecord()
        {
            bool result = this.IsParameterBoundPositionally(nameof(this.Name));
            this.WriteObject(result);
        }
    }
}

