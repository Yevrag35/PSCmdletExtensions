using MG.Posh.Extensions.Bound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Tests
{
    [Cmdlet(VerbsDiagnostic.Test, "NameBoundByLinq")]
    public class MockNameLinqCmdlet : MockLinqCmdlet
    {
        protected override void BeginProcessing()
        {
            this.MyInvocation.BoundParameters.Add("Name", new string[2] { "Bob", "Mary" });
        }
        protected override void ProcessRecord() => base.ProcessRecord();
    }
}
