using NUnit.Framework;
using CalculatorLib;
using Assert = NUnit.Framework.Assert;

namespace CalculatorLib.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        private Calculator _calc;

        [SetUp]
        public void Setup()
        {
            _calc = new Calculator();
        }

        [Test]
        public void Add_WhenCalled_ReturnsSum()
        {
            var result = _calc.Add(2, 3);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void Subtract_WhenCalled_ReturnsDifference()
        {
            var result = _calc.Subtract(5, 3);
            Assert.That(result, Is.EqualTo(2));
        }
    }
}
