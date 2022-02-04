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


        [Test]
        public void Test2()
        {
            var expr = new CompiledExpression("3+max(2,1)*  { a variable }");
            var result = expr.Evaluate(a => a == "a variable" ? 4 : throw new Exception("Unknown variable"));
            Assert.That(result, Is.EqualTo(11));
        }
    }
}
