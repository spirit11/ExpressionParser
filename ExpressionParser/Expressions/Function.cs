namespace ExpressionParser
{
	public class Function : IExpression
	{
		public Function(string v, IExpression[] values)
		{
			this.v = v;
			this.values = values;
		}

		public T GetValue<T>(IInterpretator<T> interpretator) =>
			interpretator.Function(v, values);

		private readonly string v;
		private readonly IExpression[] values;
	}
}

