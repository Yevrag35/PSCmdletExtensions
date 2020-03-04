using MG.Posh.Extensions.Pipe;
using System;
using System.Management.Automation;

namespace MG.Posh.Extensions.Tests
{
    [Cmdlet(VerbsDiagnostic.Test, "PipedObject1", ConfirmImpact = ConfirmImpact.None)]
    [OutputType(typeof(PipedTestResult))]
    [CmdletBinding(PositionalBinding = false)]
    public class TestPipedObj1 : PSCmdlet
    {
        #region FIELDS/CONSTANTS


        #endregion

        #region PARAMETERS
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        public PSObject InputObject { get; set; }

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            if (this.TryGetPipedObject(out PSObject pso))
            {

            }
        }

        #endregion
    }
}