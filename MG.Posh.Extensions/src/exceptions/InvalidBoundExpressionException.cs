using System;

namespace MG.Posh.Extensions.Bound
{
    public class InvalidBoundExpressionException : InvalidCastException
    {
        private const string DEF_MSG = "The specified expression is not a valid MemberExpression.  No member name could be parsed.";

        public InvalidBoundExpressionException() : base(DEF_MSG) { }
        public InvalidBoundExpressionException(Exception innerEx) : base(DEF_MSG, innerEx) { }
    }
}
