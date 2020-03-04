using System;

namespace MG.Posh.Extensions.Tests
{
    public enum TestExpectation
    {
        AnyNoAllNameNoInput,
        AnyNoAllNoNameInput,
        AnyAllNameInput,
        NoAnyNoAllNoNameNoInput,

        EmptyPipe,
        HasPipe,
        NoPipe,
        TryHasPipe,
        TryEmptyPipe,
        TryNoPipe
    }
}
