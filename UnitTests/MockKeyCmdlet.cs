using MG.Posh.Extensions.Bound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Tests
{
    public class MockKeyCmdlet : MockCmdlet
    {
        protected override void ProcessRecord()
        {
            base.NameBound = this.ContainsAllParameters("Name");
            base.IdBound = this.ContainsAllParameters("Id");
            base.ProcessRecord();
        }
    }
}
