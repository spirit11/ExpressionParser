using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionParser
{
    public class CompiledExpression
    {
        public CompiledExpression(string expression)
        {
            expressionTree = Parser.ParseExpression(expression);
        }

        public double Evaluate(Func<string, double> variableValuesGetter) =>
            expressionTree.GetValue(new ExpressionEvaluator(variableValuesGetter));

        private static readonly ExpressionsParser Parser = new(_ => true, PrecedenceTable);

        private static string[][] PrecedenceTable => new[] {
                new[] { "^" },
                new[] { "*", "/" },
                new[] { "+", "-" },
                new[] { ">=", "<=", ">", "<", "==", "!=" },
                new[] { "&" },
                new[] { "|" },
            };

        private readonly IExpression expressionTree;
    }
}
