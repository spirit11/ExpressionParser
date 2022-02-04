using Sprache;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExpressionParser
{
    public class ExpressionsParser
    {
        public ExpressionsParser(Func<string, bool> functionMatcher, string[][] operatorsPrecedence)
        {
            this.functionMatcher = functionMatcher;
            this.operatorsPrecedence = operatorsPrecedence;
        }

        public IExpression ParseExpression(string expression) => Formula.Parse(expression);

        private Parser<string> Name =>
            from first in Parse.Letter.Once()
            from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Many().Text()
            select first.First() + rest;

        private Parser<IExpression> Variable =>
            from n in NameInCurveParentheses().Or(Name)
            select new Variable(n);

        private Parser<T> InParentheses<T>(Func<Parser<T>> innerParser) =>
            from lparen in Parse.Char('(')
            from expr in innerParser()
            from rparen in Parse.Char(')')
            select expr;

        private Parser<string> NameInCurveParentheses() =>
            from lparen in Parse.Char('{')
            from expr in Parse.Char(c => c != '}', "variable in parentheses").Many().Text()
            from rparen in Parse.Char('}')
            select expr.Trim();

        private readonly Parser<IExpression> constant =
            from decimalString in Parse.DecimalInvariant
            select new Constant(double.Parse(decimalString, CultureInfo.InvariantCulture));

        private Parser<IEnumerable<IExpression>> ArgList =>
            Parse.ChainOperator(
                Parse.Char(','),
                Expr.Select(v => new List<IExpression> { v }),
                (_, acc, item) => { acc.AddRange(item); return acc; });

        private Parser<IExpression> Function =>
            from v in Name.Token().Where(functionMatcher)
            from arg in InParentheses(() => ArgList)
            select new Function(v, arg.ToArray());

        private Parser<IExpression> ExpressionInParentheses => InParentheses(() => Expr);

        private Parser<IExpression> Factor => Function.Or(Variable).XOr(constant).XOr(ExpressionInParentheses).Token();

        private Parser<IExpression> UnaryOperation =>
            from sign in Parse.Char('-').XOr(Parse.Char('+'))
            from factor in Factor.XOr(UnaryOperation)
            select sign == '-'
                ? new BinaryExpression("-", new Constant(0), factor)
                : factor;

        private Parser<IExpression> Expr =>
            BinaryOperation(operatorsPrecedence);

        private Parser<IExpression> BinaryOperation(string[][] operatorPrioritiesTable) =>
            operatorPrioritiesTable
                .Aggregate(UnaryOperation.XOr(Factor), (acc, operators) =>
                    Parse.ChainOperator(Operator(operators), acc, (op, left, right) => new BinaryExpression(op, left, right)));

        private Parser<string> Operator(params string[] operators) =>
            operators
                .Select(o => Parse.String(o))
                .Aggregate((l, r) => l.Or(r))
                .Token()
                .Text();

        private Parser<IExpression> Formula =>
            from e in Expr.End()
            select e;

        private readonly Func<string, bool> functionMatcher;
        private readonly string[][] operatorsPrecedence;
    }
}
