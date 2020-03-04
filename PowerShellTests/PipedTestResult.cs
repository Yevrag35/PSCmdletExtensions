using System;
using System.Management.Automation;

namespace MG.Posh.Extensions.Tests
{
    public class PipedTestResult
    {
        public bool HasPipedObject { get; }
        public object PipedObject { get; }
        public bool Try { get; }
        public bool Passed { get; }

        internal PipedTestResult(TestExpectation test, bool has, object obj)
        {
            this.HasPipedObject = has;
            this.PipedObject = obj;
            switch (test)
            {
                case TestExpectation.HasPipe:
                    this.Passed = this.HasPipedObject && obj != null;
                    break;

                case TestExpectation.EmptyPipe:
                    this.Passed = !this.HasPipedObject && obj != null;
                    break;

                default:
                    this.Passed = !this.HasPipedObject;
                    break;
            }
        }
        internal PipedTestResult(TestExpectation test, bool has, object obj, bool tried)
        {
            this.HasPipedObject = has;
            this.PipedObject = obj;
            this.Try = tried;
            switch (test)
            {
                case TestExpectation.TryHasPipe:
                    this.Passed = this.HasPipedObject && obj != null && this.Try;
                    break;

                case TestExpectation.TryEmptyPipe:
                    this.Passed = !this.HasPipedObject && obj != null && !this.Try;
                    break;

                default:
                    this.Passed = !this.HasPipedObject && obj == null && !this.Try;
                    break;
            }
        }
    }
}
