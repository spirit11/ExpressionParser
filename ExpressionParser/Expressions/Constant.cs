namespace ExpressionParser
{
	public class Constant : IExpression
	{
		public Constant(double value)
		{
			this.value = value;
		}

		public T GetValue<T>(IInterpretator<T> interpretator) =>
			interpretator.Constant(value);

		private readonly double value;
	}
}

