using MG.Posh.PSObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace TestingModule_NETFramework
{
    public sealed class TestObject
    {
        public Guid UniqueId { get; } = Guid.NewGuid();
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Cmdlet(VerbsCommon.New, "TestObject")]
    [OutputType(typeof(TestObject))]
    public sealed class NewTestObject : Cmdlet
    {
        [Parameter]
        public string Name { get; set; } = string.Empty;
        [Parameter]
        public string Description { get; set; } = string.Empty;

        [Parameter]
        public int Id { get; set; }

        protected override void ProcessRecord()
        {
            var to = new TestObject()
            {
                Description = this.Description,
                Id = this.Id,
                Name = this.Name,
            };

            this.WriteObject(to);
        }
    }

    [Cmdlet(VerbsDiagnostic.Test, "Projection")]
    public sealed class TestProjection : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public TestObject InputObject { get; set; }

        protected override void ProcessRecord()
        {
            var projectTo = this.InputObject.ProjectTo(x => new
            {
                GuysName = x.Name.Trim(),
                GuysNumber = x.Id + 2,
                UniqueGuyNumber = x.UniqueId,
                Desc = x.Description,
            });

            this.WriteObject(projectTo);
        }
    }
}

