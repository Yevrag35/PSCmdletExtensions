using MG.Posh.Extensions.Bound;
using MG.Posh.Extensions.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Tests
{
    public abstract class MockCmdlet : PSCmdlet
    {
        protected bool NameBound;
        protected bool IdBound;

        [Parameter(Mandatory = false, Position = 0, ParameterSetName = "ByName")]
        public virtual string[] Name { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ById")]
        public virtual Guid[] Id { get; set; }

        protected override void ProcessRecord()
        {
            base.WriteObject(new MockCmdletResult(NameBound, IdBound));
        }

        public class MockCmdletResult
        {
            public bool NameBound { get; }
            public bool IdBound { get; }

            public MockCmdletResult(bool name, bool id)
            {
                this.NameBound = name;
                this.IdBound = id;
            }
        }
        public virtual Dictionary<string, object> GetBoundParameters() => this.MyInvocation.BoundParameters;
    }
}
