namespace ExpressionParser
{
	public class Variable : IExpression
	{
		public Variable(string name)
		{
			Name = name;
		}

		public string Name { get; }

		public T GetValue<T>(IInterpretator<T> interpretator) =>
			interpretator.Variable(Name);
	}
}

