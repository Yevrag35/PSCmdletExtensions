using MG.Posh.Extensions.Bound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace MG.Posh.Extensions.Tests
{
    [TestClass]
    public class BoundParameterTests
    {
        [TestMethod]
        public void ContainsSingleParameter()
        {
            MockCmdlet.MockCmdletResult res = null;
            foreach (PSObject pso in this.Invoke())
            {
                if (pso.BaseObject is MockCmdlet.MockCmdletResult result)
                {
                    res = result;
                    break;
                }
            }
            Assert.IsNotNull(res);
        }

        private Collection<PSObject> Invoke()
        {
            using (var powershell = PowerShell.Create(this.NewState()))
            {
                powershell.AddCommand("Test-NameBoundByLinq");
                return powershell.Invoke();
            }
        }
        private InitialSessionState NewState()
        {
            InitialSessionState state = InitialSessionState.Create();
            state.ImportPSModulesFromPath(@"D:\Local_Repos\MG.Posh.Extensions\UnitTests\bin\Debug\netcoreapp3.1\MG.Posh.Extensions.Tests.dll");
            return state;
        }
    }
}
