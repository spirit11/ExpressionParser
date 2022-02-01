using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionParser
{
    public class ExpressionEvaluator : IInterpretator<double>
    {
        public ExpressionEvaluator(Func<string, double> variables)
        {
            this.variables = variables;
        }

        public double BinaryExpression(string operand, IExpression left, IExpression right) =>
            EvaluateBinaryFunction(operand, left.GetValue(this), right.GetValue(this));

        public double Constant(double value) => value;

        public double Function(string v, IExpression[] args)
        {
            Func<double[], double> applyFunc = v switch
            {
                "min" => arg => arg.Min(a => a),
                "max" => arg => arg.Max(a => a),
                "abs" when args.Length == 1 => arg => Math.Abs(arg[0]),
                "floor" when args.Length == 1 => arg => Math.Floor(arg[0]),
                "ceil" when args.Length == 1 => arg => Math.Ceiling(arg[0]),
                "round" when args.Length == 1 => arg => Math.Round(arg[0], MidpointRounding.AwayFromZero),
                _ => throw new UnknownFunctionException(v, args.Length),
            };
            return applyFunc(args.Select(a => a.GetValue(this)).ToArray());
        }

        public virtual double Variable(string name) => variables(name);

        private static readonly Dictionary<string, Func<double, double, double>> BinaryOperators = new()
        {
            { ">", (l, r) => l > r ? 1 : 0 },
            { ">=", (l, r) => l >= r ? 1 : 0 },
            { "<", (l, r) => l < r ? 1 : 0 },
            { "<=", (l, r) => l <= r ? 1 : 0 },
            { "!=", (l, r) => l != r ? 1 : 0 },
            { "==", (l, r) => l == r ? 1 : 0 },
            { "|", (l, r) => (l > 0 || r > 0) ? 1 : 0 },
            { "&", (l, r) => (l > 0 && r > 0) ? 1 : 0 },
            { "+", (l, r) => l + r },
            { "-", (l, r) => l - r },
            { "/", (l, r) => r == 0 ? 0 : l / r },
            { "*", (l, r) => l * r },
            { "^", (l, r) => Math.Pow(l, r) }
        };

        private static double EvaluateBinaryFunction(string name, double l, double r)
        {
            if (double.IsNaN(l) || double.IsNaN(r))
            {
                return double.NaN;
            }
            return BinaryOperators.TryGetValue(name, out var f) ? f(l, r) : throw new UnknownOperatorException(name);
        }

        private readonly Func<string, double> variables;
    }
}
