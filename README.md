# ![(icon)](https://api.nuget.org/v3-flatcontainer/mg.posh.extensions/1.2.1/icon) MG.Posh.Extensions

[![version](https://img.shields.io/nuget/v/MG.Posh.Extensions?style=flat-square)](https://www.nuget.org/packages/MG.Posh.Extensions) [![downloads](https://img.shields.io/nuget/dt/MG.Posh.Extensions?style=flat-square&color=darkgreen)](https://www.nuget.org/packages/MG.Posh.Extensions)

## BoundParameter Checking with LINQ Expression Example

```csharp
using System;
using System.Management.Automation;
using MG.Posh.Extensions.Bound;

[Cmdlet(VerbsCommon.Get, "Employee", DefaultParameterSetName="ByName")]
public class GetEmployee : PSCmdlet
{
    [Parameter(Mandatory=true, Position = 0, ParameterSetName="ByName")]
    public string Name { get; set; }
    
    [Parameter(Mandatory=true, Position = 0, ValueFromPipeline=true, ParameterSetName="ByEmployeeId")]
    public int Id { get; set; }
    
    protected override void ProcessRecord()
    {
        if (this.ContainsParameter(x => x.Name))
        {
            base.WriteVerbose("Gathering employee by name.");
        }
        // ... and so on.
    }
}

```

## PSObject creation with certain class members
```csharp
public struct Employee
{
    public int EmployeeId;
    public string FirstName;
    public string LastName;
    
    public PSObject GetName()
    {
        return PSOFactory.CreateFromObject(this, e => e.FirstName, e => e.LastName);
    }
}

// DISPLAYS
// FirstName LastName
// --------- --------
// Chuck     Norris
```
