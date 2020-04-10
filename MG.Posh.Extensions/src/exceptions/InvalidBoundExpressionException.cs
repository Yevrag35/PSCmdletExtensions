using System;

namespace MG.Posh.Extensions.Bound
{
    public class InvalidBoundExpressionException : InvalidCastException
    {
        private const string DEF_MSG = "The ";
    }
}
