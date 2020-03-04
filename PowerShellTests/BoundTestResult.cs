using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Posh.Extensions.Tests
{
    public class BoundTestResult
    {
        public bool AnyBound { get; }
        public bool AllBound { get; }
        public bool NameBound { get; }
        public bool InputBound { get; }
        public bool Passed { get; }

        internal BoundTestResult(TestExpectation test, bool any, bool all, bool n, bool io)
        {
            this.AnyBound = any;
            this.AllBound = all;
            this.NameBound = n;
            this.InputBound = io;

            switch (test)
            {
                case TestExpectation.AnyNoAllNameNoInput:
                    this.Passed = this.AnyBound && !this.AllBound && this.NameBound && !this.InputBound;
                    break;

                case TestExpectation.AnyNoAllNoNameInput:
                    this.Passed = this.AnyBound && !this.AllBound && !this.NameBound && this.InputBound;
                    break;

                case TestExpectation.AnyAllNameInput:
                    this.Passed = this.AnyBound && this.AllBound && this.NameBound && this.InputBound;
                    break;

                default:
                    this.Passed = !this.AnyBound && !this.AllBound && !this.NameBound && !this.InputBound;
                    break;
            }
        }
    }
}
