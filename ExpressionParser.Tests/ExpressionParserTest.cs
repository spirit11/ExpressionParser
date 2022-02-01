using NUnit.Framework;
using System;

namespace ExpressionParser.Tests
{
    [TestFixture]
    public class ExpressionParserTest
    {
        [Test]
        public void Test()
        {
            var expr = new CompiledExpression("3+max(2,1)*a");
            var result = expr.Evaluate(a => a == "a" ? 4 : throw new Exception("Unknown variable"));
            Assert.That(result, Is.EqualTo(11));
        }
    }
}
