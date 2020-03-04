using MG.Posh.Extensions.Pipe;
using System;
using System.Management.Automation;

namespace MG.Posh.Extensions.Tests
{
    [Cmdlet(VerbsDiagnostic.Test, "PipedObject2", ConfirmImpact = ConfirmImpact.None)]
    [OutputType(typeof(PipedTestResult))]
    [CmdletBinding(PositionalBinding = false)]
    public class TestPipedObj2 : PSCmdlet
    {
        #region FIELDS/CONSTANTS


        #endregion

        #region PARAMETERS
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        public PSObject InputObject { get; set; }

        [Parameter(Mandatory = false)]
        public TestExpectation Test { get; set; } = TestExpectation.NoPipe;

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            if (this.Test != TestExpectation.TryHasPipe && this.Test != TestExpectation.TryEmptyPipe && this.Test != TestExpectation.TryNoPipe)
            {
                bool has = this.HasPipedObject();
                PSObject pso = this.GetPipedObject();
                base.WriteObject(new PipedTestResult(this.Test, has, pso.ImmediateBaseObject));
            }
            else
            {
                bool empty = this.HasPipedObject();
                bool tryThis = this.TryGetPipedObject(out PSObject outPso);
                base.WriteObject(new PipedTestResult(this.Test, empty, outPso.ImmediateBaseObject, tryThis));
            }
        }

        #endregion
    }
}