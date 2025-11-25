namespace DUTests
{
    public class Tests
    {
        // Körs före varje test
        [SetUp]
        public void Setup()
        {
        }

        // Ett enkelt test som alltid går igenom
        [Test]
        public void Test1()
        {
            Assert.Pass(); // Markerar testet som godkänt
        }
    }
}
