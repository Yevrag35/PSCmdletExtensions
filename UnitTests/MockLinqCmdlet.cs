using MG.Posh.Extensions.Bound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace MG.Posh.Extensions.Tests
{
    public class MockLinqCmdlet : MockCmdlet
    {
        protected override void ProcessRecord()
        {
            base.NameBound = this.ContainsParameter(x => x.Name);
            base.IdBound = this.ContainsParameter(x => x.Id);
            base.ProcessRecord();
        }
    }

    //public class CustomScriptExtent : IScriptExtent
    //{
    //    private IScriptPosition _scriptPosition { get; } = new ScriptPosition("thing.ps1", 1, 0, "Test-BoundByLinq -Name \"Bob\", \"Mary\"");

    //    public int EndColumnNumber => 1;

    //    public int EndLineNumber => 1;

    //    public int EndOffset => 0;

    //    public IScriptPosition EndScriptPosition => _scriptPosition;

    //    public string File => string.Empty;

    //    public int StartColumnNumber => 1;

    //    public int StartLineNumber => 1;

    //    public int StartOffset => 0;

    //    public IScriptPosition StartScriptPosition => _scriptPosition;

    //    public string Text => string.Empty;

    //    public CustomScriptExtent() { }
    //}
}
