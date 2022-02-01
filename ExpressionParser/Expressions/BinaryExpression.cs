namespace ExpressionParser
{
	public class BinaryExpression : IExpression
	{
		public BinaryExpression(string operand, IExpression left, IExpression right)
		{
			this.operand = operand;
			this.left = left;
			this.right = right;
		}

		public T GetValue<T>(IInterpretator<T> interpretator) =>
			interpretator.BinaryExpression(operand, left, right);

		private readonly string operand;
		private readonly IExpression left;
		private readonly IExpression right;
	}
}

