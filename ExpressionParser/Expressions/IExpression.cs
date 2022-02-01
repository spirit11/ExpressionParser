namespace ExpressionParser
{
    public interface IExpression
    {
        T GetValue<T>(IInterpretator<T> interpretator);
    }
}

