using NUnit.Framework;
using CalculatorLibrary;
using System;

namespace CalculatorTests
{
    public class CalculatorTests
    {
        private Calculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = new Calculator();
        }

        [Test]
        public void Add_ValidInputs_ReturnsCorrectResult()
        {
            Assert.AreEqual(5, calculator.Add(2, 3));
            Assert.AreEqual(-1, calculator.Add(-2, 1));
            Assert.AreEqual(0, calculator.Add(0, 0));
        }

        [Test]
        public void Subtract_ValidInputs_ReturnsCorrectResult()
        {
            Assert.AreEqual(1, calculator.Subtract(3, 2));
            Assert.AreEqual(-3, calculator.Subtract(-2, 1));
            Assert.AreEqual(0, calculator.Subtract(0, 0));
        }

        [Test]
        public void Multiply_ValidInputs_ReturnsCorrectResult()
        {
            Assert.AreEqual(6, calculator.Multiply(2, 3));
            Assert.AreEqual(-2, calculator.Multiply(-2, 1));
            Assert.AreEqual(0, calculator.Multiply(0, 5));
        }

        [Test]
        public void Divide_ValidInputs_ReturnsCorrectResult()
        {
            Assert.AreEqual(2, calculator.Divide(6, 3));
            Assert.AreEqual(-2, calculator.Divide(-4, 2));
        }

        [Test]
        public void Divide_DivisionByZero_ThrowsException()
        {
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(5, 0));
        }
    }
}
