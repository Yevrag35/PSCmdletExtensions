using MG.Posh.Extensions.Bound;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace MG.Posh.Extensions.Tests
{
    [Cmdlet(VerbsDiagnostic.Test, "BoundParameter", ConfirmImpact = ConfirmImpact.None)]
    [OutputType(typeof(BoundTestResult))]
    [CmdletBinding(PositionalBinding = false)]
    public class TestBoundParameter : PSCmdlet
    {
        #region PARAMETERS
        [Parameter(Mandatory = false)]
        public string[] Name { get; set; }

        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        public PSObject InputObject { get; set; }

        [Parameter(Mandatory = false, Position = 0)]
        public TestExpectation Test { get; set; } = TestExpectation.NoAnyNoAllNoNameNoInput;

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            bool any = this.ContainsAnyParameters(x => x.InputObject, x => x.Name);
            bool all = this.ContainsAllParameters(x => x.InputObject, x => x.Name);
            bool name = this.ContainsParameter(x => x.Name);
            bool io = this.ContainsParameter(x => x.InputObject);
            base.WriteObject(new BoundTestResult(this.Test, any, all, name, io));
        }

        #endregion
    }
}