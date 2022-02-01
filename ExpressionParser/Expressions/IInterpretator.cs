namespace ExpressionParser
{
	public interface IInterpretator<T>
	{
		T Variable(string name);
		T BinaryExpression(string operand, IExpression left, IExpression right);
		T Constant(double value);
		T Function(string functionName, IExpression[] args);
	}
}

