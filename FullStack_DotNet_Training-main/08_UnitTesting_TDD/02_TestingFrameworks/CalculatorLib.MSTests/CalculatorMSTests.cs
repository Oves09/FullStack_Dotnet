using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculatorLib;


namespace CalculatorLib.MSTests
{
    [TestClass]
    public class CalculatorMSTests
    {
        private Calculator _calc;

        [TestInitialize]
        public void Init() => _calc = new Calculator();

        [TestMethod]
        public void Add_ShouldReturnCorrectSum()
        {
            Assert.AreEqual(9, _calc.Add(5, 4));
        }
    }
}
