using NUnit.Framework;

namespace ProfileBook.Tests
{
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void SimpleTest_ShouldPass()
        {
            // Test basic NUnit assertions
            Assert.AreEqual(1, 1);
            Assert.IsNotNull("test");
            Assert.IsTrue(true);
        }
    }
}
