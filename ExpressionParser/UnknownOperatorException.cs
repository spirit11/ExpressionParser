using System;

namespace ExpressionParser
{
    public class UnknownOperatorException : Exception
    {
        public UnknownOperatorException(string @operator)
            : base($"Unknown operator {@operator}")
        {
            Data.Add("operator", @operator);
        }
    }
}
