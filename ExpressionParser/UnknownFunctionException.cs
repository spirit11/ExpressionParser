using System;

namespace ExpressionParser
{
    public class UnknownFunctionException : Exception
    {
        public UnknownFunctionException(string function, int args)
            : base($"Unknown function {function} or unsupported arguments count {args}")
        {
            Data.Add("function", function);
            Data.Add("args", args);
        }
    }
}
